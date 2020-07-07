#include "mainwindow.h"
#include "movewidget.h"
#include <QApplication>
#include <QFont>
#include <QQuickWindow>
#include "DarkStyle.h"
#include "framelesswindow.h"
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    //QCoreApplication::setAttribute(Qt::AA_UseSoftwareOpenGL);
    QQuickWindow::setSceneGraphBackend(QSGRendererInterface::Software);
    QFont f("Microsoft YaHei",12);
    a.setFont(f);
    a.setStyle(new DarkStyle);
//    MoveWidget w;
//    w.setObjectName("w1001");
//    //w.setStyleSheet("*#w1001{background-color:rgb(200,200,200);border-radius:10px;border-width:2}");
//    MainWindow *pMonitorMainUI=new MainWindow(&w);
//    w.setWidget(pMonitorMainUI,640,384,10);
//    w.show();


    FramelessWindow framelessWindow;
    //framelessWindow.setWindowState(Qt::WindowFullScreen);
    //framelessWindow.setWindowTitle("test title");
    framelessWindow.setWindowIcon(a.style()->standardIcon(QStyle::SP_DesktopIcon));

    // create our mainwindow instance
    MainWindow *mainWindow = new MainWindow(&framelessWindow);

    // add the mainwindow to our custom frameless window
    framelessWindow.setContent(mainWindow);
    framelessWindow.show();


    return a.exec();
}
