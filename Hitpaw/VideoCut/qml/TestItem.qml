import QtQuick 2.12

Rectangle {
    property string contextText: "Currently only supports single file importCurrently "
    property color contextColor: globalStyle.getColor("MessageBox_contextColor","#E3E3E3")

    property string contextText1: "Currently only supports single file importCurrently only supports single file importonly supports single file import"
    property color contextColor1: globalStyle.getColor("MessageBox_contextColor1","#979798")

    property string buttonRightText: "Yes"
    property string buttonmiddleText: "Cancel"

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
        anchors.left: parent.left
        anchors.leftMargin: 30
        anchors.top: parent.top
        anchors.topMargin: 30
        width: parent.width-60
        height: 25
        text:qsTr("名称：")
        color:contextColor
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignTop
        wrapMode:Text.Wrap
        font.pixelSize:14
        Rectangle {
            anchors.left: parent.left
            anchors.leftMargin: 50
            anchors.top: parent.top
            anchors.topMargin: -5
            width: 250
            height: 25
            color:"white"
            TextInput{
                anchors.fill: parent
                anchors.margins: 2
                color: "#2E2F30"
                focus: true
                horizontalAlignment: TextInput.AlignHCenter
                verticalAlignment: TextInput.AlignVCenter
            }
        }
    }

    Text{
        id:context1
        anchors.left: parent.left
        anchors.leftMargin: 30
        anchors.top: context.bottom
        anchors.topMargin: 30
        width: parent.width-60
        height: 25
        text:qsTr("图标：")
        color:contextColor
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:14
        Rectangle {
            anchors.left: parent.left
            anchors.leftMargin: 50
            anchors.top: parent.top
            anchors.topMargin: -5
            width: 250
            height: 25
            color:"white"
            TextInput{
                anchors.fill: parent
                anchors.margins: 2
                color: "#2E2F30"
                focus: true
                horizontalAlignment: TextInput.AlignHCenter
                verticalAlignment: TextInput.AlignVCenter
            }
        }
    }
    Text{
        id:context2
        anchors.left: parent.left
        anchors.leftMargin: 30
        anchors.top: context1.bottom
        anchors.topMargin: 30
        width: parent.width-60
        height: 25
        text:qsTr("Svg：")
        color:contextColor
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:14
        Rectangle {
            anchors.left: parent.left
            anchors.leftMargin: 50
            anchors.top: parent.top
            anchors.topMargin: -5
            width: 250
            height: 25
            color:"white"
            TextInput{
                anchors.fill: parent
                anchors.margins: 2
                color: "#2E2F30"
                focus: true
                horizontalAlignment: TextInput.AlignHCenter
                verticalAlignment: TextInput.AlignVCenter
            }
        }
    }


    TextButton{
        id:buttonRight
        objectName: "buttonRight"
        anchors.left: parent.left
        anchors.leftMargin:50
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
        anchors.right: parent.right
        anchors.rightMargin:50
        anchors.bottom: parent.bottom
        anchors.bottomMargin:30
        width: 120
        height: 30
        textSize : globalStyle.getFontSize("TextButton_textSize",12)
        buttonText : buttonmiddleText
    }

}





















//import QtQuick 2.0
//import QtQuick.Controls.Styles 1.4
//import QtQuick.Controls 2.5
//Rectangle {
//    width: 640
//    height: 480
//    color: "#303030"

//    Rectangle {
//        width: parent.width
//        height: parent.height
//        anchors.centerIn: parent
//        color: "transparent"

//        Grid {
//            id: grid
//            anchors.fill: parent

//            columns: 3

//            spacing: 2
//            columnSpacing: spacing
//            rowSpacing: spacing


//            property int rowCount: Math.ceil(visibleChildren.length / columns)
//            property real cellWidth: (width - (columns - 1) * columnSpacing) / columns
//            property real cellHeight: (height - (rowCount - 1) * rowSpacing) / rowCount

//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//            Rectangle {
//                color : Qt.rgba(0,0,0,0.3);
//                width: grid.cellWidth
//                height: grid.cellHeight
//            }
//        }
//    }
//}
