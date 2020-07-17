#ifndef MAPBASEITEM_H
#define MAPBASEITEM_H
#include <QGraphicsScene>
#include <QGraphicsItem>
#include <QVariantMap>
#include <QRectF>
#include <QTimer>
#include "mapglobalbase.h"
class QGraphicsView;
class MapBaseItemHelp:public QObject
{
    Q_OBJECT
public:
    MapBaseItemHelp(QGraphicsScene* scene);
protected slots:
    void onTimer();
private:
    QTimer m_timer;
    QGraphicsScene *m_scene;
};
class MapBaseItem:public QGraphicsItem
{
public:
    friend class MapGlobalBase;
    //鼠标浮动动作
    enum HoverType{HT_Up = 0, HT_RUp, HT_Right, HT_RDown, HT_Down, HT_LDown, HT_Left, HT_LUp, HT_Center};
    //动作码
    enum OperatorMode{OM_None, OM_Scale, OM_Rotate};
    //显示类型
    enum ShowMode{SM_Normal, SM_Active};

    enum EffectMode{Effect_None, Effect_Active};

    explicit MapBaseItem(QGraphicsItem *parent = nullptr);
    ~MapBaseItem();
    int id(){return  m_itemId;}
    int type() const override {return MapGlobalBase::MapBaseItemType;}

    void setState(EffectMode mode);
    EffectMode getState();

    void setSize(QSize size);
    QSize getSize();

    void setAngle(qreal angle);
    qreal getAngle();

    void setColor(QColor color);
    QColor getColor();

    void setPix(const QString& pixName, bool isSvg=false);

    QRectF boundingRect() const override;
    QPainterPath shape() const override;
    QRectF rect() const { return m_rect; }
    void setRect(const QRectF& rect);
protected:
    void mouseMoveEvent(QGraphicsSceneMouseEvent * event) override;
    void mousePressEvent(QGraphicsSceneMouseEvent * event) override;
    void mouseReleaseEvent(QGraphicsSceneMouseEvent * event) override;
    void hoverEnterEvent(QGraphicsSceneHoverEvent * event) override;
    void hoverMoveEvent(QGraphicsSceneHoverEvent * event) override;
    void hoverLeaveEvent(QGraphicsSceneHoverEvent * event) override;
    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
    QVariant itemChange(GraphicsItemChange change, const QVariant &value) override;
protected:
    virtual void paintSelected(QPainter *painter);
    virtual void paintHover(QPainter *painter);
    virtual void paintNormal(QPainter *painter);
    void rotate(const qreal angle, QPointF point);
private:
    bool m_isSvg=false;
    quint32 m_itemId=0;//itemId
    EffectMode m_effectMode;
    QVariantMap m_valMap;
    QString m_pixName;
    QColor m_color;
    QRectF m_rect;
    QPointF m_oldPoint;
    QRectF m_oldRect;
    qreal m_margin=6;
    int m_shapeMargin=0;
    HoverType m_realHoverType;
    OperatorMode m_operatorMode;

    void paintEffect(const QRectF &dest,QPainter *painter,const QPixmap& src, const QColor& color,const QRectF &srcRect);
    void mouseMoveOnScale(const QPointF &oldPoint, const QPointF &newPoint);
    void adjustRect(QRectF &rect, const HoverType type, const qreal dx, const qreal dy);

    HoverType getRealHoverType(const QPointF &point, QGraphicsView *view) const;
    QCursor getCursor() const;
};


#endif // MAPBASEITEM_H
