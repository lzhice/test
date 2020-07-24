#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QWidget>
#include "fmovablewidget.h"

class musicCtrl: public QObject{
    Q_OBJECT
};

class MusicCtrl : public QObject
{
    Q_OBJECT
public:
    static MusicCtrl * getInstance(){static MusicCtrl _this;return &_this;}
    Q_INVOKABLE QVariantList getPoints(int maxCount);

private:
    explicit MusicCtrl(QObject *parent = nullptr):QObject(parent){}
    QHash<QString,QColor> m_colorTbl;
    QHash<QString,int> m_fontSizeTbl;
    QHash<QString,QString> m_imgPathTbl;
};
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
