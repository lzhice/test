#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QWidget>
#include "fmovablewidget.h"
#include "MediaPlayer.h"
using namespace Axe;
class QLineEdit;
class QDrag;
class MainWindow : public QWidget
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = NULL);
    ~MainWindow();
protected:
    virtual void dragEnterEvent(QDragEnterEvent *event);
    //virtual void dragMoveEvent(QDragMoveEvent *event);
    virtual void dropEvent(QDropEvent *event);
private:
    QWidget * m_pVideoRangSliderItem=NULL;
    QWidget * m_pVideoCutSetView=NULL;
    MediaPlayer m_MediaPlayer;
    QLineEdit* pLineEdit=NULL;
    qint64 m_nStartTime=0;
    qint64 m_nEndTime=0;
    QDrag *drag=NULL;
};
#endif // MAINWINDOW_H
