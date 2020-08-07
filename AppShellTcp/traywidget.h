#ifndef TRAYWIDGET_H
#define TRAYWIDGET_H
#include <QSystemTrayIcon>
#include <QSplashScreen>
class QWidget;
class QMenu;
class QAction;

class TrayWidget : public QSystemTrayIcon {
    Q_OBJECT
public:
    explicit TrayWidget(QWidget *parent = Q_NULLPTR);
    ~TrayWidget();
private:
    QWidget* m_homeWindow;
    QMenu *menu_;
    QAction *action_quit_;
    QAction *action_qt_;
    QAction *action_palySet_;
public Q_SLOTS:
    void showHome();
    void quitExe();
};

#endif // TRAYWIDGET_H
