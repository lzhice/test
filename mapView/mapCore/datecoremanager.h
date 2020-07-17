#ifndef DATECOREMANAGER_H
#define DATECOREMANAGER_H
#include <QVariant>
#include <QObject>
enum DateType{ItemDataType = 1, PathDataType};

struct ItemData{
    void setVal(QString name,QVariant val){
        valTbl.insert(name,val);
    }
    QVariant getVal(QString name){
        return valTbl.value(name);
    }
    QHash<QString/*type*/,QVariant> valTbl;
};

struct ItemDataManager{
    void setVal(int id , QString name , QVariant val){
        dataTbl[id].setVal(name,val);
    }
    QVariant getVal(int id , QString name){
        if(dataTbl.contains(id)){
            return dataTbl[id].getVal(name);
        }else{
            return QVariant();
        }
    }
    QHash<int/*type*/,ItemData> dataTbl;
};

struct PathData{
    void setVal(QString name,QVariant val){
        valTbl.insert(name,val);
    }
    QVariant getVal(QString name){
        return valTbl.value(name);
    }
    QHash<QString/*type*/,QVariant> valTbl;
};

struct PathDataManager{
    void setVal(int id , QString name , QVariant val){
        dataTbl[id].setVal(name,val);
    }
    QVariant getVal(int id , QString name){
        if(dataTbl.contains(id)){
            return dataTbl[id].getVal(name);
        }else{
            return QVariant();
        }
    }
    QHash<int/*type*/,PathData> dataTbl;
};

class DateCoreManager : public QObject
{
    Q_OBJECT
public:
    static DateCoreManager & getInstance(){
        static DateCoreManager _this;
        return _this;
    }
    void setVal(DateType type, int id , QString name , QVariant val);
    QVariant getVal(DateType type , int id , QString name);
private:
    ItemDataManager m_ItemDataManager;
    PathDataManager m_PathDataManager;
    DateCoreManager(QObject *parent = nullptr);
};

struct DeviceTemple{
    QString templeName;
    QString pixName;
};

class DeviceTempleManager : public QObject
{
    Q_OBJECT
public:
    static DeviceTempleManager & getInstance(){
        static DeviceTempleManager _this;
        return _this;
    }
    QList<QString> m_TempleTypeList;
    QHash<QString,DeviceTemple> m_TempleTbl;
private:
    DeviceTempleManager(QObject *parent = nullptr);
};
#endif // DATECOREMANAGER_H
