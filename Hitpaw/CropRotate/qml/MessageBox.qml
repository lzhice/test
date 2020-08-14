import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle {
    property string contextText: ""
    property color contextColor: globalStyle.getColor("MessageBox_contextColor","#E3E3E3")

    property string contextText1: ""
    property color contextColor1: globalStyle.getColor("MessageBox_contextColor1","#979798")

    property string buttonRightText: ""
    property string buttonmiddleText: ""
    property string buttonleftText: ""
    property int  topHeight: 30
    property int  contextTopMargin: 20
    id:root
    anchors.fill: quickRoot
    color : globalStyle.getColor("MessageBox_root","#2E2F30")
    radius: 4
    clip: true
    border.color: "#8372FF"
    border.width: 1
    Item {
        id:topRect
        x:0
        y:0
        width: parent.width
        height: topHeight
        //        MouseArea {
        //            id: mouse
        //            anchors.fill: parent
        //            propagateComposedEvents: true
        //            hoverEnabled:true
        //            onEntered: {
        //                mouse.accepted = false
        //            }
        //            onExited: {
        //                mouse.accepted = false
        //            }
        //            onClicked: {
        //                mouse.accepted = false
        //            }
        //            onPressed: {
        //                mouse.accepted = false
        //            }
        //            onReleased: {
        //                mouse.accepted = false
        //            }
        //        }
    }

    Rectangle {
        id:splistLine
        anchors.left: parent.left
        anchors.top: topRect.bottom
        anchors.right: parent.right
        anchors.leftMargin: 2
        anchors.rightMargin: 2
        height: 1
        color : globalStyle.getColor("MessageBox_splistLine","#0E0D10")
    }

    Item {
        id: name
        anchors.top: splistLine.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.bottom: parent.bottom
        MouseArea {
            anchors.fill: parent

        }
        Text{
            id:context
            anchors.top: parent.top
            anchors.topMargin: contextTopMargin
            anchors.left: parent.left
            anchors.leftMargin:30
            anchors.right: parent.right
            anchors.rightMargin:30
            height: 25
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
            anchors.top: context.bottom
            anchors.topMargin: 13
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
            visible: buttonRightText===""?false:true
            isHighlight: true
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
            visible: buttonmiddleText===""?false:true
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
            visible: buttonleftText===""?false:true
        }
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            if("MessageBox_setContextText"===eventName){
                contextText=value
            }else if("MessageBox_setContext2Text"===eventName){
                contextText1=value
            }else if("MessageBox_setButtonRightText"===eventName){
                buttonRightText=value
            }else if("MessageBox_setButtonmiddleText"===eventName){
                buttonmiddleText=value
            }else if("MessageBox_setButtonleftText"===eventName){
                buttonleftText=value
            }else if("MessageBox_setContextTopMargin"===eventName){
                contextTopMargin=value
            }else if("MessageBox_setTopHeight"===eventName){
                topHeight=value
            }
        }
    }
}
