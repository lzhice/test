#include "evacuationlampitem.h"
#include <QGraphicsView>
#include <QGraphicsScene>
#include <QPixmap>
#include <QPolygonF>
#include <QPainter>
#include <QStyleOptionGraphicsItem>
#include <QMatrix>
#include <QBuffer>
#include <QFile>
#include <QDebug>
#include <QtMath>
#include <QGraphicsSceneMouseEvent>
#include "pathitem.h"

static QBrush* baseGreenBrush = new QBrush(QColor(0, 255, 0, 120), Qt::SolidPattern);
static QBrush* baseSelcetBrush = new QBrush(QColor(255, 255, 0, 255), Qt::SolidPattern);
EvacuationLampItem::EvacuationLampItem(QGraphicsItem *parent):MapBaseItem(parent)
{
    setFlag(QGraphicsItem::ItemSendsScenePositionChanges, true);
}

void EvacuationLampItem::addOutPath(EvacuationLampItem::pathType type, quint32 pathId)
{
    m_OutPathsTbl[type].insert(pathId,pathId);
}

void EvacuationLampItem::removeOutPath(quint32 pathId)
{
    foreach (pathType type, m_OutPathsTbl.keys()) {
        m_OutPathsTbl[type].remove(pathId);
    }
}

void EvacuationLampItem::addInPath(quint32 pathId)
{
    m_InPathsTbl.insert(pathId,pathId);
}

void EvacuationLampItem::removeInPath(quint32 pathId)
{
    m_InPathsTbl.remove(pathId);
}

void EvacuationLampItem::mousePressEvent(QGraphicsSceneMouseEvent *event)
{
    MapBaseItem::mousePressEvent(event);

    if(m_selectPathType==NoPath){
        if(EvacuationLampItemHelp::getInstance().isHasSelectItem()){
            bool isOk=false;
            QRectF rc2 = rect();
            qreal mixSize=(rc2.width()<rc2.height()?rc2.width():rc2.height())/4;
            {//right
                QRectF rc3=QRectF(rc2.center().x()+rc2.width()/2-mixSize,rc2.center().y()-mixSize/2,mixSize,mixSize);
                if(rc3.contains(event->pos())){
                    EvacuationLampItemHelp::getInstance().setCurSelectItem(id(),RightPath);
                    isOk=true;
                }
            }
            if(!isOk)
            {//left
                QRectF rc3=QRectF(rc2.center().x()-rc2.width()/2,rc2.center().y()-mixSize/2,mixSize,mixSize);
                if(rc3.contains(event->pos())){
                    EvacuationLampItemHelp::getInstance().setCurSelectItem(id(),LeftPath);
                    isOk=true;
                }
            }
            if(!isOk)EvacuationLampItemHelp::getInstance().setCurSelectItem(id());
            m_selectPathType=NoPath;
            this->update();
        }else{
            bool isOk=false;
            QRectF rc2 = rect();
            qreal mixSize=(rc2.width()<rc2.height()?rc2.width():rc2.height())/4;
            {//right
                QRectF rc3=QRectF(rc2.center().x()+rc2.width()/2-mixSize,rc2.center().y()-mixSize/2,mixSize,mixSize);
                if(rc3.contains(event->pos())){
                    m_selectPathType=RightPath;
                    EvacuationLampItemHelp::getInstance().setCurSelectItem(id());
                    this->update();
                    isOk=true;
                }
            }
            if(!isOk)
            {//left
                QRectF rc3=QRectF(rc2.center().x()-rc2.width()/2,rc2.center().y()-mixSize/2,mixSize,mixSize);
                if(rc3.contains(event->pos())){
                    m_selectPathType=LeftPath;
                    EvacuationLampItemHelp::getInstance().setCurSelectItem(id());
                    this->update();
                }
            }
        }
    }else{
        bool isOk=false;
        QRectF rc2 = rect();
        qreal mixSize=(rc2.width()<rc2.height()?rc2.width():rc2.height())/4;
        {//right
            QRectF rc3=QRectF(rc2.center().x()+rc2.width()/2-mixSize,rc2.center().y()-mixSize/2,mixSize,mixSize);
            if(rc3.contains(event->pos())){
                m_selectPathType=RightPath;
                this->update();
                isOk=true;
            }
        }
        if(!isOk)
        {//left
            QRectF rc3=QRectF(rc2.center().x()-rc2.width()/2,rc2.center().y()-mixSize/2,mixSize,mixSize);
            if(rc3.contains(event->pos())){
                m_selectPathType=LeftPath;
                this->update();
            }
        }
    }
}

void EvacuationLampItem::paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget)
{
    MapBaseItem::paint(painter,option,widget);
    if(!((option->state & QStyle::State_Selected)||(option->state & QStyle::State_MouseOver))){
        m_selectPathType=NoPath;
    }
}

void EvacuationLampItem::paintSelected(QPainter *painter)
{
    MapBaseItem::paintSelected(painter);
    painter->save();
    QRectF rc2 = rect();
    qreal mixSize=(rc2.width()<rc2.height()?rc2.width():rc2.height())/4;
    painter->setPen(QColor(0, 255, 0, 160));
    {//right
        QVector<QPointF> Points;
        Points.append(QPointF(rc2.center().x()+rc2.width()/2-mixSize,rc2.center().y()-mixSize/2));
        Points.append(QPointF(rc2.center().x()+rc2.width()/2-mixSize,rc2.center().y()+mixSize/2));
        Points.append(QPointF(rc2.center().x()+rc2.width()/2,rc2.center().y()));
        QPolygonF polygonF(Points);
        if(m_selectPathType==RightPath){
            painter->setBrush(*baseSelcetBrush);
        }else{
            painter->setBrush(*baseGreenBrush);
        }
        painter->drawPolygon(polygonF);
    }

    {//left
        QVector<QPointF> Points;
        Points.append(QPointF(rc2.center().x()-rc2.width()/2+mixSize,rc2.center().y()-mixSize/2));
        Points.append(QPointF(rc2.center().x()-rc2.width()/2+mixSize,rc2.center().y()+mixSize/2));
        Points.append(QPointF(rc2.center().x()-rc2.width()/2,rc2.center().y()));
        QPolygonF polygonF(Points);

        if(m_selectPathType==LeftPath){
            painter->setBrush(*baseSelcetBrush);
        }else{
            painter->setBrush(*baseGreenBrush);
        }
        painter->drawPolygon(polygonF);
    }
    {
        QRectF rc(rc2.center().x()-rc2.width()/2+mixSize,rc2.center().y()-mixSize/12,rc2.width()-mixSize*2,mixSize/6);
        painter->setBrush(*baseGreenBrush);
        QPolygonF polygonF(rc);
        painter->drawPolygon(polygonF);
    }
    painter->restore();
}

QVariant EvacuationLampItem::itemChange(QGraphicsItem::GraphicsItemChange change, const QVariant &value)
{
    if(ItemPositionHasChanged==change){
        foreach (pathType type, m_OutPathsTbl.keys()) {
            foreach (quint32 id, m_OutPathsTbl[type].keys()) {
                MapBaseItem * pItem= MapGlobalBase::getInstance().getMapItem(id);
                if(pItem&&pItem->type()==MapGlobalBase::PathItemType){
                    PathItem * pPathItem=(PathItem*)pItem;
                    pPathItem->setStartPoint(this->scenePos(),qMax(rect().width(), rect().height()) / 2);
                }
            }
        }
        foreach (quint32 id, m_InPathsTbl.keys()) {
            MapBaseItem * pItem= MapGlobalBase::getInstance().getMapItem(id);
            if(pItem&&pItem->type()==MapGlobalBase::PathItemType){
                PathItem * pPathItem=(PathItem*)pItem;
                pPathItem->setEndPoint(this->scenePos(),qMax(rect().width(), rect().height()) / 2);
            }
        }
    }
    return MapBaseItem::itemChange(change,value);
}

QPointF EvacuationLampItem::getOutPointF(pathType type)
{
    QRectF rc2 = rect();
    if(type==RightPath)
    {//right
        return  this->mapToScene(QPointF(rc2.center().x()+rc2.width()/2,rc2.center().y()));
    }else if(type==LeftPath)
    {//left
        return this->mapToScene(QPointF(rc2.center().x()-rc2.width()/2,rc2.center().y()));
    }else{
        return QPointF();
    }
}


bool EvacuationLampItemHelp::isHasSelectItem()
{
    return m_curSelectItem==0?false:true;
}

void EvacuationLampItemHelp::setCurSelectItem(quint32 curSelectItem, EvacuationLampItem::pathType endItemPathType)
{
    qDebug()<<"setCurSelectItem1"<<curSelectItem;
    if(curSelectItem==0){
        m_curSelectItem=0;
    }else{
        if(m_curSelectItem==0){
            m_curSelectItem=curSelectItem;
        }else{
            qDebug()<<"setCurSelectItem2"<<curSelectItem;
            MapBaseItem * pMapBaseItem1 = MapGlobalBase::getInstance().getMapItem(m_curSelectItem);
            MapBaseItem * pMapBaseItem2 = MapGlobalBase::getInstance().getMapItem(curSelectItem);
            if(pMapBaseItem1&&pMapBaseItem2&&pMapBaseItem1->type()==MapGlobalBase::EvacuationLampItemType&&pMapBaseItem2->type()==MapGlobalBase::EvacuationLampItemType
                    &&pMapBaseItem1->scene()==pMapBaseItem2->scene()){
                EvacuationLampItem *pEvacuationLamp1=(EvacuationLampItem*)pMapBaseItem1;
                EvacuationLampItem *pEvacuationLamp2=(EvacuationLampItem*)pMapBaseItem2;
                if(pEvacuationLamp1->getSelectPathType()!=EvacuationLampItem::NoPath){
                    QPair<quint32,quint32> pathName;
                    pathName.first=pEvacuationLamp1->id();
                    pathName.second=pEvacuationLamp2->id();
                    if(!PathItem::hasPath(pEvacuationLamp1->scene(), pathName)){
                        PathItem * pPathItem=new PathItem(pathName);
                        pPathItem->setZValue(4);
                        pEvacuationLamp1->scene()->addItem(pPathItem);
                        pPathItem->setStartPoint(pEvacuationLamp1->scenePos(),qMax(pEvacuationLamp1->rect().width(), pEvacuationLamp1->rect().height()) / 2);
                        pPathItem->setEndPoint(pEvacuationLamp2->scenePos(),qMax(pEvacuationLamp2->rect().width(), pEvacuationLamp2->rect().height()) / 2);
                        pEvacuationLamp1->addOutPath(pEvacuationLamp1->getSelectPathType(),pPathItem->id());
                        pEvacuationLamp2->addInPath(pPathItem->id());
                        //pEvacuationLamp1->scene()->update();
                        if(endItemPathType!=EvacuationLampItem::NoPath){
                            EvacuationLampItem *pEvacuationLamp1=(EvacuationLampItem*)pMapBaseItem2;
                            EvacuationLampItem *pEvacuationLamp2=(EvacuationLampItem*)pMapBaseItem1;
                            QPair<quint32,quint32> pathName;
                            pathName.first=pEvacuationLamp1->id();
                            pathName.second=pEvacuationLamp2->id();
                            if(!PathItem::hasPath(pEvacuationLamp1->scene(), pathName)){
                                PathItem * pPathItem=new PathItem(pathName);
                                pPathItem->setZValue(4);
                                pEvacuationLamp1->scene()->addItem(pPathItem);
                                pPathItem->setStartPoint(pEvacuationLamp1->scenePos(),qMax(pEvacuationLamp1->rect().width(), pEvacuationLamp1->rect().height()) / 2);
                                pPathItem->setEndPoint(pEvacuationLamp2->scenePos(),qMax(pEvacuationLamp2->rect().width(), pEvacuationLamp2->rect().height()) / 2);
                                pEvacuationLamp1->addOutPath(endItemPathType,pPathItem->id());
                                pEvacuationLamp2->addInPath(pPathItem->id());
                                //pEvacuationLamp1->scene()->update();

                            }
                        }
                    }

                }
            }
            m_curSelectItem=0;
        }
    }
}
