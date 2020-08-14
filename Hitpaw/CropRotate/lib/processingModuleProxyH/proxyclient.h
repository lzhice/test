#ifndef PROXYCLIENT_H
#define PROXYCLIENT_H
#include <QThread>
#include <QObject>
#include <QtWebSockets/QWebSocket>
#include<QTimer>
#include <QAtomicInteger>
enum Inform{SendFault};

class ProxyClient : public QObject
{
    Q_OBJECT
public:
    explicit ProxyClient(const QUrl &url, QString name , QObject *parent = Q_NULLPTR);
    ~ProxyClient(){
    }
    void sendDate(QVariantMap vMap);
    QString name(){return m_name;}
Q_SIGNALS:
    void receivedDate(QVariantMap vMap);
    void closed();
//-------------------------------------------private--------------------------------------------------------------
private:
Q_SIGNALS:
    void sOpenWebSocket(const QUrl &url);
    void sRequest(QString message);
private  Q_SLOTS:
    void onOpenWebSocket(const QUrl &url);
    void onConnected();
    void onTextMessageReceived(QString message);
    void onTextMessageSendRequest(QString message);
private:
    QWebSocket m_webSocket;
    QUrl m_url;
    QString m_name;
    QTimer m_Timer;
    QThread m_Thread;
};

#endif // PROXYCLIENT_H