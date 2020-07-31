#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "traywidget.h"
#include <QCloseEvent>
#include "downServer.h"
#include <QFile>
#include <QMessageBox>
#include <QApplication>
#include <QTimer>
static QString getQssFromFile(QString filename)
{
    QFile f(filename);
    QString qss = "";
    if (f.open(QFile::ReadOnly))
    {
        qss = QString::fromUtf8( f.readAll() );
        f.close();
    }
    qDebug()<<qss;
    return qss;
}
static QVariantMap loadRawData(const QString &filePath)
{
    QVariantMap vMap;
    QFile file(filePath);
    if (file.open(QIODevice::ReadOnly)) {
        QDataStream stream(&file);
        stream>>vMap;
    }
    return vMap;
}

static void writeRawData(const QString &filePath,QVariantMap vMap){
    QFile file(filePath);
    if (file.open(QIODevice::WriteOnly|QIODevice::Truncate)) {
        QDataStream stream(&file);
        stream<<vMap;
    }
}
MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    m_Workspaces = QCoreApplication::applicationDirPath();
    this->setStyleSheet(getQssFromFile(":/style/menu.qss"));
    pTrayWidget=new TrayWidget(this);
    pTrayWidget->hide();

    m_DownServer= new DownServer(this);
    QVariantMap vMap=loadRawData(m_Workspaces+"config.set");
    if(vMap["portTcp"].toUInt()>1000&&vMap["portUdpServer"].toUInt()>1000&&vMap["portUdpClient"].toUInt()>1000){
        m_DownServer->setSerPort(vMap["portTcp"].toUInt(),vMap["portUdpServer"].toUInt(),vMap["portUdpClient"].toUInt());
    }
    if(!m_DownServer->startServer()){
        QMessageBox::warning(this, "网络端口错误", "请检查端口是否被其他程序占用,点击确定后程序自动退出！");
        QTimer::singleShot(100,[](){qApp->quit();});
    }else{
        this->show();
    }
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::closeEvent(QCloseEvent *event)
{
    pTrayWidget->show();
    event->ignore();
    this->hide();
}


void MainWindow::on_pushButton_clicked()
{
    if(ui->lineEdit->text().toUInt()>1000&&ui->lineEdit_2->text().toUInt()>1000&&ui->lineEdit_3->text().toUInt()>1000){
        QVariantMap vMap;
        vMap.insert("portTcp",ui->lineEdit->text().toUInt());
        vMap.insert("portUdpServer",ui->lineEdit_2->text().toUInt());
        vMap.insert("portUdpClient",ui->lineEdit_3->text().toUInt());
        writeRawData(m_Workspaces+"config.set",vMap);
        m_DownServer->setSerPort(ui->lineEdit->text().toUInt(),ui->lineEdit_2->text().toUInt(),ui->lineEdit_3->text().toUInt());
    }
}

void MainWindow::on_pushButton_2_clicked()
{
    this->close();
}
