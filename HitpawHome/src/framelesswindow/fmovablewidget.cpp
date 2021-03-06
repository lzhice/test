﻿/****************************************************************************
**
** Copyright (C) 2014 dragondjf
**
** QFramer is a frame based on Qt5.3, you will be more efficient with it. 
** As an Qter, Qt give us a nice coding experience. With user interactive experience(UE) 
** become more and more important in modern software, deveployers should consider business and UE.
** So, QFramer is born. QFramer's goal is to be a mature solution 
** which you only need to be focus on your business but UE for all Qters.
**
** Version	: 0.2.5.0
** Author	: dragondjf
** Website	: https://github.com/dragondjf
** Project	: https://github.com/dragondjf/QCFramer
** Blog		: http://my.oschina.net/dragondjf/home/?ft=atme
** Wiki		: https://github.com/dragondjf/QCFramer/wiki
** Lincence: LGPL V2
** QQ: 465398889
** Email: dragondjf@gmail.com, ding465398889@163.com, 465398889@qq.com
** 
****************************************************************************/
#include <QMouseEvent>
#include "fmovablewidget.h"

FMovableWidget::FMovableWidget(QWidget *parent) :
    QDialog(parent),leftbuttonpressed(false)
{
//    setWindowFlags(Qt::FramelessWindowHint | Qt::WindowMinimizeButtonHint);
//    setWindowFlags(Qt::Window);
    setWindowFlags(Qt::Tool|Qt::FramelessWindowHint| Qt::WindowStaysOnTopHint | Qt::X11BypassWindowManagerHint);
}


void FMovableWidget::mousePressEvent(QMouseEvent *e)
{
    if(e->button() & Qt::LeftButton)
    {
        if(!isMaximized()){
            dragPosition = e->globalPos() - frameGeometry().topLeft();
            leftbuttonpressed = true;
        }

    }
    e->accept();
}


void FMovableWidget::mouseReleaseEvent(QMouseEvent *e)
{
    leftbuttonpressed = false;
    e->accept();
}

void FMovableWidget::keyPressEvent(QKeyEvent *e)
{
    if(e->key() != Qt::Key_Escape){
        QDialog::keyPressEvent(e);
    }
}

void FMovableWidget::mouseMoveEvent(QMouseEvent *e)
{
    if(leftbuttonpressed)
    {
        move(e->globalPos() - dragPosition);
    }
    e->accept();
}

