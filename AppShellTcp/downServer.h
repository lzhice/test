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
    explicit DownServer(quint16 port, quint8 devCount, QObject *parent = 0);
    void sendData(quint8 floorNum, quint8 roomNum, QByteArray bytes);
signals:
    void sigReadyRead(quint8 cmd,QByteArray bytes);
    void sigDevDisconnectChange(QHash<quint8,quint8> disconnectDevs);
public slots:
    bool startServer();
private slots:
    void readyRead();
    void disconnected();
    void processPendingDatagram();
protected:
    void incomingConnection(qintptr socketfd);
private:
    QUdpSocket m_udpSender;
    quint16 m_udpSendPort=20000;

    QUdpSocket m_udpReceiver;
    quint16 m_udpReceivePort=20001;

    quint8 m_devCount;
    quint16 m_port;
    QTimer m_timer;
    QTimer m_serchTimer;
    QTimer m_startTcpTimer;
    QHash<QPair<quint8,quint8>,QTcpSocket*> m_TcpSocketTbl;
    QHash<QTcpSocket*,int> m_TcpByteLenTbl;
    QHash<QTcpSocket*,QByteArray> m_TcpByteArrayTbl;

    inline void removeClient(QTcpSocket *client);
    inline void writeData(QTcpSocket *client,const QByteArray &data);
};

#endif // EVENTSERVER_H
