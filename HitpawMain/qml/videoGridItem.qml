import QtQuick 2.0

Item {
    property color bkColor: Qt.rgba(46/255,47/255,51/255,1)
    property color boardColor: Qt.rgba(132/255,111/255,248/255,1)
    property int boardWidth: 2

    property int logoX: 28
    property int logoY: 28
    property int logoWidth: 38
    property int logoHeight: 38

    property int titleX: 27
    property int titleY: parent.height/3*1.2
    property int titleHeight: 30
    property color titleColor: Qt.rgba(231/255,231/255,231/255,1)
    property string titleText: "Cut Video"

    property int contextX: 28
    property int contextY: titleY+titleHeight+5
    property int contextHeight: 38
    property color contextColor: Qt.rgba(90/255,90/255,90/255,1)
    property string contextText: "contextText"

    property string logoPaht:"file:///C:/Users/Admin/Documents/untitled5/1.jpg"
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
           source: logoPaht
       }
       Text{
           x:titleX
           y:titleY
           width:parent.width-x
           height: titleHeight
           text: titleText
           color:titleColor
           horizontalAlignment: Text.AlignLeft
           verticalAlignment: Text.AlignVCenter
           wrapMode:Text.Wrap
           font.pixelSize:19
           //font.bold: true
       }
       Text{
           x:contextX
           y:contextY
           width:parent.width-x
           height: contextHeight
           text: contextText
           color:contextColor
           horizontalAlignment: Text.AlignLeft
           verticalAlignment: Text.AlignTop
           wrapMode:Text.Wrap
           font.pixelSize:13
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
