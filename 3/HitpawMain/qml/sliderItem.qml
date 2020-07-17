import QtQuick 2.5
import QtQuick.Controls 1.4
import QtQuick.Controls.Styles 1.4
Rectangle {
    property string titleText:"Brightness"
    property string upArrow : "qrc:/img/upArrow.png"
    property string downArrow : "qrc:/img/downArrow.png"
    property color spinBoxTextColor: globalStyle.getColor("spinBoxTextColor","#E3E3E3")
    property color spinBoxSelectionTextColor: globalStyle.getColor("spinBoxSelectionTextColor","#3B3C3D")
    property color spinBoxSelectionColor: globalStyle.getColor("spinBoxSelectionColor","#F7B500")
    property color titleColor: globalStyle.getColor("SliderItemText","#979798")
    id:root
    color : "#2E2F30"
    MouseArea {
        id: dragRegion
        anchors.fill: parent
    }
    Text{
        anchors.bottom:spinBox.top
        anchors.bottomMargin: 3
        x:slider.x
        width:slider.width
        height: 13
        text: titleText +" "
        color:titleColor
        horizontalAlignment: Text.AlignLeft
        verticalAlignment: Text.AlignTop
        wrapMode:Text.Wrap
        font.pixelSize:globalStyle.getFontSize("titleText",12)
        font.weight:Font.Medium
    }
    SpinBox {
        id:spinBox
        anchors.verticalCenter: parent.verticalCenter
        anchors.right: parent.right
        //anchors.rightMargin: 10
        width: 64
        height:24
        decimals: 0
        horizontalAlignment:Qt.AlignLeft
        font.pixelSize:globalStyle.getFontSize("spinBox",12)
        //font.weight:Font.Medium
        activeFocusOnPress:false
        Rectangle{
            anchors.fill: parent
            anchors.margins: 5
            anchors.rightMargin: 18
            color: "#454649"
            Text{
                anchors.fill: parent
                text: " "+spinBox.value+" "
                color:titleColor
                horizontalAlignment: Text.AlignLeft
                verticalAlignment: Text.AlignVCenter
                wrapMode:Text.Wrap
                font.pixelSize:12
                font.weight:Font.Medium
            }
        }


        style: SpinBoxStyle{
            background: Rectangle {
                implicitWidth: 64
                implicitHeight: 24
                border.color: "#3B3C3D"
                //radius: 1
                border.width: 1
                color: "#454649"
            }
            incrementControl :Rectangle {
                implicitWidth: 18
                implicitHeight: 12
                color: "transparent"
                //radius: 1
                border.width: 0
                Image {
                    anchors.bottom: parent.bottom
                    anchors.bottomMargin: 2
                    anchors.horizontalCenter: parent.horizontalCenter
                    width: 8
                    height: 3
                    source: styleData.upHovered && !styleData.upPressed
                            ?  upArrow : (styleData.upPressed ?  upArrow :  upArrow)
                }
            }
            decrementControl :Rectangle {
                implicitWidth: 18
                implicitHeight: 12
                color: "transparent"
                BorderImage {
                    anchors.top: parent.top
                    anchors.topMargin: 2
                    anchors.horizontalCenter: parent.horizontalCenter
                    width: 8
                    height: 3
                    source: styleData.upHovered && !styleData.upPressed
                            ?  downArrow : (styleData.upPressed ?  downArrow :  downArrow)
                }
            }

            textColor: spinBoxTextColor
            selectionColor:"transparent"
            selectedTextColor:spinBoxTextColor
        }
    }
    Slider {
        id:slider

        width: 126
        height: 10
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
                implicitWidth: 5
                implicitHeight: 10
//                Rectangle{
//                    anchors.top: parent.top
//                    anchors.topMargin: -20
//                    anchors.horizontalCenter: parent.horizontalCenter
//                    width: 38
//                    height: 18
//                    color : Qt.rgba(255/255,255/255,255/255,1)
//                    Text{
//                        anchors.fill: parent
//                        text: control.value.toFixed(2);
//                        color: "black";
//                        verticalAlignment: Text.AlignVCenter
//                        horizontalAlignment: Text.AlignHCenter
//                    }
//                }

            }
            groove: Rectangle {
                implicitHeight: 2
                color: Qt.rgba(75/255,74/255,80/255,1)
                radius: 3;
                Rectangle {
                    implicitHeight: 2
                    color: Qt.rgba(119/255,104/255,195/255,1)
                    implicitWidth: styleData.handlePosition
                    radius: 3;
                }
            }
        }
    }
}
