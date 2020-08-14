import QtQuick 2.12
import QtShark.Images 1.0
Rectangle{
    property string sliderHandSource:"qrc:/img/sliderHand@1x.png"
    property string playerStatePic: "qrc:/img/stop.png"
    property real millisecondTotal:8888889;

    property int curValue: 0
    property int startValue: 0
    property int endValue: 0

    property point clickPos: "0,0"
    property int actType: 0
    property int rangV:5;
    property int rangH:rangMargins;
    property int sliderHeight:50;
    property int sliderHeightMargins:5;
    property int rangMargins:14;

    property real panValue: 0
    property real rangLeftValue: 0
    property real rangRightValue: 0

    property real rangWidth:(rootVideoRang.width-rangMargins*2)
    property bool isSetValue: false
    property bool isSizeChanged: false
    property bool isPressState: false
    onIsPressStateChanged: {
        if(!isPressState){
            eventManager.sendToWidgetStart("VideoRangSliderItem");
            eventManager.addValue("VideoRangSliderItem","isPressState");
            eventManager.addValue("VideoRangSliderItem",false);
            eventManager.sendToWidgetEnd("VideoRangSliderItem");
        }
    }

    onCurValueChanged: {
        if(!isSizeChanged){
            eventManager.sendToWidgetStart("VideoRangSliderItem");
            eventManager.addValue("VideoRangSliderItem","curValue");
            eventManager.addValue("VideoRangSliderItem",curValue);
            eventManager.sendToWidgetEnd("VideoRangSliderItem");
        }
    }
    onStartValueChanged: {
        if(!isSizeChanged){
            eventManager.sendToWidgetStart("VideoRangSliderItem");
            eventManager.addValue("VideoRangSliderItem","startValue");
            eventManager.addValue("VideoRangSliderItem",startValue);
            eventManager.sendToWidgetEnd("VideoRangSliderItem");
            updateTimeText();
        }
    }
    onEndValueChanged: {
        if(!isSizeChanged){
            eventManager.sendToWidgetStart("VideoRangSliderItem");
            eventManager.addValue("VideoRangSliderItem","endValue");
            eventManager.addValue("VideoRangSliderItem",endValue);
            eventManager.sendToWidgetEnd("VideoRangSliderItem");
            updateTimeText();
        }
    }
    id:root
    color : "transparent"
    Image {
        id: playButton
        anchors.left: parent.left
        anchors.leftMargin: (rootVideoRang.anchors.leftMargin-width)/2
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
                eventManager.sendToWidget("VideoRangSliderItem_PlayButton","onClicked")
            }
            onPressed: {
                effectItem.visible=false
            }
            onReleased: {
                effectItem.visible=false
            }
        }
    }
    Rectangle {
        id:rootVideoRang
        anchors.right: parent.right
        anchors.top: parent.top
        anchors.bottom: parent.bottom
        anchors.left: parent.left
        anchors.leftMargin: 64-5
        color : "transparent"
        onWidthChanged: {
            isSizeChanged=true;
            //////console.log("onWidthChanged--------------:")
            var tmcurValue=curValue;
            setStartValue(startValue)
            setEndValue(endValue)
            setMillisecondValue(tmcurValue)
            timerUpdate.stop();
            timerUpdate.start();
            isSizeChanged=false;
        }
        Timer {
            id: timerUpdate;
            interval: 50;//设置定时器定时时间为500ms,默认1000ms
            repeat:false //是否重复定时,默认为false
            running:false //是否开启定时，默认是false，当为true的时候，进入此界面就开始定时
            triggeredOnStart:false// 是否开启定时就触发onTriggered，一些特殊用户可以用来设置初始值。
            onTriggered: {
                ////console.log("timerUpdate--------------:")
                isSizeChanged=true;
                var tmcurValue=curValue;
                setStartValue(startValue)
                setEndValue(endValue)
                setMillisecondValue(tmcurValue)
                isSizeChanged=false;
            }
            //restart ,start,stop,定时器的调用方式，顾名思义
        }
        Rectangle{
            anchors.verticalCenter: parent.verticalCenter
            anchors.left: parent.left
            anchors.right: parent.right
            anchors.leftMargin:rangMargins
            anchors.rightMargin:rangMargins
            height: sliderHeight
            color : "transparent"
            clip: true
            onWidthChanged: {

            }
            ListView {
                anchors.left: parent.left
                anchors.right: parent.right
                anchors.top: parent.top
                anchors.bottom: parent.bottom
                anchors.topMargin: 5
                anchors.bottomMargin: 5
                model: listModel
                layoutDirection: Qt.LeftToRight
                orientation:ListView.Horizontal
                onWidthChanged: {
                    console.log("VideoRangSliderItem ListView onWidthChanged:",width/(sliderHeight-10))
                    var count=width/(sliderHeight-10)
                    model=count
                    eventManager.sendToWidgetStart("videoThumbUpdate");
                    eventManager.addValue("videoThumbUpdate",count);
                    eventManager.addValue("videoThumbUpdate",(sliderHeight-10));
                    eventManager.sendToWidgetEnd("videoThumbUpdate");
                }
                delegate:Image {
                    source: QtImages.load("videoThumb_"+index, "");
                    width: sliderHeight-10
                    height: sliderHeight-10
                }
            }
            MouseArea {
                anchors.fill: parent
            }
        }

        Rectangle{
            anchors.fill: parent
            color : "transparent"
            Rectangle {
                id:r1
                x:0
                y:0
                width: r5.x
                height: r5.y
                visible: true
                color : "transparent"
            }
            Rectangle {
                id:r2
                x:r5.x
                y:0
                width: r5.width
                height: r5.y
                visible: true
                color : "transparent"
            }
            Rectangle {
                id:r3
                x:r5.x+r5.width
                y:0
                width: parent.width-(r5.x+r5.width)-rangMargins
                height: r5.y
                visible: true
                color : "transparent"
            }
            Rectangle {
                id:r4
                x:rangMargins
                y:r5.y
                width: r5.x
                height: r5.height
                visible: true
                color : Qt.rgba(0,0,0,0.5)
            }

            Rectangle {
                id:r6
                x:r3.x
                y:r5.y
                width: r3.width
                height: r5.height
                visible: true
                color : Qt.rgba(0,0,0,0.5)
            }
            Rectangle {
                id:r7
                x:0
                y:r5.y+r5.height
                width: r5.x
                height: parent.height-y
                visible: true
                color : "transparent"
            }
            Rectangle {
                id:r8
                x:r5.x
                y:r5.y+r5.height
                width: r5.width
                height: parent.height-y
                visible: true
                color : "transparent"
            }
            Rectangle {
                id:r9
                x:r3.x
                y:r5.y+r5.height
                width: r3.width
                height: parent.height-y
                visible: true
                color : "transparent"
            }

            Rectangle {
                id:r5
                //anchors.horizontalCenter : parent.horizontalCenter
                anchors.verticalCenter: parent.verticalCenter
                x:0
                width: rangMargins*2+2
                height: sliderHeight
                color : "transparent"
                onXChanged: {
                    if(!isSetValue){
                        startValue=x/rangWidth*millisecondTotal
                        ////console.log("r5 onXChanged1----------------:",endValue);
                        endValue=(x+r5.width-2*rangMargins)/rangWidth*millisecondTotal
                        ////console.log("r5 onXChanged2----------------:",endValue);
                    }isSetValue=false
                }
                Rectangle {
                    id:r5_left_shadow
                    anchors.left: parent.left
                    anchors.top: parent.top
                    width: rangMargins/2
                    height: sliderHeight
                    color : Qt.rgba(0,0,0,0.5);
                }
                Rectangle {
                    id:r5_right_shadow
                    anchors.right: parent.right
                    anchors.top: parent.top
                    width: rangMargins/2
                    height: sliderHeight
                    color : Qt.rgba(0,0,0,0.5);
                }
                BorderImage {
                    anchors.fill: parent
                    source: sliderHandSource
                    border.bottom: sliderHeightMargins
                    border.left: rangMargins+1
                    border.right: rangMargins+1
                    border.top: sliderHeightMargins
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
                        isPressState=true
                    }
                    onReleased: {
                        if(actType!==1){
                            var mouseItem=r5.mapToItem(rootVideoRang,mouse.x,mouse.y)
                            var delta = Qt.point( mouseItem.x-progressPin.x, mouseItem.y-progressPin.y)
                            if(progressPin.x+delta.x>=r5.x&&(progressPin.x+delta.x+progressPin.width)<=(r5.x+r5.width)){
                                //////console.log("progress  0")
                                progressPin.x=(progressPin.x+delta.x)
                            }else{
                                if(progressPin.x+delta.x<r5.x){
                                    //////console.log("progress  1")
                                    progressPin.x=r5.x
                                }
                                if((progressPin.x+delta.x+progressPin.width)>(r5.x+r5.width)){
                                    //////console.log("progress  2")
                                    progressPin.x=(r5.x+r5.width)-progressPin.width
                                }
                            }
                        }
                        actType=0
                        isPressState=false
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        // //////console.log("onPositionChanged:",mouse.y)
                        //if(cursorShape!==Qt.OpenHandCursor)cursorShape=Qt.OpenHandCursor
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        if(mouse.x>rootVideoRang.width)return
                        if(actType===-1){
                            //////console.log("delta.x",Math.abs(delta.x))
                            if(Math.abs(delta.x)>5){
                                actType=1
                            }
                        }
                        if(actType===1){
                            var r5Oldx=r5.x
                            var r5Oldw=r5.width
                            if(r5.x+delta.x>=0&&(r5.x+delta.x+r5.width)<=rootVideoRang.width){
                                r5.x=(r5.x+delta.x)
                            }else{
                                if(r5.x+delta.x<0){
                                    r5.x=0
                                }
                                if((r5.x+delta.x+r5.width)>rootVideoRang.width){
                                    r5.x=rootVideoRang.width-r5.width
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
                            ////console.log("r5_right onXChanged1----------------:",endValue);
                            endValue=(x+r5.x-rangMargins)/rangWidth*millisecondTotal
                            if(endValue<=startValue){
                                if(endValue+10<millisecondTotal){
                                    endValue+=10
                                }else{
                                    startValue-=10
                                }
                            }

                            ////console.log("r5_right onXChanged1----------------:",endValue);
                        }isSetValue=false
                    }
                    Rectangle {
                        y:r5_right.height-12-sliderHeightMargins
                        x: (endValue/millisecondTotal)<=0.5 ?  0+rangMargins: -width
                        width:62
                        height: 12
                        color : Qt.rgba(88/255,88/255,88/255,0.7);
                        border.color: "red"
                        Text{
                            anchors.fill: parent
                            text:  " " +millisecondToDate(endValue) + " "
                            color:Qt.rgba(1/255,255/255,255/255,1);
                            horizontalAlignment: (endValue/millisecondTotal)<0.5 ?Text.AlignLeft : Text.AlignRight
                            verticalAlignment: Text.AlignBottom
                            wrapMode:Text.Wrap
                            font.pixelSize:10
                        }
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
                            isPressState=true
                        }
                        onReleased: {
                            actType=0
                            isPressState=false
                        }
                        onPositionChanged: {
                            //鼠标偏移量
                            if(actType===2){
                                var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)

                                if((r5.x+delta.x+r5.width)<=rootVideoRang.width){
                                    if((r5.width+delta.x)<rangH*2){
                                        r5.width=rangH*2;
                                    }else{
                                        r5.width+=delta.x
                                    }
                                }else{
                                    r5.width=rootVideoRang.width-r5.x
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
                    Rectangle {
                        id:r5_left_text
                        y:sliderHeightMargins
                        x: (r5.x/rangWidth)>=0.5 ? -width : 0+rangMargins
                        width:62
                        height: 12
                        color : Qt.rgba(88/255,88/255,88/255,0.7);
                        border.color: "red"
                        Text{
                            anchors.fill: parent
                            text:  (r5.x/rangWidth)>=0.5 ?" " +millisecondToDate(startValue) + " " : " " + millisecondToDate(startValue)+ " "
                            color:Qt.rgba(1/255,255/255,255/255,1);
                            horizontalAlignment: (r5.x/rangWidth)<0.5 ? Text.AlignLeft: Text.AlignRight
                            verticalAlignment: Text.AlignTop
                            wrapMode:Text.Wrap
                            font.pixelSize:10
                        }
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
                            isPressState=true
                        }
                        onReleased: {
                            actType=0
                            isPressState=false
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
                    //////console.log("onXChanged: isSetValue",isSetValue)
                    if(!isSetValue){
                        if(x<=r5.x+rangMargins){
                            //////console.log("setMillisecondValue(startValue)",startValue)
                            setMillisecondValue(startValue)
                        }else{
                            ////console.log("setMillisecondValue(endValue)",x,r5.x+r5.width-rangMargins);
                            if(x+width>=r5.x+r5.width-rangMargins){
                                //////console.log("setMillisecondValue(endValue)",endValue)
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
                    color : "#FF5C00"
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
                        isPressState=true
                    }
                    onReleased: {
                        actType=0
                        isPressState=false
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

                            if(progressPin.x+progressPin.width>=r5.x+r5.width-rangMargins){
                                setMillisecondValue(endValue)
                            }

                        }
                    }
                }
            }
            Rectangle{
                id:progressPinHandle
                anchors.horizontalCenter:progressPin.horizontalCenter
                anchors.bottom: progressPin.top
                anchors.bottomMargin: -16
                width: 20
                height: 20
                color : "transparent"

                Rectangle{
                    anchors.horizontalCenter:parent.horizontalCenter
                    anchors.bottom: parent.bottom
                    width: 7
                    height: 16
                    color : "#FF5C00"
                }
                Text{
                    y:8
                    anchors.right: panValue>=0.5 ? progressPinHandle.left : undefined
                    anchors.left: panValue<0.5 ? progressPinHandle.right : undefined
                    anchors.rightMargin: panValue>=0.5 ? 15 : undefined
                    anchors.leftMargin: panValue<0.5 ?15 : undefined
                    width:300
                    height: 15
                    text:  panValue>=0.5 ?" " +millisecondToDate(curValue) + " " : " " + millisecondToDate(curValue)+ " "
                    color:Qt.rgba(139/255,163/255,60/255,1);
                    horizontalAlignment: panValue<0.5 ?Text.AlignLeft : Text.AlignRight
                    verticalAlignment: Text.AlignVCenter
                    wrapMode:Text.Wrap
                    font.pixelSize:12
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
                        isPressState=true

                    }
                    onReleased: {
                        actType=0
                        isPressState=false
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
                            if(progressPin.x+progressPin.width>=r5.x+r5.width-rangMargins){
                                setMillisecondValue(endValue)
                            }
                        }
                    }
                }
            }

            Rectangle{
                anchors.centerIn: progressPin
                width: 8
                height: progressPin.height
                color : "transparent"
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
                        isPressState=true
                    }
                    onReleased: {
                        actType=0
                        isPressState=false
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
                                }
                            }
                            if(progressPin.x+progressPin.width>=r5.x+r5.width-rangMargins){
                                setMillisecondValue(endValue)
                            }
                        }
                    }
                }
            }
        }

    }
    function updateTimeText(){
        var textValue=" " +millisecondToDate(startValue) + " / " +millisecondToDate(endValue) + " "
        eventManager.sendToQml("updateTimeText",textValue)
    }

    function setStartValue(val){
        ////console.log("setStartValue------------------:",val)

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
        ////console.log("setEndValue------------------:",val)
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
        ////console.log("setMillisecondValue------------------:",val,curValue)
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
            if("VideoRangSliderItem"===eventName){
                //console.log("VideoRangSliderItem onEmitQmlEvent:",value["event"])
                if(value["event"]==="setMaxValue"){
                    millisecondTotal=value["value"]
                    setStartValue(0)
                    setEndValue(parseInt(millisecondTotal/2))
                    setStartValue(0)
                }else if(value["event"]==="setStartTime"){
                    if(value["value"]!==startValue)
                        setStartValue(value["value"])
                }else if(value["event"]==="setEndTime"){
                    if(value["value"]!==endValue)
                        setEndValue(value["value"])
                }else if(value["event"]==="curValue"){
                    if(value["value"]!==curValue&&value["value"]<endValue){
                        isSizeChanged=true
                        setMillisecondValue(value["value"])
                        isSizeChanged=false
                    }
                }else if(value["event"]==="playState"){
                    console.log("VideoRangSliderItem onEmitQmlEvent: playState",value["value"])
                    if(value["value"]){
                        playerStatePic="qrc:/img/stop.png"
                    }else{
                        playerStatePic="qrc:/img/play.png"
                    }
                }
                // //console.log("VideoRangSliderItem onEmitQmlEvent:",maxValue)
            }
        }
    }
}


