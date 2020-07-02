#include "mainwindow.h"

#include <QApplication>
#include <QFont>
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    QFont f("Microsoft YaHei",12);
    a.setFont(f);
    MainWindow w;
    w.show();
    return a.exec();
}
