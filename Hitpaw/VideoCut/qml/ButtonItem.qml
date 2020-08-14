import QtQuick 2.0
import QtQuick.Controls.Styles 1.4
import QtQuick.Controls 2.5
Button {
    property string objectName: ""
    property int border1: 1
    id: myButton
    hoverEnabled: true
    ToolTip.delay: 800
    ToolTip.timeout: 3000
    ToolTip.visible: hovered
    ToolTip.text: text
    width:25
    height: 25
    background:Rectangle{
        implicitWidth:25
        implicitHeight:25
        color:"lightgray"
        border.width:border1
        //                border.color:(control.hovered||control.pressed)?"blue":"green"
        //radius: 12
        clip:true
    }
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
            myButton.border1 = 0
        }
        onExited: {
            myButton.border1 = 1
            effectItem.visible=false
        }
        onClicked: {
            eventManager.sendToWidget(myButton.objectName,"onClicked")
        }
        onPressed: {
            effectItem.visible=true
        }
        onReleased: {
            effectItem.visible=false
        }
    }

}
