#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QWidget>
#include "fmovablewidget.h"
#include "MediaPlayer.h"
using namespace Axe;
class QLineEdit;
class MainWindow : public QWidget
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = NULL);
    ~MainWindow();

private:
    QWidget * m_pVideoRangSliderItem=NULL;
    QWidget * m_pVideoCutSetView=NULL;
    MediaPlayer m_MediaPlayer;
    QLineEdit* pLineEdit=NULL;


};
#endif // MAINWINDOW_H
