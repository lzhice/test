#ifndef AlarmItem_H
#define AlarmItem_H

#include <QImageReader>
#include <QGraphicsItem>
#include <QSize>
#include "mapbaseitem.h"

class AlarmItem : public MapBaseItem
{
public:
    explicit AlarmItem(QGraphicsItem *parent = nullptr);
    int type() const override {return MapGlobalBase::DeviceItemType;}
    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
private:
};
#endif // AlarmItem_H
