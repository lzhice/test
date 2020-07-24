import QtQuick 2.0
import QtQuick.Controls 2.5

Rectangle {
    property string objectName: ""
    property int textSize : globalStyle.getFontSize("TextButton_textSize",12)
    property string buttonText : ""
    id:button
    color: "transparent"
    border.width: 1
    border.color : globalStyle.getColor("TextButton_borderColor","#8372FF")
    Text{
        id:button_text
        anchors.fill: parent
        text:buttonText
        color:button.color===globalStyle.getColor("TextButton_color","transparent")?
                  globalStyle.getColor("TextButton_text_normalColor","#8372FF"):
                  globalStyle.getColor("TextButton_text_haverColor","#FFFFFF")
        horizontalAlignment: Text.AlignHCenter
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:textSize
        font.weight:button.color===globalStyle.getColor("TextButton_color","transparent")?Font.Bold:Font.Bold
    }
    MouseArea {
        id: mouseA
        anchors.fill: parent
        hoverEnabled:true
        onEntered: {
            button.color = globalStyle.getColor("TextButton_color","#8372FF")
        }
        onExited: {
            button.color = "transparent";
            button_effectItem.visible=false
        }
        onClicked: {
            eventManager.sendToWidget(button.objectName,"onClicked")
        }
        onPressed: {
            button_effectItem.visible=true
        }
        onReleased: {
            button_effectItem.visible=false
        }
    }
    Rectangle{
        id:button_effectItem
        anchors.fill: parent
        visible: false
        clip:true
        color : Qt.rgba(0.6,0.6,0.6,0.4) ;
    }
}
