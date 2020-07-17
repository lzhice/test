#ifndef EvacuationLampItem_H
#define EvacuationLampItem_H
#include <QObject>
#include <QImageReader>
#include <QGraphicsItem>
#include <QSize>
#include <QHash>
#include "mapbaseitem.h"
class EvacuationLampItem : public MapBaseItem
{
public:
    friend class EvacuationLampItemHelp;
    enum pathType{NoPath=0,LeftPath,RightPath};
    explicit EvacuationLampItem(QGraphicsItem *parent = nullptr);
    int type() const override {return MapGlobalBase::EvacuationLampItemType;}

    void setDirection(MapGlobalBase::Direction direction){m_Direction=direction;setPix(m_DirectionPixTbl[direction]);this->update();}
    MapGlobalBase::Direction getDirection(){return m_Direction;}
    void setDirectionPixmap(MapGlobalBase::Direction direction,const QString& pixName);

    pathType getSelectPathType(){return m_selectPathType;}
    void addOutPath(pathType type,quint32 pathId);
    void removeOutPath(quint32 pathId);
    void addInPath(quint32 pathId);
    void removeInPath(quint32 pathId);

protected:
    void mousePressEvent(QGraphicsSceneMouseEvent * event) override;
    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
    virtual void paintSelected(QPainter *painter) override;
    QVariant itemChange(GraphicsItemChange change, const QVariant &value) override;
private:
    pathType m_selectPathType=NoPath;
    MapGlobalBase::Direction m_Direction=MapGlobalBase::Direction_None;
    QHash<MapGlobalBase::Direction,QString> m_DirectionPixTbl;
    QHash< MapGlobalBase::Direction,QList<quint32> > m_DirectionPathsTbl;
    QHash<pathType, QHash<quint32,quint32> > m_OutPathsTbl;
    QHash<quint32,quint32> m_InPathsTbl;
    QPointF getOutPointF(pathType type);
};
class EvacuationLampItemHelp:public QObject
{
    Q_OBJECT
public:
    static EvacuationLampItemHelp & getInstance(){
        static EvacuationLampItemHelp _this;
        return _this;
    }
    bool isHasSelectItem();
    void setCurSelectItem(quint32 curSelectItem, EvacuationLampItem::pathType endItemPathType =EvacuationLampItem::NoPath);
private:
    quint32 m_curSelectItem=0;
    EvacuationLampItemHelp(QObject *parent = nullptr):QObject(parent){}
};

#endif // EvacuationLampItem_H
