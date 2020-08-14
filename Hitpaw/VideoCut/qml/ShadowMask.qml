import QtQuick 2.0
import QtQuick.Controls.Styles 1.4
import QtQuick.Controls 2.5

Rectangle {
    property color gridColor:  Qt.rgba(0,0,0,0.1);
    property color gridRectColor:  Qt.rgba(0,0,0,0.2);
    property int margin : -1
    property color borderLineColor: globalStyle.getColor("VideoSelectRangItem_fillLineColor","#F7B500")
    width: parent.width
    height: parent.height
    anchors.centerIn: parent
    color: "transparent"
    border.width: 1
    border.color:borderLineColor
    Grid {
        id: grid
        anchors.fill: parent

        columns: 3

        spacing: 0
        columnSpacing: spacing
        rowSpacing: spacing
        clip: true

        property int rowCount: Math.ceil(visibleChildren.length / columns)
        property real cellWidth: (((width - (columns - 1) * columnSpacing) / columns)+1).toFixed(0)
        property real cellHeight: (((height - (rowCount - 1) * rowSpacing) / rowCount)+1).toFixed(0)

        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color :gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
        }
    }
    Image {
        anchors.left: parent.left
        anchors.top: parent.top
        anchors.leftMargin: margin
        anchors.topMargin: margin
        width: 28
        height: 28
        source: "qrc:/img/Combined Shape Copy 2_LT.png"
    }
    Image {
        anchors.right: parent.right
        anchors.top: parent.top
        anchors.rightMargin: margin
        anchors.topMargin: margin
        width: 28
        height: 28
        source: "qrc:/img/Combined Shape Copy 2_RT.png"
    }
    Image {
        anchors.left: parent.left
        anchors.bottom: parent.bottom
        anchors.leftMargin: margin
        anchors.bottomMargin: margin
        width: 28
        height: 28
        source: "qrc:/img/Combined Shape Copy 2_LB.png"
    }
    Image {
        anchors.right: parent.right
        anchors.bottom: parent.bottom
        anchors.rightMargin: margin
        anchors.bottomMargin: margin
        width: 28
        height: 28
        source: "qrc:/img/Combined Shape Copy 2_RB.png"
    }
}

