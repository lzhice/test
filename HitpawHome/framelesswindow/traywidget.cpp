#include "traywidget.h"

#include <QMenu>
#include <QAction>
#include <QIcon>
#include <QApplication>
#include "mainwindow.h"
TrayWidget::TrayWidget(QWidget *parent)
    : QSystemTrayIcon(parent)
    , m_homeWindow(parent)
    , menu_(new QMenu(parent))
    , action_quit_(new QAction(QIcon(":/img/play.png"),tr("Quit"), parent))
    , action_qt_(new QAction(QIcon(":/img/play.png"),tr("Home"), parent))
{
    menu_->addAction(action_qt_);
    menu_->addSeparator();
    menu_->addAction(action_quit_);
    setContextMenu(menu_);
    setIcon(QIcon(":/img/play.png"));
    show();
    connect(action_qt_, &QAction::triggered, this, &TrayWidget::showHome);
    connect(action_quit_, &QAction::triggered, this, &TrayWidget::quitExe);
    connect(this, &QSystemTrayIcon::activated,
            [this](QSystemTrayIcon::ActivationReason reason){
        if (reason == QSystemTrayIcon::DoubleClick) {
            showHome();
        }
    }
    );
}

TrayWidget::~TrayWidget()
{

}



void TrayWidget::showHome() {
    if(m_homeWindow){
        m_homeWindow->show();
    }
    this->hide();
}


void TrayWidget::quitExe()
{
    qApp->quit();
}