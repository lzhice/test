#include "traywidget.h"

#include "../cmenu.h"
#include <QAction>
#include <QIcon>
#include <QApplication>
#include "mainwindow.h"
#include <QWidgetAction>
#include <QSlider>
#include <QHBoxLayout>
#include "qmlwidgetcreator.h"
TrayWidget::TrayWidget(QWidget *parent)
    : QSystemTrayIcon(parent)
    , m_homeWindow(parent)
    , menu_(new CMenu(parent))
    , action_quit_(new QAction(QIcon(":/img/play.png"),tr("Quit"), parent))
    , action_qt_(new QAction(QIcon(":/img/play.png"),tr("Home"), parent))
{
    QWidgetAction *pVoice = new QWidgetAction(this);

    QWidget *pVoiceWdt = new QWidget();


    QHBoxLayout *layout = new QHBoxLayout(pVoiceWdt);
    layout->setSpacing(0);
    layout->setMargin(0);
    {
        QWidget * pMenuUpdateItem=QmlWidgetCreator::createQmlWidget("qrc:/qml/MenuUpdateItem.qml",NULL);
        pMenuUpdateItem->setMinimumWidth(100);
        pMenuUpdateItem->setMinimumHeight(25);
        //QSlider *pSlider = new QSlider(Qt::Horizontal);
        layout->addWidget(pMenuUpdateItem);
    }


    pVoiceWdt->setLayout(layout);

    pVoice->setDefaultWidget(pVoiceWdt);

    QMenu* menu_2=new CMenu(parent);
    menu_2->addAction(action_qt_);
    menu_2->addAction(action_qt_);
    menu_2->addAction(action_qt_);
    menu_2->addAction(action_qt_);
    menu_->addAction(menu_2->menuAction());


    menu_->addAction(action_qt_);
    menu_->addSeparator();
    menu_->addAction(action_quit_);
    menu_->addAction(pVoice);
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

    connect(pVoice, &QAction::triggered, [=](){
        qDebug()<<"111111111111111111";
    });
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
