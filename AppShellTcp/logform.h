#ifndef LOGFORM_H
#define LOGFORM_H

#include <QWidget>

namespace Ui {
class LogForm;
}

class LogForm : public QWidget
{
    Q_OBJECT

public:
    explicit LogForm(QWidget *parent = nullptr);
    ~LogForm();
    static LogForm* getInstance(){return _this;}
    void sendUdpLog(const QString & logText);
    void revUdpLog(const QString & logText);
    void sendTcpLog(const QString & logText);
    void revTcpLog(const QString & logText);

Q_SIGNALS:
    void sigUdpDate(const QByteArray &bytes);
protected:
    virtual void closeEvent(QCloseEvent *event);
private slots:
    void on_pushButton_clicked();

private:
    Ui::LogForm *ui;
    static LogForm * _this;
};

#endif // LOGFORM_H
