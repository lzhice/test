#ifndef PROCESSINGMODULEPROXY_H
#define PROCESSINGMODULEPROXY_H
#include <QHash>
#include <QObject>
class QProcess;
class ProcessingModuleProxy : public QObject
{
    Q_OBJECT
public:
    static ProcessingModuleProxy& getInstance(){
        static ProcessingModuleProxy _this;
        return _this;
    }
    void closeProcess(QString path);
    void startProcess(QString path, QStringList args);
signals:

public slots:
private:
    QHash<QString ,QProcess* > m_ProcessTbl;
    explicit ProcessingModuleProxy(QObject *parent = nullptr);
};




#endif // PROCESSINGMODULEPROXY_H
