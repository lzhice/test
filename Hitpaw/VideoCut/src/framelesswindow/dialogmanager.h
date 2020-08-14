#ifndef DIALOGMANAGER_H
#define DIALOGMANAGER_H

#include <QObject>
#include <QHash>
class DialogManager : public QObject
{
    Q_OBJECT
public:
    static DialogManager * getInstance(){static DialogManager _this;return &_this;}
    void addDialog(QObject *obj){m_dialogTbl.insert(obj,obj);}
    void removeDialog(QObject *obj){m_dialogTbl.remove(obj);}
    bool hasDialog(){return m_dialogTbl.size()>0;}
signals:
private:
    QHash<QObject*,QObject*> m_dialogTbl;
    explicit DialogManager(QObject *parent = nullptr);
};

#endif // DIALOGMANAGER_H
