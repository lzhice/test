#ifndef MessageTipBox_H
#define MessageTipBox_H

#include <QDialog>
/*
    property string contextText: "Currently only supports single file importCurrently "
    property color contextColor: globalStyle.getColor("MessageBox_contextColor","#E3E3E3")

    property string contextText1: "Currently only supports single file importCurrently only supports single file importonly supports single file import"
    property color contextColor1: globalStyle.getColor("MessageBox_contextColor1","#979798")

    property string buttonRightText: "Yes"
    property string buttonmiddleText: "Cancel"
    property string buttonleftText: "Remove watermark"
    property int  topHeight: 30
    property int  contextTopMargin: 20
*/
class MessageTipBox : public QDialog
{
    Q_OBJECT
public:
    enum ButtonType{
        ButtonRight,
        Buttonmiddle,
        Buttonleft
    };
    explicit MessageTipBox(QWidget *parent = nullptr);
    ~MessageTipBox();
    void setFixedSize(int w, int h);
    void setContextText(const QString& text);
    void setContext2Text(const QString& text);

    void setButtonRightText(const QString& text);
    void setButtonmiddleText(const QString& text);
    void setButtonleftText(const QString& text);

    void setContextTopMargin(int margin);
    void setTopHeight(int topHeight);
signals:
    void buttonEvent(ButtonType type);
protected:
    void mouseMoveEvent(QMouseEvent *e);
    void mousePressEvent(QMouseEvent *e);
    void mouseReleaseEvent(QMouseEvent *e);
    void keyPressEvent(QKeyEvent * e);
private:
    QPoint dragPosition;
    bool leftbuttonpressed;
    QWidget * m_messageBox;
};

#endif // MessageTipBox
