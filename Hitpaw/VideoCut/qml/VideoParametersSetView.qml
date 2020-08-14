import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle{
    id:root
    color : "#2E2F30"
    //anchors.margins: 20
    Item {
        anchors.fill: parent
        anchors.bottomMargin: 50
        Grid {
            id: grid
            anchors.fill: parent

            columns: 1

            spacing: 0
            columnSpacing: spacing
            rowSpacing: spacing
            //clip: true

            property int rowCount: Math.ceil(visibleChildren.length / columns)
            property real cellWidth: parent.width
            property real cellHeight: 55

            Item {
                width: grid.cellWidth
                height: grid.cellHeight
                SliderItem {
                    titleText:"Brightness"
                    anchors.centerIn: parent
                    width: grid.cellWidth-40
                    height: grid.cellHeight
                }
            }

            Item {
                width: grid.cellWidth
                height: grid.cellHeight
                SliderItem {
                    titleText:"Soturation"
                    anchors.centerIn: parent
                    width: grid.cellWidth-40
                    height: grid.cellHeight
                }
            }

            Item {
                width: grid.cellWidth
                height: grid.cellHeight
                SliderItem {
                    titleText:"Contrast"
                    anchors.centerIn: parent
                    width: grid.cellWidth-40
                    height: grid.cellHeight
                }
            }

            Item {
                width: grid.cellWidth
                height: grid.cellHeight
                SliderItem {
                    titleText:"Transparency"
                    anchors.centerIn: parent
                    width: grid.cellWidth-40
                    height: grid.cellHeight
                }
            }
        }
    }

}
