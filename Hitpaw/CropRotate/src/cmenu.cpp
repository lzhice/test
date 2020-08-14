#include "cmenu.h"
#include <QEvent>
#include <Windows.h>
#include <WinUser.h>
CMenu::CMenu(QWidget *parent):QMenu(parent)
{

}

CMenu::CMenu(const QString &title, QWidget *parent):QMenu(title,parent)
{

}


bool CMenu::event(QEvent *event)
{
    static bool class_amended = false;
//    if (event->type() == QEvent::WinIdChange)
//    {
//        HWND hwnd = reinterpret_cast<HWND>(winId());
//        if (class_amended == false)
//        {
//            class_amended = true;
//            DWORD class_style = GetClassLong(hwnd, GCL_STYLE);
//            class_style &= ~CS_DROPSHADOW;
//            SetClassLong(hwnd, GCL_STYLE, class_style);
//        }

//    }
    return QWidget::event(event);
}
