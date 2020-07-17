#include "mapbaseitem.h"
#include <QGraphicsSceneMouseEvent>
#include <QPolygonF>
#include <QPainter>
#include <QStyleOptionGraphicsItem>
#include <QMatrix>
#include <QGraphicsView>
#include <math.h>
#include <QDebug>
#include "pixmanager.h"
bool s_isEffect=false;
static const int  MinRectSize=1;
static const int  MaxZ=1000;
static const int  NorZ=10;
QPen* m_baseSelectedPen = new QPen(QColor(0, 255, 0, 255), 2, Qt::DotLine, Qt::SquareCap, Qt::MiterJoin);//lzh
QBrush* m_baseSelectedBrush = new QBrush(QColor(55, 55, 55, 44), Qt::SolidPattern);
QBrush* m_baseRedBrush = new QBrush(QColor(255, 0, 0, 160), Qt::SolidPattern);
QBrush* m_baseGreenBrush = new QBrush(QColor(0, 255, 0, 160), Qt::SolidPattern);
QBrush* m_baseYellowBrush = new QBrush(QColor(255, 255, 0, 160), Qt::SolidPattern);
static double distance(const QPointF &point, const QLineF &line)
{
    double d = 0.0;
    QPointF point1(line.p1());
    QPointF point2(line.p2());
    d = (fabs(double((point2.y() - point1.y()) * point.x() +(point1.x() - point2.x()) * point.y() + ((point2.x() * point1.y()) -(point1.x() * point2.y()))))) / (sqrt(pow(double(point2.y() - point1.y()), 2) + pow(double(point1.x() - point2.x()), 2)));
    return d;
}
static double getAngle360(const double angle)
{
    int multiplying=angle/360;
    double tmp=(multiplying==0)?angle:(angle-multiplying*360);
    if(tmp<0){
        tmp=360+tmp;
    }
    return tmp;
}
static QPolygonF rectToPolygon(const QRectF &rect, const QMatrix &matrix)
{
    QPolygonF polygon;
    polygon.append(matrix.map(rect.topLeft()));
    polygon.append(matrix.map(rect.topRight()));
    polygon.append(matrix.map(rect.bottomRight()));
    polygon.append(matrix.map(rect.bottomLeft()));
    return polygon;
}

static void adjustRectUp(QRectF &rect, const qreal d)
{
    if(d == 0)return;
    if(rect.top()+d<rect.center().y()-MinRectSize){
        rect.adjust(0, d, 0, -d);
    }else{
        rect.adjust(0, rect.center().y()-MinRectSize-rect.top(), 0, rect.center().y()+MinRectSize-rect.bottom());
    }
}

static void adjustRectDown(QRectF &rect, const qreal d)
{
    adjustRectUp(rect,-d);
}

static void adjustRectLeft(QRectF &rect, const qreal d)
{
    if(d == 0)return;
    if(rect.left()+d<rect.center().x()-MinRectSize){
        rect.adjust(d, 0, -d, 0);
    }else{
        rect.adjust(rect.center().x()-MinRectSize-rect.left(), 0, rect.center().x()+MinRectSize-rect.right(), 0);
    }
}

static void adjustRectRight(QRectF &rect, const qreal d)
{
    adjustRectLeft(rect,-d);
}

static void adjustRectLeftRight(QRectF &rect, const qreal dLeft,const qreal dRight)
{
    qreal r=(rect.width()+rect.height())/2;
    if((dLeft>0&&dRight>0)&&(dLeft<r&&dRight<r)){
        rect.adjust(dLeft, 0,-dRight, 0);
    }
}

MapBaseItemHelp::MapBaseItemHelp(QGraphicsScene *scene)
{
    connect(&m_timer,SIGNAL(timeout()),this,SLOT(onTimer()));
    m_timer.start(500);
    m_scene=scene;
}

void MapBaseItemHelp::onTimer()
{
    s_isEffect=!s_isEffect;
    if(m_scene){
        m_scene->items();
        foreach(QGraphicsItem * item,m_scene->items()){
            if(item->type()>=5555){
                MapBaseItem *pItem=(MapBaseItem*)item;
                if(pItem->getState()==MapBaseItem::Effect_Active){
                    pItem->update();
                }
            }
        }
    }
}


MapBaseItem::MapBaseItem(QGraphicsItem *parent):QGraphicsItem(parent)
{
    //setFlag(QGraphicsItem::ItemIsMovable, true);
    setAcceptHoverEvents(true);
    setFlag(QGraphicsItem::ItemIsSelectable, true);
    m_color=QColor(233,0,0,188);
    MapGlobalBase::getInstance().addMapItem(this);
}

MapBaseItem::~MapBaseItem()
{
    //MapGlobalBase::getInstance().delMapItem(this->id());
}

void MapBaseItem::setState(MapBaseItem::EffectMode mode)
{
    m_effectMode=mode;
}

MapBaseItem::EffectMode MapBaseItem::getState()
{
    return m_effectMode;
}

void MapBaseItem::setSize(QSize size)
{
    setRect(QRectF(QPointF(-1*size.width()/2,-1*size.height()/2),QSizeF(size.width(), size.height())));
    m_valMap["size"]=size;
}

QSize MapBaseItem::getSize()
{
    return m_valMap.value("size").toSize();
}

void MapBaseItem::setAngle(qreal angle)
{
    prepareGeometryChange();
    rotate(-m_valMap.value("angle").toReal() + angle, m_rect.center());
}

qreal MapBaseItem::getAngle()
{
    return getAngle360(m_valMap.value("angle").toReal());
}

void MapBaseItem::setColor(QColor color)
{
    m_color=color;
}

QColor MapBaseItem::getColor()
{
    return m_color;
}

void MapBaseItem::setPix(const QString &pixName,bool isSvg)
{
    m_pixName=pixName;
    //m_isSvg=isSvg;
}

QRectF MapBaseItem::boundingRect() const
{
    return m_rect.adjusted(-m_margin, -m_margin, +m_margin, +m_margin);
}

QPainterPath MapBaseItem::shape() const
{
    QPainterPath path;
    path.addRect(m_rect.adjusted(-m_shapeMargin, -m_shapeMargin, +m_shapeMargin, +m_shapeMargin));
    return path;
}

void MapBaseItem::setRect(const QRectF &rect)
{
    prepareGeometryChange();
    m_rect =rect;
    if(m_rect.width() > 100000)
        m_rect.setWidth(100000);
    if(m_rect.height() > 100000)
        m_rect.setHeight(100000);
    update();
    itemChange(ItemPositionHasChanged,QVariant());
}

void MapBaseItem::mouseMoveEvent(QGraphicsSceneMouseEvent *event)
{
    if(m_realHoverType != HT_Center)
    {
        if(m_operatorMode == OM_Scale && Qt::LeftButton == event->buttons())
            return mouseMoveOnScale(m_oldPoint, event->pos());
    }

    QGraphicsItem::mouseMoveEvent(event);
}

void MapBaseItem::mousePressEvent(QGraphicsSceneMouseEvent *event)
{
    m_oldPoint = event->pos();
    m_oldRect = m_rect;
    QGraphicsItem::mousePressEvent(event);
}

void MapBaseItem::mouseReleaseEvent(QGraphicsSceneMouseEvent *event)
{
    QGraphicsItem::mouseReleaseEvent(event);
}

void MapBaseItem::hoverEnterEvent(QGraphicsSceneHoverEvent *event)
{
    QGraphicsItem::hoverEnterEvent(event);
}

void MapBaseItem::hoverMoveEvent(QGraphicsSceneHoverEvent *event)
{
    m_realHoverType = getRealHoverType(event->screenPos(), scene()->views()[0]);
    setCursor(getCursor());
    QGraphicsItem::hoverMoveEvent(event);
}

void MapBaseItem::hoverLeaveEvent(QGraphicsSceneHoverEvent *event)
{
    QGraphicsItem::hoverLeaveEvent(event);
}

void MapBaseItem::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
    if(option->state & QStyle::State_Selected)
    {
        paintSelected(painter);
    }
    else if(option->state & QStyle::State_MouseOver)
    {
        paintHover(painter);
    }
    else{
        paintNormal(painter);
    }
}

QVariant MapBaseItem::itemChange(QGraphicsItem::GraphicsItemChange change, const QVariant &value)
{
    if(change == ItemSelectedHasChanged)
    {
        if(value.toBool()){
            setZValue(MaxZ);
            m_operatorMode = OM_Scale;
        }else
        {
            m_operatorMode = OM_None;
            setZValue(NorZ);
        }
        return QVariant();
    }
    return QGraphicsItem::itemChange(change, value);
}

void MapBaseItem::paintSelected(QPainter *painter)
{
    paintNormal(painter);
    painter->save();
    QRectF rc = rect();
    const QTransform &oldTransform = painter->transform();
    const QMatrix &oldMatrix = oldTransform.toAffine();
    QPolygonF polygon = rectToPolygon(rc, oldMatrix);
    painter->resetTransform();
    painter->setPen(*m_baseSelectedPen);
    painter->setBrush(*m_baseSelectedBrush);
    painter->drawPolygon(polygon);
    painter->setTransform(oldTransform);
    painter->restore();
}

void MapBaseItem::paintHover(QPainter *painter)
{
    paintSelected(painter);
    //paintNormal(painter);
}

void MapBaseItem::paintEffect(const QRectF &dest,QPainter *painter,const QPixmap& src, const QColor& color,const QRectF &srcRect)
{
    QImage srcImage;
    QImage destImage;
    srcImage = src.toImage();
    srcImage = srcImage.convertToFormat(srcImage.hasAlphaChannel() ? QImage::Format_ARGB32 : QImage::Format_ARGB32);
    destImage = QImage(srcImage.size(), srcImage.format());
    // do colorizing
    QPainter destPainter(&destImage);
    //destPainter.setOpacity(0.1);
    destPainter.setCompositionMode(QPainter::CompositionMode_DestinationAtop);
    destPainter.fillRect(srcImage.rect(), color);
    destPainter.end();
    if (srcImage.hasAlphaChannel())
        destImage.setAlphaChannel(srcImage.alphaChannel());
    painter->drawImage(dest, destImage);
}

void MapBaseItem::paintNormal(QPainter *painter)
{
    painter->save();
    QRectF r=this->scene()->views()[0]->mapFromScene(this->mapToScene(rect())).boundingRect();
    if((r.width()<r.height()?r.width():r.height())<100){
        m_isSvg=false;
    }else{
        m_isSvg=true;
    }
    QPixmap* pix=PixManager::getInstance().getPixmap(m_pixName);
    if(pix){
        painter->drawPixmap(m_rect.toRect(),*pix);
    }
    if(m_effectMode==Effect_Active){
        if(s_isEffect){
            painter->setPen(m_color);
            painter->setBrush(m_color);
            painter->drawRect(m_rect);
        }
    }
    painter->restore();
}


void MapBaseItem::mouseMoveOnScale(const QPointF &oldPoint, const QPointF &newPoint)
{
    //qDebug()<<"mouseMoveOnScale"<<oldPoint<<newPoint;
    QRectF rect = m_oldRect;
    qreal moveX = newPoint.x() - oldPoint.x();
    qreal moveY = newPoint.y() - oldPoint.y();
    adjustRect(rect, m_realHoverType, moveX, moveY);
    setRect(rect);
}

void MapBaseItem::adjustRect(QRectF &rect, const HoverType type, const qreal dx, const qreal dy)
{
    switch(type)
    {
    case HT_Left:
        adjustRectLeft(rect, dx);
        break;
    case HT_Right:
        adjustRectRight(rect, dx);
        break;
    case HT_Up:
        adjustRectUp(rect, dy);
        break;
    case HT_Down:
        adjustRectDown(rect, dy);
        break;
    case HT_LUp:
        adjustRectLeft(rect, dx);
        adjustRectUp(rect, dy);
        break;
    case HT_RUp:
        adjustRectRight(rect, dx);
        adjustRectUp(rect, dy);
        break;
    case HT_LDown:
        adjustRectLeft(rect, dx);
        adjustRectDown(rect, dy);
        break;
    case HT_RDown:
        adjustRectRight(rect, dx);
        adjustRectDown(rect, dy);
        break;
    default:
        break;
    }
}
void MapBaseItem::rotate(const qreal angle, QPointF point)
{
    qreal dx = point.x();
    qreal dy = point.y();
    this->setTransform(transform().translate(dx, dy));
    this->setTransform(transform().rotate(angle));
    this->setTransform(transform().translate(-dx, -dy));

    m_valMap["angle"]=m_valMap.value("angle").toReal() + angle;
}
MapBaseItem::HoverType MapBaseItem::getRealHoverType(const QPointF &point, QGraphicsView *view) const
{
    int border = 4;
    HoverType type = HT_Center;
    if(!view)
        return type;
    QPointF topLeft = view->mapToGlobal(view->mapFromScene(mapToScene(m_rect.topLeft())));
    QPointF topRight = view->mapToGlobal(view->mapFromScene(mapToScene(m_rect.topRight())));
    QPointF bottomLeft = view->mapToGlobal(view->mapFromScene(mapToScene(m_rect.bottomLeft())));
    QPointF bottomRight = view->mapToGlobal(view->mapFromScene(mapToScene(m_rect.bottomRight())));
    int n = 0;
    if(border >= distance(point, QLineF(topLeft, topRight)))
        n += 1;
    if(border >= distance(point, QLineF(bottomLeft, bottomRight)))
        n += 2;
    if(border >= distance(point, QLineF(topLeft, bottomLeft)))
        n += 4;
    if(border >= distance(point, QLineF(topRight, bottomRight)))
        n += 8;
    switch(n)
    {
    case 1:
        type = HT_Up;
        break;
    case 2:
        type = HT_Down;
        break;
    case 4:
        type = HT_Left;
        break;
    case 8:
        type = HT_Right;
        break;
    case 5:
        type = HT_LUp;
        break;
    case 9:
        type = HT_RUp;
        break;
    case 6:
        type = HT_LDown;
        break;
    case 10:
        type = HT_RDown;
        break;
    }
    return type;
}
QCursor MapBaseItem::getCursor() const
{
    if(m_realHoverType == HT_Left || m_realHoverType == HT_Right)
        return Qt::SizeHorCursor;
    else if(m_realHoverType == HT_Up || m_realHoverType == HT_Down)
        return Qt::SizeVerCursor;
    else if(m_realHoverType == HT_RUp || m_realHoverType == HT_LDown)
        return Qt::SizeBDiagCursor;
    else if(m_realHoverType == HT_LUp || m_realHoverType == HT_RDown)
        return Qt::SizeFDiagCursor;
    else
        return Qt::ArrowCursor;
}
