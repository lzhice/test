#ifndef QMLWIDGETCREATOR_H
#define QMLWIDGETCREATOR_H
#include <QHash>
#include <QObject>
#include <QQuickWidget>
#include <QResizeEvent>
#include <QWidget>
#include <QPushButton>
class TestWidget:public QPushButton{
    Q_OBJECT
public:
    explicit TestWidget(QWidget *parent = nullptr);
protected:
    //virtual void paintEvent(QPaintEvent *e) override;
    QImage m_Image;
};

class QuickWidget:public QQuickWidget{
    Q_OBJECT
public:
    explicit QuickWidget(QWidget *parent = nullptr);
public Q_SLOTS:
    void setEnabled(bool enabled);
protected:
    virtual void resizeEvent(QResizeEvent *rsizeEvent) override;
    virtual void paintEvent(QPaintEvent *e) override;
    QImage m_Image;
    QSize m_oldSize;
};
class QmlEventManager: public QObject
{
    Q_OBJECT
public:
    static QmlEventManager * getInstatnce(QObject * keyObject){
        if(!m_QmlEventManagerTbl.contains(keyObject)){
            m_QmlEventManagerTbl.insert(keyObject,new QmlEventManager(keyObject));
        }
        return m_QmlEventManagerTbl[keyObject];
    }
    ~QmlEventManager(){m_QmlEventManagerTbl.remove(this->parent());}
    Q_INVOKABLE void sendToWidget(const QString& eventName,const QVariant& value);
    Q_INVOKABLE void sendToQml(const QString& eventName,const QVariant& value);
    Q_INVOKABLE void sendToWidgetStart(const QString& eventName);
    Q_INVOKABLE void addValue(const QString &eventName , const QVariant& value);
    Q_INVOKABLE void sendToWidgetEnd(const QString& eventName);
signals:
    void emitWidgetEvent(const QString& eventName,const QVariant& value);
    void emitQmlEvent(const QString& eventName,const QVariant& value);

private:
    QHash<QString,QVariantList> m_sendToQmlValueList;
    Q_INVOKABLE explicit QmlEventManager(QObject *parent=nullptr):QObject(parent){}
    static QHash<QObject *,QmlEventManager*> m_QmlEventManagerTbl;
};

class QmlWidgetCreator
{
public:
    static QWidget * createQmlWidget(const QString& qmlFilePath, QWidget *parent = nullptr);
    static QWidget * createQmlWidget(const QString& qmlFilePath, const QHash<QString, QObject *>& contextPropertyTbl, QWidget *parent = nullptr);
    static QList<QWidget*>  getAllQmlWidget(){return s_QmlWidgetTbl.keys();}
private:
    static QHash<QWidget*,QWidget*> s_QmlWidgetTbl;
};

#endif // QMLWIDGETCREATOR_H
