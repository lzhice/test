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
static QByteArray upHeadBytes=QByteArray::fromHex("70756D70");
static QByteArray downHeadBytes=QByteArray::fromHex("686F7374");
static int heartOutTimerCount=3;
DownServer::DownServer(quint16 port, quint8 devCount, QObject *parent) :
    QTcpServer(parent),m_port(port),m_devCount(devCount)
{
    connect(&m_timer,SIGNAL(timeout()),this,SLOT(onHeartbeatTest()));
    connect(&m_serchTimer,SIGNAL(timeout()),this,SLOT(onSerch()));
    connect(&m_startTcpTimer,SIGNAL(timeout()),this,SLOT(onStartTcp()));
    m_timer.start(10000);
    m_serchTimer.start(500);
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
    }
}

bool DownServer::startServer()
{
    bool re=this->listen(QHostAddress::Any,m_port);
    if(re){
        m_udpReceiver.bind(QHostAddress("127.0.0.1"),m_udpReceivePort,QUdpSocket::DontShareAddress);
        connect(&m_udpReceiver,SIGNAL(readyRead()),this,SLOT(processPendingDatagram()));
    }
    return re;
}

void DownServer::readyRead()
{
    QTcpSocket *client = (QTcpSocket*)sender();

    if(client&&client->bytesAvailable()==24){
        QByteArray bytes=client->readAll();
        quint8 floorNum=(quint8)bytes.at(0);
        quint8 roomNum=(quint8)bytes.at(1);
        if(bytes.at(2)==0x04){//楼层房号应答

        }else if(bytes.at(2)==0x05){//应答心跳包

        }else{
            QByteArray datagram=bytes.mid(2,22)+bytes.mid(0,2);
            m_udpSender.writeDatagram(datagram.data(),datagram.size(),
                                      QHostAddress("127.0.0.1"),m_udpSendPort);
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
    while(m_udpReceiver.hasPendingDatagrams()&&m_udpReceiver.pendingDatagramSize()==24)
    {
        QByteArray bytes;
        bytes.resize(m_udpReceiver.pendingDatagramSize());
        QHostAddress srcAddress;
        quint16 nSrcPort;
        m_udpReceiver.readDatagram(bytes.data(),bytes.size(),&srcAddress,&nSrcPort);
        if((quint8)bytes.at(0)==0x01){
            quint8 floorNum=(quint8)bytes.at(1);
            quint8 roomNum=(quint8)bytes.at(2);
            sendData(floorNum,roomNum,bytes);
        }
    }
}


