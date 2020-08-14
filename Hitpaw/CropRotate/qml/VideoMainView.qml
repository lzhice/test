import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle {
    property int itemWidth: 278
    property int itemHeight: 140
    property int leftRightMargins: 80
    property int margins: 10
    property int scrollBarWidth: 4
    property int gridTopMargin: 56
    property int scrollBarRightMargin: 3
    property int  mainRightMargin: 10
    radius: 10
    anchors.fill: quickRoot
    color : globalStyle.getColor("VideoGridItem_titleText","#0F0F10")
    Rectangle {
        anchors.left: parent.left
        anchors.top: parent.top
        width: 10
        height: 10
        color : globalStyle.getColor("VideoGridItem_titleText","#0F0F10")
    }
//    Rectangle {
//        anchors.left: parent.left
//        anchors.bottom: parent.bottom
//        width: 10
//        height: 10
//        color : globalStyle.getColor("VideoGridItem_titleText","#0F0F10")
//    }
    Rectangle {
        anchors.right: parent.right
        anchors.top: parent.top
        width: 10
        height: 10
        color : globalStyle.getColor("VideoGridItem_titleText","#0F0F10")
    }
//    Rectangle {
//        anchors.right: parent.right
//        anchors.bottom: parent.bottom
//        width: 10
//        height: 10
//        color : globalStyle.getColor("VideoGridItem_titleText","#0F0F10")
//    }
    Flickable {
        id:root
        anchors.fill: parent
        contentHeight: gridView.contentHeight+gridTopMargin+(gridTopMargin-2*margins)// gridView.cellHeight * listModel.count/3+120
        clip: true
        boundsBehavior:Flickable.StopAtBounds
        Rectangle{
            id:bkScrollBarRect
            anchors.top: parent.top
            anchors.right: parent.right
            anchors.bottom: parent.bottom
            anchors.rightMargin: mainRightMargin-scrollBarRightMargin
            width: scrollBarWidth
            color : globalStyle.getColor("VideoGridItem_titleText","#3B3C3D")
        }
        ScrollBar.vertical: ScrollBar {
            anchors.top: parent.top
            anchors.topMargin: -1
            anchors.right: parent.right
            anchors.rightMargin: mainRightMargin-2-scrollBarRightMargin
            width: scrollBarWidth*2
            height:parent.height
            visible: (parent.height-contentItem.height)<10?false:true
            onVisibleChanged: {
                bkScrollBarRect.visible=visible
            }

            contentItem : Rectangle{
                color : globalStyle.getColor("VideoGridItem_titleText","#8372FF")
                visible: parent.height===0?false:true

            }
            onHeightChanged: {
                //console.log("contentItem.height:",contentItem.height)
                //console.log("parent.height:",parent.height)
            }

        }
        ListModel {
            id:listModel
            ListElement {
                name: "Cut Video"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Trim&Rotate"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Add Music"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Add Subtitle"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Speed"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Video to GIF"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Conversion"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Adjust"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Stop Motion"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }

            ListElement {
                name: "Stop Motion"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
        }

        GridView {
            id:gridView
            anchors.fill: parent
            anchors.leftMargin: leftRightMargins-margins+2
            anchors.rightMargin: leftRightMargins-margins-14

            anchors.topMargin: gridTopMargin

            //height: root.contentHeight
            cellWidth: itemWidth+2*margins; cellHeight: itemHeight+2*margins


            Component {
                id: contactsDelegate

                Rectangle {
                    color : globalStyle.getColor("VideoGridItem_titleText","#0F0F10")
                    width: itemWidth+2*margins; height: itemHeight+2*margins
                    VideoGridItem{
                        anchors.fill: parent
                        anchors.margins: margins
                        titleText:name
                        contextText:number
                    }
                }

            }

            model: listModel
            delegate: contactsDelegate
            focus: true
        }
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            var obj=value;
            //console.log("onEmitQmlEvent:",eventName,value["4"][2])
        }
    }
}
