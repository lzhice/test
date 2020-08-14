import QtQuick 2.5
import QtQuick.Controls 1.4
import QtQuick.Controls.Styles 1.4
Rectangle {
    property real millisecondTotal:00;
    property int curValue: 0
    property string playerStatePic: "qrc:/img/stop.png"
    property string curTimeText:""
    property string curTotalTimeText:""
    onCurValueChanged: {
        curTimeText=millisecondToDate(curValue)
    }

    anchors.fill: quickRoot
    color : "#2e2f30"
    radius: 10
    Item{
        anchors.fill: parent
        Image {
            id: playButton
            anchors.left: parent.left
            anchors.leftMargin: 15
            anchors.verticalCenter: parent.verticalCenter
            source: playerStatePic
            width: 40; height: 40
            Rectangle{
                id:effectItem
                anchors.fill: parent
                visible: false
                //radius: 12
                clip:true
                color : Qt.rgba(0.6,0.6,0.6,0.8) ;
            }
            MouseArea {
                id:mouseArea
                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                }
                onExited: {
                    effectItem.visible=false
                }
                onClicked: {
                    eventManager.sendToWidget("VideoPlaySliderItem_PlayButton","onClicked")
                }
                onPressed: {
                    effectItem.visible=false
                }
                onReleased: {
                    effectItem.visible=false
                }
            }
        }
        Item {
            id: timeItem
            anchors.left: playButton.right
            anchors.leftMargin: 9
            anchors.verticalCenter: parent.verticalCenter
            width: 155
            height: 15
            Text{
                id:textTime
                text:" "+curTimeText +" / " +curTotalTimeText+" ";
                color:"#E3E3E3"
                horizontalAlignment: Text.AlignHCenter
                verticalAlignment: Text.AlignVCenter
                font.pixelSize:12
            }
        }
        Item {
            id: playSlider
            anchors.left: timeItem.right
            anchors.leftMargin: 10
            anchors.verticalCenter: parent.verticalCenter
            anchors.right: parent.right
            anchors.rightMargin: 20
            height: 20
            Slider {
                id:slider
                anchors.fill: parent
                minimumValue: 0;
                maximumValue: millisecondTotal;
                stepSize: 1;
                value: curValue;
                onValueChanged:{
                    curValue=value
                }

                style: SliderStyle
                {
                    handle: Rectangle
                    {
                        anchors.centerIn: parent;
                        color:control.pressed ? Qt.rgba(255/255,222/255,10/255,1):Qt.rgba(255/255,193/255,2/255,1);
                        border.color: "gray";
                        border.width: 0;
                        implicitWidth: 12
                        implicitHeight: 12
                        radius: 12;
                    }
                    groove: Rectangle {
                        implicitHeight: 4
                        color: Qt.rgba(75/255,74/255,80/255,1)
                        radius: 3;
                        Rectangle {
                            implicitHeight: 4
                            color: Qt.rgba(119/255,104/255,195/255,1)
                            implicitWidth: styleData.handlePosition
                            radius: 3;
                        }
                    }
                }
            }
        }
    }
    Rectangle {
        id:rightBottom
        anchors.bottom: parent.bottom
        anchors.right: parent.right
        width: 10
        height: 10
        radius: 0
        color : "#2e2f30"
    }
    function formatDate(val){
        if(val<0){
            val=0;
        }
        if(val<10){
            return "00"+val
        }else if(val<100){
            return "0"+val
        }else{
            return val
        }
    }
    function formatDate2(val){
        if(val<0){
            val=0;
        }
        if(val<10){
            return "0"+val
        }else{
            return val
        }
    }
    function millisecondToDate(msd) {
        var time = parseFloat(msd) / 1000;
        var tmpTime=parseInt(msd-parseInt(time)*1000)
        if (null !== time && "" !== time) {
            if (time > 60 ) {
                time = formatDate(parseInt(time / 60.0)) + ":" + formatDate2(parseInt((parseFloat(time / 60.0) -
                                                                                       parseInt(time / 60.0)) * 60))
            }else {
                time = "000:" +formatDate2(parseInt(time))
            }
        }
        return time+ ":" +formatDate(parseInt(tmpTime));
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            if("VideoPlaySliderItem"===eventName){
                if(value["event"]==="setMaxValue"){
                    millisecondTotal=value["value"]
                    curTimeText="000:00:000"
                    curTotalTimeText=millisecondToDate(millisecondTotal)
                }else if(value["event"]==="curValue"){
                    curValue=value["value"]
                }else if(value["event"]==="playState"){
                    console.log("VideoPlaySliderItem onEmitQmlEvent: playState",value["value"])
                    if(value["value"]){
                        playerStatePic="qrc:/img/stop.png"
                    }else{
                        playerStatePic="qrc:/img/play.png"
                    }
                }
            }
        }
    }
}
