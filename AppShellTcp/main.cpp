#include "mainwindow.h"
#include <QDateTime>
#include <QApplication>
#include "DarkStyle.h"
#include <QMessageBox>
#include <QObject>
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    a.setStyle(new DarkStyle);
    if(QDateTime::fromString("20200802","yyyyMMdd")<QDateTime::currentDateTime()&&QDateTime::currentDateTime()<QDateTime::fromString("20200820","yyyyMMdd")){

    }else{
        QMessageBox::warning(NULL, QObject::tr("提示"), QObject::tr("测试期限已到期，请联系软件作者！"));
        return 1;
    }

    MainWindow w;
    return a.exec();
}
