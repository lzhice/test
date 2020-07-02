#include "mainwindow.h"
#include "qmlwidgetcreator.h"
#include <QHBoxLayout>

MainWindow::MainWindow(QWidget *parent)
    : QWidget(parent)
{
    QHBoxLayout * layout=new QHBoxLayout(this);
    layout->setSpacing(0);
    layout->setMargin(1);
    {
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/ControlPan.qml",this);
        layout->addWidget (item);
        item->setFixedWidth (70);
    }
    {
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/VideoMainView.qml",this);
        layout->addWidget (item);
    }
    this->setLayout (layout);
    this->setFixedSize (1270,715);
}

MainWindow::~MainWindow()
{

}

