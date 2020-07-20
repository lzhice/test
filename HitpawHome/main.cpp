#include "mainwindow.h"
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
    QFont f("Arial",12);
    f.setWeight(QFont::Medium);
    a.setFont(f);
    a.setStyle(new DarkStyle);
    FramelessWindow framelessWindow;
    //framelessWindow.setReSizeEnable(false);
    framelessWindow.setTopBarHeight(30);
    //framelessWindow.setWindowState(Qt::WindowFullScreen);
    //framelessWindow.setWindowTitle("test title");
    framelessWindow.setWindowIcon(a.style()->standardIcon(QStyle::SP_DesktopIcon));

    MainWindow *mainWindow = new MainWindow(&framelessWindow);
    framelessWindow.setContent(mainWindow);
    framelessWindow.show();


    return a.exec();
}
