#include "logform.h"
#include "ui_logform.h"
LogForm * LogForm::_this=NULL;
LogForm::LogForm(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::LogForm)
{
    ui->setupUi(this);
    _this=this;
    ui->plainTextEdit->setMaximumBlockCount(20000);
    ui->plainTextEdit_2->setMaximumBlockCount(20000);
}

LogForm::~LogForm()
{
    delete ui;
}

void LogForm::sendUdpLog(const QString &logText)
{
    static quint64 num=0;
    ui->plainTextEdit->appendPlainText(QString("[Send] %1:%2 \n").arg(++num).arg(logText));
}

void LogForm::revUdpLog(const QString &logText)
{
    static quint64 num=0;
    ui->plainTextEdit->appendPlainText(QString("[Receive] %1:%2 \n").arg(++num).arg(logText));
}

void LogForm::sendTcpLog(const QString &logText)
{
    static quint64 num=0;
    ui->plainTextEdit_2->appendPlainText(QString("[Send] %1:%2 \n").arg(++num).arg(logText));
}

void LogForm::revTcpLog(const QString &logText)
{
    static quint64 num=0;
    ui->plainTextEdit_2->appendPlainText(QString("[Receive] %1:%2 \n").arg(++num).arg(logText));
}

void LogForm::closeEvent(QCloseEvent *event)
{
    event->ignore();
    this->hide();
}
