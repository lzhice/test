#include "qmlwidgetcreator.h"
#include <QQuickWidget>
#include <QQmlContext>
#include <QQuickItem>
#include <QQuickView>
#include <QQmlEngine>
#include <QPalette>
QWidget *QmlWidgetCreator::createQmlWidget( const QString &qmlFilePath, QWidget *parent)
{
    //QHash<QString, QObject *> nullTbl;
    return createQmlWidget(qmlFilePath,QHash<QString, QObject *>(),parent);
}

QWidget *QmlWidgetCreator::createQmlWidget(const QString &qmlFilePath, const QHash<QString, QObject *> &contextPropertyTbl, QWidget *parent)
{
    QQuickWidget * quickWidget=new QQuickWidget();
    quickWidget->setWindowFlag(Qt::FramelessWindowHint);
    quickWidget->setClearColor(QColor(Qt::transparent));
    quickWidget->setAttribute(Qt::WA_AlwaysStackOnTop);
    quickWidget->setResizeMode(QQuickWidget::SizeRootObjectToView);

    quickWidget->rootContext()->setContextProperty("quickWidget", quickWidget);
    quickWidget->rootContext()->setContextProperty("quickRoot", quickWidget->rootObject());

    foreach (QString strProperty, contextPropertyTbl.keys()) {
        if(contextPropertyTbl[strProperty]){
            quickWidget->rootContext()->setContextProperty(strProperty, contextPropertyTbl[strProperty]);
        }
    }
    quickWidget->setSource(QUrl(qmlFilePath));
    quickWidget->setParent(parent);
    return quickWidget;
}
