import QtQuick 2.0

Item {
    property color bkColor: "#2E2F30"
    property color boardColor: "#8372FF"
    property int boardWidth: 2

    property int logoX: 12
    property int logoY: 16
    property int logoWidth: 40
    property int logoHeight: 40

    property int titleX: 18
    property int titleY: 59
    property int titleHeight: 23
    property color titleColor: Qt.rgba(231/255,231/255,231/255,1)
    property string titleText: "Cut Video"

    property int contextX: 18
    property int contextY: 88
    property int contextHeight: 32
    property color contextColor: Qt.rgba(90/255,90/255,90/255,1)
    property string contextText: "contextText"

    property string logoPath:"file:///C:/Users/Admin/Documents/untitled5/1.jpg"
    id:root
    Rectangle{
        id:mainItem
        anchors.fill: parent
        visible: true
        radius: 22
        clip:true
        color : bkColor ;
        border.color:boardColor;
        border.width : 0
        MouseArea {
            anchors.fill: parent
            hoverEnabled:true
            onEntered: {
                mainItem.border.width = boardWidth
            }
            onExited: {
                mainItem.border.width = 0
                effectItem.visible=false
            }
            onClicked: {
                eventManager.sendToWidget("111","1111121212")
            }
            onPressed: {
                effectItem.visible=true
            }
            onReleased: {
                effectItem.visible=false
            }
        }
       Image{
           x:logoX
           y:logoY
           width:logoWidth
           height: logoHeight
           source: logoPath
       }
       Text{
           x:titleX
           y:titleY
           width:parent.width-x*2
           height: titleHeight
           text: titleText
           color:titleColor
           horizontalAlignment: Text.AlignLeft
           verticalAlignment: Text.AlignVCenter
           wrapMode:Text.Wrap
           font.pixelSize:globalStyle.getFontSize("VideoGridItem_titleText",18)
           //font.bold: true
       }
       Text{
           x:contextX
           y:contextY
           width:parent.width-x*2
           height: contextHeight
           text: contextText
           color:mainItem.border.width===0?contextColor:boardColor
           horizontalAlignment: Text.AlignLeft
           verticalAlignment: Text.AlignTop
           wrapMode:Text.Wrap
           font.pixelSize:globalStyle.getFontSize("VideoGridItem_contextText",12)
       }

       Rectangle{
           id:effectItem
           anchors.fill: parent
           visible: false
           radius: mainItem.radius
           clip:true
           color : Qt.rgba(0.6,0.6,0.6,0.4) ;
       }
    }
}
