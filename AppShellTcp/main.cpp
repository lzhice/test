#include "mainwindow.h"
#include <QDateTime>
#include <QApplication>
#include "DarkStyle.h"
#include <QMessageBox>
#include <QObject>
#include "logmanager.h"
#include <windows.h>
#include "downServer.h"
LONG CreateCrashHandler(EXCEPTION_POINTERS *pException){
    //创建 Dump 文件
    LOG_ERROR() << "-------------------------------------errStart";
    foreach (logOut* plogOut, logOut::getLogList()) {
        if(plogOut){
             LOG_ERROR() << plogOut->m_logText;
        }
    }
    LOG_ERROR() << "-------------------------------------errEnd";

    QDateTime CurDTime = QDateTime::currentDateTime();
    QString current_date = CurDTime.toString("yyyy_MM_dd_hh_mm_ss");
    //dmp文件的命名
    QString dumpText = "Dump_"+current_date+".dmp";
    EXCEPTION_RECORD *record = pException->ExceptionRecord;
    QString errCode(QString::number(record->ExceptionCode, 16));
    QString errAddr(QString::number((uint)record->ExceptionAddress, 16));
    QString errFlag(QString::number(record->ExceptionFlags, 16));
    QString errPara(QString::number(record->NumberParameters, 16));
    HANDLE DumpHandle = CreateFile((LPCWSTR)dumpText.utf16(),
             GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
    //创建消息提示
    QMessageBox::warning(NULL,"Dump",QString("ErrorCode%1  ErrorAddr：%2  ErrorFlag:%3 ErrorPara:%4").arg(errCode).arg(errAddr).arg(errFlag).arg(errPara),
        QMessageBox::Ok);
    return 0;
}
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)CreateCrashHandler);
    LogManager logInstance;
    logInstance.InitLog("./log",QString("AppShellTcp_%1").arg(QDateTime::currentDateTime().toString("yyyyMMddhhmmss")));
//    qDebug()<<"test";
//    qDebug()<<logInstance.getLogFilePath();
//    qWarning() << "Use qWarning";
//    qCritical() << "Use qCritical";
//    LOG_TRACE() << "Use LOG_DEBUG";
//    LOG_DEBUG() << "Use LOG_DEBUG";
//    LOG_INFO() << "Use LOG_INFO";
//    LOG_WARNING() << "Use Warning";
//    LOG_ERROR() << "Use LOG_ERROR";
    a.setStyle(new DarkStyle);
    if(QDateTime::fromString("20200802","yyyyMMdd")<QDateTime::currentDateTime()&&QDateTime::currentDateTime()<QDateTime::fromString("20200825","yyyyMMdd")){

    }else{
        QMessageBox::warning(NULL, QObject::tr("提示"), QObject::tr("测试期限已到期，请联系软件作者！"));
        return 1;
    }

    MainWindow w;
    return a.exec();
}
