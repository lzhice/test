#include "downServer.h"
#include <QStringList>
#include <QDebug>
#include <QFile>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonArray>
#include <QVariant>
#include <QCoreApplication>
#include <QDateTime>
#include "logform.h"
DownServer::DownServer( QObject *parent) :
    QTcpServer(parent)
{

}
void DownServer::sendData(quint8 floorNum, quint8 roomNum, QByteArray bytes)
{
    if(m_TcpSocketTbl[QPair<quint8,quint8>(floorNum,roomNum)])
        writeData(m_TcpSocketTbl[QPair<quint8,quint8>(floorNum,roomNum)],bytes);
}

void DownServer::incomingConnection(qintptr socketfd)
{
    qDebug()<<"incomingConnection"<<socketfd;
    QTcpSocket *client = new QTcpSocket(this);
    client->setSocketDescriptor(socketfd);
    connect(client, SIGNAL(readyRead()), this, SLOT(readyRead()));
    connect(client, SIGNAL(disconnected()), this, SLOT(disconnected()));
    //writeData(client,QByteArray(1,0x06)+QByteArray(23, 0x00));
}

void DownServer::close()
{
    foreach (QTcpSocket* pTcpSocket, m_TcpSocketTbl.values()) {
        if(pTcpSocket)pTcpSocket->close();
    }
    QTcpServer::close();
}

void DownServer::removeClient(QTcpSocket *client)
{
    if(!client) return;
    if(m_TcpSocketTbl.values().contains(client)){
        m_TcpSocketTbl.remove(m_TcpSocketTbl.key(client));
    }
}

void DownServer::writeData(QTcpSocket *client, const QByteArray &data)
{
    if(client){
        client->write(data);
        if(isCanLog()){
            sendTcpLog(QString("[%1:%2 ]").arg(client->peerAddress().toString()).arg(client->peerPort())+data.toHex(' ').toUpper());
        }
    }
}

bool DownServer::isCanLog()
{
    if(LogForm::getInstance()&&LogForm::getInstance()->isVisible()){
        return true;
    }return false;
}

void DownServer::sendUdpLog(const QString &logText)
{
    if(LogForm::getInstance()){
        LogForm::getInstance()->sendUdpLog(logText);
    }
}

void DownServer::revUdpLog(const QString &logText)
{
    if(LogForm::getInstance()){
        LogForm::getInstance()->revUdpLog(logText);
    }
}

void DownServer::sendTcpLog(const QString &logText)
{
    if(LogForm::getInstance()){
        LogForm::getInstance()->sendTcpLog(logText);
    }
}

void DownServer::revTcpLog(const QString &logText)
{
    if(LogForm::getInstance()){
        LogForm::getInstance()->revTcpLog(logText);
    }
}

bool DownServer::startServer()
{
    qDebug()<<m_port<<m_udpReceivePort<<m_udpSendPort;

    bool re=this->listen(QHostAddress::Any,m_port);
    if(re){
       re = m_udpReceiver.bind(QHostAddress("127.0.0.1"),m_udpReceivePort,QUdpSocket::DontShareAddress);
        connect(&m_udpReceiver,SIGNAL(readyRead()),this,SLOT(processPendingDatagram()));
    }
    return re;
}

void DownServer::readyRead()
{
    QTcpSocket *client = (QTcpSocket*)sender();
    qDebug()<<"DownServer::readyRead()"<<client->bytesAvailable();
    if(client&&client->bytesAvailable()>=24){
        if(client->bytesAvailable()>24){
            if(isCanLog()){
                revTcpLog(QString("err--------------------------client->bytesAvailable()>24"));
            }
        }
        QByteArray bytes=client->readAll();
        if(isCanLog()){
            revTcpLog(QString("[%1:%2 ]").arg(client->peerAddress().toString()).arg(client->peerPort())+bytes.toHex(' ').toUpper());
        }
        quint8 floorNum=(quint8)bytes.at(0);
        quint8 roomNum=(quint8)bytes.at(1);
        if(bytes.at(2)==0x05){//应答心跳包
            m_TcpSocketTbl.insert(QPair<quint8,quint8>(floorNum,roomNum),client);
            writeData(client,QByteArray(1, 0x06)+bytes.mid(0,2)+QByteArray(21, 0x00));
        }else{
            QByteArray datagram=bytes.mid(2,22)+bytes.mid(0,2);
            m_udpReceiver.writeDatagram(datagram.data(),datagram.size(),
                                      QHostAddress("127.0.0.1"),m_udpSendPort);
            if(isCanLog()){
                revUdpLog(QString("[%1:%2 ]").arg("127.0.0.1").arg(m_udpSendPort)+bytes.toHex(' ').toUpper());
            }
        }
    }
}
void DownServer::disconnected()
{
    QTcpSocket *client = (QTcpSocket*)sender();
    if(client){
        client->close();
        removeClient(client);
        client->deleteLater();
    }
}

void DownServer::processPendingDatagram()
{
    while(m_udpReceiver.hasPendingDatagrams()&&m_udpReceiver.pendingDatagramSize()>=24)
    {
        if(m_udpReceiver.pendingDatagramSize()>24){
            if(isCanLog()){
                revUdpLog(QString("err--------------------------udp->pendingDatagramSize()>24"));
            }
        }
        QByteArray bytes;
        bytes.resize(m_udpReceiver.pendingDatagramSize());
        QHostAddress srcAddress;
        quint16 nSrcPort;
        m_udpReceiver.readDatagram(bytes.data(),bytes.size(),&srcAddress,&nSrcPort);
        if(isCanLog()){
            revUdpLog(QString("[%1:%2 ]").arg(srcAddress.toString()).arg(nSrcPort)+bytes.toHex(' ').toUpper());
        }
        if((quint8)bytes.at(0)!=0x00){
            quint8 floorNum=(quint8)bytes.at(1);
            quint8 roomNum=(quint8)bytes.at(2);
            sendData(floorNum,roomNum,bytes);
        }else{//广播cmd
            foreach (QTcpSocket* pTcpSocket, m_TcpSocketTbl.values()) {
                if(pTcpSocket)writeData(pTcpSocket,bytes);
            }
        }
    }
}


