#ifndef MARGINWIDGET_H
#define MARGINWIDGET_H

#include <QWidget>

class MarginWidget : public QWidget
{
    Q_OBJECT
public:
    enum MarginAlignment{AlignNone,AlignVCenter,AlignHCenter,AlignCenter};
    explicit MarginWidget(QWidget *parent = 0);
    void setMargin(int left,int right, int top, int bottom ,MarginAlignment aligType= AlignNone);
    void setGeometry(const QRect & r);
    void updateGeometry();
protected:
    bool isEnabledGeometry;
private:
    int m_left;
    int m_right;
    int m_top;
    int m_bottom;
    MarginAlignment m_Alignment;
};

#endif // MARGINWIDGET_H
