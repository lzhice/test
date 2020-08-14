import QtQuick 2.12
import QtQuick.Controls 1.4
Rectangle{
    property double textWidth: textFontMetrics.boundingRect(resizable.text).width
    anchors.fill: quickRoot
    color: "transparent"
    Text{

        id:resizable
        anchors.fill:parent
        height: 50
        text:"contextText1"
        color:"white"
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignVCenter
        font.pixelSize:12

        FontMetrics {
            id: textFontMetrics
            font.family: resizable.font.family
            font.pixelSize:resizable.font.pixelSize
        }
        Image{
            id:img
            anchors.top: parent.top
            anchors.left: parent.left
            anchors.leftMargin: textWidth
            width: 24
            height: 24
            antialiasing: true
            source: "qrc:/img/upgrade备份@2x.png"
        }
    }
    MouseArea {
        id: mouse
        anchors.fill: parent
        propagateComposedEvents: true
        hoverEnabled:true
        onEntered: {
            resizable.color="red"
            mouse.accepted = false
        }
        onExited: {
            resizable.color="white"
            mouse.accepted = false
        }
        onClicked: {
            mouse.accepted = false
        }
        onPressed: {
            mouse.accepted = false
        }
        onReleased: {
            mouse.accepted = false
        }
    }

}


