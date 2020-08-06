#include "mainwindow.h"
#include "qmlwidgetcreator.h"
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QLineEdit>
#include <QDebug>
#include <QEventLoop>
#include "proxyserver.h"

MainWindow::MainWindow(QWidget *parent)
    : QWidget(parent)
{
    this->setAcceptDrops(true);
    setObjectName("contentWidget");
    setStyleSheet("#contentWidget{ background-color: #2e2f30; border:10px solid palette(shadow);border-radius:0px 0px 10px 10px;}");
    ProxyServer *pProxyServer=new ProxyServer(55555,this);
    qDebug()<< "pProxyServer->getSeverPort()"<<pProxyServer->getSeverPort();

    QHBoxLayout * hlayout=new QHBoxLayout();
    hlayout->setSpacing(0);
    hlayout->setMargin(0);

    QWidget *vWidget=new QWidget(this);
    QVBoxLayout * vlayout=new QVBoxLayout();
    vlayout->setSpacing(0);
    vlayout->setMargin(0);

    QWidget * pvideo=m_MediaPlayer.createPreview();
    pvideo->setStyleSheet("background-color: rgb(0, 255, 0);");
    pvideo->setParent(this);
    qDebug()<<"m_MediaPlayer.openMedia"<< m_MediaPlayer.openMedia("D:/project/Hitpaw/HitpawHome/FLV2.flv");
    m_MediaPlayer.play();
    vlayout->addWidget(pvideo);


    {//video slider
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/ControlPan.qml",this);
        item->setFixedHeight(30+88);
        m_pVideoRangSliderItem=item;
        {
            QVariantMap vMap;
            vMap.insert("event","setMaxValue");
            vMap.insert("value",m_MediaPlayer.duration());
            QmlEventManager::getInstatnce(m_pVideoRangSliderItem)->sendToQml("VideoRangSliderItem",vMap);

            m_nStartTime=0;
            m_nEndTime=m_MediaPlayer.duration();
        }

        vlayout->addWidget (item);
        connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,
                [=](const QString& eventName,const QVariant& value){
            if(m_pVideoCutSetView&&eventName=="VideoRangSliderItem"&&value.toList().size()>1){

                if(value.toList().at(0).toString()=="curValue"){
                    qint64 curValue=value.toList().at(1).toULongLong();
                    qDebug()<<"seek"<<curValue;
                    qDebug()<<"do m_MediaPlayer.pauses()~~~~~~~~3";
                    m_MediaPlayer.pause();
                    m_MediaPlayer.seek(curValue);

                }else if(value.toList().at(0)=="startValue"){
                    qint64 startValue=value.toList().at(1).toULongLong();
                    QVariantMap vMap;
                    vMap.insert("event","setStartTime");
                    vMap.insert("value",startValue);
                    QmlEventManager::getInstatnce(m_pVideoCutSetView)->sendToQml("VideoCutSetView",vMap);
                    m_nStartTime=startValue;
                    qDebug()<<"m_nStartTime<<m_nEndTime"<<m_nStartTime<<m_nEndTime;
                    m_MediaPlayer.setPlayRange(m_nStartTime,m_nEndTime);
                }else if(value.toList().at(0)=="endValue"){
                    qint64 endValue=value.toList().at(1).toULongLong();
                    QVariantMap vMap;
                    vMap.insert("event","setEndTime");
                    vMap.insert("value",endValue);
                    QmlEventManager::getInstatnce(m_pVideoCutSetView)->sendToQml("VideoCutSetView",vMap);
                    m_nEndTime=endValue;
                    qDebug()<<"m_nStartTime<<m_nEndTime"<<m_nStartTime<<m_nEndTime;
                    m_MediaPlayer.setPlayRange(m_nStartTime,m_nEndTime);
                }else if(value.toList().at(0)=="isPressState"){
                    //m_MediaPlayer.play();
                }
            }else if(eventName=="VideoRangSliderItem_PlayButton"){
                qDebug()<<"m_MediaPlayer.isPlaying()"<<m_MediaPlayer.isPlaying();
                if(m_MediaPlayer.isPlaying()){
                    qDebug()<<"do m_MediaPlayer.pauses()~~~~~~~~";
                    m_MediaPlayer.pause();
                }else{
                    qDebug()<<"do m_MediaPlayer.play()~~~~~~~~";
                    m_MediaPlayer.play();
                }
            }
        });
    }
    hlayout->addWidget(vWidget);
    {// CutSet View
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoCutSetView.qml",this);

        hlayout->addWidget (item);
        item->setFixedWidth(240);
        m_pVideoCutSetView=item;
        {
            QVariantMap vMap;
            vMap.insert("event","setMaxValue");
            vMap.insert("value",m_MediaPlayer.duration());
            QmlEventManager::getInstatnce(m_pVideoCutSetView)->sendToQml("VideoCutSetView",vMap);
        }
        connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,
                [=](const QString& eventName,const QVariant& value){
            if(value.toList().size()==3 && "toEditState"==value.toList().at(0).toString()){
                QRect rect=value.toList().at(1).toRectF().toRect();
                QString textValue =value.toList().at(2).toString();
                if(!pLineEdit){
                    pLineEdit=new QLineEdit(item);
                    pLineEdit->setWindowFlags(Qt::Widget | Qt::FramelessWindowHint | Qt::WindowSystemMenuHint | Qt::WindowStaysOnTopHint);
                    pLineEdit->setStyleSheet("background-color:#000000; selection-background-color:#5E5F62;font-size:12px;");
                }else{
                    pLineEdit->disconnect();
                }
                pLineEdit->setText(textValue);
                pLineEdit->selectAll();
                connect(pLineEdit,&QLineEdit::editingFinished,[this,item,eventName](){
                    if(pLineEdit->isVisible()&&!pLineEdit->text().trimmed().isEmpty()){
                        qDebug()<<"QLineEdit::editingFinished!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
                        QmlEventManager::getInstatnce(item)->sendToQml(eventName,pLineEdit->text());
                    }
                    pLineEdit->setVisible(false);
                });
                pLineEdit->setGeometry(rect);
                pLineEdit->setFocus();
                pLineEdit->setVisible(true);
            }else if(eventName=="VideoCutSetView_timeUpdate"&&value.toList().size()==2){
                int startTime=value.toList()[0].toInt();
                int endTime=value.toList()[1].toInt();
                if(m_pVideoRangSliderItem){

                    {
                        QVariantMap vMap;
                        vMap.insert("event","setStartTime");
                        vMap.insert("value",startTime);
                        QmlEventManager::getInstatnce(m_pVideoRangSliderItem)->sendToQml("VideoRangSliderItem",vMap);
                    }

                    {
                        QVariantMap vMap;
                        vMap.insert("event","setEndTime");
                        vMap.insert("value",endTime);
                        QmlEventManager::getInstatnce(m_pVideoRangSliderItem)->sendToQml("VideoRangSliderItem",vMap);
                    }
                }


            }
        });
    }
    connect(&m_MediaPlayer,&MediaPlayer::positionChanged,[=](qint64 pos){
        if(m_MediaPlayer.isPlaying()){
            QVariantMap vMap;
            vMap.insert("event","curValue");
            vMap.insert("value",pos);
            QmlEventManager::getInstatnce(m_pVideoRangSliderItem)->sendToQml("VideoRangSliderItem",vMap);
            if(pos>=m_nEndTime){

                qDebug()<<"do m_MediaPlayer.pauses()~~~~~~~~2"<<pos<<m_nEndTime;
                m_MediaPlayer.pause();
            }
        }
    });
    connect(&m_MediaPlayer,&MediaPlayer::stateChanged,[=](Axe::MediaPlayerState state){
        QVariantMap vMap;
        vMap.insert("event","playState");
        if(m_MediaPlayer.isPlaying()){
            vMap.insert("value",true);
        }else{
            vMap.insert("value",false);
        }
        qDebug()<<"stateChanged~~~~~~~~~~~~~~~~~~~~~~~"<<vMap;
        QmlEventManager::getInstatnce(m_pVideoRangSliderItem)->sendToQml("VideoRangSliderItem",vMap);
    });
    vWidget->setLayout (vlayout);
    this->setLayout(hlayout);
    setMinimumSize(1100-60,620-30);
}

MainWindow::~MainWindow()
{
    m_MediaPlayer.stop();
}
#include <QDrag>
#include <QDragEnterEvent>
#include <QDropEvent>
#include <QMimeData>
#include <QApplication>
void MainWindow::dragEnterEvent(QDragEnterEvent *event)
{

    if(drag) {
        event->acceptProposedAction();
        QWidget::dragEnterEvent(event);
        event->accept();
        return;
    }else{
        event->ignore();
    }


}


void MainWindow::dropEvent(QDropEvent *event)
{
    QString name = event->mimeData()->urls().first().toString();
    qDebug()<<name;
    event->setDropAction(Qt::MoveAction);
    event->acceptProposedAction();
    drag=NULL;
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

