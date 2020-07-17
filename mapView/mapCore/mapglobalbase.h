#ifndef MAPGLOBALBASE_H
#define MAPGLOBALBASE_H

#include <QObject>
#include <QGraphicsItem>
class MapBaseItem;
class MapGlobalBase : public QObject
{
    Q_OBJECT
public:
    friend class MapBaseItem;
    enum mapItemType{MapBaseItemType = 5555, PathItemType, DeviceItemType,EvacuationLampItemType};
    enum Direction{Direction_None = 0, Direction_Flash = 16, Direction_AllMask = 15, Direction_Up = 8, Direction_Down = 4, Direction_Left = 2, Direction_Right = 1};
    static MapGlobalBase & getInstance(){static MapGlobalBase _this;return _this;}
    MapBaseItem * getMapItem(quint32 id);
private:
    quint32 m_baseMapItemId=1;
    QHash<quint32,MapBaseItem*> m_ItemTbl;
    void addMapItem(MapBaseItem* item);
    void delMapItem(quint32 id);
    explicit MapGlobalBase(QObject *parent = nullptr);
};
#endif // MAPGLOBALBASE_H
