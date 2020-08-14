import QtQuick 2.12
import QtGraphicalEffects 1.12
import QtQuick.Controls 2.5
Rectangle{
    property string selectbuttonText: ""
    property color splitLineColor: "#000000"
    property bool isSetTime:false
    property int maxValue: 8888889
    property int curStartValue: 0
    property int curEndValue: 0

    onCurStartValueChanged: {
        timerUpdate.stop();
        timerUpdate.start();
    }
    onCurEndValueChanged: {
        timerUpdate.stop();
        timerUpdate.start();
    }
    Timer {
        id: timerUpdate;
        interval: 100;//设置定时器定时时间为500ms,默认1000ms
        repeat:false //是否重复定时,默认为false
        running:false //是否开启定时，默认是false，当为true的时候，进入此界面就开始定时
        triggeredOnStart:false// 是否开启定时就触发onTriggered，一些特殊用户可以用来设置初始值。
        onTriggered: {
            timeUpdate();
        }
    }
    id:root
    anchors.fill: quickRoot
    radius: 10
    color : "#2E2F30"
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
        anchors.topMargin: 0
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 20+16+24+8
        Rectangle{
            anchors.fill: parent
            color: "#2E2F30"

            Text{
                id:text1
                anchors.left: parent.left
                anchors.top: parent.top
                anchors.topMargin: 20
                height: 8
                width: parent.width
                text: qsTr("Start")
                color:"#E3E3E3"
                horizontalAlignment: Text.AlignLeft
                verticalAlignment: Text.AlignVCenter
                wrapMode:Text.Wrap
                font.pixelSize:globalStyle.getFontSize("TextButton_textSize",12)
            }
            Item{
                anchors.top: text1.bottom
                anchors.topMargin: 10
                anchors.left: parent.left
                anchors.right: parent.right
                height: 24
                VideoTimeItem{
                    id : startMinute
                    objectName:"startMinute"
                    anchors.top: parent.top
                    anchors.left: parent.left
                    anchors.leftMargin: 0
                    height: 24
                    width: 64
                    minimumValue:0
                    maximumValue:999
                    valueLen:3
                    onValueChanged: {
                        if(checkStartUpdate()){
                            oldValue=value
                        }else{
                            value=oldValue
                        }
                    }
                }
                VideoTimeItem{
                    id : startSecond
                    objectName:"startSecond"
                    anchors.top: parent.top
                    anchors.left: startMinute.right
                    anchors.leftMargin: 4
                    height: 24
                    width: 64
                    minimumValue:0
                    maximumValue:9999999
                    valueLen:2
                    onValueChanged: {
                        if(checkStartUpdate()){
                            var nMinute = parseFloat(value) / 60
                            value=value-parseInt(nMinute)*60
                            startMinute.value+=parseInt(nMinute)
                            oldValue=value
                        }else{
                            value=oldValue
                        }
                    }
                }
                VideoTimeItem{
                    id : startMillisecond
                    objectName:"startMillisecond"
                    anchors.top: parent.top
                    anchors.left: startSecond.right
                    anchors.leftMargin: 4
                    height: 24
                    width: 64
                    minimumValue:0
                    maximumValue:9999999
                    valueLen:3
                    stepSize:100
                    onValueChanged: {
                        if(checkStartUpdate()){
                            var nSecond = parseFloat(value) / 1000
                            value=value-parseInt(nSecond)*1000
                            startSecond.value+=parseInt(nSecond)
                            oldValue=value
                        }else{
                            value=oldValue
                        }
                    }
                }
            }
        }
    }

    Item {
        id: row4
        anchors.top: row3.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 0
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 20+16+24+8
        Rectangle{
            anchors.fill: parent
            color: "#2E2F30"

            Text{
                id:text2
                anchors.left: parent.left
                anchors.top: parent.top
                anchors.topMargin: 20
                height: 8
                width: parent.width
                text: qsTr("End")
                color:"#E3E3E3"
                horizontalAlignment: Text.AlignLeft
                verticalAlignment: Text.AlignVCenter
                wrapMode:Text.Wrap
                font.pixelSize:globalStyle.getFontSize("TextButton_textSize",12)
            }
            Item{
                anchors.top: text2.bottom
                anchors.topMargin: 10
                anchors.left: parent.left
                anchors.right: parent.right
                height: 24
                VideoTimeItem{
                    id : endMinute
                    objectName:"endMinute"
                    anchors.top: parent.top
                    anchors.left: parent.left
                    anchors.leftMargin: 0
                    height: 24
                    width: 64
                    minimumValue:0
                    maximumValue:999
                    valueLen:3
                    onValueChanged: {
                        if(checkEndUpdate()){
                            oldValue=value
                        }else{
                            value=oldValue
                        }
                    }
                }
                VideoTimeItem{
                    id : endSecond
                    objectName:"endSecond"
                    anchors.top: parent.top
                    anchors.left: endMinute.right
                    anchors.leftMargin: 4
                    height: 24
                    width: 64
                    minimumValue:0
                    maximumValue:1000*60
                    valueLen:2
                    onValueChanged: {
                        if(checkEndUpdate()){
                            var nMinute = parseFloat(value) / 60
                            value=value-parseInt(nMinute)*60
                            endMinute.value+=parseInt(nMinute)
                        }else{
                            value=oldValue
                        }

                    }
                }
                VideoTimeItem{
                    id : endMillisecond
                    objectName:"endMillisecond"
                    anchors.top: parent.top
                    anchors.left: endSecond.right
                    anchors.leftMargin: 4
                    height: 24
                    width: 64
                    minimumValue:0
                    maximumValue:1000*60*1000
                    valueLen:3
                    stepSize:100
                    onValueChanged: {
                        if(checkEndUpdate()){
                            var nSecond = parseFloat(value) / 1000
                            value=value-parseInt(nSecond)*1000
                            endSecond.value+=parseInt(nSecond)
                        }else{
                            value=oldValue
                        }

                    }
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
    function checkStartUpdate() {
        var val=(startMinute.value*60+startSecond.value)*1000+startMillisecond.value
        var valEnd=(endMinute.value*60+endSecond.value)*1000+endMillisecond.value
        if(val<=maxValue&&val<=valEnd-10){
            curStartValue=val
            return true;
        }else{
            return false;
        }
    }
    function checkEndUpdate() {
        var val=(endMinute.value*60+endSecond.value)*1000+endMillisecond.value
        if(val<=maxValue){
            curEndValue=val
            return true;
        }else{
            return false;
        }
    }
    function timeUpdate() {
        //console.log("timeUpdate--------------------------------------:",curStartValue,curEndValue);
        eventManager.sendToWidgetStart("VideoCutSetView_timeUpdate");
        eventManager.addValue("VideoCutSetView_timeUpdate",curStartValue);
        eventManager.addValue("VideoCutSetView_timeUpdate",curEndValue);
        eventManager.sendToWidgetEnd("VideoCutSetView_timeUpdate");
    }
    function setStartTime(nMillisecond) {
        startMinute.value=0
        startSecond.value=0
        startMillisecond.value=0
        startMillisecond.value=nMillisecond
    }
    function setEndTime(nMillisecond) {
        endMinute.value=0
        endSecond.value=0
        endMillisecond.value=0
        endMillisecond.value=nMillisecond
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            if("VideoCutSetView"===eventName){
                if(value["event"]==="setMaxValue"){
                    maxValue=value["value"]
                    console.log("VideoCutSetView onEmitQmlEvent:",maxValue)
                }else if(value["event"]==="setStartTime"){
                    setStartTime(value["value"])
                }else if(value["event"]==="setEndTime"){
                    setEndTime(value["value"])
                }

            }
        }
    }
}
