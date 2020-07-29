#include "mainwindow.h"
#include "qmlwidgetcreator.h"
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QDebug>
#include "proxyserver.h"
MainWindow::MainWindow(QWidget *parent)
    : QWidget(parent)
{
    QVBoxLayout * vlayout=new QVBoxLayout();
    vlayout->setSpacing(0);
    vlayout->setMargin(0);
    while (1) {
        ProxyServer *pProxyServer=new ProxyServer(5556,this);
        qDebug()<< "pProxyServer->getSeverPort()"<<pProxyServer->getSeverPort();
        if(pProxyServer->getSeverPort()>5600){
            break;
        }
    }


//    {
//        QHash<QString, QObject *> contextPropertyTbl;
//        contextPropertyTbl.insert("mainWidget",parent);
//        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/topPan.qml",contextPropertyTbl,this);
//        vlayout->addWidget(item);
//        item->setFixedHeight(30);
//        //item->setStyleSheet("background-color: rgb(255, 255, 255);border-radius:10px;");
//    }

    {
        QWidget *hWidget=new QWidget(this);

        QHBoxLayout * hlayout=new QHBoxLayout();
        hlayout->setSpacing(0);
        hlayout->setMargin(2);
        hWidget->setLayout(hlayout);
        {
            QHash<QString, QObject *> contextPropertyTbl;
            contextPropertyTbl.insert("musicCtrl",MusicCtrl::getInstance());
            QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/ControlPan.qml",contextPropertyTbl,this);
            //item->setStyleSheet("background-color: rgb(255, 255, 255);");
            hlayout->addWidget (item);
            //item->setFixedWidth (700);
            item1=item;
            connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,this,&MainWindow::onQmlEvent);
        }
//        {
//            QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoMainView.qml",this);
//            connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,this,&MainWindow::onQmlEvent);
//            hlayout->addWidget (item);

//            //item->setStyleSheet("background-color: rgb(0, 0, 0);");
//        }

        vlayout->addWidget(hWidget);
    }

    this->setLayout (vlayout);

    setMinimumSize(1100-60,620-30);

      //setFixedSize(196,870);
//    setMaximumSize(520,170);
    //this->resize(520,870);
    //setFixedSize(520,350);
    //setFixedWidth(240);

}

MainWindow::~MainWindow()
{

}

void MainWindow::onQmlEvent(const QString &eventName, const QVariant &value)
{
    qDebug()<<"onQmlEvent"<<eventName<<value;
    static int val=90000;
    QVariantMap vMap;
    QVariantList vList;
    vList<<1<<2<<3<<4<<5;
    vMap.insert("1",val+=(10));
    vMap.insert("2",2);
    vMap.insert("3",3);
    vMap.insert("4",vList);
    QmlEventManager::getInstatnce(item1)->sendToQml("toqml",vMap);
}
/*


*/
#include <QTime>
static int getRandomNum(int min, int max)
{
    Q_ASSERT(min < max);
    static bool seedStatus;
    if (!seedStatus)
    {
        qsrand(QTime(0, 0, 0).secsTo(QTime::currentTime()));
        seedStatus = true;
    }
    int nRandom = qrand() % (max - min);
    nRandom = min + nRandom;

    return nRandom;
}
QVariantList MusicCtrl::getPoints(int maxCount)
{
    QVariantList valueList;
    for (int i = 0; i < maxCount; i++) {
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
        valueList<<getRandomNum(1,20);
    }
    qDebug()<<"MusicCtrl::getPoints~~~~~~~~~~~~~~~~~"<<valueList.size();
    return valueList;
}
