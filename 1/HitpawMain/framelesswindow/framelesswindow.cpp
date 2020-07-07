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

#include "framelesswindow.h"
#include <QApplication>
#include <QDesktopWidget>
#include <QGraphicsDropShadowEffect>
#include <QScreen>
#include <QDesktopWidget>
#include "ui_framelesswindow.h"
#include <QDebug>
FramelessWindow::FramelessWindow(QWidget *parent)
    : QWidget(parent),
      ui(new Ui::FramelessWindow),
      m_bMousePressed(false),
      m_pMousedObject(NULL),
      m_bDragTop(false),
      m_bDragLeft(false),
      m_bDragRight(false),
      m_bDragBottom(false),
      m_isCanUpdate(false){
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
    connect(&m_timer,SIGNAL(timeout()),this,SLOT(onTimer()));
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
    //styleWindow(true, false);
}

void FramelessWindow::on_maximizeButton_clicked() {
    //ui->verticalLayout_2->setContentsMargins(0,0,0,0);
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
    ui->windowContent->layout()->addWidget(w);
    //    QWidget *bottomWidget=new QWidget();
    //    bottomWidget->setObjectName("bottomWidget");
    //    bottomWidget->setStyleSheet("#bottomWidget{border:0px solid palette(shadow);border-radius:10px 10px 0px 0px;}");
    //    ui->windowContent->layout()->addWidget(bottomWidget);
    //    bottomWidget->setFixedHeight(10);
    this->setMinimumSize(w->width(),w->height()+ui->windowTitlebar->height());
    move ((QApplication::desktop()->width() - w->width())/2,(QApplication::desktop()->height() - (w->height()+ui->windowTitlebar->height()))/2);
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
                                                  "background-color:palette(shadow); height:20px;}"));
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
                                                  "background-color:palette(shadow); height:20px;}"));
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
                                                  "background-color:palette(dark); height:20px;}"));
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
                                                  "background-color:palette(dark); height:20px;}"));
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

void FramelessWindow::onTimer()
{
    m_isCanUpdate=true;
}

void FramelessWindow::mouseDoubleClickEvent(QMouseEvent *event) {
    Q_UNUSED(event);
}

void FramelessWindow::checkBorderDragging(const QPoint &globalMousePos, QObject *obj) {
    if (isMaximized()) {
        return;
    }
    static int count =0;
    if (m_bMousePressed) {
        QScreen *screen = QGuiApplication::primaryScreen();
        // available geometry excludes taskbar
        QRect availGeometry = screen->availableGeometry();
        int h = availGeometry.height();
        int w = availGeometry.width();
        QList<QScreen *> screenlist = screen->virtualSiblings();
        if (screenlist.contains(screen)) {
            QSize sz = QApplication::desktop()->size();
            h = sz.height();
            w = sz.width();
        }

        // top right corner
        if (m_bDragTop && m_bDragRight) {
            int diff =
                    globalMousePos.x() - (m_StartGeometry.x() + m_StartGeometry.width());
            int neww = m_StartGeometry.width() + diff;
            diff = globalMousePos.y() - m_StartGeometry.y();
            int newy = m_StartGeometry.y() + diff;
            if (neww > 0 && newy > 0 && newy < h - 50) {
                QRect newg = m_StartGeometry;
                newg.setWidth(neww);
                newg.setX(m_StartGeometry.x());
                newg.setY(newy);
                setGeometry(newg);
            }
        }
        // top left corner
        else if (m_bDragTop && m_bDragLeft) {
            int diff = globalMousePos.y() - m_StartGlobalMousePos.y();
            int newy = m_StartGeometry.y() + diff;
            diff = globalMousePos.x() - m_StartGeometry.x();
            int newx = m_StartGeometry.x() + diff;
            if (newy > minimumHeight() && newx > minimumWidth()) {
                QRect newg = m_StartGeometry;
                newg.setY(newy);
                newg.setX(newx);
                setGeometry(newg);
            }
        }
        else if (m_bDragBottom && m_bDragRight) {
            int diff =globalMousePos.y() - (m_StartGeometry.y() + m_StartGeometry.height());
            int newh = m_StartGeometry.height() + diff;
            diff = globalMousePos.x() - (m_StartGeometry.x() + m_StartGeometry.width());
            int neww = m_StartGeometry.width() + diff;
            if (newh > 0 && neww > 0) {
                QRect newg = m_StartGeometry;
                newg.setHeight(newh);
                newg.setWidth(neww);
                setGeometry(newg);
            }
        }
        else if (m_bDragBottom && m_bDragLeft) {
            int diff = globalMousePos.y() - (m_StartGeometry.y() + m_StartGeometry.height());
            int newh = m_StartGeometry.height() + diff;
            diff = globalMousePos.x() - m_StartGeometry.x();
            int newx = m_StartGeometry.x() + diff;
            if (newh > 0 && newx > 0) {
                QRect newg = m_StartGeometry;
                newg.setX(newx);
                newg.setHeight(newh);
                setGeometry(newg);
            }
        } else if (m_bDragTop) {
            int diff = globalMousePos.y() - m_StartGeometry.y();
            int newy = m_StartGeometry.y() + diff;
            if (newy > 0 && newy < h - 50) {
                QRect newg = m_StartGeometry;
                newg.setY(newy);
                setGeometry(newg);
            }
        } else if (m_bDragLeft) {
            int diff = globalMousePos.x() - m_StartGeometry.x();
            int newx = m_StartGeometry.x() + diff;
            if (newx > 0 && newx < w - 50) {
                QRect newg = m_StartGeometry;
                newg.setX(newx);
                setGeometry(newg);
            }
        } else if (m_bDragRight) {
            int diff =
                    globalMousePos.x() - (m_StartGeometry.x() + m_StartGeometry.width());
            int neww = m_StartGeometry.width() + diff;
            if (neww > 0) {
                QRect newg = m_StartGeometry;
                newg.setWidth(neww);
                newg.setX(m_StartGeometry.x());
                setGeometry(newg);
            }
        } else if (m_bDragBottom) {
            int diff =
                    globalMousePos.y() - (m_StartGeometry.y() + m_StartGeometry.height());
            int newh = m_StartGeometry.height() + diff;
            if (newh > 0) {
                QRect newg = m_StartGeometry;
                newg.setHeight(newh);
                newg.setY(m_StartGeometry.y());
                setGeometry(newg);
            }
        }
    } else {
        // no mouse pressed
        if (leftTopBorderHit(globalMousePos)) {
            setCursor(Qt::SizeFDiagCursor);
        }else if (rightBorderHit(globalMousePos) && bottomBorderHit(globalMousePos)) {
            setCursor(Qt::SizeFDiagCursor);
        }else if (rightBorderHit(globalMousePos) && topBorderHit(globalMousePos)) {
            setCursor(Qt::SizeBDiagCursor);
        } else if (leftBorderHit(globalMousePos) &&
                   bottomBorderHit(globalMousePos)) {
            setCursor(Qt::SizeBDiagCursor);
        } else {
            if (topBorderHit(globalMousePos)) {
                setCursor(Qt::SizeVerCursor);
            } else if (leftBorderHit(globalMousePos)) {
                setCursor(Qt::SizeHorCursor);
            } else if (rightBorderHit(globalMousePos)) {
                setCursor(Qt::SizeHorCursor);
            } else if (bottomBorderHit(globalMousePos)) {
                setCursor(Qt::SizeVerCursor);
            } else {
                //qDebug()<<"1:m_bDragLeft==false";
                m_bDragTop = false;
                m_bDragLeft = false;
                m_bDragRight = false;
                m_bDragBottom = false;
                setCursor(Qt::ArrowCursor);
            }
        }
    }
}

// pos in global virtual desktop coordinates
bool FramelessWindow::leftBorderHit(const QPoint &pos) {
    const QRect &rect = this->geometry();
    if (pos.x() >= rect.x() && pos.x() <= rect.x() + CONST_DRAG_BORDER_SIZE) {
        return true;
    }
    return false;
}

bool FramelessWindow::rightBorderHit(const QPoint &pos) {
    const QRect &rect = this->geometry();
    int tmp = rect.x() + rect.width();
    if (pos.x() <= tmp && pos.x() >= (tmp - CONST_DRAG_BORDER_SIZE)) {
        return true;
    }
    return false;
}

bool FramelessWindow::topBorderHit(const QPoint &pos) {
    const QRect &rect = this->geometry();
    if (pos.y() >= rect.y() && pos.y() <= rect.y() + CONST_DRAG_BORDER_SIZE) {
        return true;
    }
    return false;
}

bool FramelessWindow::bottomBorderHit(const QPoint &pos) {
    const QRect &rect = this->geometry();
    int tmp = rect.y() + rect.height();
    if (pos.y() <= tmp && pos.y() >= (tmp - CONST_DRAG_BORDER_SIZE)) {
        return true;
    }
    return false;
}

bool FramelessWindow::leftTopBorderHit(const QPoint &pos)
{
    QRect rect = this->geometry();
    rect.setWidth(CONST_DRAG_BORDER_SIZE*2);
    rect.setHeight(CONST_DRAG_BORDER_SIZE*2);
    if(rect.contains(pos)){
        return true;
    }else{
        return false;
    }
}

void FramelessWindow::mousePressEvent(QMouseEvent *event) {
    if (isMaximized()) {
        return;
    }
    qDebug()<<"mousePressEvent";
    m_bMousePressed = true;
    m_timer.start(30);
    m_isCanUpdate=true;
    m_StartGeometry = this->geometry();


    QPoint globalMousePos = event->globalPos();
    m_StartGlobalMousePos=globalMousePos;
    m_isCanUpdate=true;
    if (leftTopBorderHit(globalMousePos)) {
        m_bDragTop = true;
        m_bDragLeft = true;
        setCursor(Qt::SizeFDiagCursor);
    }else if (rightBorderHit(globalMousePos) && bottomBorderHit(globalMousePos)) {
        m_bDragBottom = true;
        m_bDragRight = true;
        setCursor(Qt::SizeFDiagCursor);
    }else if (rightBorderHit(globalMousePos) && topBorderHit(globalMousePos)) {
        m_bDragRight = true;
        m_bDragTop = true;
        setCursor(Qt::SizeBDiagCursor);
    } else if (leftBorderHit(globalMousePos) && bottomBorderHit(globalMousePos)) {
        m_bDragLeft = true;
        m_bDragBottom = true;
        setCursor(Qt::SizeBDiagCursor);
    } else {
        if (topBorderHit(globalMousePos)) {
            m_bDragTop = true;
            setCursor(Qt::SizeVerCursor);
        } else if (leftBorderHit(globalMousePos)) {
            m_bDragLeft = true;
            setCursor(Qt::SizeHorCursor);
        } else if (rightBorderHit(globalMousePos)) {
            m_bDragRight = true;
            setCursor(Qt::SizeHorCursor);
        } else if (bottomBorderHit(globalMousePos)) {
            m_bDragBottom = true;
            setCursor(Qt::SizeVerCursor);
        }
    }
}

void FramelessWindow::mouseReleaseEvent(QMouseEvent *event) {
    Q_UNUSED(event);
    m_timer.stop();
    m_isCanUpdate=false;
    if (isMaximized()) {
        return;
    }

    m_bMousePressed = false;
    bool bSwitchBackCursorNeeded =
            m_bDragTop || m_bDragLeft || m_bDragRight || m_bDragBottom;
    //qDebug()<<"2:m_bDragLeft==false";
    m_bDragTop = false;
    m_bDragLeft = false;
    m_bDragRight = false;
    m_bDragBottom = false;
    if (bSwitchBackCursorNeeded) {
        setCursor(Qt::ArrowCursor);
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
            if(m_bMousePressed){
                if(m_isCanUpdate){
                    checkBorderDragging(pMouse->globalPos(),obj);
                    m_isCanUpdate=false;
                }
            }else{
                checkBorderDragging(pMouse->globalPos(),obj);
            }
        }
    }
    // press is triggered only on frame window
    else if (event->type() == QEvent::MouseButtonPress) {
        QMouseEvent *pMouse = dynamic_cast<QMouseEvent *>(event);
        if (pMouse) {
            if(!m_bMousePressed){
                mousePressEvent(pMouse);
                m_pMousedObject=obj;
            }
        }
    } else if (event->type() == QEvent::MouseButtonRelease) {
        if (m_bMousePressed) {
            QMouseEvent *pMouse = dynamic_cast<QMouseEvent *>(event);
            if (pMouse) {
                mouseReleaseEvent(pMouse);
            }
        }
    }

    return QWidget::eventFilter(obj, event);
}

