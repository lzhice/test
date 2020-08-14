import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle {
    property string timeText:""
    anchors.fill: quickRoot
    color : "#2e2f30"
    radius: 10

    Rectangle {
        id:timePan
        anchors.top: parent.top
        anchors.left: parent.left
        anchors.right: parent.right
        height: 30
        color : globalStyle.getColor("TimePan","#2e2f30")
        Rectangle {
            id:line
            anchors.bottom: parent.bottom
            anchors.left: parent.left
            anchors.right: parent.right
            height: 1
            color : globalStyle.getColor("line","#080019")
        }
        Text{
            anchors.fill: parent
            text:  " " +timeText+ " "
            color:Qt.rgba(1/255,255/255,255/255,1);
            horizontalAlignment: Text.AlignHCenter
            verticalAlignment: Text.AlignVCenter
            font.pixelSize:12
        }
    }
    VideoRangSliderItem{
        anchors.top: timePan.bottom
        anchors.bottom: parent.bottom
        anchors.bottomMargin: 0
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.leftMargin: 5
        anchors.rightMargin: 5
        height: sliderHeight
    }
//    Rectangle {
//        id:leftBottom
//        anchors.top: parent.top
//        anchors.left: parent.left
//        width: 10
//        height: 10
//        radius: 0
//        color : globalStyle.getColor("TimePan","#2e2f30")
//    }
//    Rectangle {
//        id:leftTop
//        anchors.top: parent.top
//        anchors.left: parent.left
//        width: 10
//        height: 10
//        radius: 0
//        color : globalStyle.getColor("TimePan","#2e2f30")
//    }
//    Rectangle {
//        id:rightTop
//        anchors.top: parent.top
//        anchors.right: parent.right
//        width: 10
//        height: 10
//        radius: 0
//        color : globalStyle.getColor("TimePan","#2e2f30")
//    }
    Rectangle {
        id:rightBottom
        anchors.bottom: parent.bottom
        anchors.right: parent.right
        width: 10
        height: 10
        radius: 0
        color : "#2e2f30"
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            if("updateTimeText"===eventName){

                timeText=value
            }
        }
    }
}
