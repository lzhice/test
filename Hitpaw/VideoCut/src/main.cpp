#include "mainwindow.h"
#include <QApplication>
#include <QFont>
#include <QQuickWindow>
#include "DarkStyle.h"
#include "framelesswindow.h"
#include "ImagesModule.h"
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    QtImagesModule::initialize();
//    QQuickWindow::setTextRenderType(QQuickWindow::QtTextRendering);
//    QQuickWindow::setDefaultAlphaBuffer(true);
    QQuickWindow::setSceneGraphBackend(QSGRendererInterface::Software);
//    QCoreApplication::setAttribute(Qt::AA_UseHighDpiPixmaps);
//    QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);
    //QCoreApplication::setAttribute(Qt::AA_UseSoftwareOpenGL);
    QFont f("Arial",12);
    f.setWeight(QFont::Medium);
    a.setFont(f);
    a.setStyle(new DarkStyle);
    FramelessWindow framelessWindow;
    framelessWindow.setTopBarHeight(24);
    framelessWindow.setWindowIcon(a.style()->standardIcon(QStyle::SP_DesktopIcon));

    MainWindow *mainWindow = new MainWindow(&framelessWindow);
    framelessWindow.setContent(mainWindow);
    framelessWindow.show();


    return a.exec();
}
