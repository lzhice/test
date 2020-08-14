#include "mainwindow.h"
#include "qmlwidgetcreator.h"
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QLineEdit>
#include <QFileDialog>
#include <QDebug>
#include <QEventLoop>
#include "proxyserver.h"
#include <QDragEnterEvent>
#include <QDropEvent>
#include <QMimeData>
#include <QApplication>
#include "framelesswindow.h"
#include "messagetipbox.h"
MainWindow::MainWindow(QWidget *parent)
    : QWidget(parent),m_pTitleBar(parent)
{

    QApplication::instance()->installEventFilter(this);
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
    //
    {
        m_pVideoCutNullView=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoCutNullView.qml",this);
        vlayout->addWidget (m_pVideoCutNullView);
        connect(QmlEventManager::getInstatnce(m_pVideoCutNullView),&QmlEventManager::emitWidgetEvent,
                [=](const QString& eventName,const QVariant& value){
            if(eventName=="OpenFile"){
                qDebug()<<value.toString();
                if(!openVideoFile(value.toString().replace("file:///",""))){
                    showTipMessageBox(tr("The format is not supported"));
                }
            }else if(eventName=="MultipleFiles"){
                showTipMessageBox(tr("Currently only supports single file import"));
            }else if(eventName=="buttonOpenVideo"){
                QString file = QFileDialog::getOpenFileName(this, tr("selecct video file"),"");
                if(file!=""){
                    if(!openVideoFile(file)){
                        showTipMessageBox(tr("The format is not supported"));
                    }
                }else{

                }
            }
        });
    }
    //
    m_pVideo=m_MediaPlayer.createPreview();
    m_pVideo->setStyleSheet("background-color: rgb(0, 255, 0);");
    m_pVideo->setParent(this);
    {
        QVBoxLayout * vlayoutVideo=new QVBoxLayout();
        vlayoutVideo->setSpacing(0);
        vlayoutVideo->setMargin(0);
        {
            {//VideoSelectRangItem
                QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoSelectRangItem.qml",this);
                //item->setFixedHeight(107);
                m_pVideoSelectRangItem=item;
                vlayoutVideo->addWidget (item);
                {
                    QVariantMap vMap;
                    vMap.insert("event","setMaxValue");
                    vMap.insert("value",0);
                    QmlEventManager::getInstatnce(m_pVideoSelectRangItem)->sendToQml("VideoRangSliderItem",vMap);

                }
                connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,
                        [=](const QString& eventName,const QVariant& value){
                    if(eventName=="OpenFile"){
                        qDebug()<<value.toString();
                        if(!openVideoFile(value.toString().replace("file:///",""))){
                            showTipMessageBox(tr("The format is not supported"));
                        }
                    }else if(eventName=="MultipleFiles"){
                        showTipMessageBox(tr("Currently only supports single file import"));
                    }
                });
            }
        }
        m_pVideo->setLayout(vlayoutVideo);
    }
    vlayout->addWidget(m_pVideo);
    {//video slider
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoPlaySliderItem.qml",this);
        item->setFixedHeight(107);
        m_pVideoPlaySliderItem=item;
        vlayout->addWidget (item);
        {
            QVariantMap vMap;
            vMap.insert("event","setMaxValue");
            vMap.insert("value",0);
            QmlEventManager::getInstatnce(m_pVideoPlaySliderItem)->sendToQml("VideoRangSliderItem",vMap);
        }
        connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,
                [=](const QString& eventName,const QVariant& value){
            if(eventName=="VideoPlaySliderItem_PlayButton"){
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
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoRotateAndSizeSetView.qml",this);

        hlayout->addWidget (item);
        item->setFixedWidth(240);
        m_pVideoRotateAndSizeSetView=item;
        {
            QVariantMap vMap;
            vMap.insert("event","setMaxValue");
            vMap.insert("value",0);
            QmlEventManager::getInstatnce(m_pVideoRotateAndSizeSetView)->sendToQml("VideoCutSetView",vMap);
        }
        connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,
                [=](const QString& eventName,const QVariant& value){
            qDebug()<<eventName;
            if(eventName=="buttonOpenVideo"){
                QString file = QFileDialog::getOpenFileName(this, tr("selecct video file"),"");
                if(file!=""){
                    if(!openVideoFile(file)){
                        showTipMessageBox(tr("The format is not supported"));
                    }
                }else{

                }
            }else if(eventName=="scale_Original"){

            }else if(eventName=="scale_1_1"){

            }else if(eventName=="scale_4_3"){

            }else if(eventName=="scale_9_16"){

            }else if(eventName=="scale_free"){

            }else if(eventName=="scale_3_4"){

            }else if(eventName=="scale_16_9"){

            }else if(eventName=="RotateLeft"){

            }else if(eventName=="RotateRight"){

            }else if(eventName=="Mirror_H"){

            }else if(eventName=="Mirror_V"){

            }
        });
    }
    connect(&m_MediaPlayer,&MediaPlayer::positionChanged,[=](qint64 pos){
        qDebug()<<"MediaPlayer::positionChanged###################################:"<<pos;
        if(m_MediaPlayer.isPlaying()){
            QVariantMap vMap;
            vMap.insert("event","curValue");
            vMap.insert("value",pos);
            QmlEventManager::getInstatnce(m_pVideoPlaySliderItem)->sendToQml("VideoPlaySliderItem",vMap);

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
        QmlEventManager::getInstatnce(m_pVideoPlaySliderItem)->sendToQml("VideoPlaySliderItem",vMap);
    });
    vWidget->setLayout (vlayout);
    this->setLayout(hlayout);
    setMinimumSize(1100,620-24);
    showVideoMain(false);
}

MainWindow::~MainWindow()
{
    m_MediaPlayer.stop();
}

bool MainWindow::openVideoFile(const QString &file)
{

    m_MediaPlayer.stop();
    if(m_MediaPlayer.openMedia(file)){
        m_curFilePath=file;
        {
            QVariantMap vMap;
            vMap.insert("event","setMaxValue");
            vMap.insert("value",m_MediaPlayer.duration());
            QmlEventManager::getInstatnce(m_pVideoPlaySliderItem)->sendToQml("VideoPlaySliderItem",vMap);
        }
        showVideoMain(true);
        return true;
    }else{
        return false;
    }

}

void MainWindow::showVideoMain(bool isShow)
{
    qDebug()<<"showVideoMain1";
    m_pVideo->setVisible(isShow);
    qDebug()<<"showVideoMain2";
    m_pVideoPlaySliderItem->setVisible(isShow);
    qDebug()<<"showVideoMain3";
    m_pVideoCutNullView->setVisible(!isShow);
    qDebug()<<"showVideoMain4";
}
void MainWindow::showTipMessageBox(QString text, QString butteonText)
{
    QTimer::singleShot(1,[=](){
        MessageTipBox messageBox;
        messageBox.setTopHeight(30);
        messageBox.setContextTopMargin(27);
        messageBox.setContextText(text);
        if(butteonText==""){
            messageBox.setButtonRightText(tr("Got It"));
        }else{
            messageBox.setButtonRightText(butteonText);
        }
        messageBox.setFixedSize(520,200);
        connect(&messageBox,&MessageTipBox::buttonEvent,[&messageBox](MessageTipBox::ButtonType type){
            if(type==MessageTipBox::ButtonRight){
                messageBox.close();
            }
        });
        messageBox.exec();
    });

}
void MainWindow::dragEnterEvent(QDragEnterEvent *event)
{
    event->acceptProposedAction();
}


void MainWindow::dropEvent(QDropEvent *event)
{
    int fileCount = event->mimeData()->urls().size();
    if(fileCount==1){
        QString name = event->mimeData()->urls().first().toString();
        qDebug()<<name;
        if(!openVideoFile(name.replace("file:///",""))){
            showTipMessageBox(tr("The format is not supported"));
        }
    }else if(fileCount>1){
        showTipMessageBox(tr("Currently only supports single file import"));
    }
}

bool MainWindow::eventFilter(QObject *watched, QEvent *event)
{
    if(watched==m_pVideo&&event->type() == QEvent::MouseButtonDblClick){
        FramelessWindow * pFramelessWindow= dynamic_cast<FramelessWindow*>(m_pTitleBar);
        if(pFramelessWindow){
            qDebug()<<"MouseButtonDblClick111";
            pFramelessWindow->doDoubleClick();
        }

    }

    return QWidget::eventFilter(watched, event);
}

void MainWindow::updateVideoThumb(int index, QImage image)
{
    qDebug()<<"videoThumb~~~~~~~~~~~~~~~~~~~~~~~########################################"<<index<<image;
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

