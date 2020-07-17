#include "pixmapitem.h"
#include <QGraphicsView>
#include <QGraphicsScene>
#include <QPixmap>
#include <QPolygon>
#include <QRectF>
#include <QDebug>
#include <QGraphicsSceneWheelEvent>
#include "pixmanager.h"
PixmapItem::PixmapItem(const QString& file ,QGraphicsItem *parent):MapBaseItem(parent),m_centerPoint(0,0),m_centerRang(0,0,0,0)
{
    m_pixmap=file;
    delta=0;
    scale=1;
}

void PixmapItem::paintNormal(QPainter *painter)
{
    QPixmap* pix=PixManager::getInstance().getPixmap(m_pixmap);
    painter->setRenderHint(QPainter::SmoothPixmapTransform,true);
    qreal wsizeH=pix->width()/(pix->height()*1.0);
    m_pixmapRect=rect();
    qDebug()<<"wsizeH"<<pix->width()<<pix->height()<<delta;
    if(wsizeH<1){
        qreal pixHeight=rect().width()/wsizeH;
        if(pixHeight<rect().height()){
            qreal pixWidth=rect().height()*wsizeH;
            qreal w=(rect().width()-pixWidth)/2;
            m_pixmapRect.adjust(w,0,-w,0);
        }else{
            qreal h=(rect().height()-pixHeight)/2;
            m_pixmapRect.adjust(0,h,0,-h);
        }

    }else{
        qreal pixWidth=rect().height()*wsizeH;
        if(pixWidth<rect().width()){
            qreal pixHeight=rect().width()/wsizeH;
            qreal h=(rect().height()-pixHeight)/2;
            m_pixmapRect.adjust(0,h,0,-h);
        }else{
            qreal w=(rect().width()-pixWidth)/2;
            m_pixmapRect.adjust(w,0,-w,0);
        }

    }
    m_pixmapRect=m_pixmapRect.adjusted(m_pixmapRect.width()*delta,m_pixmapRect.height()*delta,m_pixmapRect.width()*delta*-1,m_pixmapRect.height()*delta*-1);
    m_centerRang=m_pixmapRect.adjusted(rect().width()/2,rect().height()/2,-rect().width()/2,-rect().height()/2);

    justCenter();

    QPixmap bls ;
    if(pixmapRectWidth==m_pixmapRect.width()*scale&&pixmapRectHeight==m_pixmapRect.height()*scale){
        bls=m_pixmapCache;
    }else{
        bls= pix->scaled(m_pixmapRect.width()*scale,m_pixmapRect.height()*scale,Qt::KeepAspectRatio,Qt::SmoothTransformation);
        pixmapRectWidth=m_pixmapRect.width()*scale;
        pixmapRectHeight=m_pixmapRect.height()*scale;
        m_pixmapCache=bls;
    }

    qDebug()<<"rect()"<<rect();
    qDebug()<<"m_pixmapRect"<<m_pixmapRect<<bls.width()<<bls.height();

    QRectF tmpRect;
    tmpRect.setX(m_centerPoint.x()+m_pixmapRect.width()*scale/2-rect().width()*scale/2);
    tmpRect.setY(m_centerPoint.y()+m_pixmapRect.height()*scale/2-rect().height()*scale/2);
    tmpRect.setWidth(rect().width()*scale);
    tmpRect.setHeight(rect().height()*scale);
    qDebug()<<"m_pixmapRect2"<<m_pixmapRect;
    qDebug()<<"tmpRect     2"<<tmpRect;
    if(pix){
        painter->drawPixmap(rect().toRect(),bls.copy(tmpRect.toRect()));
    }
}

void PixmapItem::wheelEvent(QGraphicsSceneWheelEvent *event)
{
    if (event->delta() > 0){
        delta=delta-0.1;
        if(delta<-2)delta=-2;

    }else{
        delta=delta+0.1;
        if(delta>0)delta=0;
    }
    event->accept();
    this->update();
}

void PixmapItem::mousePressEvent(QGraphicsSceneMouseEvent *event)
{
    MapBaseItem::mousePressEvent(event);
    m_oldCenterPoints=m_centerPoint;
    m_oldPoints=event->pos();
    qDebug()<<"mousePressEvent"<<event->pos();
}

void PixmapItem::mouseMoveEvent(QGraphicsSceneMouseEvent *event)
{
    qDebug()<<"m_centerRang"<<m_centerRang;
    if(Qt::LeftButton == event->buttons()){
        QPointF tmpPos=m_centerPoint;
        m_centerPoint=m_oldCenterPoints+(event->pos()-m_oldPoints)*-2;
        justCenter();
        this->update();
        qDebug()<<"mouseMoveEvent"<<event->pos();

    }
    MapBaseItem::mouseMoveEvent(event);
}

void PixmapItem::mouseReleaseEvent(QGraphicsSceneMouseEvent *event)
{
    MapBaseItem::mouseReleaseEvent(event);
}

void PixmapItem::dragMoveEvent(QGraphicsSceneDragDropEvent *event)
{
    MapBaseItem::dragMoveEvent(event);
    qDebug()<<"dragMoveEvent"<<event;
}

void PixmapItem::justCenter()
{
    if(m_centerPoint.x()<m_centerRang.left()){
        m_centerPoint.setX(m_centerRang.left());
    }else if(m_centerPoint.x()>m_centerRang.right()){
        m_centerPoint.setX(m_centerRang.right());
    }
    if(m_centerPoint.y()<m_centerRang.top()){
        m_centerPoint.setY(m_centerRang.top());
    }else if(m_centerPoint.y()>m_centerRang.bottom()){
        m_centerPoint.setY(m_centerRang.bottom());
    }
}
