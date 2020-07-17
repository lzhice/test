#include "mapwindow.h"
#include <QHBoxLayout>
#include "pixmanager.h"
#include "graphicsview.h"
#include "imageitem.h"
#include "pathitem.h"
#include <QDebug>
#include "evacuationlampitem.h"
#include "layerscene.h"
#include <QFileDialog>
#include "pixmapitem.h"
static QByteArray loadRawData(const QString &filePath)
{
    QFile file(filePath);
    if (file.open(QIODevice::ReadOnly)) {
        return file.readAll();
    }
    return QByteArray();
}

MapWindow::MapWindow(QWidget *parent)
    : QWidget(parent)
{
    QString workspaces = QCoreApplication::applicationDirPath()+"/map/";
    //qDebug()<<workspaces;
    m_mapScene = new QGraphicsScene(this);
    m_mapScene->setBackgroundBrush(Qt::white);
    MapBaseItemHelp *pMapBaseItemHelp=new MapBaseItemHelp(m_mapScene);
    m_mapView = new GraphicsView(m_mapScene, this);
    //m_mapView->setDragMode(QGraphicsView::ScrollHandDrag);
    m_mapView->setRenderHints(QPainter::Antialiasing | QPainter::SmoothPixmapTransform);
    QHBoxLayout *layout = new QHBoxLayout(this);
    layout->addWidget(m_mapView);

    PixManager::getInstance().setPixmap("map.jpg",QPixmap(workspaces+"map.jpg"));
    PixManager::getInstance().setSvgRender("name",workspaces+"1.svg");
    QString file = workspaces+"1.map.txt";// QFileDialog::getOpenFileName(this, tr("选择文件"),"");

    //ImageMapItem * m_backgroundItem=ImageMapItem::createItem(loadRawData(file));
    PixmapItem * m_backgroundItem=new PixmapItem(QString("map.jpg"));
    //m_backgroundItem->setPix(QString("map.jpg"));
    //MapBaseItem * m_backgroundItem=new MapBaseItem(QString(workspaces+"map.jpg"));

    {

        m_backgroundItem->setZValue(1);
        m_backgroundItem->setSize(QSize(135,135));
        m_mapScene->addItem(m_backgroundItem);
        //m_mapScene->setBackground(m_backgroundItem);
        m_backgroundItem->setPos(0,0);

    }

    {
        PixmapItem * m_backgroundItem=new PixmapItem(QString("map.jpg"));
        m_backgroundItem->setZValue(1);
        m_backgroundItem->setSize(QSize(35,35));
        m_mapScene->addItem(m_backgroundItem);
        //m_mapScene->setBackground(m_backgroundItem);
        m_backgroundItem->setPos(500,0);

    }
    m_mapScene->setSceneRect(m_mapScene->itemsBoundingRect());
    //    int maxCount=0;
    //    for(int j=0;j<1;++j)
    //    for(int i=0;i<1;++i){
    //        EvacuationLampItem* item=new EvacuationLampItem();
    //        if(++maxCount<1000)
    //        item->setState(MapBaseItem::Effect_Active);
    //        item->setPix("name",true);
    //        item->setSize(QSize(35,35));
    //        //item->setAngle(30);
    //        item->setPos(100+i*100,100+j*100);
    //        item->setZValue(3);
    //        m_mapScene->addItem(item);
    //    }

    m_mapView->show();
}

MapWindow::~MapWindow()
{
}

