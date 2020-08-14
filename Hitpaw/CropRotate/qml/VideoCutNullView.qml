import QtQuick 2.12
import QtGraphicalEffects 1.12
import QtQuick.Controls 2.5
Rectangle{
    property color splitLineColor: "#000000"

    property string textContext1: " "+qsTr("Drag and Drop a file here")+" "
    property string textContext2: " "+qsTr("or")+" "
    property string textContext3: " "+qsTr("Click to choice local file")+" "
    id: root
    anchors.fill: quickRoot
    color : globalStyle.getColor("VideoCutNullView_root","#2E2F30")
    radius: 10

    Rectangle {
        id:leftBottom
        anchors.top: parent.top
        anchors.left: parent.left
        width: 10
        height: 10
        radius: 0
        color : "#2e2f30"
    }
    Rectangle {
        id:rightBottom
        anchors.bottom: parent.bottom
        anchors.right: parent.right
        width: 10
        height: 10
        radius: 0
        color : "#2e2f30"
    }
    Item{
        id: lineRect
        anchors.centerIn: parent
        width: 480
        height: 270
        Image{
            id:lineRect_img
            anchors.fill:parent
            mipmap: true
            smooth: true
            antialiasing: true
            source: "qrc:/img/Rectangle Copy 23@2x.png"
            DropArea{
                anchors.fill: parent
                onEntered: {
                }
                onDropped: {
                    drag.accepted = true
                    var fileCount=drop.urls.length
                    if (fileCount===1) {
                        eventManager.sendToWidget("OpenFile",drop.urls[0])
                    }else{
                        eventManager.sendToWidget("MultipleFiles","")//
                    }

                }
            }
        }
        ImageButton{
            objectName: "buttonOpenVideo"
            id:lineRect_add_img
            anchors.top: parent.top
            anchors.topMargin: 80
            anchors.horizontalCenter: parent.horizontalCenter
            width: 40
            height: 40
            smooth: true
            antialiasing: true
            imageSrc: "qrc:/img/add@2x.png"
        }
        Text{
            id:text1
            anchors.top: lineRect_add_img.bottom
            anchors.topMargin: 14
            anchors.left: parent.left
            anchors.right: parent.right
            height: 20
            text:textContext1
            color:globalStyle.getColor("VideoCutNullView_text","#E3E3E3")
            horizontalAlignment: Text.AlignHCenter
            verticalAlignment: Text.AlignVCenter
            clip: true
            font.pixelSize:14
        }
        Text{
            id:text2
            anchors.top: text1.bottom
            anchors.topMargin: 0
            anchors.left: parent.left
            anchors.right: parent.right
            height: 20
            text:textContext2
            color:globalStyle.getColor("VideoCutNullView_text","#E3E3E3")
            horizontalAlignment: Text.AlignHCenter
            verticalAlignment: Text.AlignVCenter
            clip: true
            font.pixelSize:14
        }
        Text{
            id:text3
            anchors.top: text2.bottom
            anchors.topMargin: 0
            anchors.left: parent.left
            anchors.right: parent.right
            height: 20
            text:textContext3
            color:globalStyle.getColor("VideoCutNullView_text","#E3E3E3")
            horizontalAlignment: Text.AlignHCenter
            verticalAlignment: Text.AlignVCenter
            clip: true
            font.pixelSize:14
        }

    }
    Rectangle {
        anchors.left: parent.left
        anchors.top: parent.top
        width: parent.width
        height: 1
        color: splitLineColor
    }

}
