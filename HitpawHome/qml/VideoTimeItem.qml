import QtQuick 2.5
import QtQuick.Controls 1.4
import QtQuick.Controls.Styles 1.4

SpinBox {
    property string objectName: ""
    property int  valueLen: 2
    property string upArrow : "qrc:/img/upArrow.png"
    property string downArrow : "qrc:/img/downArrow.png"
    property color spinBoxTextColor: globalStyle.getColor("spinBoxTextColor","#E3E3E3")
    property color spinBoxSelectionTextColor: globalStyle.getColor("spinBoxSelectionTextColor","#3B3C3D")
    property color spinBoxSelectionColor: globalStyle.getColor("spinBoxSelectionColor","#F7B500")
    property color titleColor: globalStyle.getColor("SliderItemText","#979798")
    property string tmpValue:""
    property int oldValue: 0
    id:spinBox
    decimals: 0
    horizontalAlignment:Qt.AlignLeft
    font.pixelSize:globalStyle.getFontSize("spinBox",12)
    //font.weight:Font.Medium
    activeFocusOnPress:false
    onValueChanged: {
        ////console.log("VideoTimeItem onValueChanged:",spinBox.value)
        valText.text=" "+formatDate(spinBox.value)+" "
    }

    Rectangle{
        anchors.fill: parent
        anchors.margins: 5
        anchors.rightMargin: 18
        color: "#454649"
        Text{
            id: valText
            anchors.fill: parent
            //text: " "+formatDate(spinBox.value)+" "
            color:titleColor
            horizontalAlignment: Text.AlignLeft
            verticalAlignment: Text.AlignVCenter
            font.pixelSize:12
            clip: true
            MouseArea{
                id:mouse2
                acceptedButtons: Qt.LeftButton
                enabled:true
                anchors.fill: parent
                onClicked: {

//                    eventManager.sendToWidgetStart(spinBox.objectName);
//                    eventManager.addValue(spinBox.objectName,"toEditState");
//                    eventManager.addValue(spinBox.objectName,spinBox.mapToGlobal(0,0));
//                    eventManager.addValue(spinBox.objectName,spinBox.width);
//                    eventManager.addValue(spinBox.objectName,spinBox.height);
//                    eventManager.addValue(spinBox.objectName,spinBox.value);
//                    eventManager.sendToWidgetEnd(spinBox.objectName);
                    eventManager.sendToWidgetStart(spinBox.objectName);
                    eventManager.addValue(spinBox.objectName,"toEditState");
                    eventManager.addValue(spinBox.objectName,spinBox.mapToItem(quickRoot,0,0,spinBox.width,spinBox.height));
                    eventManager.addValue(spinBox.objectName,spinBox.value);
                    eventManager.sendToWidgetEnd(spinBox.objectName);

                }
            }
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
    function formatDate(val){
        spinBox.tmpValue=val
        while(spinBox.tmpValue.length<valueLen){
            spinBox.tmpValue="0"+spinBox.tmpValue
        }
        return spinBox.tmpValue
    }
    Component.onCompleted: {
        valText.text=" "+formatDate(spinBox.value)+" "
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            if(spinBox.objectName===eventName){

                spinBox.value=value
            }
        }
    }
}

