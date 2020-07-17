#include "pathitem.h"
#include <QPainter>
#include <QDebug>
QHash< QPair<quint32,quint32>, quint32 > PathItem::s_pathTbl;
static QPolygonF getPolygonFromRect(const QRectF rect)
{
    qreal width = rect.width();
    qreal height = rect.height();
    qreal height2 = height / 4;
    QPolygonF polygon;
    if(width > height2 * 3)
    {
        polygon<<QPointF(rect.topLeft().x(), rect.topLeft().y() + height2)
              <<QPointF(rect.topRight().x() - height2 * 3, rect.topRight().y() + height2)
             <<QPointF(rect.topRight().x() - height2 * 3, rect.topRight().y())
            <<QPointF(rect.topRight().x(), rect.topRight().y() + height2 * 2)
           <<QPointF(rect.bottomRight().x() - height2 * 3, rect.bottomRight().y())
          <<QPointF(rect.bottomRight().x() - height2 * 3, rect.bottomRight().y() - height2)
         <<QPointF(rect.bottomLeft().x(), rect.bottomLeft().y() - height2);
    }
    else
    {
        polygon<<rect.topLeft()<<QPointF(rect.topRight().x(), rect.topRight().y() + height2 * 2)<<rect.bottomLeft();
    }
    return polygon;
}
static void adjustRectLeftRight(QRectF &rect, const qreal dLeft,const qreal dRight)
{
    qreal r=(rect.width()+rect.height())/2;
    if((dLeft>0&&dRight>0)&&(dLeft<r&&dRight<r)){
        rect.adjust(dLeft, 0,-dRight, 0);
    }
}

PathItem::PathItem(QPair<quint32,quint32> pathName, QGraphicsItem *parent) :
    MapBaseItem(parent),m_length(0.0)
{
    initPathBeelineItem();

    m_pathName=pathName;
    s_pathTbl.insert(pathName,id());
    //qDebug()<<s_pathTbl;
}

QPainterPath PathItem::shape() const
{
    QPainterPath path;
    path.addPolygon(m_polygon);
    return path;
}

void PathItem::setStartPoint(QPointF point, qreal startLen)
{
    prepareGeometryChange();
    m_startPoint = point;
    m_startLen=startLen;
    if(scene()){
        pointTrack();
        this->update();
    }
}

void PathItem::setEndPoint(QPointF point,qreal endtLen)
{
    prepareGeometryChange();
    m_endPoint = point;
    m_endLen=endtLen;
    if(scene()){
        pointTrack();
        this->update();
    }
}

void PathItem::setArrowSize(qreal size)
{
     m_arrowSize=size;
     if(scene()){
         pointTrack();
         this->update();
     }
}

bool PathItem::hasPath(QGraphicsScenePtr pScene, QPair<quint32,quint32> pathName)
{
    //qDebug()<<"PathItem::hasPath"<<pathName;
    if(pScene&&s_pathTbl.contains(pathName)){
        MapBaseItem * item=MapGlobalBase::getInstance().getMapItem(s_pathTbl[pathName]);
        if(item&&item->scene()==pScene){
            return true;
        }
    }return false;
}

void PathItem::pointTrack()
{
    QPointF startPoint = m_startPoint;
    QPointF endPoint = m_endPoint;
    resetTransform();
    setPos(startPoint);
    QLineF line(startPoint, endPoint);
    m_length = line.length();
    //qDebug()<<"m_length:"<<m_length<<startPoint<<endPoint;
    qreal height = m_arrowSize/1.7;//lzh:控制箭头大小
    QRectF rc(0,height/2, m_length, height);
    adjustRectLeftRight(rc, m_startLen,m_endLen);
    //adjustRectLeft(rc, m_startLen);
    //adjustRectRight(rc, -m_endLen);
    m_polygon = getPolygonFromRect(rc);
    QPointF linearStartPoint(rc.topLeft().x(), rc.topLeft().y() + height / 2);
    QPointF linearStopPoint(rc.topRight().x(), rc.topRight().y() + height / 2);
    m_greenLinearGradient1.setStart(linearStartPoint);
    m_greenLinearGradient1.setFinalStop(linearStopPoint);
    m_greenLinearGradient.setStart(linearStartPoint);
    m_greenLinearGradient.setFinalStop(linearStopPoint);
    m_grayLinearGradient.setStart(linearStartPoint);
    m_grayLinearGradient.setFinalStop(linearStopPoint);
    m_previewLinearGradient.setStart(linearStartPoint);
    m_previewLinearGradient.setFinalStop(linearStopPoint);
    m_editLinearGradient.setStart(linearStartPoint);
    m_editLinearGradient.setFinalStop(linearStopPoint);
    setRect(rc);
    moveBy(0, -height);
    rotate(360 - line.angle(), QPointF(0, height));
    //calcFixedWeight();
}

quint32 PathItem::getWeight()
{
//    QPointF startPoint = m_startPoint;
//    QPointF endPoint = m_endPoint;
//    setPos(startPoint);
//    QLineF line(startPoint, endPoint);
//    m_length = line.length();
//    qDebug()<<"m_length:"<<m_length;
    return m_length;
}

void PathItem::mouseMoveEvent(QGraphicsSceneMouseEvent *event)
{
    QGraphicsItem::mouseMoveEvent(event);
}

void PathItem::hoverMoveEvent(QGraphicsSceneHoverEvent *event)
{
    QGraphicsItem::hoverMoveEvent(event);
}

void PathItem::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
    paintGeneral(painter,widget);
}


void PathItem::paintGeneral(QPainter *painter, QWidget *widget)//lzh改变箭头大小
{
    Q_UNUSED(widget);
//    qreal startPointSize=(m_arrowSize)/2;
//    qreal endPointSize=(m_arrowSize)/2;
//    qreal size=startPointSize<endPointSize?startPointSize:endPointSize;
//    m_size=size;
////    if( m_size -size>0.01||m_size -size<-0.01){
////        m_size=size;
////        pointTrack();
////    }
//    pointTrack();

    //qDebug()<<m_startPoint<<m_endPoint<<m_size;
    paintPreview(painter);
}

void PathItem::paintNone(QPainter *painter)//lzh模拟火警发生隐藏
{
    painter->save();
    painter->setPen(Qt::NoPen);
    painter->drawPolygon(m_polygon);
    painter->restore();
}

void PathItem::paintPreview(QPainter *painter)
{
    painter->save();
    painter->setPen(Qt::NoPen);
    painter->setBrush(m_previewLinearGradient);
    painter->drawPolygon(m_polygon);
    painter->restore();
}

void PathItem::paintEdit(QPainter *painter)
{
    painter->save();
    painter->setPen(Qt::NoPen);
    painter->setBrush(m_editLinearGradient);
    painter->drawPolygon(m_polygon);
    painter->restore();
}

void PathItem::paintStart(QPainter *painter)
{
    painter->save();
    painter->setPen(Qt::NoPen);
    painter->setBrush(m_greenLinearGradient);
    painter->drawPolygon(m_polygon);
    painter->restore();
}

void PathItem::initPathBeelineItem()
{
    m_greenLinearGradient1.setColorAt(0, QColor(110, 166, 52,0));
    m_greenLinearGradient1.setColorAt(1, QColor(255, 199, 19,180));
    m_greenLinearGradient.setColorAt(0, QColor(110, 166, 52,0));
    m_greenLinearGradient.setColorAt(1, Qt::green);
    m_grayLinearGradient.setColorAt(0, QColor(188, 188, 188,0));
    m_grayLinearGradient.setColorAt(1, QColor(73, 199, 248));
    m_previewLinearGradient.setColorAt(0, QColor(255, 135, 23,0));
    m_previewLinearGradient.setColorAt(1, QColor(0, 255, 0));
    m_editLinearGradient.setColorAt(0, QColor(188, 188, 188,0));
    m_editLinearGradient.setColorAt(1, Qt::blue);
    setFlag(QGraphicsItem::ItemIsMovable, false);
    setAcceptHoverEvents(false);
    setFlag(QGraphicsItem::ItemIsSelectable, false);
}
