#ifndef CMENU_H
#define CMENU_H

#include <QMenu>
class CMenu : public QMenu
{
public:
    explicit CMenu(QWidget *parent = nullptr);
    explicit CMenu(const QString &title, QWidget *parent = nullptr);
protected:
    bool event(QEvent *) override;
};

#endif // CMENU_H
