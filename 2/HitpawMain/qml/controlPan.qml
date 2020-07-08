import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle {
    anchors.fill: quickRoot
    color : Qt.rgba(46/255,47/255,51/255,1)
    radius: 10
//    MouseArea {
//        id: dragRegion
//        anchors.fill: parent
//        property point clickPos: "0,0"
////        onPressed: {
////            clickPos  = Qt.point(mouse.x,mouse.y)
////        }
////        onPositionChanged: {
////            //鼠标偏移量
////            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
////            mainWidget.pos.x=(mainWidget.x+delta.x)
////            mainWidget.pos.y=(mainWidget.y+delta.y)
////        }
//    }

    VideoRangSliderItem{
        anchors.fill:parent
    }
}
