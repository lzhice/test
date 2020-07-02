import QtQuick 2.0
import QtQuick.Controls 2.5
Rectangle {
    property int margins: 14
    property int scrollBarWidth: 4
    property int gridTopMargin: 65
    property int scrollBarRightMargin: 3
    anchors.fill: quickRoot
    color : Qt.rgba(16/255,15/255,20/255,1) ;
    Flickable {
        id:root
        anchors.fill: parent

        contentHeight: gridView.contentHeight+gridTopMargin+(gridTopMargin-2*margins)// gridView.cellHeight * listModel.count/3+120
        clip: true

        Rectangle{
            anchors.top: parent.top
            anchors.right: parent.right
            anchors.bottom: parent.bottom
            anchors.rightMargin: 16-scrollBarRightMargin
            width: scrollBarWidth
            color : Qt.rgba(60/255,59/255,64/255,1) ;
        }
        ScrollBar.vertical: ScrollBar {
            anchors.top: parent.top
            anchors.right: parent.right
            anchors.bottom: parent.bottom
            anchors.rightMargin: 16-2-scrollBarRightMargin
            width: scrollBarWidth*2
            contentItem : Rectangle{
                color : Qt.rgba(132/255,111/255,248/255,1) ;
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
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Jim Williams"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "John Brown"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Bill Smyth"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Jim Williams"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "John Brown"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Bill Smyth"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
            ListElement {
                name: "Sam Wise"
                number: "Lorem ipsum dolor sit amet,consectetur adipiscing"
            }
        }

        GridView {
            id:gridView
            anchors.fill: parent
            anchors.leftMargin: 95-margins*2
            anchors.rightMargin: 95-margins*2

            anchors.topMargin: gridTopMargin

            //height: root.contentHeight
            cellWidth: 325+2*margins; cellHeight: 164+2*margins


            Component {
                id: contactsDelegate

                Rectangle {
                    color : Qt.rgba(16/255,15/255,20/255,1) ;
                    width: 325+2*margins; height: 164+2*margins
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
}
