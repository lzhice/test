#include "movewidget.h"
#include <QMouseEvent>
#include <QStyleOption>
#include <QPainter>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QDebug>
#include <QLabel>
#include <QObject>
#include <QPushButton>
#include <QGraphicsDropShadowEffect>
#include "qmlwidgetcreator.h"
MoveWidget::MoveWidget(QWidget *parent) : MarginWidget(parent),m_isEnabledMove(true),
    m_isStart(false),m_isEnabledChangeSize(true),isArrow(false),m_isMinimized(false)
{
    //
    setWindowFlags(Qt::FramelessWindowHint);
    setAttribute(Qt::WA_TranslucentBackground);
    //setAttribute(Qt::WA_NoSystemBackground, true);
    //this->setAutoFillBackground(true);
    //setWindowFlags(Qt::Tool|Qt::FramelessWindowHint| Qt::WindowStaysOnTopHint | Qt::X11BypassWindowManagerHint);
    //setStyleSheet("background-color: rgb(255, 255, 255);border-radius:10px;");

    posBy = pos_none;
    setMouseTracking(true);
    connect(&m_timer,SIGNAL(timeout()),this,SLOT(arrows_update()));
    m_timer.start(100);
    connect(&m_timerDelay,SIGNAL(timeout()),this,SLOT(onDelayUpdate()));
    this->installEventFilter(this);

}

void MoveWidget::setEnabledMove(bool isEnabled)
{
    m_isEnabledMove=isEnabled;
}

void MoveWidget::setEnabledChangeSize(bool isEnabled)
{
    m_isEnabledChangeSize=isEnabled;
}

void MoveWidget::setWidget(QWidget *widget, int minimumWidth, int minimumHeight, int nMarginSize,const QString& title,bool iscloseItem)
{
    border_px=nMarginSize;
    QGraphicsDropShadowEffect *shadow = new QGraphicsDropShadowEffect(this);
    shadow->setOffset(0, 0);
    shadow->setColor(Qt::black);
    shadow->setBlurRadius(nMarginSize);
    this->setGraphicsEffect(shadow);

    widget->installEventFilter( this );
    QVBoxLayout *pLayout=new QVBoxLayout();
    pLayout->setSizeConstraint(QLayout::SetNoConstraint);
    pLayout->setMargin(nMarginSize);
    pLayout->setContentsMargins(nMarginSize,nMarginSize,nMarginSize,nMarginSize);
    pLayout->setSpacing(0);
    widget->setContentsMargins(0,0,0,0);
    if(title!="")
    {
        QWidget *titleWidget=new QWidget();
        titleWidget->setFixedHeight(25);
        titleWidget->setStyleSheet("QWidget {background-color:rgb(236,236,236);border:1px solid rgb(200,200,200);border-bottom:0px;}");
        QLabel * titleLabel= new QLabel();
        titleLabel->setText(title);
        titleLabel->setStyleSheet("QWidget {border:0px;}");
        QPushButton * closeButton=new QPushButton();
        closeButton->setAutoDefault(false);
        closeButton->setDefault(false);
        //为了避免默认按钮的快捷键，需要构造一个无用按钮,
        QPushButton * closeButton1=new QPushButton(this);
        closeButton1->setAutoDefault(true);
        closeButton1->setDefault(true);
        closeButton1->setFixedSize(0,0);
        //----------------------------------------------------------
        connect(closeButton, SIGNAL(clicked()),this,SLOT(slotVisible()));
        closeButton->setText("×");
        closeButton->setStyleSheet("QPushButton{border:0px;font: 16pt '微软雅黑'}\
                                   QPushButton::hover:!pressed{font: 17pt '微软雅黑';color:red}");
                                   closeButton->setCursor(Qt::PointingHandCursor);

                closeButton->setFixedSize(20,20);
        QHBoxLayout * pHLayout=new QHBoxLayout();
        titleWidget->setLayout(pHLayout);
        pHLayout->addWidget(titleLabel,0,Qt::AlignLeft);
        pHLayout->addWidget(closeButton,0,Qt::AlignRight);
        pLayout->addWidget(titleWidget);

        pHLayout->setSizeConstraint(QLayout::SetNoConstraint);
        pHLayout->setMargin(0);
        pHLayout->setContentsMargins(0,0,0,0);
        pHLayout->setSpacing(0);
        titleWidget->setContentsMargins(2,2,2,2);

        if(!iscloseItem)closeButton->hide();
    }

    mainWidget=widget;
    pLayout->addWidget(widget);

    hWidget=new QWidget(this);
    pLayout->addWidget(hWidget);
    hWidget->hide();
    this->setLayout(pLayout);
    this->setMinimumSize(minimumWidth,minimumHeight);
    this->resize(minimumWidth,minimumHeight);


}

void MoveWidget::slotVisible(bool v)
{
    this->setVisible(v);
    emit sigVisible(v);

}


void MoveWidget::mousePressEvent(QMouseEvent *event)
{
    m_timer.stop();
    m_timerDelay.stop();
    event->ignore();
    if (event->button() == Qt::LeftButton)
    {
        foreach (QWidget* pWidget, QmlWidgetCreator::getAllQmlWidget()) {
            ((QuickWidget*)pWidget)->setEnabled (false);
        }
        win_pos = event->globalPos() - frameGeometry().topLeft();
        m_isStart=true;
        //mainWidget->hide();
        //hWidget->show();
    }
}

void MoveWidget::mouseMoveEvent(QMouseEvent *event)
{
    if (event->buttons() & Qt::LeftButton){

        if(posBy == pos_none&&m_isEnabledMove&&m_isStart){

            move(event->globalPos() - win_pos);
        }
        else if(m_isEnabledChangeSize){
            int m_top,m_bottom,m_left,pright;
            m_top = frameGeometry().top();
            m_bottom =frameGeometry().bottom();
            m_left = frameGeometry().left();
            pright = frameGeometry().right();
            if(posBy & top&&m_isEnabledMove){
                if(height() == minimumHeight()){
                    m_top = qMin(event->y()+this->pos().y(),m_top);
                }
                else if(height() == maximumHeight()){
                    m_top = qMax(event->y()+this->pos().y(),m_top);
                }
                else{
                    m_top = event->y()+this->pos().y();
                }
            }
            else if(posBy & bottom){
                if(height() == minimumHeight()){
                    m_bottom = qMax(event->y()+this->pos().y(),m_top);
                }
                else if(height() == maximumHeight()){
                    m_bottom = qMin(event->y()+this->pos().y(),m_top);
                }
                else{
                    m_bottom = event->y()+this->pos().y();
                }
            }

            if(posBy & left){
                if(width() == minimumWidth()){
                    m_left = qMin(event->x()+this->pos().x(),m_left);
                }
                else if(width() == maximumWidth()){
                    m_left = qMax(event->x()+this->pos().x(),m_left);
                }
                else{
                    m_left = event->x()+this->pos().x();
                }
            }
            else if(posBy & right){
                if(width() == minimumWidth()){
                    pright = qMax(event->x()+this->pos().x(),pright);
                }
                else if(width() == maximumWidth()){
                    pright = qMin(event->x()+this->pos().x(),pright);
                }
                else{
                    pright =event->x()+this->pos().x();
                }
            }
            isEnabledGeometry=false;
            QRect rc=QRect(QPoint(m_left,m_top),QPoint(pright,m_bottom));
            this->resize(rc.width(),rc.height());
            setGeometry(rc);
            isEnabledGeometry=true;

        }
    }
    else {
        arrows_update();
        isArrow=false;
        m_timerDelay.stop();
        m_timerDelay.start(100);
    }

}
void MoveWidget::arrows_update()
{

    if( this->windowState() == Qt::WindowFullScreen ){
        posBy = pos_none;
        QCursor my_cursor;
        my_cursor = cursor();
        my_cursor.setShape(Qt::ArrowCursor);
        setCursor(my_cursor);
        m_timerDelay.stop();
        return;
    }
    int diffLeft = abs(mapFromGlobal(cursor().pos()).x() -frameGeometry().left()+this->pos().x());
    int diffRight = abs(mapFromGlobal(cursor().pos()).x() -frameGeometry().right()+this->pos().x());
    int diffTop = abs(mapFromGlobal(cursor().pos()).y() - frameGeometry().top()+this->pos().y());
    int diffBottom = abs(mapFromGlobal(cursor().pos()).y() - frameGeometry().bottom()+this->pos().y());
    QCursor my_cursor;
    my_cursor = cursor();
    int borderWidth=border_px;
    if(diffTop < borderWidth){
        if(diffLeft < borderWidth){
            posBy = top_Left;
            my_cursor.setShape(Qt::SizeFDiagCursor);
        }
        else if(diffRight < borderWidth){
            posBy = topRight;
            my_cursor.setShape(Qt::SizeBDiagCursor);
        }
        else{
            posBy = top;
            my_cursor.setShape(Qt::SizeVerCursor);
        }
    }
    else if(diffBottom < borderWidth){
        if(diffLeft < borderWidth){
            posBy = bottomLeft;
            my_cursor.setShape(Qt::SizeBDiagCursor);
        }
        else if(diffRight < borderWidth){
            posBy = bottomRight;
            my_cursor.setShape(Qt::SizeFDiagCursor);
        }
        else{
            posBy = bottom;
            my_cursor.setShape(Qt::SizeVerCursor);
        }
    }
    else if(diffLeft < border_px){
        posBy = left;
        my_cursor.setShape(Qt::SizeHorCursor);
    }
    else if(diffRight < border_px){
        posBy = right;
        my_cursor.setShape(Qt::SizeHorCursor);
    }
    else{
        posBy = pos_none;
        my_cursor.setShape(Qt::ArrowCursor);
        m_timerDelay.stop();
    }
    setCursor(my_cursor);
}

void MoveWidget::onDelayUpdate()
{
    arrows_update();
}
void MoveWidget::mouseReleaseEvent(QMouseEvent *event)
{
    m_timer.start(100);
    event->ignore();
    if(posBy != pos_none){
        arrows_update();
    }
    foreach (QWidget* pWidget, QmlWidgetCreator::getAllQmlWidget()) {
        ((QuickWidget*)pWidget)->setEnabled (true);
    }
    m_isStart=false;
    //mainWidget->show();
    //hWidget->hide();
}

bool MoveWidget::eventFilter(QObject *o, QEvent *e)
{
    if (o == this)
    {
        //窗口状态被改变的事件.
        if(e->type() == QEvent::WindowStateChange)
        {
            if (this->windowState() == Qt::WindowMinimized){
                m_isMinimized=true;
            }else if(this->windowState() == Qt::WindowNoState){
                bool isFULL=false;
                if(m_isMinimized){
                    if(m_saveWinState== Qt::WindowFullScreen){
                        isFULL=true;
                    }
                }
                m_isMinimized=false;
                if(isFULL){
                    this->setWindowState(Qt::WindowFullScreen);
                }
            }
            return true;
        }
    }
    return QObject::eventFilter(o,e);
}
