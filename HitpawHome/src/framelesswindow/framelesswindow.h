/*
###############################################################################
#                                                                             #
# The MIT License                                                             #
#                                                                             #
# Copyright (C) 2017 by Juergen Skrotzky (JorgenVikingGod@gmail.com)          #
#               >> https://github.com/Jorgen-VikingGod                        #
#                                                                             #
# Sources: https://github.com/Jorgen-VikingGod/Qt-Frameless-Window-DarkStyle  #
#                                                                             #
###############################################################################
*/

#ifndef FRAMELESSWINDOW_H
#define FRAMELESSWINDOW_H

#include <QWidget>
#include <QTimer>
#include "traywidget.h"
namespace Ui {
class FramelessWindow;
}
#define PADDING 5
#define Content_PADDING 15
class FramelessWindow : public QWidget {
Q_OBJECT
    enum Direction {
        UP=0,
        DOWN=1,
        LEFT,
        RIGHT,
        LEFTTOP,
        LEFTBOTTOM,
        RIGHTBOTTOM,
        RIGHTTOP,
        MOVE,
        NONE
};
public:
    explicit FramelessWindow(QWidget *parent = Q_NULLPTR);
    virtual ~FramelessWindow();
    void setContent( QWidget *w );
    void setReSizeEnable(bool isEnable);
    void setTopBarHeight(int height);
private:
    void styleWindow(bool bActive, bool bNoState);
public slots:
    void setWindowTitle(const QString &text);
    void setWindowIcon(const QIcon &ico);
private slots:
    void on_applicationStateChanged(Qt::ApplicationState state);
    void on_minimizeButton_clicked();
    void on_restoreButton_clicked();
    void on_maximizeButton_clicked();
    void on_closeButton_clicked();
    void on_windowTitlebar_doubleClicked();
protected:
    virtual void changeEvent(QEvent *event);
    virtual void mouseDoubleClickEvent(QMouseEvent *event);
    virtual void checkBorderDragging(QMouseEvent *event, QObject * obj);
    virtual void mousePressEvent(QMouseEvent *event);
    virtual void mouseReleaseEvent(QMouseEvent *event);
    virtual bool eventFilter(QObject *obj, QEvent *event);
protected:
    virtual void closeEvent(QCloseEvent *event);
private:
    Ui::FramelessWindow *ui;
    const quint8 CONST_DRAG_BORDER_SIZE = 15;
    bool m_reSizeEnable=true;
    bool m_bMousePressed;
    QPoint m_StartGlobalMousePos;
    QRect m_StartGeometry;
    QObject *m_pMousedObject;
    TrayWidget * pTrayWidget;
private:
    QPoint m_movePoint;  //鼠标的位置
    Direction dir;        // 窗口大小改变时，记录改变方向
void region(const QPoint &currentGlobalPoint);  //鼠标的位置,改变光标
};

#endif  // FRAMELESSWINDOW_H
