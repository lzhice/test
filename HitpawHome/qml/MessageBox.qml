import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle {
    property string contextText: "Currently only supports single file importCurrently "
    property color contextColor: globalStyle.getColor("MessageBox_contextColor","#E3E3E3")

    property string contextText1: "Currently only supports single file importCurrently only supports single file importonly supports single file import"
    property color contextColor1: globalStyle.getColor("MessageBox_contextColor1","#979798")

    property string buttonRightText: "Yes"
    property string buttonmiddleText: "Cancel"
    property string buttonleftText: "Remove watermark"

    id:root
    anchors.fill: quickRoot
    color : globalStyle.getColor("MessageBox_root","#2E2F30")
    radius: 4
    Rectangle {
        id:splistLine
        x:0
        y:0
        width: parent.width
        height: 1
        color : globalStyle.getColor("MessageBox_splistLine","#0E0D10")
    }
    Text{
        id:context
        anchors.top: parent.top
        anchors.topMargin: 27
        anchors.left: parent.left
        anchors.leftMargin:30
        anchors.right: parent.right
        anchors.rightMargin:30
        height: 50
        text:contextText
        color:contextColor
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignTop
        wrapMode:Text.Wrap
        font.pixelSize:globalStyle.getFontSize("MessageBox_context",18)
        //font.weight:Font.Thin
    }
    Text{
        id:context1
        anchors.top: parent.top
        anchors.topMargin: 58
        anchors.left: parent.left
        anchors.leftMargin:30
        anchors.right: parent.right
        anchors.rightMargin:30
        height: 50
        text:contextText1
        color:contextColor1
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignTop
        wrapMode:Text.Wrap
        font.pixelSize:globalStyle.getFontSize("MessageBox_context1",12)
        font.weight:Font.Medium
    }
    TextButton{
        id:buttonRight
        objectName: "buttonRight"
        anchors.right: parent.right
        anchors.rightMargin:30
        anchors.bottom: parent.bottom
        anchors.bottomMargin:30
        width: 120
        height: 30
        textSize : globalStyle.getFontSize("TextButton_textSize",12)
        buttonText : buttonRightText
    }

    TextButton{
        id:buttonmiddle
        objectName: "buttonmiddle"
        anchors.right: buttonRight.left
        anchors.rightMargin:20
        anchors.bottom: parent.bottom
        anchors.bottomMargin:30
        width: 120
        height: 30
        textSize : globalStyle.getFontSize("TextButton_textSize",12)
        buttonText : buttonmiddleText
    }

    TextButton{
        id:buttonleft
        objectName: "buttonleft"
        anchors.right: buttonmiddle.left
        anchors.rightMargin:60
        anchors.bottom: parent.bottom
        anchors.bottomMargin:30
        width: 140
        height: 30
        textSize : globalStyle.getFontSize("TextButton_textSize",12)
        buttonText : buttonleftText
    }
}
