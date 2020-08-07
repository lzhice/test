#ifndef EVENTSERVER_H
#define EVENTSERVER_H
#include <QThread>
#include <QTcpServer>
#include <QTcpSocket>
#include <QByteArray>
#include <QTimer>
#include <QUdpSocket>
class DownServer : public QTcpServer
{
    Q_OBJECT
public:
    friend class EventTransmitManager;
    explicit DownServer( QObject *parent = 0);
    void setSerPort(quint16 portTcp,quint16 portUdpServer,quint16 portUdpClient){m_port=portTcp;m_udpReceivePort=portUdpServer;m_udpSendPort=portUdpClient;}
signals:
    void sigReadyRead(quint8 cmd,QByteArray bytes);
    void sigDevDisconnectChange(QHash<quint8,quint8> disconnectDevs);
public slots:
    bool startServer();

private slots:
    void readyRead();
    void disconnected();
    void processPendingDatagram();
    void sendData(quint8 floorNum, quint8 roomNum, QByteArray bytes);
protected:
    void incomingConnection(qintptr socketfd);
    void close();
private:
    QUdpSocket m_udpSender;
    quint16 m_udpSendPort=20000;

    QUdpSocket m_udpReceiver;
    quint16 m_udpReceivePort=20001;

    quint16 m_port=55555;
    QHash<QPair<quint8,quint8>,QTcpSocket*> m_TcpSocketTbl;

    inline void removeClient(QTcpSocket *client);
    inline void writeData(QTcpSocket *client,const QByteArray &data);
    bool isCanLog();
    void sendUdpLog(const QString & logText);
    void revUdpLog(const QString & logText);
    void sendTcpLog(const QString & logText);
    void revTcpLog(const QString & logText);
};

#endif // EVENTSERVER_H
