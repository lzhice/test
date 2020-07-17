#ifndef Path
#define Path

#include "mapbaseitem.h"
#include<QHash>
#include <QLinearGradient>
//路径连线item
class QGraphicsScene;
typedef QGraphicsScene* QGraphicsScenePtr;
class PathItem : public MapBaseItem
{
public:
    explicit PathItem(QPair<quint32,quint32> pathName, QGraphicsItem *parent = nullptr);
    int type() const{return MapGlobalBase::PathItemType;}
    QPainterPath shape() const;
    void setStartPoint(QPointF point,qreal startLen);
    void setEndPoint(QPointF point,qreal endtLen);
    void setArrowSize(qreal size) ;
    static bool hasPath(QGraphicsScenePtr pScene, QPair<quint32,quint32> pathName);
protected:
    void afterSetStatus(short status, unsigned char value);
    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget);

    void paintGeneral(QPainter *painter, QWidget *widget);
    void paintNone(QPainter *painter);
    void paintPreview(QPainter *painter);
    void paintEdit(QPainter *painter);
    void paintStart(QPainter *painter);
    void pointTrack();
    quint32 getWeight();//计算权重

    void mouseMoveEvent(QGraphicsSceneMouseEvent * event);
    void hoverMoveEvent(QGraphicsSceneHoverEvent * event);

private:
    void initPathBeelineItem();
public:
    const static QString type_s;
private:
protected:
    QPair<quint32,quint32> m_pathName;
    QPointF m_startPoint;
    QPointF m_endPoint;
    qreal m_startLen=0;
    qreal m_endLen=0;

    qreal m_arrowSize=30;
    qreal m_length=0.0;
    QPolygonF m_polygon;
    QLinearGradient m_greenLinearGradient1;
    QLinearGradient m_greenLinearGradient;
    QLinearGradient m_grayLinearGradient;
    QLinearGradient m_previewLinearGradient;
    QLinearGradient m_editLinearGradient;
    static QHash< QPair<quint32,quint32>, quint32 > s_pathTbl;
};

#endif // CPATHBEELINEITEM_H
