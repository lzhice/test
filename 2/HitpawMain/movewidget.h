#ifndef MOVEWIDGET_H
#define MOVEWIDGET_H

#include <QWidget>
#include <QPainter>
#include <QStyleOption>
#include <QTimer>
#include "marginwidget.h"

class MoveWidget : public MarginWidget
{
    Q_OBJECT
public:
    explicit MoveWidget(QWidget *parent = nullptr);
    void setEnabledMove(bool isEnabled);
    void setEnabledChangeSize(bool isEnabled);
    void setWidget(QWidget * widget, int minimumWidth, int minimumHeight, int nMarginSize=0, const QString& title="", bool iscloseItem=false);
    void saveWindowState(){m_saveWinState=this->windowState();}

signals:
    void sigVisible(bool v);
public slots:
    void slotVisible(bool v=false);
protected:
    enum {pos_none,top = 0x01,bottom = 0x02,left = 0x04,right = 0x08,top_Left = 0x01 | 0x04,topRight = 0x01 | 0x08,bottomLeft = 0x02 | 0x04,bottomRight = 0x02 | 0x08} posBy;
    Qt::WindowStates m_saveWinState;
    bool m_isMinimized;
    bool isArrow;
    bool m_isStart;
    bool m_isEnabledMove;
    bool m_isEnabledChangeSize;
    QTimer m_timer;
    QTimer m_timerDelay;
    QPoint win_pos;
    int  border_px;
    void mousePressEvent(QMouseEvent *event);
    void mouseMoveEvent(QMouseEvent *event);
    void mouseReleaseEvent(QMouseEvent* event);
    void paintEvent(QPaintEvent *e){    QStyleOption opt;    opt.init(this);    QPainter p(this);    style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);    QWidget::paintEvent(e);}
    bool eventFilter(QObject *o, QEvent *e);//事件过滤，主要用来过滤窗口最大最小化
protected slots:
    void arrows_update();
    void onDelayUpdate();
private:
    QWidget *mainWidget=NULL;
    QWidget *hWidget=NULL;
};

#endif // MOVEWIDGET_H
