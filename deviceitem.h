#ifndef DeviceItem_H
#define DeviceItem_H

#include <QImageReader>
#include <QGraphicsItem>
#include <QSize>
#include "mapbaseitem.h"

class DeviceItem : public MapBaseItem
{
public:
    explicit DeviceItem(QGraphicsItem *parent = nullptr);
    int type() const override {return MapGlobalBase::DeviceItemType;}
    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
private:
};
#endif // DeviceItem_H
