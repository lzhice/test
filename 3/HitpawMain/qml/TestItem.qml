import QtQuick 2.0
import QtQuick.Controls.Styles 1.4
import QtQuick.Controls 2.5
Rectangle{
    id:root
    anchors.fill: quickRoot
    color : Qt.rgba(46/255,47/255,51/255,1)
    border.color: Qt.rgba(200/255,200/255,200/255,0.5)
    border.width: 1
    Text{
        anchors.left: parent.left
        anchors.leftMargin: 20
        anchors.verticalCenter: parent.verticalCenter
        width: 400
        height: parent.height
        text: " 地图图层"
        color:"white"
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:15
        //font.bold: true
    }
    ButtonItem{
        id:button_cut
        objectName:"cut"
        anchors.verticalCenter: parent.verticalCenter
        anchors.right: parent.right
        anchors.rightMargin: 10
        text: "cut me"
    }
    ButtonItem{
        id:button_cut1
        objectName:"cut1"
        anchors.verticalCenter: parent.verticalCenter
        anchors.right: button_cut.left
        anchors.rightMargin: 10
        text: "cut me1"
    }
    ButtonItem{
        id:button_cut2
        objectName:"cut2"
        anchors.verticalCenter: parent.verticalCenter
        anchors.right: button_cut1.left
        anchors.rightMargin: 10
        text: "cut me2"
    }
    ButtonItem{
        id:button_cut3
        objectName:"cut3"
        anchors.verticalCenter: parent.verticalCenter
        anchors.right: button_cut2.left
        anchors.rightMargin: 10
        text: "cut me3"
    }
}
