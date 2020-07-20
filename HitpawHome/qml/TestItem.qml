import QtQuick 2.0
import QtQuick.Controls.Styles 1.4
import QtQuick.Controls 2.5
Rectangle {
    width: 640
    height: 480
    color: "#303030"

    Rectangle {
        width: parent.width
        height: parent.height
        anchors.centerIn: parent
        color: "transparent"

        Grid {
            id: grid
            anchors.fill: parent

            columns: 3

            spacing: 2
            columnSpacing: spacing
            rowSpacing: spacing


            property int rowCount: Math.ceil(visibleChildren.length / columns)
            property real cellWidth: (width - (columns - 1) * columnSpacing) / columns
            property real cellHeight: (height - (rowCount - 1) * rowSpacing) / rowCount

            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
            Rectangle {
                color : Qt.rgba(0,0,0,0.3);
                width: grid.cellWidth
                height: grid.cellHeight
            }
        }
    }
}
