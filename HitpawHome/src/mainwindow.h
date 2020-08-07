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
    void openVideoFile(const QString &file);
    void showVideoMain(bool isShow);
protected:
    virtual void dragEnterEvent(QDragEnterEvent *event);
    //virtual void dragMoveEvent(QDragMoveEvent *event);
    virtual void dropEvent(QDropEvent *event);
    bool eventFilter(QObject *watched, QEvent *event);
private:
    QWidget * m_pTitleBar=NULL;
    QWidget * m_pVideoRangSliderItem=NULL;
    QWidget * m_pVideoCutSetView=NULL;
    QWidget * m_pVideoCutNullView=NULL;
    QWidget * m_pVideo=NULL;
    MediaPlayer m_MediaPlayer;
    QLineEdit* pLineEdit=NULL;
    qint64 m_nStartTime=0;
    qint64 m_nEndTime=0;
    QDrag *drag=NULL;
};
#endif // MAINWINDOW_H
