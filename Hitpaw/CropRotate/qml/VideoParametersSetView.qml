import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle{
    property color splitLineColor: "#000000"
    id:root
    color : "#2E2F30"
    //anchors.margins: 20
    Rectangle {
        id:rightTop
        anchors.top: parent.top
        anchors.right: parent.right
        width: 10
        height: 10
        radius: 0
        color : "#2e2f30"
    }
    Rectangle {
        id:leftBottom
        anchors.bottom: parent.bottom
        anchors.left: parent.left
        width: 10
        height: 10
        radius: 0
        color : "#2e2f30"
    }
    Rectangle {
        anchors.left: parent.left
        anchors.top: parent.top
        width: 1
        height: parent.height
        color: splitLineColor
    }
    Item {
        id: row0
        anchors.top: parent.top
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 0
        anchors.leftMargin: 0
        anchors.rightMargin: 0
        height: 1
        Rectangle{
            anchors.fill: parent
            color: splitLineColor
        }
    }
    Item {
        id: row1
        anchors.top: row0.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 0
        anchors.leftMargin: 20
        anchors.rightMargin: 0
        height: 70
        Rectangle{
            anchors.verticalCenter: parent.verticalCenter
            anchors.left: parent.left
            width: 200
            height: 30
            ImageTextButton{
                anchors.fill: parent
                objectName: "buttonOpenVideo"
                buttonText : " "+qsTr("Replace Video")+" "
                imageSrc: "qrc:/img/drawable-xxxhdpi_add.png"
            }
        }
    }
    Item {
        id: row2
        anchors.top: row1.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 0
        anchors.leftMargin: 0
        anchors.rightMargin: 0
        height: 1
        Rectangle{
            anchors.fill: parent
            color: splitLineColor
        }
    }
    Item {
        id: row3
        anchors.top: row2.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 20

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
    Item {
        id: row9
        anchors.bottom: parent.bottom
        anchors.bottomMargin: 29
        anchors.left: parent.left
        anchors.leftMargin: 20
        width: 200
        height: 30
        TextButton{
            id:buttonRight
            objectName: "buttonExport"
            anchors.fill: parent
            textSize : globalStyle.getFontSize("TextButton_textSize",12)
            buttonText : " "+qsTr("Cut")+" "
        }
    }
}
