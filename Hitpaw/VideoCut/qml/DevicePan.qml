import QtQuick 2.0
import QtQuick.Controls.Styles 1.4
import QtQuick.Controls 2.5

Rectangle {
    property color gridColor:  "#F7B500"
    property color gridRectColor: Qt.rgba(0.1,0.1,0.1,0.6)//Qt.rgba(247/255,181/255,0,0.5) // Qt.rgba(0.6,0.6,0.6,0.4)
    property int margin : -1
    property color borderLineColor: "#F7B500"
    property string selectbuttonText: ""
    width: parent.width
    height: parent.height
    anchors.centerIn: parent
    color: "transparent"
    border.width: 0
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
        property real cellWidth: 64//(((width - (columns - 1) * columnSpacing) / columns)+1).toFixed(0)
        property real cellHeight:64// (((height - (rowCount - 1) * rowSpacing) / rowCount)+1).toFixed(0)

        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton1"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color :gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton2"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton3"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton4"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton5"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton6"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton7"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton8"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
        Rectangle {
            color : gridRectColor
            width: grid.cellWidth
            height: grid.cellHeight
            border.width: 1
            border.color:gridColor
            ImageButton{
                anchors.fill: parent
                imageWidth:(parent.width<parent.height?parent.width:parent.height)*0.5
                imageHeight: imageWidth
                imageSrc: "qrc:/img/play.png"
                objectName:"ImageButton9"
                selectbutton:selectbuttonText
                onIsPresssChanged: {
                    if(isPresss){
                        selectbuttonText=objectName;
                    }
                }
            }
        }
    }
}

