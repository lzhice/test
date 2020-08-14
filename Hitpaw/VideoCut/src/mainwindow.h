#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QWidget>
#include "fmovablewidget.h"
#include "MediaPlayer.h"
#include "MediaViewer.h"
using namespace Axe;
class QLineEdit;
class QDrag;
class MainWindow : public QWidget
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = NULL);
    ~MainWindow();
    bool openVideoFile(const QString &file);
    void showVideoMain(bool isShow);
    void showTipMessageBox(QString text,QString butteonText="");
protected:
    virtual void dragEnterEvent(QDragEnterEvent *event);
    virtual void dropEvent(QDropEvent *event);
    bool eventFilter(QObject *watched, QEvent *event);
protected slots:
    void updateVideoThumb(int index, QImage image);
private:
    QString   m_curFilePath="";
    QWidget * m_pTitleBar=NULL;
    QWidget * m_pVideoRangSliderItem=NULL;
    QWidget * m_pVideoCutSetView=NULL;
    QWidget * m_pVideoCutNullView=NULL;
    QWidget * m_pVideo=NULL;
    MediaPlayer m_MediaPlayer;
    MediaViewer* m_pMediaViewer=NULL;
    QLineEdit* pLineEdit=NULL;
    qint64 m_nStartTime=0;
    qint64 m_nEndTime=0;
};
#endif // MAINWINDOW_H
