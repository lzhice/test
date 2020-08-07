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
        }
        Image{
            id:lineRect_add_img
            anchors.top: parent.top
            anchors.topMargin: 80
            anchors.horizontalCenter: parent.horizontalCenter
            width: 40
            height: 40
            mipmap: true
            smooth: true
            antialiasing: true
            source: "qrc:/img/add@2x.png"
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
