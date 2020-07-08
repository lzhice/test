import QtQuick 2.0
import QtQuick.Controls 2.5

RangeSlider {
    id: root

    property color checkedColor: "#3498DB"
    property double contextHeight: 0
    property double minWidth: 60
    from:0
    to:1
    first.value: 0.25
    second.value: 0.75
    snapMode:RangeSlider.SnapAlways
    first.onVisualPositionChanged: {
        if((second.visualPosition-first.visualPosition)*root.width<minWidth){
            first.value=second.visualPosition-(minWidth/root.width)
        }
        console.log("first.onVisualPositionChanged:",first.visualPosition)
    }
    second.onVisualPositionChanged: {
        if((second.visualPosition-first.visualPosition)*root.width<minWidth){
            second.value=first.visualPosition+(minWidth/root.width)
        }
        console.log("first.onVisualPositionChanged:",first.visualPosition)
    }

//    Rectangle {
//        x: root.first.visualPosition * parent.width
//        width: root.second.visualPosition * parent.width - x
//        height: contextHeight
//        color: Qt.rgba(248/255,184/255,2/255,0)
//        border.color: Qt.rgba(248/255,184/255,2/255,1)
//    }
    first.handle: Rectangle {
        x: root.leftPadding + first.visualPosition * (root.availableWidth - width)
        y: root.topPadding + root.availableHeight / 2 - height / 2
        implicitWidth: 30
        implicitHeight: 110
        color: first.pressed ? Qt.darker(root.checkedColor, 1.2) : root.checkedColor
        border.color: Qt.darker(root.checkedColor, 0.93)
//        BorderImage {
//            anchors.fill: parent
//            source: "file:///C:/Users/Admin/Documents/untitled5/1.png"
//        }
    }

    second.handle: Rectangle {
        x: root.leftPadding + second.visualPosition * (root.availableWidth - width)
        y: root.topPadding + root.availableHeight / 2 - height / 2
        implicitWidth: 30
        implicitHeight: 110
        color: second.pressed ? Qt.darker(root.checkedColor, 1.2) : root.checkedColor
        border.color: Qt.darker(root.checkedColor, 0.93)
//        BorderImage {
//            anchors.fill: parent
//            source: "file:///C:/Users/Admin/Documents/untitled5/2.png"
//        }
    }

    background: Rectangle {
        x: root.leftPadding
        y: root.topPadding + root.availableHeight / 2 - height / 2
        implicitWidth: 300
        implicitHeight: 110
        width: root.availableWidth
        height: implicitHeight
        color: Qt.rgba(62/255,61/255,67/255,0)

        Rectangle {
            id:r0
            x: 0
            width: root.first.visualPosition * parent.width
            height: parent.height
            color: Qt.rgba(62/255,61/255,67/255,0.8)

        }
        Rectangle {
            id:r1
            x: root.first.visualPosition * parent.width
            width: root.second.visualPosition * parent.width - x
            height: parent.height
            color: Qt.rgba(62/255,61/255,67/255,0)
            border.color: Qt.rgba(247/255,181/255,0/255,1)
            border.width: 6
//            BorderImage {
//                anchors.fill: parent
//                source: "file:///C:/Users/Admin/Documents/untitled5/item.png"
//                border.bottom: 20
//                border.left: 30
//                border.right: 30
//                border.top: 20
//            }

        }
        Rectangle {
            x: r1.x+r1.width
            width: parent.width - x
            height: parent.height
            color: Qt.rgba(62/255,61/255,67/255,0.8)

        }

    }


}
