#include "messagetipbox.h"
#include <QVBoxLayout>
#include <QMouseEvent>
#include <QGraphicsDropShadowEffect>
#include "qmlwidgetcreator.h"
#include "dialogmanager.h"

static int boxMargin=5;
MessageTipBox::MessageTipBox(QWidget *parent):QDialog(parent)
{
    setWindowFlags(Qt::Tool|Qt::FramelessWindowHint| Qt::WindowStaysOnTopHint | Qt::X11BypassWindowManagerHint);
    setAttribute(Qt::WA_NoSystemBackground, true);
    setAttribute(Qt::WA_TranslucentBackground);

    QVBoxLayout * vlayout=new QVBoxLayout(this);
    vlayout->setSpacing(0);
    vlayout->setMargin(boxMargin);
    {//qml box
        QWidget * item=QmlWidgetCreator::createQmlWidget("qrc:/qml/MessageBox.qml",this);
        m_messageBox=item;
        QGraphicsDropShadowEffect *windowShadow = new QGraphicsDropShadowEffect;
        windowShadow->setBlurRadius(8.4);
        windowShadow->setColor(QColor(8,0,25,0.3*255));
        windowShadow->setOffset(0.0);
        item->setGraphicsEffect(windowShadow);

        vlayout->addWidget (item);
        {
            QVariantMap vMap;
            vMap.insert("event","setMaxValue");
            vMap.insert("value",0);
            QmlEventManager::getInstatnce(item)->sendToQml("MessageBox",vMap);

        }
        connect(QmlEventManager::getInstatnce(item),&QmlEventManager::emitWidgetEvent,
                [=](const QString& eventName,const QVariant& value){
            if(eventName=="buttonRight"){
                buttonEvent(ButtonRight);
            }else if(eventName=="buttonleft"){
                buttonEvent(ButtonRight);
            }else if(eventName=="buttonmiddle"){
                buttonEvent(Buttonmiddle);
            }
        });
    }
    DialogManager::getInstance()->addDialog(this);
}

MessageTipBox::~MessageTipBox()
{
    DialogManager::getInstance()->removeDialog(this);
}

void MessageTipBox::setFixedSize(int w, int h)
{
    QDialog::setFixedSize(w+boxMargin*2,h+boxMargin*2);
}

void MessageTipBox::setContextText(const QString &text)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setContextText",text);
}

void MessageTipBox::setContext2Text(const QString &text)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setContext2Text",text);
}

void MessageTipBox::setButtonRightText(const QString &text)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setButtonRightText",text);
}

void MessageTipBox::setButtonmiddleText(const QString &text)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setButtonmiddleText",text);
}

void MessageTipBox::setButtonleftText(const QString &text)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setButtonleftText",text);
}

void MessageTipBox::setContextTopMargin(int margin)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setContextTopMargin",margin);
}

void MessageTipBox::setTopHeight(int topHeight)
{
    QmlEventManager::getInstatnce(m_messageBox)->sendToQml("MessageBox_setTopHeight",topHeight);
}
void MessageTipBox::mousePressEvent(QMouseEvent *e)
{
    if(e->button() & Qt::LeftButton)
    {
        if(!isMaximized()){
            dragPosition = e->globalPos() - frameGeometry().topLeft();
            leftbuttonpressed = true;
        }
    }
    e->accept();
}


void MessageTipBox::mouseReleaseEvent(QMouseEvent *e)
{
    leftbuttonpressed = false;
    e->accept();
}

void MessageTipBox::keyPressEvent(QKeyEvent *e)
{
    if(e->key() != Qt::Key_Escape){
        QDialog::keyPressEvent(e);
    }
}

void MessageTipBox::mouseMoveEvent(QMouseEvent *e)
{
    if(leftbuttonpressed)
    {
        move(e->globalPos() - dragPosition);
    }
    e->accept();
}

