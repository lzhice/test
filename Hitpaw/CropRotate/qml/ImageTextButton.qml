import QtQuick 2.12
import QtQuick.Controls 2.5
import QtGraphicalEffects 1.12
Rectangle {
    id:button
    enum Style {
            Rimless,
            Bordered
    }
    property string objectName: ""
    property int textSize : globalStyle.getFontSize("TextButton_textSize",12)
    property string buttonText : ""
    property color bkColor: globalStyle.getColor("bkBorderColor_color","#3B3C3D")
    property color bkBorderColor: "transparent"
    property string imageSrc: ""
    property int imageWidth: 24
    property int imageHeight: 24
    property bool isEnable: true
    property int textLeftMargins: 64
    property int textRightMargins: 0
    property int textTopMargins: 8
    property int textBottomMargins: 7
    property int imgLeftMargins: 25
    color: bkColor
    border.width: bkBorderColor===globalStyle.getColor("bkBorderColor_color","transparent")?0:1
    border.color : bkBorderColor

    Image{
        id:button_img
        anchors.verticalCenter: parent.verticalCenter
        anchors.left: parent.left
        anchors.leftMargin:imgLeftMargins
        width: imageWidth
        height: imageHeight
        mipmap: true
        smooth: true
        antialiasing: true
        source: imageSrc
    }
    Text{
        id:button_text
        anchors.left: parent.left
        anchors.leftMargin: textLeftMargins
        anchors.right: parent.right
        anchors.rightMargin: textRightMargins
        anchors.top: parent.top
        anchors.topMargin: textTopMargins
        anchors.bottom: parent.bottom
        anchors.bottomMargin: textBottomMargins
        text:buttonText
        color:isEnable?globalStyle.getColor("TextButton_text_normalColor","#E3E3E3"):globalStyle.getColor("TextButton_text_normalColor","#5E5F62")
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:textSize
    }

    MouseArea {
        id: mouseA
        anchors.fill: parent
        hoverEnabled:true
        visible: isEnable
        onEntered: {

        }
        onExited: {
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
