#include "marginwidget.h"
#include <QDebug>
MarginWidget::MarginWidget(QWidget *parent) :
    QWidget(parent),m_left(-1),m_right(-1),m_top(-1),m_bottom(-1),m_Alignment(AlignNone),isEnabledGeometry(true)
{

}

void MarginWidget::setMargin(int left, int right, int top, int bottom,MarginAlignment aligType)
{

    m_left=left;
    m_right=right;
    m_top=top;
    m_bottom=bottom;
    m_Alignment=aligType;
}

void MarginWidget::setGeometry(const QRect &r)
{
    if(isEnabledGeometry){
        if(this->parentWidget()){
            int parentWidth=this->parentWidget()->size().width();
            int parentHeight=this->parentWidget()->size().height();
            int width=r.width();
            int height=r.height();
            QPoint pos=r.topLeft();
            if(m_Alignment==AlignNone){
                if(m_right>-1&&m_left>-1)
                {
                    pos.setX(m_left);
                    width=parentWidth-m_left-m_right;
                }else{
                    if(m_left>-1){
                        pos.setX(m_left);
                    }
                    if(m_right>-1&&m_left<0){
                        pos.setX(parentWidth-width-m_right);
                    }
                }
                if(m_bottom>-1&&m_top>-1){
                    pos.setY(m_top);
                    height=parentHeight-m_top-m_bottom;
                }else{
                    if(m_top>-1){
                        pos.setY(m_top);
                    }
                    if(m_bottom>-1&&m_top<0){
                        pos.setY(parentHeight-height-m_bottom);
                    }
                }
            }else if(m_Alignment==AlignHCenter){//横
                pos.setX((parentWidth-width)/2);
                if(m_bottom>-1&&m_top>-1){
                    pos.setY(m_top);
                    height=parentHeight-m_top-m_bottom;
                }else{
                    if(m_top>-1){
                        pos.setY(m_top);
                    }
                    if(m_bottom>-1&&m_top<0){
                        pos.setY(parentHeight-height-m_bottom);
                    }
                }
            }else if(m_Alignment==AlignVCenter){//竖
                pos.setY((parentHeight-height)/2);
                if(m_right>-1&&m_left>-1)
                {
                    pos.setX(m_left);
                    width=parentWidth-m_left-m_right;
                }else{
                    if(m_left>-1){
                        pos.setX(m_left);
                    }
                    if(m_right>-1&&m_left<0){
                        pos.setX(parentWidth-width-m_right);
                    }
                }
            }else if(m_Alignment==AlignCenter){//中心
                pos.setX((parentWidth-width)/2);
                pos.setY((parentHeight-height)/2);
            }
            QWidget::setGeometry(QRect(pos,QSize(width,height)));
        }else{
            QWidget::setGeometry(r);
        }
    }else{
        QWidget::setGeometry(r);
    }
}

void MarginWidget::updateGeometry()
{

    setGeometry(this->geometry());
}
