#include "datecoremanager.h"

void DateCoreManager::setVal(DateType type, int id, QString name, QVariant val)
{
    if(type==ItemDataType){
        m_ItemDataManager.setVal(id,name,val);
    }else if(type==PathDataType){
        m_PathDataManager.setVal(id,name,val);
    }
}

QVariant DateCoreManager::getVal(DateType type, int id, QString name)
{
    if(type==ItemDataType){
        return m_ItemDataManager.getVal(id,name);
    }else if(type==PathDataType){
        return m_PathDataManager.getVal(id,name);
    }return QVariant();
}

DateCoreManager::DateCoreManager(QObject *parent) : QObject(parent)
{

}
