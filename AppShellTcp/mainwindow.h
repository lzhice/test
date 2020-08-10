#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE
class TrayWidget;
class DownServer;
class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();
    void showLogForm(bool b=true);
protected:
    virtual void closeEvent(QCloseEvent *event);
private slots:
    void on_pushButton_clicked();

    void on_pushButton_2_clicked();

private:
    Ui::MainWindow *ui;
    TrayWidget * pTrayWidget;
    DownServer * m_DownServer;
    QString m_Workspaces;
};
#endif // MAINWINDOW_H
