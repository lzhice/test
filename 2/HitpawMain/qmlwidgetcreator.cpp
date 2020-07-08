#include "qmlwidgetcreator.h"

#include <QQmlContext>
#include <QQuickItem>
#include <QQuickView>
#include <QQmlEngine>
#include <QPalette>
#include <QPainter>
#include <QColor>
#include <QDebug>
QHash<QObject *,QmlEventManager*> QmlEventManager::m_QmlEventManagerTbl=QHash<QObject *,QmlEventManager*>();
QHash<QWidget*,QWidget*> QmlWidgetCreator::s_QmlWidgetTbl;

QuickWidget::QuickWidget(QWidget *parent):QQuickWidget(parent)
{
}

void QuickWidget::setEnabled(bool enabled)
{
    if(enabled){
        if(!this->isEnabled()){
            QQuickWidget::setEnabled(enabled);
            this->quickWindow()->setVisible(true);
            QResizeEvent tmpEvent(QSize(m_oldSize.width(),m_oldSize.height()),m_oldSize);
            QQuickWidget::resizeEvent (&tmpEvent);
        }
    }else{
        if(this->isEnabled()){
            QQuickWidget::setEnabled(enabled);
            m_Image=grabFramebuffer();
            this->quickWindow()->setVisible(false);
        }

    }

}

void QuickWidget::resizeEvent(QResizeEvent * rsizeEvent)
{
    if(this->isEnabled ()){
        QQuickWidget::resizeEvent (rsizeEvent);
    }else{
        //update ();
        //QWidget::resizeEvent (rsizeEvent);
    }
    m_oldSize=rsizeEvent->size();
}

void QuickWidget::paintEvent(QPaintEvent *e)
{
    if(this->isEnabled ()){
        QQuickWidget::paintEvent(e);
    }else{
        QPainter painter(this);
        painter.drawImage(rect(),m_Image);
        QWidget::paintEvent (e);
    }
}


QWidget *QmlWidgetCreator::createQmlWidget( const QString &qmlFilePath, QWidget *parent)
{
    return createQmlWidget(qmlFilePath,QHash<QString, QObject *>(),parent);
}

QWidget *QmlWidgetCreator::createQmlWidget(const QString &qmlFilePath, const QHash<QString, QObject *> &contextPropertyTbl, QWidget *parent)
{
    QQuickWidget * quickWidget=new QuickWidget();
    quickWidget->setWindowFlag(Qt::FramelessWindowHint);
    quickWidget->setClearColor(QColor(Qt::transparent));
    quickWidget->setAttribute(Qt::WA_AlwaysStackOnTop);
    quickWidget->setResizeMode(QQuickWidget::SizeRootObjectToView);

    quickWidget->rootContext()->setContextProperty("quickWidget", quickWidget);
    quickWidget->rootContext()->setContextProperty("eventManager", QmlEventManager::getInstatnce(quickWidget));
    quickWidget->rootContext()->setContextProperty("quickRoot", quickWidget->rootObject());

    foreach (QString strProperty, contextPropertyTbl.keys()) {
        if(contextPropertyTbl[strProperty]){
            quickWidget->rootContext()->setContextProperty(strProperty, contextPropertyTbl[strProperty]);
        }
    }
    quickWidget->setSource(QUrl(qmlFilePath));
    quickWidget->setParent(parent);
    s_QmlWidgetTbl.insert (quickWidget,quickWidget);
    return quickWidget;
}

void QmlEventManager::sendToWidget(const QString &eventName, const QVariant &value)
{
    emitWidgetEvent(eventName,value);
}

void QmlEventManager::sendToQml(const QString &eventName, const QVariant &value)
{
    emitQmlEvent(eventName,value);
}

TestWidget::TestWidget(QWidget *parent):QPushButton(parent)
{
    m_Image.load("C:/Users/Admin/Documents/untitled5/1.jpg");
    setStyleSheet("background-color: rgb(255, 255, 255);");
}

//void TestWidget::paintEvent(QPaintEvent *e)
//{
//    QPainter painter(this);
//    painter.drawImage(rect(),m_Image);
//    QWidget::paintEvent (e);
//}
