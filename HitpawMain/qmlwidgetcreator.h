#ifndef QMLWIDGETCREATOR_H
#define QMLWIDGETCREATOR_H

#include <QObject>
class QQuickWidget;
class QmlWidgetCreator
{
public:
    static QWidget* createQmlWidget(const QString& qmlFilePath, QWidget *parent = nullptr);
    static QWidget *createQmlWidget(const QString& qmlFilePath, const QHash<QString, QObject *>& contextPropertyTbl, QWidget *parent = nullptr);
private:

};

#endif // QMLWIDGETCREATOR_H
