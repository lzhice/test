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
#include "ui_framelesswindow.h"
#include "framelesswindow.h"
#include <QApplication>
#include <QDesktopWidget>
#include <QGraphicsDropShadowEffect>
#include <QScreen>
#include <QDesktopWidget>
#include <QMouseEvent>
#include <QDebug>

FramelessWindow::FramelessWindow(QWidget *parent)
    : QWidget(parent),
      ui(new Ui::FramelessWindow),
      m_bMousePressed(false){
    setWindowFlags(Qt::FramelessWindowHint | Qt::WindowSystemMenuHint);
    // append minimize button flag in case of windows,
    // for correct windows native handling of minimize function
#if defined(Q_OS_WIN)
    setWindowFlags(windowFlags() | Qt::WindowMinimizeButtonHint);
#endif
    setAttribute(Qt::WA_NoSystemBackground, true);
    setAttribute(Qt::WA_TranslucentBackground);

    ui->setupUi(this);
    ui->restoreButton->setVisible(false);

    // shadow under window title text
    QGraphicsDropShadowEffect *textShadow = new QGraphicsDropShadowEffect;
    textShadow->setBlurRadius(4.0);
    textShadow->setColor(QColor(0, 0, 0));
    textShadow->setOffset(0.0);
    ui->titleText->setGraphicsEffect(textShadow);

    // window shadow
    QGraphicsDropShadowEffect *windowShadow = new QGraphicsDropShadowEffect;
    windowShadow->setBlurRadius(9.0);
    windowShadow->setColor(Qt::black);
    windowShadow->setOffset(0.0);
    ui->windowFrame->setGraphicsEffect(windowShadow);

    QObject::connect(qApp, &QGuiApplication::applicationStateChanged, this,
                     &FramelessWindow::on_applicationStateChanged);
    setMouseTracking(true);

    // important to watch mouse move from all child widgets
    QApplication::instance()->installEventFilter(this);
}

FramelessWindow::~FramelessWindow() { delete ui; }

void FramelessWindow::on_restoreButton_clicked() {
    //ui->verticalLayout_2->setContentsMargins(5,5,5,5);
    ui->restoreButton->setVisible(false);

    ui->maximizeButton->setVisible(true);
    setWindowState(Qt::WindowNoState);
    // on MacOS this hack makes sure the
    // background window is repaint correctly
    //    hide();
    //    show();
    this->update();
}

void FramelessWindow::on_maximizeButton_clicked() {
    ui->restoreButton->setVisible(true);
    ui->maximizeButton->setVisible(false);
    this->setWindowState(Qt::WindowMaximized);
    this->showMaximized();
    styleWindow(true, false);

}

void FramelessWindow::changeEvent(QEvent *event) {
    if (event->type() == QEvent::WindowStateChange) {
        if (windowState().testFlag(Qt::WindowNoState)) {
            ui->restoreButton->setVisible(false);
            ui->maximizeButton->setVisible(true);
            styleWindow(true, true);
            event->ignore();
        } else if (windowState().testFlag(Qt::WindowMaximized)) {
            ui->restoreButton->setVisible(true);
            ui->maximizeButton->setVisible(false);
            styleWindow(true, false);
            event->ignore();
        }
    }
    event->accept();
}

void FramelessWindow::setContent(QWidget *w) {
    w->setObjectName("contentWidget");
    w->setStyleSheet("#contentWidget{border:1px solid palette(shadow);border-radius:0px 0px 10px 10px;}");
    ui->windowContent->layout()->setMargin(0);
    ui->windowContent->layout()->setSpacing(0);
    ui->windowContent->layout()->addWidget(w);
    //    QWidget *bottomWidget=new QWidget();
    //    bottomWidget->setObjectName("bottomWidget");
    //    bottomWidget->setStyleSheet("#bottomWidget{border:0px solid palette(shadow);border-radius:10px 10px 0px 0px;}");
    //    ui->windowContent->layout()->addWidget(bottomWidget);
    //    bottomWidget->setFixedHeight(10);
    //this->setMinimumSize(w->width(),w->height()+ui->windowTitlebar->height());
    //this->setMinimumSize(w->width(),(w->height()+ui->windowTitlebar->height()));
    //setFixedSize(520,200+ui->windowTitlebar->height());
    move ((QApplication::desktop()->width() - w->width())/2,(QApplication::desktop()->height() - (w->height()+ui->windowTitlebar->height()))/2);
}

void FramelessWindow::setReSizeEnable(bool isEnable)
{
    if(m_reSizeEnable!=isEnable){
        m_reSizeEnable=isEnable;
        if(m_reSizeEnable){
            ui->closeButton->setVisible(m_reSizeEnable);
            ui->maximizeButton->setVisible(m_reSizeEnable);
            ui->minimizeButton->setVisible(m_reSizeEnable);
        }else{
            ui->closeButton->setVisible(m_reSizeEnable);
            ui->maximizeButton->setVisible(m_reSizeEnable);
            ui->minimizeButton->setVisible(m_reSizeEnable);
            ui->restoreButton->setVisible(m_reSizeEnable);
        }
    }

}

void FramelessWindow::setTopBarHeight(int height)
{
    ui->windowTitlebar->setFixedHeight(height);
}

void FramelessWindow::setWindowTitle(const QString &text) {
    ui->titleText->setText(text);
}

void FramelessWindow::setWindowIcon(const QIcon &ico) {
    ui->icon->setPixmap(ico.pixmap(16, 16));
}

void FramelessWindow::styleWindow(bool bActive, bool bNoState) {
    if (bActive) {
        if (bNoState) {
            layout()->setMargin(15);
            ui->windowTitlebar->setStyleSheet(QStringLiteral(
                                                  "#windowTitlebar{border: 0px none palette(shadow); "
                                                  "border-top-left-radius:10px; border-top-right-radius:10px; "
                                                  "background-color:palette(shadow); height:24px;}"));
            ui->windowFrame->setStyleSheet(QStringLiteral(
                                               "#windowFrame{border:1px solid palette(highlight); border-radius:10px "
                                               "10px 10px 10px; background-color:palette(Window);}"));
            QGraphicsEffect *oldShadow = ui->windowFrame->graphicsEffect();
            if (oldShadow) delete oldShadow;
            QGraphicsDropShadowEffect *windowShadow = new QGraphicsDropShadowEffect;
            windowShadow->setBlurRadius(9.0);
            windowShadow->setColor(Qt::black);
            windowShadow->setOffset(0.0);
            ui->windowFrame->setGraphicsEffect(windowShadow);
        } else {
            layout()->setMargin(0);
            ui->windowTitlebar->setStyleSheet(QStringLiteral(
                                                  "#windowTitlebar{border: 0px none palette(shadow); "
                                                  "border-top-left-radius:0px; border-top-right-radius:0px; "
                                                  "background-color:palette(shadow); height:24px;}"));
            ui->windowFrame->setStyleSheet(QStringLiteral(
                                               "#windowFrame{border:1px solid palette(dark); border-radius:0px 0px "
                                               "0px 0px; background-color:palette(Window);}"));
            QGraphicsEffect *oldShadow = ui->windowFrame->graphicsEffect();
            if (oldShadow) delete oldShadow;
            ui->windowFrame->setGraphicsEffect(nullptr);
        }  // if (bNoState) else maximize
    } else {
        if (bNoState) {
            layout()->setMargin(15);
            ui->windowTitlebar->setStyleSheet(QStringLiteral(
                                                  "#windowTitlebar{border: 0px none palette(shadow); "
                                                  "border-top-left-radius:10px; border-top-right-radius:10px; "
                                                  "background-color:palette(dark); height:24px;}"));
            ui->windowFrame->setStyleSheet(QStringLiteral(
                                               "#windowFrame{border:1px solid #000000; border-radius:10px 10px 10px "
                                               "10px; background-color:palette(Window);}"));
            QGraphicsEffect *oldShadow = ui->windowFrame->graphicsEffect();
            if (oldShadow) delete oldShadow;
            QGraphicsDropShadowEffect *windowShadow = new QGraphicsDropShadowEffect;
            windowShadow->setBlurRadius(9.0);
            windowShadow->setColor(Qt::black);
            windowShadow->setOffset(0.0);
            ui->windowFrame->setGraphicsEffect(windowShadow);
        } else {
            layout()->setMargin(0);
            ui->windowTitlebar->setStyleSheet(QStringLiteral(
                                                  "#titlebarWidget{border: 0px none palette(shadow); "
                                                  "border-top-left-radius:0px; border-top-right-radius:0px; "
                                                  "background-color:palette(dark); height:24px;}"));
            ui->windowFrame->setStyleSheet(QStringLiteral(
                                               "#windowFrame{border:1px solid palette(shadow); border-radius:0px "
                                               "0px 0px 0px; background-color:palette(Window);}"));
            QGraphicsEffect *oldShadow = ui->windowFrame->graphicsEffect();
            if (oldShadow) delete oldShadow;
            ui->windowFrame->setGraphicsEffect(nullptr);
        }  // if (bNoState) { else maximize
    }    // if (bActive) { else no focus
}

void FramelessWindow::on_applicationStateChanged(Qt::ApplicationState state) {
    if (windowState().testFlag(Qt::WindowNoState)) {
        if (state == Qt::ApplicationActive) {
            styleWindow(true, true);
        } else {
            styleWindow(false, true);
        }
    } else if (windowState().testFlag(Qt::WindowFullScreen)) {
        if (state == Qt::ApplicationActive) {
            styleWindow(true, false);
        } else {
            styleWindow(false, false);
        }
    }
}

void FramelessWindow::on_minimizeButton_clicked() {
    setWindowState(Qt::WindowMinimized);
}

void FramelessWindow::on_closeButton_clicked() { close(); }

void FramelessWindow::on_windowTitlebar_doubleClicked() {
    if (windowState().testFlag(Qt::WindowNoState)) {
        on_maximizeButton_clicked();
        this->activateWindow();
    } else if (windowState().testFlag(Qt::WindowFullScreen)||windowState().testFlag(Qt::WindowMaximized)) {
        on_restoreButton_clicked();
        this->activateWindow();
    }
}

void FramelessWindow::mouseDoubleClickEvent(QMouseEvent *event) {
    if(m_reSizeEnable&&ui->titleText->geometry().adjusted(0,-1,0,-1).contains(ui->titleText->mapFromGlobal(event->globalPos()))){
        on_windowTitlebar_doubleClicked();
    }
}

void FramelessWindow::checkBorderDragging(QMouseEvent *event, QObject *obj) {
    if (isMaximized()) {
        return;
    }
    {
        QPoint globalPos=event->globalPos();
        QRect rect = this->rect();
        QPoint topLeft = this->mapToGlobal(rect.topLeft());
        QPoint bottomRight = this->mapToGlobal(rect.bottomRight());

        if (this->windowState() != Qt::WindowMaximized)
        {
            if(!m_bMousePressed)  //没有按下左键时
            {
                this->region(globalPos); //窗口大小的改变——判断鼠标位置，改变光标形状
            }
            else
            {
                if(dir != MOVE)
                {
                    QRect newRect(topLeft, bottomRight); //定义一个矩形
                    switch(dir)
                    {
                    case LEFT:

                        if(bottomRight.x() - (globalPos.x()-Content_PADDING) < this->minimumWidth())
                        {
                            newRect.setLeft(bottomRight.x()-this->minimumWidth());  //小于界面的最小宽度时，设置为左上角横坐标为窗口x
                            //只改变左边界
                        }
                        else
                        {
                            newRect.setLeft(globalPos.x()-Content_PADDING);
                        }
                        break;
                    case RIGHT:
                        newRect.setWidth(globalPos.x()+Content_PADDING - topLeft.x());  //只能改变右边界
                        break;
                    case UP:
                        if(bottomRight.y() - (globalPos.y()-Content_PADDING) < this->minimumHeight())
                        {
                            newRect.setY(bottomRight.y()-this->minimumHeight());
                        }
                        else
                        {
                            newRect.setY(globalPos.y()-Content_PADDING);
                        }
                        break;
                    case DOWN:
                        newRect.setHeight(globalPos.y()+Content_PADDING - topLeft.y());
                        break;
                    case LEFTTOP:
                        if(bottomRight.x() - (globalPos.x()-Content_PADDING) < this->minimumWidth())
                        {
                            newRect.setX(bottomRight.x()-this->minimumWidth());
                        }
                        else
                        {
                            newRect.setX((globalPos.x()-Content_PADDING));
                        }

                        if(bottomRight.y() - (globalPos.y()-Content_PADDING) < this->minimumHeight())
                        {
                            newRect.setY(bottomRight.y() - this->minimumHeight());
                        }
                        else
                        {
                            newRect.setY((globalPos.y()-Content_PADDING));
                        }
                        break;
                    case RIGHTTOP:
                        if (globalPos.x()+Content_PADDING - topLeft.x() >= this->minimumWidth())
                        {
                            newRect.setWidth(globalPos.x()+Content_PADDING - topLeft.x());
                        }
                        else
                        {
                            newRect.setWidth(this->minimumWidth());
                        }
                        if (bottomRight.y() - (globalPos.y()-Content_PADDING) >= this->minimumHeight())
                        {
                            newRect.setY((globalPos.y()-Content_PADDING));
                        }
                        else
                        {
                            newRect.setY(bottomRight.y() - this->minimumHeight());
                        }
                        break;
                    case LEFTBOTTOM:
                        if (bottomRight.x() - (globalPos.x()-Content_PADDING) >= this->minimumWidth())
                        {
                            newRect.setX((globalPos.x()-Content_PADDING));
                        }
                        else
                        {
                            newRect.setX(bottomRight.x() - this->minimumWidth());
                        }
                        if (globalPos.y() - topLeft.y() >= this->minimumHeight())
                        {
                            newRect.setHeight(globalPos.y()+Content_PADDING - topLeft.y());
                        }
                        else
                        {
                            newRect.setHeight(this->minimumHeight());
                        }
                        break;
                    case RIGHTBOTTOM:
                        newRect.setWidth(globalPos.x()+Content_PADDING - topLeft.x());
                        newRect.setHeight(globalPos.y()+Content_PADDING - topLeft.y());
                        break;
                    default:
                        break;
                    }
                    setGeometry(newRect);
                }
                else
                {
                    move(globalPos - m_movePoint); //移动窗口
                    event->accept();
                }
            }
        }
    }
}



void FramelessWindow::mousePressEvent(QMouseEvent *event) {
    if (isMaximized()) {
        return;
    }
    if(event->button()==Qt::LeftButton)
    {
        m_bMousePressed = true;
        if(dir != MOVE)
        {
            m_StartGeometry=this->geometry();
            m_movePoint = event->globalPos() - frameGeometry().topLeft();
        }
        else
        {
            m_movePoint = event->globalPos() - frameGeometry().topLeft();
        }
    }
}

void FramelessWindow::mouseReleaseEvent(QMouseEvent *event) {

    m_bMousePressed = false;
    if (dir!=NONE&&dir != MOVE)
    {
        this->releaseMouse(); //释放鼠标抓取
        this->setCursor(QCursor(Qt::ArrowCursor));
    }
}


bool FramelessWindow::eventFilter(QObject *obj, QEvent *event) {
    if (isMaximized()) {
        return QWidget::eventFilter(obj, event);
    }

    // check mouse move event when mouse is moved on any object
    if (event->type() == QEvent::MouseMove) {
        QMouseEvent *pMouse = dynamic_cast<QMouseEvent *>(event);
        if (pMouse) {
            checkBorderDragging(pMouse,obj);
        }
    }
    // press is triggered only on frame window
    else if (event->type() == QEvent::MouseButtonPress) {
        QMouseEvent *pMouse = dynamic_cast<QMouseEvent *>(event);
        if (pMouse) {
            mousePressEvent(pMouse);
        }
    } else if (event->type() == QEvent::MouseButtonRelease) {
        if (m_bMousePressed) {
            QMouseEvent *pMouse = dynamic_cast<QMouseEvent *>(event);
            if (pMouse) {
                mouseReleaseEvent(pMouse);
            }
            m_bMousePressed=false;
            //this->setCursor(QCursor(Qt::ArrowCursor));
        }
    }else if(event->type() == QEvent::MouseButtonDblClick){
        m_bMousePressed=false;
    }

    return QWidget::eventFilter(obj, event);
}

void FramelessWindow::region(const QPoint &currentGlobalPoint)
{
    // 获取窗体在屏幕上的位置区域，topLeft为坐上角点，rightButton为右下角点
    QRect rect = this->ui->windowFrame->rect();

    QPoint topLeft = this->ui->windowFrame->mapToGlobal(rect.topLeft()); //将左上角的(0,0)转化为全局坐标
    QPoint rightButton = this->ui->windowFrame->mapToGlobal(rect.bottomRight());

    int x = currentGlobalPoint.x(); //当前鼠标的坐标
    int y = currentGlobalPoint.y();
    if(m_reSizeEnable){
        if(((topLeft.x() + PADDING >= x) && (topLeft.x() <= x))
                && ((topLeft.y() + PADDING >= y) && (topLeft.y() <= y)))
        {
            // 左上角
            dir = LEFTTOP;
            this->setCursor(QCursor(Qt::SizeFDiagCursor));  // 设置光标形状
        }else if(((x >= rightButton.x() - PADDING) && (x <= rightButton.x()))
                 && ((y >= rightButton.y() - PADDING) && (y <= rightButton.y())))
        {
            // 右下角
            dir = RIGHTBOTTOM;
            this->setCursor(QCursor(Qt::SizeFDiagCursor));
        }else if(((x <= topLeft.x() + PADDING) && (x >= topLeft.x()))
                 && ((y >= rightButton.y() - PADDING) && (y <= rightButton.y())))
        {
            //左下角
            dir = LEFTBOTTOM;
            this->setCursor(QCursor(Qt::SizeBDiagCursor));
        }else if(((x <= rightButton.x()) && (x >= rightButton.x() - PADDING))
                 && ((y >= topLeft.y()) && (y <= topLeft.y() + PADDING)))
        {
            // 右上角
            dir = RIGHTTOP;
            this->setCursor(QCursor(Qt::SizeBDiagCursor));
        }else if((x <= topLeft.x() + PADDING) && (x >= topLeft.x()))
        {
            // 左边
            dir = LEFT;
            this->setCursor(QCursor(Qt::SizeHorCursor));
        }else if((x <= rightButton.x()) && (x >= rightButton.x() - PADDING))
        {
            // 右边
            dir = RIGHT;
            this->setCursor(QCursor(Qt::SizeHorCursor));
        }else if((y >= topLeft.y()) && (y <= topLeft.y() + PADDING))
        {
            // 上边
            dir = UP;
            this->setCursor(QCursor(Qt::SizeVerCursor));
        }else if((y <= rightButton.y()) && (y >= rightButton.y() - PADDING))
        {
            // 下边
            dir = DOWN;
            this->setCursor(QCursor(Qt::SizeVerCursor));
        }else if(ui->titleText->geometry().contains(ui->titleText->mapFromGlobal(currentGlobalPoint)))
        {
            // 默认
            dir = MOVE;
            this->setCursor(QCursor(Qt::ArrowCursor));
        }else{
            if(dir!=NONE){
                qDebug()<<"dir!=NONE"<<dir;
                dir=NONE;
                this->setCursor(QCursor(Qt::ArrowCursor));

            }
        }
    }else{
        if(ui->titleText->geometry().contains(ui->titleText->mapFromGlobal(currentGlobalPoint)))
        {
            // 默认
            dir = MOVE;
            this->setCursor(QCursor(Qt::ArrowCursor));
        }else{
            if(dir!=NONE){
                qDebug()<<"dir!=NONE"<<dir;
                dir=NONE;
                this->setCursor(QCursor(Qt::ArrowCursor));

            }
        }
    }

}

