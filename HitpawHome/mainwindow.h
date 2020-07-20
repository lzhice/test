#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QWidget>
#include "fmovablewidget.h"

class MainWindow : public QWidget
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = NULL);
    ~MainWindow();
public slots:
    void onQmlEvent(const QString& eventName,const QVariant& value);

private:
    QWidget * item1;

};
#endif // MAINWINDOW_H
