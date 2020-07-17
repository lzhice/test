import QtQuick 2.0
import QtQuick.Controls 1.4
import QtQuick.Controls.Styles 1.4
Rectangle {
    property string contextText: "Currently only supports single file import"
    property color contextColor: globalStyle.getColor("LoadingBox_contextColor","#E3E3E3")

    property string contextText1: "Currently only supports single file importCurrently only supports single file importonly supports single file import"
    property color contextColor1: globalStyle.getColor("LoadingBox_contextColor1","#979798")

    property string buttonRightText: "Yes"
    property string buttonmiddleText: "Cancel"
    property string buttonleftText: "Remove watermark"

    id:root
    anchors.fill: quickRoot
    color : globalStyle.getColor("LoadingBox_root","#2E2F30")
    radius: 4
    Rectangle {
        id:splistLine
        x:0
        y:0
        width: parent.width
        height: 1
        color : globalStyle.getColor("LoadingBox_splistLine","#0E0D10")
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
        font.pixelSize:globalStyle.getFontSize("LoadingBox_context",18)
        //font.weight:Font.Thin
    }
    Rectangle{
        id:sliderRectangle
        anchors.top: context.top
        anchors.topMargin: 35
        anchors.left: parent.left
        anchors.leftMargin:30
        anchors.right: parent.right
        anchors.rightMargin:30
        height: 20
        color: "transparent"
        Slider {
            id:slider
            anchors.fill: parent
            style: SliderStyle
            {
                handle: Rectangle
                {
                    anchors.centerIn: parent;
                    color: globalStyle.getColor("Slider_handleColor","#8372FF")
                    border.color: "gray"
                    border.width: 0
                    width: 0
                    height: 0
                    radius: 0
                }
                groove: Rectangle {
                    implicitHeight: 4
                    color: globalStyle.getColor("Slider_grooveColor","#454649")
                    radius: 3;

                    Rectangle {
                        implicitHeight: 4
                        color: globalStyle.getColor("Slider_grooveColor1","#8372FF")
                        implicitWidth: styleData.handlePosition
                        radius: 3
                    }
                }
            }
        }
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
