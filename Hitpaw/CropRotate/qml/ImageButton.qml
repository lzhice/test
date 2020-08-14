import QtQuick 2.12
import QtQuick.Controls 2.5
import QtGraphicalEffects 1.12
Rectangle {
    id:button
    enum Style {
            Rimless,
            Bordered
    }
    property string objectName: ""
    property int textSize : globalStyle.getFontSize("TextButton_textSize",12)
    property string buttonText : ""
    property color bkColor: globalStyle.getColor("bkBorderColor_color","#3B3C3D")
    property color bkBorderColor: "transparent"
    property string imageSrc: ""
    property int imageWidth: 30
    property int imageHeight: 30
    property string selectbutton: ""
    property bool isPresss: false
    onSelectbuttonChanged: {

        if(selectbutton===objectName){
            button.isPresss=true
        }else{
            button.isPresss=false
            //button_text.visible=false
        }
        bkColor=isPresss?"transparent":globalStyle.getColor("bkBorderColor_color","#3B3C3D")
    }

    color: bkColor
    border.width: bkBorderColor===globalStyle.getColor("bkBorderColor_color","transparent")?0:1
    border.color : bkBorderColor

    Image{
        id:button_img
        anchors.horizontalCenter: parent.horizontalCenter
        anchors.verticalCenter: parent.verticalCenter
        width: imageWidth
        height: imageHeight
        mipmap: true
        smooth: true
        antialiasing: true
        source: imageSrc
    }
    GaussianBlur {
        anchors.fill: button_img
        source: button_img
        radius: 6
        samples: 160
        deviation:0.9
    }
    MouseArea {
        id: mouseA
        anchors.fill: parent
        hoverEnabled:true
        onEntered: {
            //button_text.visible=true
        }
        onExited: {
            button_effectItem.visible=false
        }
        onClicked: {
            eventManager.sendToWidget(button.objectName,"onClicked")
            button.isPresss=true
        }
        onPressed: {
            button_effectItem.visible=true
        }
        onReleased: {
            button_effectItem.visible=false
        }
    }
//    Rectangle{
//        id:button_text
//        anchors.bottom: parent.bottom
//        anchors.bottomMargin: 1
//        width: parent.height
//        height: parent.height
//        visible: button.isPresss
//        clip:true
//        color : Qt.rgba(247/255,181/255,0,0.9)
//    }

    Rectangle{
        id:button_effectItem
        anchors.fill: parent
        visible: false
        clip:true
        color : Qt.rgba(0.6,0.6,0.6,0.4)
        border.color: "#F7B500"
        border.width: 0
    }
}
