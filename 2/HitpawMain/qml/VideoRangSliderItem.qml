import QtQuick 2.12
import QtQuick.Window 2.12

Item {
    id:root
    property real millisecondTotal:5944000;

    property int curValue: 0
    property int startValue: 0
    property int endValue: 0

    property point clickPos: "0,0"
    property int actType: 0
    property int rangV:5;
    property int rangH:30;
    property int sliderHeight:90;
    property int rangMargins:30;

    property real panValue: 0
    property real rangLeftValue: 0
    property real rangRightValue: 0

    property real rangWidth:(root.width-rangMargins*2)
    property bool isSetValue: false

    onWidthChanged: {
        console.log("onWidthChanged--------------:")
        setStartValue(startValue)
        setEndValue(endValue)
        setMillisecondValue(curValue)
        timerUpdate.stop();
        timerUpdate.start();
    }
    Timer {
        id: timerUpdate;
        interval: 50;//设置定时器定时时间为500ms,默认1000ms
        repeat: false //是否重复定时,默认为false
        running: false //是否开启定时，默认是false，当为true的时候，进入此界面就开始定时
        triggeredOnStart: false // 是否开启定时就触发onTriggered，一些特殊用户可以用来设置初始值。
        onTriggered: {
            console.log("timerUpdate--------------:")
            setStartValue(startValue)
            setEndValue(endValue)
            setMillisecondValue(curValue)
        }
        //restart ,start,stop,定时器的调用方式，顾名思义


    }
    ListModel {
        id:listModel
        ListElement {
            name: "Cut Video"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "Trim&Rotate"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
        ListElement {
            name: "Add Music"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "Add Subtitle"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
        ListElement {
            name: "Speed"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "Video to GIF"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
        ListElement {
            name: "Conversion"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "Adjust"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
        ListElement {
            name: "Stop Motion"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "Sam Wise"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
        ListElement {
            name: "Jim Williams"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "John Brown"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
        ListElement {
            name: "Bill Smyth"
            pic: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        }
        ListElement {
            name: "Sam Wise"
            pic: "file:///C:/Users/Admin/Documents/untitled5/0.png"
        }
    }
    Rectangle{
        anchors.verticalCenter: parent.verticalCenter
        x:0
        width: parent.width
        height: sliderHeight
        color : Qt.rgba(245,222,222,1);
        onWidthChanged: {

        }
        clip: true
        ListView {

            anchors.fill: parent
            model: listModel
            layoutDirection: Qt.LeftToRight
            orientation:ListView.Horizontal
            delegate:Image {
                source: pic
                width: sliderHeight
                height: sliderHeight
            }
        }
        MouseArea {
            anchors.fill: parent
        }
    }

    Item{
        anchors.fill: parent
        Rectangle {
            id:r1
            x:0
            y:0
            width: r5.x
            height: r5.y
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }
        Rectangle {
            id:r2
            x:r5.x
            y:0
            width: r5.width
            height: r5.y
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }
        Rectangle {
            id:r3
            x:r5.x+r5.width
            y:0
            width: parent.width-(r5.x+r5.width)
            height: r5.y
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }
        Rectangle {
            id:r4
            x:0
            y:r5.y
            width: r5.x
            height: r5.height
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }

        Rectangle {
            id:r6
            x:r3.x
            y:r5.y
            width: r3.width
            height: r5.height
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }
        Rectangle {
            id:r7
            x:0
            y:r5.y+r5.height
            width: r5.x
            height: parent.height-y
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }
        Rectangle {
            id:r8
            x:r5.x
            y:r5.y+r5.height
            width: r5.width
            height: parent.height-y
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }
        Rectangle {
            id:r9
            x:r3.x
            y:r5.y+r5.height
            width: r3.width
            height: parent.height-y
            visible: true
            color : Qt.rgba(0,0,0,0.5);
        }

        Rectangle {

            id:r5
            //anchors.horizontalCenter : parent.horizontalCenter
            anchors.verticalCenter: parent.verticalCenter
            x:0
            width: rangMargins*2+2
            height: sliderHeight
            color : Qt.rgba(0,0,0,0.0);
            onXChanged: {
                if(!isSetValue){
                    startValue=x/rangWidth*millisecondTotal
                    console.log("r5 onXChanged1----------------:",endValue);
                    endValue=(x+r5.width-2*rangMargins)/rangWidth*millisecondTotal
                    console.log("r5 onXChanged2----------------:",endValue);
                }isSetValue=false
            }
            BorderImage {
                anchors.fill: parent
                source: "file:///C:/Users/Admin/Documents/untitled5/item.png"
                border.bottom: 20
                border.left: 30
                border.right: 30
                border.top: 20
            }
            MouseArea {
                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.OpenHandCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }

                onPressed: {
                    if(cursorShape===Qt.OpenHandCursor){
                        actType=-1
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }
                }
                onReleased: {

                    if(actType!==1){
                        var mouseItem=r5.mapToItem(root,mouse.x,mouse.y)
                        var delta = Qt.point( mouseItem.x-progressPin.x, mouseItem.y-progressPin.y)
                        if(progressPin.x+delta.x>=r5.x&&(progressPin.x+delta.x+progressPin.width)<=(r5.x+r5.width)){
                            //console.log("progress  0")
                            progressPin.x=(progressPin.x+delta.x)
                        }else{
                            if(progressPin.x+delta.x<r5.x){
                                //console.log("progress  1")
                                progressPin.x=r5.x
                            }
                            if((progressPin.x+delta.x+progressPin.width)>(r5.x+r5.width)){
                                //console.log("progress  2")
                                progressPin.x=(r5.x+r5.width)-progressPin.width
                            }
                        }
                    }
                    actType=0
                }
                onPositionChanged: {
                    //鼠标偏移量
                    // //console.log("onPositionChanged:",mouse.y)
                    //if(cursorShape!==Qt.OpenHandCursor)cursorShape=Qt.OpenHandCursor
                    var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                    if(mouse.x>root.width)return
                    if(actType===-1){
                        //console.log("delta.x",Math.abs(delta.x))
                        if(Math.abs(delta.x)>5){
                            actType=1
                        }
                    }
                    if(actType===1){
                        var r5Oldx=r5.x
                        var r5Oldw=r5.width
                        if(r5.x+delta.x>=0&&(r5.x+delta.x+r5.width)<=root.width){
                            r5.x=(r5.x+delta.x)
                        }else{
                            if(r5.x+delta.x<0){
                                r5.x=0
                            }
                            if((r5.x+delta.x+r5.width)>root.width){
                                r5.x=root.width-r5.width
                            }
                        }
                        if(r5Oldx!==r5.x||r5Oldw!==r5.width)
                            progressPin.x=r5.x+rangMargins
                    }
                }
            }
            //-----------------------------------------------------------

            Rectangle {
                id:r5_right
                x:r5.width-width
                y:0
                width: rangH
                height: r5.height
                color : Qt.rgba(122,33,0,0);
                onXChanged: {
                    if(!isSetValue){
                        console.log("r5_right onXChanged1----------------:",endValue);
                        endValue=(x+r5.x-rangMargins)/rangWidth*millisecondTotal
                        console.log("r5_right onXChanged1----------------:",endValue);
                    }isSetValue=false
                }
                Text{
                    y:parent.height/3*2+8
                    anchors.left: (endValue/millisecondTotal)<=0.5 ? r5_right.right : undefined
                    anchors.right: (endValue/millisecondTotal)>0.5 ? r5_right.left : undefined
                    width:300
                    height: 20
                    text:  " " +millisecondToDate(endValue) + " "
                    color:Qt.rgba(1/255,255/255,255/255,1);
                    horizontalAlignment: (endValue/millisecondTotal)<0.5 ?Text.AlignLeft : Text.AlignRight
                    verticalAlignment: Text.AlignVCenter
                    wrapMode:Text.Wrap
                    font.pixelSize:11
                }
                MouseArea {
                    anchors.fill: parent
                    hoverEnabled:true
                    onEntered: {
                        cursorShape=Qt.SizeHorCursor
                    }
                    onExited: {
                        cursorShape=Qt.ArrowCursor
                    }
                    onPressed: {
                        if(cursorShape===Qt.SizeHorCursor){
                            actType=2
                            clickPos  = Qt.point(mouse.x,mouse.y)
                        }
                    }
                    onReleased: {
                        actType=0
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        if(actType===2){
                            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)

                            if((r5.x+delta.x+r5.width)<=root.width){
                                if((r5.width+delta.x)<rangH*2){
                                    r5.width=rangH*2;
                                }else{
                                    r5.width+=delta.x
                                }
                            }else{
                                r5.width=root.width-r5.x
                            }
                            progressPin.x=r5.x+r5.width-rangMargins
                        }
                    }
                }
            }
            Rectangle {
                id:r5_left
                x:0
                y:0
                width: rangH
                height: r5.height
                color : Qt.rgba(122,33,0,0);

                Text{
                    y:parent.height/3*2-2
                    anchors.right: (r5.x/rangWidth)>=0.5 ? r5_left.left : undefined
                    anchors.left: (r5.x/rangWidth)<0.5 ? r5_left.right : undefined
                    width:300
                    height: 20
                    text:  (r5.x/rangWidth)>=0.5 ?" " +millisecondToDate(startValue) + " " : " " + millisecondToDate(startValue)+ " "
                    color:Qt.rgba(1/255,255/255,255/255,1);
                    horizontalAlignment: (r5.x/rangWidth)<0.5 ?Text.AlignLeft : Text.AlignRight
                    verticalAlignment: Text.AlignVCenter
                    wrapMode:Text.Wrap
                    font.pixelSize:11
                }
                MouseArea {
                    anchors.fill: parent
                    hoverEnabled:true
                    onEntered: {
                        cursorShape=Qt.SizeHorCursor
                    }
                    onExited: {
                        cursorShape=Qt.ArrowCursor
                    }
                    onPressed: {
                        if(cursorShape===Qt.SizeHorCursor){
                            actType=2
                            clickPos  = Qt.point(mouse.x,mouse.y)
                        }
                    }
                    onReleased: {
                        actType=0
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        if(actType===2){
                            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                            if(r5.x+delta.x>=0){
                                if((r5.width-delta.x)<rangH*2){
                                    r5.x=r5.x+r5.width-rangH*2;
                                    r5.width=rangH*2;
                                }else{
                                    r5.x=(r5.x+delta.x)
                                    r5.width-=delta.x
                                }
                            }else{
                                if(r5.x+delta.x<0){
                                    r5.width-=-r5.x;
                                    r5.x=0
                                }
                            }
                            progressPin.x=r5.x+rangMargins
                        }
                    }
                }
            }

        }

        Rectangle{
            id:progressPin
            anchors.verticalCenter: parent.verticalCenter
            x:rangMargins
            width: 1
            height: sliderHeight+40
            color : Qt.rgba(245,222,0,0);
            onXChanged: {
                //console.log("onXChanged: isSetValue",isSetValue)
                if(!isSetValue){
                    if(x<=r5.x+rangMargins){
                        //console.log("setMillisecondValue(startValue)",startValue)
                        setMillisecondValue(startValue)
                    }else{
                        console.log("setMillisecondValue(endValue)",x,r5.x+r5.width-rangMargins);
                        if(x+width>=r5.x+r5.width-rangMargins){
                            //console.log("setMillisecondValue(endValue)",endValue)
                            setMillisecondValue(endValue)
                        }else{
                            var panPos=x-rangMargins
                            panValue=panPos/rangWidth
                            curValue=panValue*millisecondTotal
                        }
                    }
                }isSetValue=false
            }
            Rectangle{
                id:pin
                anchors.horizontalCenter: parent.horizontalCenter
                width: 1
                height: sliderHeight+40
                color : Qt.rgba(255/255,90/255,0,1);
            }
            MouseArea {
                property point clickPos: "0,0"
                property int actType: 0

                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeHorCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }

                onPressed: {
                    if(cursorShape===Qt.SizeHorCursor){
                        actType=1
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }

                }
                onReleased: {
                    actType=0
                }

                onPositionChanged: {

                    //鼠标偏移量
                    var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                    if(actType===1){
                        if(progressPin.x+delta.x>=(r5.x+rangMargins)&&(progressPin.x+delta.x+progressPin.width)<=((r5.x+rangMargins)+(r5.width-rangMargins*2))){
                            progressPin.x=(progressPin.x+delta.x)
                        }else{
                            if(progressPin.x+delta.x<(r5.x+rangMargins)){
                                progressPin.x=(r5.x+rangMargins)
                            }
                            if((progressPin.x+delta.x+progressPin.width)>((r5.x+rangMargins)+(r5.width-rangMargins*2))){
                                progressPin.x=((r5.x+rangMargins)+(r5.width-rangMargins*2))-progressPin.width
                            }
                        }
                    }
                }
            }
        }
        Rectangle{
            id:progressPinHandle
            anchors.horizontalCenter:progressPin.horizontalCenter
            anchors.bottom: progressPin.top
            anchors.bottomMargin: -10
            width: 20
            height: 20
            color : Qt.rgba(245,222,0,0);

            Rectangle{
                anchors.horizontalCenter:parent.horizontalCenter
                anchors.bottom: parent.bottom
                width: 7
                height: 15
                color : Qt.rgba(255/255,90/255,0,1);
            }
            Text{
                anchors.right: panValue>=0.5 ? progressPinHandle.left : undefined
                anchors.left: panValue<0.5 ? progressPinHandle.right : undefined
                anchors.rightMargin: panValue>=0.5 ? 15 : undefined
                anchors.leftMargin: panValue<0.5 ?15 : undefined
                width:300
                height: 20
                text:  panValue>=0.5 ?" " +millisecondToDate(curValue) + " " : " " + millisecondToDate(curValue)+ " "
                color:Qt.rgba(139/255,163/255,60/255,1);
                horizontalAlignment: panValue<0.5 ?Text.AlignLeft : Text.AlignRight
                verticalAlignment: Text.AlignVCenter
                wrapMode:Text.Wrap
                font.pixelSize:13
            }
            MouseArea {
                property point clickPos: "0,0"
                property int actType: 0

                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeHorCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }

                onPressed: {
                    if(cursorShape===Qt.SizeHorCursor){
                        actType=1
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }

                }
                onReleased: {
                    actType=0
                }

                onPositionChanged: {
                    //鼠标偏移量
                    if(r5.width===rangMargins*2) return;
                    var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                    if(actType===1){
                        if(progressPin.x+delta.x>=(r5.x+rangMargins)&&(progressPin.x+delta.x+progressPin.width)<((r5.x+rangMargins)+(r5.width-rangMargins*2))){
                            progressPin.x=(progressPin.x+delta.x)
                        }else{
                            if(progressPin.x+delta.x<(r5.x+rangMargins)){
                                progressPin.x=(r5.x+rangMargins)
                            }
                            if((progressPin.x+delta.x+progressPin.width)>=((r5.x+rangMargins)+(r5.width-rangMargins*2))){
                                progressPin.x=((r5.x+rangMargins)+(r5.width-rangMargins*2))-progressPin.width
                            }
                        }
                    }
                }
            }
        }

        Rectangle{
            anchors.centerIn: progressPin
            width: 8
            height: progressPin.height
            color : Qt.rgba(0,222,0,0);
            MouseArea {
                property point clickPos: "0,0"
                property int actType: 0

                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeHorCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }

                onPressed: {
                    if(cursorShape===Qt.SizeHorCursor){
                        actType=1
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }

                }
                onReleased: {
                    actType=0
                }

                onPositionChanged: {
                    //鼠标偏移量
                    if(r5.width===rangMargins*2) return;
                    var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                    if(actType===1){
                        if(progressPin.x+delta.x>=(r5.x+rangMargins)&&(progressPin.x+delta.x+progressPin.width)<((r5.x+rangMargins)+(r5.width-rangMargins*2))){
                            progressPin.x=(progressPin.x+delta.x)
                        }else{
                            if(progressPin.x+delta.x<(r5.x+rangMargins)){
                                progressPin.x=(r5.x+rangMargins)
                            }
                            if((progressPin.x+delta.x+progressPin.width)>((r5.x+rangMargins)+(r5.width-rangMargins*2))){
                                progressPin.x=((r5.x+rangMargins)+(r5.width-rangMargins*2))-progressPin.width
                                console.log("progressPin.x",progressPin.x)
                                console.log("onPositionChanged",r5.x+r5.width-rangMargins);
                            }
                        }
                    }
                }
            }
        }
    }
    //    function millisecondToDate(msd) {
    //        var time = parseFloat(msd) / 1000;
    //        if (null !== time && "" !== time) {
    //            if (time > 60 && time < 60 * 60) {
    //                time = parseInt(time / 60.0) + "分钟" + parseInt((parseFloat(time / 60.0) -
    //                    parseInt(time / 60.0)) * 60)// + "秒";
    //            }
    //            else if (time >= 60 * 60 && time < 60 * 60 * 24) {
    //                time = parseInt(time / 3600.0) + "小时" + parseInt((parseFloat(time / 3600.0) -
    //                    parseInt(time / 3600.0)) * 60) + "分钟" +
    //                    parseInt((parseFloat((parseFloat(time / 3600.0) - parseInt(time / 3600.0)) * 60) -
    //                    parseInt((parseFloat(time / 3600.0) - parseInt(time / 3600.0)) * 60)) * 60)// + "秒";
    //            }
    //            else {
    //                time = parseInt(time)// + "秒";
    //            }
    //        }
    //        return time;
    //    }

    function setStartValue(val){
        console.log("setStartValue------------------:",val)

        if(val>=endValue){
            val=endValue-10
        }
        if(val>millisecondTotal||val<0){
            return
        }
        startValue=val
        var rightOldx=r5.x+r5.width
        var tmps=(startValue/millisecondTotal)
        isSetValue=true
        r5.x=(tmps*rangWidth).toFixed(0)
        isSetValue=true
        r5.width=rightOldx-r5.x
        isSetValue=false

        setMillisecondValue(val)
    }
    function setEndValue(val){
        console.log("setEndValue------------------:",val)
        if(val<=startValue){
            val=startValue+10
        }
        if(val>millisecondTotal||val<0){
            return
        }
        endValue=val
        isSetValue=true
        var rightx=((endValue/millisecondTotal)*rangWidth+2*rangMargins).toFixed(0)
        r5.width=rightx-r5.x
        isSetValue=false
        setMillisecondValue(val)
    }
    function setMillisecondValue(val){
        if(val>millisecondTotal||val<0){
            return
        }
        console.log("setMillisecondValue------------------:",val,curValue)
        curValue=val
        panValue=curValue/millisecondTotal
        isSetValue=true
        progressPin.x= (panValue*rangWidth+rangMargins).toFixed(0)
        isSetValue=false

    }

    function formatDate(val){
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
        var tmpTime=parseInt((time-parseInt(time))*60)
        //console.log("millisecondToDate---------------:",tmpTime)
        if (null !== time && "" !== time) {
            if (time > 60 && time < 60 * 60) {
                time = "00:" +formatDate(parseInt(time / 60.0)) + ":" + formatDate(parseInt((parseFloat(time / 60.0) -
                                                                                             parseInt(time / 60.0)) * 60))
            }
            else if (time >= 60 * 60 && time < 60 * 60 * 24) {
                time = formatDate(parseInt(time / 3600.0)) + ":" + formatDate(parseInt((parseFloat(time / 3600.0) -
                                                                                        parseInt(time / 3600.0)) * 60)) + ":" +
                        formatDate(parseInt((parseFloat((parseFloat(time / 3600.0) - parseInt(time / 3600.0)) * 60) -
                                             parseInt((parseFloat(time / 3600.0) - parseInt(time / 3600.0)) * 60)) * 60))
            }
            else {
                time = "00:00:" +formatDate(parseInt(time))
            }
        }

        return time+ ":" +formatDate(parseInt(tmpTime));
    }
    Connections
    {
        target:eventManager
        onEmitQmlEvent:
        {
            var obj=value;
            //console.log("VideoRangSliderItem onEmitQmlEvent:",eventName,curValue["1"])

            setEndValue(value["1"])

        }
    }
}
