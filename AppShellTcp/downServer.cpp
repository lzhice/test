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
QList<logOut*> logOut::logOutList;
DownServer::DownServer( QObject *parent) :
    QTcpServer(parent)
{
    connect(LogForm::getInstance(),SIGNAL(sigUdpDate(QByteArray)),this,SLOT(dealSigUdpDate(QByteArray)),Qt::UniqueConnection);
    connect(&m_timer,&QTimer::timeout,[=](){
        logOut log("DownServer::QTimer::timeout()");
        if(liveReceiverData<0){
            m_udpReceiver.close();
            m_udpReceiver.bind(QHostAddress("127.0.0.1"),m_udpReceivePort,QUdpSocket::DontShareAddress);
        }else{
            --liveReceiverData;
        }
    });
    m_timer.start(800);
}
void DownServer::sendData(quint8 floorNum, quint8 roomNum, QByteArray bytes)
{
    logOut log("DownServer::sendData()");
    if(m_TcpSocketTbl[QPair<quint8,quint8>(floorNum,roomNum)])
        writeData(m_TcpSocketTbl[QPair<quint8,quint8>(floorNum,roomNum)],bytes);
}

void DownServer::dealSigUdpDate(const QByteArray &bytes)
{

    //qDebug()<<"LogForm::setUdpDate*****************************************"<<m_udpReceiver.errorString()<<re;

//    qDebug()<< m_udpReceiver.writeDatagram(bytes.data(),bytes.size(),
//                                QHostAddress("127.0.0.1"),m_udpSendPort);
//    qDebug()<<"m_udpReceiver.waitForBytesWritten()"<<m_udpReceiver.waitForBytesWritten();
//    if(isCanLog()){
//        sendUdpLog(QString("[%1:%2 ]").arg("127.0.0.1").arg(m_udpSendPort)+bytes.toHex(' ').toUpper());
//    }
}

void DownServer::incomingConnection(qintptr socketfd)
{
    logOut log("DownServer::incomingConnection()");
    QTcpSocket *client = new QTcpSocket(this);
    client->setSocketDescriptor(socketfd);
    connect(client, SIGNAL(readyRead()), this, SLOT(readyRead()));
    connect(client, SIGNAL(disconnected()), this, SLOT(disconnected()));
    //writeData(client,QByteArray(1,0x06)+QByteArray(23, 0x00));
}

void DownServer::close()
{
    logOut log("DownServer::close()");
    foreach (QTcpSocket* pTcpSocket, m_TcpSocketTbl.values()) {
        if(pTcpSocket)pTcpSocket->close();
    }
    m_TcpSocketTbl.clear();
    QTcpServer::close();
}
//typedef QPair<quint8,quint8> Pair;
//void DownServer::removeClient(QTcpSocket *client)
//{
//    logOut log("DownServer::removeClient()");
//    if(!client) return;
//    foreach (Pair key, m_TcpSocketTbl.keys()) {
//        if(m_TcpSocketTbl[key]==client){
//            m_TcpSocketTbl.remove(key);
//        }
//    }
//}
void DownServer::removeClient(QTcpSocket *client)
{
    logOut log("DownServer::removeClient()");
    if(!client) return;
    while(1){
        if(m_TcpSocketTbl.values().contains(client)){
            m_TcpSocketTbl.remove(m_TcpSocketTbl.key(client));
        }
        if(!m_TcpSocketTbl.values().contains(client)){
            break;
        }
    }

    if(m_TcpSocketTbl.values().contains(client)){
        qDebug()<<"removeClient m_TcpSocketTbl";
    }
}

void DownServer::writeData(QTcpSocket *client, const QByteArray &data)
{
    logOut log("DownServer::writeData()");
    if(client){
        client->write(data);
        if(isCanLog()){
            sendTcpLog(QString("[%1:%2 ]").arg(client->peerAddress().toString()).arg(client->peerPort())+data.toHex(' ').toUpper());
        }
    }
}

bool DownServer::isCanLog()
{
    logOut log("DownServer::isCanLog()");
    if(LogForm::getInstance()&&LogForm::getInstance()->isVisible()){

        return true;
    }return false;
}

void DownServer::sendUdpLog(const QString &logText)
{
    logOut log("DownServer::sendUdpLog()");
    if(LogForm::getInstance()){
        LogForm::getInstance()->sendUdpLog(logText);
    }
}

void DownServer::revUdpLog(const QString &logText)
{
    logOut log("DownServer::revUdpLog()");
    if(LogForm::getInstance()){
        LogForm::getInstance()->revUdpLog(logText);
    }
}

void DownServer::sendTcpLog(const QString &logText)
{
    logOut log("DownServer::sendTcpLog()");
    if(LogForm::getInstance()){
        LogForm::getInstance()->sendTcpLog(logText);
    }
}

void DownServer::revTcpLog(const QString &logText)
{
    logOut log("DownServer::revTcpLog()");
    if(LogForm::getInstance()){
        LogForm::getInstance()->revTcpLog(logText);
    }
}
/*
    void connected();
    void disconnected();
    void stateChanged(QAbstractSocket::SocketState);
    void error(QAbstractSocket::SocketError);
*/
bool DownServer::startServer()
{
    //qDebug()<<m_port<<m_udpReceivePort<<m_udpSendPort;
    logOut log("DownServer::startServer()");
    bool re=this->listen(QHostAddress::Any,m_port);
    connect(&m_udpReceiver,SIGNAL(readyRead()),this,SLOT(processPendingDatagram()));
    //m_TcpSocketTbl.value( QPair<quint8,quint8>(0,0))->close();
    return re;

}

void DownServer::readyRead()
{
    QTcpSocket *client = (QTcpSocket*)sender();

    //qDebug()<<"DownServer::readyRead()"<<client->bytesAvailable();
    if(client&&client->bytesAvailable()>=24){
        logOut log("DownServer::readyRead()");
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
                sendUdpLog(QString("[%1:%2 ]").arg("127.0.0.1").arg(m_udpSendPort)+bytes.toHex(' ').toUpper());
            }
        }
    }
}
void DownServer::disconnected()
{
    QTcpSocket *client = (QTcpSocket*)sender();
    if(client){
        logOut log("DownServer::disconnected()");
        removeClient(client);
        client->close();
        client->deleteLater();
    }
}

void DownServer::processPendingDatagram()
{

    while(m_udpReceiver.hasPendingDatagrams()&&m_udpReceiver.pendingDatagramSize()>=24)
    {
        logOut log("DownServer::processPendingDatagram()");
        liveReceiverData=5;

        QByteArray bytesAll;
        bytesAll.resize(m_udpReceiver.pendingDatagramSize());
        QHostAddress srcAddress;
        quint16 nSrcPort;
        m_udpReceiver.readDatagram(bytesAll.data(),bytesAll.size(),&srcAddress,&nSrcPort);
//        if(bytesAll.size()>24){
//            if(isCanLog()){
//                revUdpLog(QString("err--------------------------udp->pendingDatagramSize()>24--")+QString::number(bytesAll.size()));

//                revUdpLog(QString("[%1:%2 ]").arg(srcAddress.toString()).arg(nSrcPort)+bytesAll.toHex(' ').toUpper());
//            }

//        }
        QByteArray bytes;
        while (1) {
            if(bytesAll.size()<24){
                if(bytesAll.size()>0){
                    if(isCanLog()){
                        revUdpLog(QString("err--------------------------lose data:")+bytesAll.toHex(' ').toUpper());
                    }
                }
                break;
            }
            bytes=bytesAll.mid(0,24);
            bytesAll.remove(0,24);
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
}



logOut::logOut(const QString &logText)
{
    m_logText=logText;
    logOutList.append(this);
    //qDebug()<<QString("[in]:")+m_logText;
}

logOut::~logOut()
{
    //qDebug()<<QString("[out]:")+m_logText;
    for(int i=logOutList.size()-1;i>-1;--i){
        if(logOutList[i]==this){
            logOutList.removeAt(i);
            break;
        }
    }
}
