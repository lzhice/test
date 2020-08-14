import QtQuick 2.0
import QtQuick.Controls 2.5
Item{
    id:root
    property real rangX: 0.0
    property real rangY: 0.0
    property real rangWidth: 0.5
    property real rangHeight: 0.5

    property point clickPos: "0,0"
    property int actType: 0
    property int rangV_Anchors:10;
    property int rangH:1;
    property int rangH_Anchors:10;
    property bool widthEnable:true;
    property bool heightEnable:true;
    property bool isSizeChanged:false;
    property color borderLineColor: globalStyle.getColor("VideoSelectRangItem_borderLineColor","transparent")
    property color noSelectRangColor: Qt.rgba(0,0,0,0.5)
    property int miniWH: 30
    DropArea{
        anchors.fill: parent
        onEntered: {
        }
        onDropped: {
            drag.accepted = true
            var fileCount=drop.urls.length
            if (fileCount===1) {
                eventManager.sendToWidget("OpenFile",drop.urls[0])
            }else{
                eventManager.sendToWidget("MultipleFiles","")
            }

        }
    }
    onWidthChanged: {
        if(width>1){
            isSizeChanged=true
            r5.x=(rangX*width).toFixed(0)
            r5.width=(rangWidth*width).toFixed(0)
            isSizeChanged=false
        }
    }
    onHeightChanged: {
        if(height>1){
            isSizeChanged=true
            r5.y=(rangY*height).toFixed(0)
            r5.height=(rangHeight*height).toFixed(0)
            isSizeChanged=false
        }
    }

    Rectangle {
        id:r1
        x:0
        y:0
        width: r5.x
        height: r5.y
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r1MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }
    Rectangle {
        id:r2
        x:r5.x
        y:0
        width: r5.width
        height: r5.y
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r2MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }
    Rectangle {
        id:r3
        x:r5.x+r5.width
        y:0
        width: parent.width-(r5.x+r5.width)
        height: r5.y
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r3MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor
            }
        }
    }
    Rectangle {
        id:r4
        x:0
        y:r5.y
        width: r5.x
        height: r5.height
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r4MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }

    Rectangle {
        id:r6
        x:r3.x
        y:r5.y
        width: r3.width
        height: r5.height
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r6MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }
    Rectangle {
        id:r7
        x:0
        y:r5.y+r5.height
        width: r5.x
        height: parent.height-y
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r7MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }
    Rectangle {
        id:r8
        x:r5.x
        y:r5.y+r5.height
        width: r5.width
        height: parent.height-y
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r8MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }
    Rectangle {
        id:r9
        x:r3.x
        y:r5.y+r5.height
        width: r3.width
        height: parent.height-y
        visible: true
        color : noSelectRangColor
        border.width: 0
        MouseArea {
            id:r9MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.ArrowCursor

            }
        }
    }

    Rectangle {
        id:r5
        x:540
        y:100
        width: 50
        height: 300
        visible: true
        color : Qt.rgba(0,0,0,0.0);
        border.width: 0
        ShadowMask{
            anchors.fill: parent
            Image {
                anchors.verticalCenter: parent.verticalCenter
                anchors.horizontalCenter: parent.horizontalCenter
                width: 30
                height: 30
                visible: parent.width<30||parent.height<30?false:true
                source: "qrc:/img/centerStart.png"
            }
        }

        onXChanged: {
            if(isSizeChanged)return
            if(x<0){
                x=0
            }
            if(x>parent.width){
                x=parent.width
            }
            if((x+width>parent.width)&&widthEnable){

                if(width>parent.width){
                    width=parent.width
                }
                x=parent.width-width
            }
            rangX=x/parent.width

        }
        onYChanged: {
            if(isSizeChanged)return
            if(y<0){
                y=0
            }
            if(y>parent.height){
                y=parent.height
            }
            if((y+height>parent.height)&&heightEnable){
                if(height>parent.height){
                    height=parent.height
                }
                y=parent.height-height
            }
            rangY=y/parent.height
        }
        onWidthChanged: {
            if(isSizeChanged)return
            if(x+width>parent.width){
                if(x<0){
                    x=0
                }
                if(x>parent.width){
                    x=parent.width
                }
                width=parent.width-x
            }
            rangWidth=width/parent.width
        }
        onHeightChanged: {
            if(isSizeChanged)return
            if(y+height>parent.height){
                if(y<0){
                    y=0
                }
                if(y>parent.height){
                    y=parent.height
                }
                height=parent.height-y
            }
            rangHeight=height/parent.height
        }

        MouseArea {
            id:r5MouseArea
            anchors.fill: parent
            hoverEnabled:true
            onPressed: {
                if(cursorShape===Qt.SizeAllCursor){
                    actType=1
                    clickPos  = Qt.point(mouse.x,mouse.y)
                }
            }
            onReleased: {
                actType=0
                cursorShape=Qt.ArrowCursor
            }

            onPositionChanged: {
                //鼠标偏移量
                cursorShape=Qt.SizeAllCursor
                if(actType===1){
                    var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                    r5.x=(r5.x+delta.x)
                    r5.y=(r5.y+delta.y)
                    cursorShape=Qt.SizeAllCursor
                    //console.log("actType===1:")
                }
            }
        }

        //-----------------------------------------------------------
        Rectangle {
            id:r5_left
            x:-width/2
            y:0
            width: rangH
            height: r5.height
            color : borderLineColor
            MouseArea {
                id:r5_leftMouseArea
                anchors.fill: parent
            }
            Rectangle {
                id:r5_left_rect
                anchors.horizontalCenter: parent.horizontalCenter
                anchors.verticalCenter: parent.verticalCenter
                width: rangH_Anchors
                height:parent.height
                color : "transparent"
                MouseArea {
                    anchors.fill: parent
                    hoverEnabled:true
                    onPressed: {
                        if(cursorShape===Qt.SizeHorCursor){
                            r5_leftMouseArea.cursorShape=Qt.SizeHorCursor
                            r5MouseArea.cursorShape=Qt.SizeHorCursor
                            r4MouseArea.cursorShape=Qt.SizeHorCursor
                            actType=2
                            clickPos  = Qt.point(mouse.x,mouse.y)
                        }
                    }
                    onReleased: {
                        actType=0
                        cursorShape=Qt.ArrowCursor
                        r5_leftMouseArea.cursorShape=Qt.ArrowCursor
                        r5MouseArea.cursorShape=Qt.ArrowCursor
                        r4MouseArea.cursorShape=Qt.ArrowCursor
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        cursorShape=Qt.SizeHorCursor
                        if(actType===2){
                            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                            doLeft(delta)
                        }
                    }
                }
            }
        }
        Rectangle {
            id:r5_right
            x:r5.width-(width/2)
            y:0
            width: rangH
            height: r5.height
            color : borderLineColor



            MouseArea {
                id:r5_rightMouseArea
                anchors.fill: parent
            }
            Rectangle {
                id:r5_right_rect
                anchors.horizontalCenter: parent.horizontalCenter
                anchors.verticalCenter: parent.verticalCenter
                width: rangH_Anchors
                height:parent.height
                color : "transparent"
                MouseArea {
                    anchors.fill: parent
                    hoverEnabled:true
                    onPressed: {
                        if(cursorShape===Qt.SizeHorCursor){
                            r5_rightMouseArea.cursorShape=Qt.SizeHorCursor
                            r5MouseArea.cursorShape=Qt.SizeHorCursor
                            r6MouseArea.cursorShape=Qt.SizeHorCursor
                            actType=2
                            clickPos  = Qt.point(mouse.x,mouse.y)
                        }
                    }
                    onReleased: {
                        actType=0
                        r5_rightMouseArea.cursorShape=Qt.ArrowCursor
                        r5MouseArea.cursorShape=Qt.ArrowCursor
                        r6MouseArea.cursorShape=Qt.ArrowCursor
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        cursorShape=Qt.SizeHorCursor
                        if(actType===2){
                            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                            if((r5.width+delta.x)<miniWH){
                                r5.width=miniWH;
                            }else{
                                r5.width+=delta.x
                            }

                        }
                    }
                }
            }
        }

        Rectangle {
            id:r5_up
            x:0
            y:-height/2
            width: r5.width
            height: rangH
            color : borderLineColor
            MouseArea {
                id:r5_upMouseArea
                anchors.fill: parent
            }
            Rectangle {
                id:r5_up_rect
                anchors.horizontalCenter: parent.horizontalCenter
                anchors.verticalCenter: parent.verticalCenter
                width: parent.width
                height:rangH_Anchors
                color : "transparent"
                MouseArea {
                    anchors.fill: parent
                    hoverEnabled:true
                    onPressed: {
                        if(cursorShape===Qt.SizeVerCursor){
                            r5_upMouseArea.cursorShape=Qt.SizeVerCursor
                            r5MouseArea.cursorShape=Qt.SizeVerCursor
                            r2MouseArea.cursorShape=Qt.SizeVerCursor
                            actType=2
                            clickPos  = Qt.point(mouse.x,mouse.y)
                        }
                    }
                    onReleased: {
                        actType=0
                        r5_upMouseArea.cursorShape=Qt.ArrowCursor
                        r5MouseArea.cursorShape=Qt.ArrowCursor
                        r2MouseArea.cursorShape=Qt.ArrowCursor
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        cursorShape=Qt.SizeVerCursor
                        if(actType===2){
                            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                            doUp(delta)
                        }
                    }
                }
            }
        }

        Rectangle {
            id:r5_down
            x:0
            y:r5.height-height/2
            width: r5.width
            height: rangH
            color : borderLineColor
            MouseArea {
                id:r5_downMouseArea
                anchors.fill: parent
            }
            Rectangle {
                id:r5_down_rect
                anchors.horizontalCenter: parent.horizontalCenter
                anchors.verticalCenter: parent.verticalCenter
                width: parent.width
                height:rangH_Anchors
                color : "transparent"
                MouseArea {
                    anchors.fill: parent
                    hoverEnabled:true
                    onPressed: {
                        if(cursorShape===Qt.SizeVerCursor){
                            r5_downMouseArea.cursorShape=Qt.SizeVerCursor
                            r5MouseArea.cursorShape=Qt.SizeVerCursor
                            r8MouseArea.cursorShape=Qt.SizeVerCursor
                            actType=2
                            clickPos  = Qt.point(mouse.x,mouse.y)
                        }
                    }
                    onReleased: {
                        actType=0
                        r5_downMouseArea.cursorShape=Qt.ArrowCursor
                        r5MouseArea.cursorShape=Qt.ArrowCursor
                        r8MouseArea.cursorShape=Qt.ArrowCursor
                    }
                    onPositionChanged: {
                        //鼠标偏移量
                        cursorShape=Qt.SizeVerCursor
                        if(actType===2){
                            var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                            if((r5.height+delta.y)<miniWH){
                                r5.height=miniWH;
                            }else{
                                r5.height+=delta.y
                            }
                        }
                    }
                }
            }
        }
        //-------------
        Rectangle {
            id:r5_1
            x:-width/2
            y:-height/2
            width: rangV_Anchors
            height: rangV_Anchors
            color : "transparent"
            MouseArea {
                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeFDiagCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }
                onPressed: {
                    if(cursorShape===Qt.SizeFDiagCursor){
                        r5MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r1MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r2MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r4MouseArea.cursorShape=Qt.SizeFDiagCursor
                        actType=2
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }
                }
                onReleased: {
                    actType=0
                    r5MouseArea.cursorShape=Qt.ArrowCursor
                    r1MouseArea.cursorShape=Qt.ArrowCursor
                    r2MouseArea.cursorShape=Qt.ArrowCursor
                    r4MouseArea.cursorShape=Qt.ArrowCursor
                }
                onPositionChanged: {
                    //鼠标偏移量
                    if(actType===2){
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        doLeft(delta)
                        doUp(delta)
                    }
                }
            }
        }

        Rectangle {
            id:r5_2
            x:r5.width-(width/2)
            y:-height/2
            width: rangV_Anchors
            height: rangV_Anchors
            color : "transparent"
            MouseArea {
                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeBDiagCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }
                onPressed: {
                    if(cursorShape===Qt.SizeBDiagCursor){
                        r5MouseArea.cursorShape=Qt.SizeBDiagCursor
                        r2MouseArea.cursorShape=Qt.SizeBDiagCursor
                        r3MouseArea.cursorShape=Qt.SizeBDiagCursor
                        r6MouseArea.cursorShape=Qt.SizeBDiagCursor
                        actType=2
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }
                }
                onReleased: {
                    actType=0
                    r5MouseArea.cursorShape=Qt.ArrowCursor
                    r2MouseArea.cursorShape=Qt.ArrowCursor
                    r3MouseArea.cursorShape=Qt.ArrowCursor
                    r6MouseArea.cursorShape=Qt.ArrowCursor
                }
                onPositionChanged: {

                    if(actType===2){
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        if((r5.width+delta.x)<miniWH){
                            r5.width=miniWH;
                        }else{
                            r5.width+=delta.x
                        }

                        doUp(delta)

                    }
                }
            }
        }
        Rectangle {
            id:r5_3
            x:-width/2
            y:r5.height-(height/2)
            width: rangV_Anchors
            height: rangV_Anchors
            color : "transparent"
            MouseArea {
                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeBDiagCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }
                onPressed: {
                    if(cursorShape===Qt.SizeBDiagCursor){
                        r5MouseArea.cursorShape=Qt.SizeBDiagCursor
                        r4MouseArea.cursorShape=Qt.SizeBDiagCursor
                        r7MouseArea.cursorShape=Qt.SizeBDiagCursor
                        r8MouseArea.cursorShape=Qt.SizeBDiagCursor
                        actType=2
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }
                }
                onReleased: {
                    actType=0
                    r5MouseArea.cursorShape=Qt.ArrowCursor
                    r4MouseArea.cursorShape=Qt.ArrowCursor
                    r7MouseArea.cursorShape=Qt.ArrowCursor
                    r8MouseArea.cursorShape=Qt.ArrowCursor
                }
                onPositionChanged: {
                    //鼠标偏移量
                    if(actType===2){
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        doLeft(delta)

                        if((r5.height+delta.y)<miniWH){
                            r5.height=miniWH;
                        }else{
                            r5.height+=delta.y
                        }
                    }
                }
            }
        }
        Rectangle {
            id:r5_4
            x:r5.width-(width/2)
            y:r5.height-(height/2)
            width: rangV_Anchors
            height: rangV_Anchors
            color : "transparent"
            MouseArea {
                anchors.fill: parent
                hoverEnabled:true
                onEntered: {
                    cursorShape=Qt.SizeFDiagCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }
                onPressed: {
                    if(cursorShape===Qt.SizeFDiagCursor){
                        r5MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r6MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r8MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r9MouseArea.cursorShape=Qt.SizeFDiagCursor
                        actType=2
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }
                }
                onReleased: {
                    actType=0
                    r5MouseArea.cursorShape=Qt.ArrowCursor
                    r6MouseArea.cursorShape=Qt.ArrowCursor
                    r8MouseArea.cursorShape=Qt.ArrowCursor
                    r9MouseArea.cursorShape=Qt.ArrowCursor
                }
                onPositionChanged: {
                    //鼠标偏移量
                    if(actType===2){
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        if((r5.width+delta.x)<miniWH){
                            r5.width=miniWH;
                        }else{
                            r5.width+=delta.x
                        }
                        if((r5.height+delta.y)<miniWH){
                            r5.height=miniWH;
                        }else{
                            r5.height+=delta.y
                        }
                    }
                }
            }
        }
    }
    function doLeft(delta){
        var oldX=r5.x
        var oldRightX=oldX+r5.width

        if((r5.width-delta.x)<miniWH){
            r5.width=miniWH;
            r5.x=oldRightX-miniWH
        }else{
            widthEnable=false
            if((r5.x+delta.x)<0){
                r5.x=0
                r5.width=oldRightX
            }else{
                r5.x=(r5.x+delta.x)
                r5.width=oldRightX-r5.x
            }
            widthEnable=true
        }
    }
    function doUp(delta){
        var oldY=r5.y
        var oldRightY=oldY+r5.height

        if((r5.height-delta.y)<miniWH){
            r5.height=miniWH;
            r5.y=oldRightY-miniWH
        }else{
            heightEnable=false
            if((r5.y+delta.y)<0){
                r5.y=0
                r5.height=oldRightY
            }else{
                r5.y=(r5.y+delta.y)
                r5.height=oldRightY-r5.y
            }
            heightEnable=true
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             actType=2
                        clickPos  = Qt.point(mouse.x,mouse.y)
                        setRootCursorShape(Qt.SizeBDiagCursor)
                    }
                }
                onReleased: {
                    actType=0
                    r5MouseArea.cursorShape=Qt.ArrowCursor
                    r4MouseArea.cursorShape=Qt.ArrowCursor
                    r7MouseArea.cursorShape=Qt.ArrowCursor
                    r8MouseArea.cursorShape=Qt.ArrowCursor
                    setRootCursorShape(Qt.ArrowCursor)
                }
                onPositionChanged: {
                    //鼠标偏移量
                    if(actType===0){
                        cursorShape=Qt.SizeBDiagCursor
                    }else{
                        cursorShape=root.rootCursorShape
                    }
                    if(actType===2){
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        doLeft(delta)

                        if(aspectRatio===0){
                            if((r5.height+delta.y)<miniWH){
                                r5.height=miniWH;
                            }else{
                                r5.height+=delta.y
                            }
                        }
                    }
                }
            }
        }
        Rectangle {
            id:r5_4
            x:r5.width-(width/2)
            y:r5.height-(height/2)
            width: rangV_Anchors
            height: rangV_Anchors
            color : "transparent"
            MouseArea {
                id:r5_4_MouseArea
                anchors.fill: parent
                hoverEnabled:true
                //                onEntered: {
                //                    if(actType===0)
                //                    cursorShape=Qt.SizeFDiagCursor
                //                }
                //                onExited: {
                //                    cursorShape=Qt.ArrowCursor
                //                }
                onPressed: {
                    if(cursorShape===Qt.SizeFDiagCursor){
                        r5MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r6MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r8MouseArea.cursorShape=Qt.SizeFDiagCursor
                        r9MouseArea.cursorShape=Qt.SizeFDiagCursor
                        actType=2
                        clickPos  = Qt.point(mouse.x,mouse.y)
                        setRootCursorShape(Qt.SizeFDiagCursor)
                    }
                }
                onReleased: {
                    actType=0
                    r5MouseArea.cursorShape=Qt.ArrowCursor
                    r6MouseArea.cursorShape=Qt.ArrowCursor
                    r8MouseArea.cursorShape=Qt.ArrowCursor
                    r9MouseArea.cursorShape=Qt.ArrowCursor
                    setRootCursorShape(Qt.ArrowCursor)
                }
                onPositionChanged: {
                    //鼠标偏移量
                    if(actType===0){
                        cursorShape=Qt.SizeFDiagCursor
                    }else{
                        cursorShape=root.rootCursorShape
                    }
                    if(actType===2){
                        var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                        if(aspectRatio===0){
                            if((r5.width+delta.x)<miniWH){
                                r5.width=miniWH;
                            }else{
                                r5.width+=delta.x
                            }
                            if((r5.height+delta.y)<miniWH){
                                r5.height=miniWH;
                            }else{
                                r5.height+=delta.y
                            }
                        }else{
                            if((r5.height+delta.y)<miniWH){
                                r5.height=miniWH;
                            }else{
                                r5.height+=delta.y
                            }
                        }
                    }
                }
            }
        }
    }
    function setRootCursorShape(cursor){
        root.rootCursorShape=cursor
        r1MouseArea.cursorShape=cursor
        r2MouseArea.cursorShape=cursor
        r3MouseArea.cursorShape=cursor
        r4MouseArea.cursorShape=cursor
        r5MouseArea.cursorShape=cursor
        r6MouseArea.cursorShape=cursor
        r7MouseArea.cursorShape=cursor
        r8MouseArea.cursorShape=cursor
        r9MouseArea.cursorShape=cursor

        r5_leftMouseArea.cursorShape=cursor
        r5_rightMouseArea.cursorShape=cursor
        r5_upMouseArea.cursorShape=cursor
        r5_downMouseArea.cursorShape=cursor

        r5_1_MouseArea.cursorShape=cursor
        r5_2_MouseArea.cursorShape=cursor
        r5_3_MouseArea.cursorShape=cursor
        r5_4_MouseArea.cursorShape=cursor

        r5_left_rectMouseArea.cursorShape=cursor
        r5_right_rectMouseArea.cursorShape=cursor
        r5_up_rectMouseArea.cursorShape=cursor
        r5_down_rectMouseArea.cursorShape=cursor

    }
    function doLeft(delta){
        var oldX=r5.x
        var oldRightX=oldX+r5.width
        var w=-1
        var r5x=-1
        if((r5.width-delta.x)<miniWH){
            w=miniWH;
            r5x=oldRightX-miniWH
        }else{
            //widthEnable=false
            if((r5.x+delta.x)<0){
                r5x=0
                w=oldRightX
            }else{
                r5x=(r5.x+delta.x)
                w=oldRightX-r5x
            }
            //widthEnable=true
        }
        if(w>-1&&r5x>-1){
            var re=testAspectRatioW(w)
            if(re===0){
                widthEnable=false
                r5.x=r5x
                r5.width=w
                widthEnable=true
            }else if(re>0){
                var miniW=re
                if(r5.width>=miniW){
                    r5.height=miniW*aspectRatio
                    r5.width=miniW
                    r5.x=oldRightX-miniW

                }
            }else if(re===-1){
                if(r5.height<(root.height-r5.y)&&r5.width<(root.height-r5.y)*aspectRatio){
                    r5.width=(root.height-r5.y)*aspectRatio
                    r5.x=oldRightX-r5.width
                    r5.height=(root.height-r5.y)
                }
            }
        }
    }
    function doUp(delta){
        var oldY=r5.y
        var oldRightY=oldY+r5.height
        var h=-1
        var r5y=-1
        if((r5.height-delta.y)<miniWH){
            h=miniWH;
            r5y=oldRightY-miniWH
        }else{
            if((r5.y+delta.y)<0){
                r5y=0
                h=oldRightY
            }else{
                r5y=(r5.y+delta.y)
                h=oldRightY-r5y
            }
        }
        if(h>-1&&r5y>-1){
            if(testAspectRatioH(h)){
                heightEnable=false
                r5.y=r5y
                r5.height=h
                heightEnable=true
            }
        }

        if(h>-1&&r5y>-1){
            var re=testAspectRatioH(h)
            if(re===0){
                heightEnable=false
                r5.y=r5y
                r5.height=h
                heightEnable=true
            }else if(re>0){
                var miniH=re
                if(r5.height>=miniH){
                    r5.width=miniH/aspectRatio
                    r5.height=miniH
                    r5.y=oldRightY-miniH

                }
            }else if(re===-1){
                if(r5.width<(root.width-r5.x)&&r5.height<(root.width-r5.x)/aspectRatio){
                    r5.height=(root.width-r5.x)/aspectRatio
                    r5.y=oldRightY-r5.height
                    r5.width=(root.width-r5.x)
                }
            }
        }
    }

    function doAspectRatioW(){
        var re=true;
        if(aspectRatio>0){
            var w=r5.width
            var miniW=miniWH
            var miniH=miniWH
            if(aspectRatio<1){
                miniH =miniW/aspectRatio
            }else{
                miniW=miniH*aspectRatio
            }

            if(w<miniW){
                r5.width=miniW;
                r5.height=miniW/aspectRatio
                return false
            }

            var h=w/aspectRatio
            if(r5.y+h<root.height){
                r5.width=w
                r5.height=h
            }else{
                h=root.height-r5.y
                r5.width=h*aspectRatio
                r5.height=h
                re=false
            }
        }
        return re
    }
    function doAspectRatioH(){
        var re=true;
        if(aspectRatio>0){
            var h=r5.height
            var miniW=miniWH
            var miniH=miniWH
            if(aspectRatio<1){
                miniH =miniW/aspectRatio
            }else{
                miniW=miniH*aspectRatio
            }

            if(h<miniH){
                r5.height=miniH;
                r5.width=miniH*aspectRatio
                return false
            }

            var w=h*aspectRatio
            if(r5.x+w<root.width){
                r5.width=w
                r5.height=h
            }else{
                w=root.width-r5.x
                r5.width=w
                r5.height=w/aspectRatio
                re=false
            }
        }
        return re
    }

    function testAspectRatioW(r5Width){
        var re=0;
        if(aspectRatio>0){
            var w=r5Width
            var miniW=miniWH
            var miniH=miniWH
            if(aspectRatio<1){
                miniH =miniW/aspectRatio
            }else{
                miniW=miniH*aspectRatio
            }

            if(w<=miniW){
                w=miniW;
                return miniW
            }

            var h=w/aspectRatio
            if(r5.y+h<root.height){
            }else{
                //                h=root.height-r5.y
                //                r5.width=h*aspectRatio
                //                r5.height=h
                re=-1
            }
        }
        return re
    }
    function testAspectRatioH(r5Height){
        var re=0;
        if(aspectRatio>0){
            var h=r5Height
            var miniW=miniWH
            var miniH=miniWH
            if(aspectRatio<1){
                miniH =miniW/aspectRatio
            }else{
                miniW=miniH*aspectRatio
            }

            if(h<miniH){
                h=miniH;
                return miniH
            }

            var w=h*aspectRatio
            if(r5.x+w<root.width){
            }else{
                re=-1
            }
        }
        return re
    }

}
