import QtQuick 2.5
import QtQuick.Controls 1.4
import QtQuick.Controls.Styles 1.4
Rectangle {
    property color titleColor: Qt.rgba(190/255,90/255,90/255,1)
    id:root
    color : Qt.rgba(46/255,47/255,51/255,1)
    radius: 10
    MouseArea {
        id: dragRegion
        anchors.fill: parent
    }
    Text{
        anchors.bottom:slider.top
        x:slider.x
        width:slider.width
        height: 38
        text: "Brightness"
        color:titleColor
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:13
        //font.bold: true
    }
    SpinBox {
        id:spinBox
        anchors.verticalCenter: parent.verticalCenter
        anchors.right: parent.right
        width: 96
        height:32
        decimals: 2
        horizontalAlignment:Qt.AlignLeft
        style: SpinBoxStyle{
            background: Rectangle {
                implicitWidth: 96
                implicitHeight: 32
                border.color: Qt.rgba(70/255,69/255,75/255,1)
                //radius: 1
                border.width: 2
                color: Qt.rgba(62/255,61/255,67/255,1)
            }
            incrementControl :Rectangle {
                implicitWidth: 15
                implicitHeight: 20
                border.color: Qt.rgba(122/255,122/255,122/255,1)
                //radius: 1
                border.width: 1
                BorderImage {
                    anchors.fill: parent
                    source: styleData.upHovered && !styleData.upPressed
                            ?  "file:///C:/Users/Admin/Documents/untitled5/2.png" : (styleData.upPressed ?  "file:///C:/Users/Admin/Documents/untitled5/1.jpg" :  "file:///C:/Users/Admin/Documents/untitled5/1.jpg")
                }
            }
            decrementControl :Rectangle {
                implicitWidth: 15
                implicitHeight: 20
                border.color: Qt.rgba(122/255,122/255,122/255,1)
                //radius: 1
                border.width: 1
                color: Qt.rgba(46/255,47/255,51/255,1)
            }

            textColor: Qt.rgba(0/255,0/255,0/255,1)
            selectionColor:Qt.rgba(66/255,66/255,66/255,1)
            selectedTextColor:Qt.rgba(255/255,255/255,255/255,1)
        }
    }
    Slider {
        id:slider

        width: parent.width-spinBox.width-30
        height: 50
        anchors.verticalCenter: parent.verticalCenter
        anchors.left: parent.left
        anchors.leftMargin: 0
        style: SliderStyle
        {
            handle: Rectangle
            {
                anchors.centerIn: parent;
                color:control.pressed ? Qt.rgba(255/255,222/255,10/255,1):Qt.rgba(255/255,193/255,2/255,1);
                border.color: "gray";
                border.width: 0;
                width: 12;
                height: 28;
                radius: 1;
                Rectangle{
                    anchors.top: parent.top
                    anchors.topMargin: -20
                    anchors.horizontalCenter: parent.horizontalCenter
                    width: 38
                    height: 18
                    color : Qt.rgba(255/255,255/255,255/255,1)
                    Text{
                        anchors.fill: parent
                        text: control.value.toFixed(2);
                        color: "black";
                        verticalAlignment: Text.AlignVCenter
                        horizontalAlignment: Text.AlignHCenter
                    }
                }

            }
            groove: Rectangle {
                implicitHeight: 8
                color: Qt.rgba(75/255,74/255,80/255,1)
                radius: 3;
                Rectangle {
                    implicitHeight: 8
                    color: Qt.rgba(119/255,104/255,195/255,1)
                    implicitWidth: styleData.handlePosition
                    radius: 3;
                }
            }
        }
    }
}
