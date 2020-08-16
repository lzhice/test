#include "logform.h"
#include "ui_logform.h"
#include <QDateTime>
#include "logmanager.h"
static quint64 num=0;
LogForm * LogForm::_this=NULL;
LogForm::LogForm(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::LogForm)
{
    ui->setupUi(this);
    _this=this;
    ui->plainTextEdit->setMaximumBlockCount(50000);
    ui->plainTextEdit_2->setMaximumBlockCount(50000);
    ui->pushButton->hide();
}

LogForm::~LogForm()
{
    delete ui;
}

void LogForm::sendUdpLog(const QString &logText)
{

    QString str=QString("%1 :").arg(QDateTime::currentDateTime().toString("yyyyMMdd hh:mm:ss:zzz")) + QString("[Send] %1:%2 \n").arg(++num).arg(logText);
    ui->plainTextEdit->appendPlainText(str);
    LOG_INFO() << str;
}

void LogForm::revUdpLog(const QString &logText)
{
    QString str=QString("%1 :").arg(QDateTime::currentDateTime().toString("yyyyMMdd hh:mm:ss:zzz")) + QString("[Receive] %1:%2 \n").arg(++num).arg(logText);
    ui->plainTextEdit->appendPlainText(str);
    LOG_INFO() << str;
}

void LogForm::sendTcpLog(const QString &logText)
{
    QString str=QString("%1 :").arg(QDateTime::currentDateTime().toString("yyyyMMdd hh:mm:ss:zzz")) + QString("[Send] %1:%2 \n").arg(++num).arg(logText);
    ui->plainTextEdit_2->appendPlainText(str);
    LOG_INFO() << str;
}

void LogForm::revTcpLog(const QString &logText)
{   QString str=QString("%1 :").arg(QDateTime::currentDateTime().toString("yyyyMMdd hh:mm:ss:zzz")) + QString("[Receive] %1:%2 \n").arg(++num).arg(logText);
    ui->plainTextEdit_2->appendPlainText(str);
    LOG_INFO() << str;
}

void LogForm::closeEvent(QCloseEvent *event)
{
    event->ignore();
    this->hide();
}

void LogForm::on_pushButton_clicked()
{
   emit sigUdpDate(QByteArray(24,'0'));
}
