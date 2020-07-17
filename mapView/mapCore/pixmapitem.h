#ifndef PIXMAPITEM_H
#define PIXMAPITEM_H

#include "mapbaseitem.h"
class PixmapItem : public MapBaseItem
{
public:
    explicit PixmapItem(const QString& file="",QGraphicsItem *parent = nullptr);
    //void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
    virtual void paintNormal(QPainter *painter) override;
    qreal scale;
protected:
    virtual void wheelEvent(QGraphicsSceneWheelEvent *event) override;
    virtual void mousePressEvent(QGraphicsSceneMouseEvent *event)override;
    virtual void mouseMoveEvent(QGraphicsSceneMouseEvent *event)override;
    virtual void mouseReleaseEvent(QGraphicsSceneMouseEvent *event)override;
    virtual void dragMoveEvent(QGraphicsSceneDragDropEvent *event)override;
    void justCenter();
    QString m_pixmap;
    QRectF m_pixmapRect;
    qreal delta;
    QPointF m_centerPoint;
    QPointF m_oldCenterPoints;
    QPointF m_oldPoints;
    QRectF m_centerRang;
    QPixmap m_pixmapCache;
    qreal pixmapRectWidth=0;
    qreal pixmapRectHeight=0;


};

#endif // PIXMAPITEM_H
