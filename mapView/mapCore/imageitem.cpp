#include "imageitem.h"
#include <QGraphicsView>
#include "layerscene.h"
#include <QPixmap>
#include <QPolygon>
#include <QBuffer>
#include <QFile>
#include <QDebug>
#include <QtMath>
static const int nBlockCount=1;
QByteArray loadRawData(const QString &str)
{
    QFile file(str);
    if (file.open(QIODevice::ReadOnly)) {
        return file.readAll();
    }
    return QByteArray();
}
ImageItem::ImageItem(const QString &file, QGraphicsItem *parent):QGraphicsItem(parent),m_size(0,0)
{
    if(file!=""){
        setImageBytes(loadRawData(file));
    }
}

ImageItem::ImageItem(const QByteArray &bytes, QGraphicsItem *parent)
{
    setImageBytes(bytes);
}

//void ImageItem::setScene(QGraphicsScene *pScene)
//{
//    if(pScene){
//        pScene->addItem(this);
//    }
//}

void ImageItem::setImageBytes(const QByteArray &bytes)
{
    m_bytes=bytes;
    QBuffer b;
    b.setData(m_bytes);
    b.open(QIODevice::ReadOnly);
    QImageReader imageReader(&b);
    if(imageReader.supportsOption(QImageIOHandler::Size)){
        m_size=imageReader.size();
    }
}

void ImageItem::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
    Q_UNUSED(widget);
    painter->setRenderHint(QPainter::Antialiasing,true);

    QRect r =scene()->views()[0]->mapFromScene(this->mapToScene(boundingRect()/*.adjusted(-1,-1,-1,-1)*/)).boundingRect();
    QSize scaledSize=QSize((r.height()*m_size.width()/m_size.height()),r.height());

    if(m_scaledSize!=scaledSize){
        QBuffer b;
        b.setData(m_bytes);
        b.open(QIODevice::ReadOnly);
        QImageReader imageReader(&b);
        if(scaledSize.width()<m_size.width())
            imageReader.setScaledSize(scaledSize);
        m_cacheImage=imageReader.read();
        m_scaledSize=scaledSize;
    }

    QRect r1 =scene()->views()[0]->mapFromScene(scene()->sceneRect()).boundingRect();
    qreal sw= scene()->width()/r1.width();
    qreal sh= scene()->height()/r1.height();
    painter->drawImage(QRectF(QPointF(0,0),QSizeF(m_size.width()+sw,m_size.height()+sh))/*.adjusted(-1,-1,-1,-1)*/,m_cacheImage);
}

QRectF ImageItem::boundingRect() const
{
    return QRectF(QPointF(0,0),QSizeF(m_size.width()*1.000,m_size.height()*1.000));
}

QByteArray ImageItem::getRawData()
{
    return m_bytes;
}

ImageMapItem::ImageMapItem(const QString &file)
{
    if(file!=""){
        setImageBytes(loadRawData(file));
    }
}

ImageMapItem::ImageMapItem(const QByteArray &bytes)
{
    setImageBytes(bytes);
}

void ImageMapItem::setImageBytes(const QByteArray &bytes)
{
    QBuffer b;
    b.setData(bytes);
    b.open(QIODevice::ReadOnly);
    QImageReader imageReader(&b);
    if(imageReader.supportsOption(QImageIOHandler::Size)){
        m_size=imageReader.size();
    }

    {

        int w=m_size.width()/nBlockCount;
        int h=m_size.height()/nBlockCount;

        QBuffer b;
        b.setData(bytes);
        b.open(QIODevice::ReadOnly);
        QImageReader imageReader(&b);
        imageReader.setScaledSize(QSize(w*nBlockCount,h*nBlockCount));

        QImage image=imageReader.read();
        m_size=image.size();
        QSize itemSize = QSize(w,h);
        for(int j=0;j<nBlockCount;++j){
            for(int i=0;i<nBlockCount;++i)
            {
                QByteArray arr;
                QBuffer buffer(&arr);
                buffer.open(QIODevice::WriteOnly);
                image.copy(QRect(QPoint(i*w,j*h),itemSize)).save(&buffer,"jpg");
                ImageItem * item=m_ImageItemGridTbl.value(QPair<quint32,quint32>(i,j));
                if(!item){
                    item=new ImageItem(arr);
                    m_ImageItemGridTbl.insert(QPair<quint32,quint32>(i,j),item);
                }else{
                    item->setImageBytes(arr);
                }
                item->setZValue(2);
                item->setParentItem(this);
                item->setPos(i*w,j*h);
            }
        }
    }
}

void ImageMapItem::setImageBytes(const QString file)
{
    setImageBytes(loadRawData(file));
}

void ImageMapItem::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{

}

QRectF ImageMapItem::boundingRect() const
{
    return QRectF(QPointF(0,0),m_size);
}

QByteArray ImageMapItem::getRawData()
{
    QByteArray data;
    QDataStream stream(&data, QIODevice::WriteOnly);
    stream<<m_size;
    QHash<QPair<quint32,quint32>,QByteArray> dataBytesTbl;
    QList< QPair<quint32,quint32> > itemKeys=m_ImageItemGridTbl.keys();
    for(int i=0;i<itemKeys.size();++i){
       ImageItem *pImageItem = m_ImageItemGridTbl[ itemKeys[i] ];
       if(pImageItem){
           dataBytesTbl.insert(itemKeys[i],pImageItem->getRawData());
       }
    }
    stream<<dataBytesTbl;
    //qDebug()<<m_size<<data.size();
    return data;
}

ImageMapItem *ImageMapItem::createItem(const QByteArray &bytes)
{
    //qDebug()<<"bytes.size():"<<bytes.size();
    QDataStream stream(const_cast<QByteArray*>(&bytes), QIODevice::ReadOnly);
    QSizeF size;
    int bytsSize=0;
    QHash<QPair<quint32,quint32>,QByteArray> dataBytesTbl;
    stream>>bytsSize>>size>>dataBytesTbl;
    //qDebug()<<bytsSize<<size<<dataBytesTbl.keys();
    return new ImageMapItem(size,dataBytesTbl);
}

ImageMapItem::ImageMapItem(QSizeF size, QHash<QPair<quint32, quint32>, QByteArray> dataBytesTbl):m_size(size)
{
    int w=m_size.width()/nBlockCount;
    int h=m_size.height()/nBlockCount;
    QList< QPair<quint32,quint32> > itemKeys=dataBytesTbl.keys();

    for(int i=0;i<itemKeys.size();++i){
        //qDebug()<<itemKeys[i];
        QByteArray & arr=dataBytesTbl[ itemKeys[i] ];
        {
            ImageItem * item=new ImageItem(arr);
            m_ImageItemGridTbl.insert(itemKeys[i],item);
            item->setZValue(2);
            item->setParentItem(this);
            item->setPos(itemKeys[i].first*w,itemKeys[i].second*h);
        }
    }
}
