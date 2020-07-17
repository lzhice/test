#include "mapglobalbase.h"
#include "mapbaseitem.h"
#include <QDebug>
const quint32 maxItemId=999999999;
void MapGlobalBase::addMapItem(MapBaseItem *item)
{

    if(item){

        if(item->id()==0){

            for(quint32 i=m_baseMapItemId;i<maxItemId;++i){
                if(!m_ItemTbl.contains(i)){
                    item->m_itemId=m_baseMapItemId++;
                    //qDebug()<<"addMapItem:"<<item->id();
                    break;
                }
            }
        }m_ItemTbl.insert(item->id(),item);
    }
}

void MapGlobalBase::delMapItem(quint32 id)
{
    m_ItemTbl.remove(id);
}

MapBaseItem *MapGlobalBase::getMapItem(quint32 id)
{
    if(m_ItemTbl.contains(id)){
        return  m_ItemTbl[id];
    }else{
        return NULL;
    }
}

MapGlobalBase::MapGlobalBase(QObject *parent) : QObject(parent)
{

}
