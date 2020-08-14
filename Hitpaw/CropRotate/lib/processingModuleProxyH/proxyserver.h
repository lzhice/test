#ifndef PROXYSERVER_H
#define PROXYSERVER_H
#include<QTimer>
#include<QHash>
#include <QObject>
#include<QDebug>
class WebSocketServer;
class ProxyServer : public QObject
{
    Q_OBJECT
public:
    explicit ProxyServer(quint16 port,QObject *parent = nullptr);
    int getPid(QString strClientName);
    quint16 getSeverPort();
    void sendDataToClient(QString clientName,QVariantMap vMap);
Q_SIGNALS:
    void clientConnected(QString strClientName);
    void clientClosed(QString strClientName);
    void receivedDate(QString clientName,QVariantMap vMap);
protected slots:
    virtual void dealrequest(QString clientName, QString context);

private:
    WebSocketServer* m_lServer;
};

#endif // PROXYSERVER_H
