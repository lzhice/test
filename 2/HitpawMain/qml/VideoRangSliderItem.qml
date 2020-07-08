import QtQuick 2.12
import QtQuick.Window 2.12

Item {
    id:root
    property point clickPos: "0,0"
    property int actType: 0
    property int rangV:5;
    property int rangH:30;
    Image {
        id: name
        source: "file:///C:/Users/Admin/Documents/untitled5/1.jpg"
        anchors.fill: parent
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
            x:540
            y:100
            width: 50
            height: 300
            visible: true
            color : Qt.rgba(0,0,0,0.0);
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
                    console.log("r5:onEntered")
                    cursorShape=Qt.SizeAllCursor
                }
                onExited: {
                    cursorShape=Qt.ArrowCursor
                }

                onPressed: {
                    if(cursorShape===Qt.SizeAllCursor){
                        actType=1
                        clickPos  = Qt.point(mouse.x,mouse.y)
                    }

                }
                onReleased: {
                    actType=0
                }

                onPositionChanged: {
                    //鼠标偏移量
                    if(mouse.x>root.width)return
                    var delta = Qt.point(mouse.x-clickPos.x, mouse.y-clickPos.y)
                    if(actType===1){
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
                        }
                    }
                }
            }

        }
    }
}
