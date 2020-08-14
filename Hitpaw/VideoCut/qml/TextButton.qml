import QtQuick 2.0
import QtQuick.Controls 2.5

Rectangle {
    id:button
    enum Style {
            Rimless,
            Bordered
    }
    property string objectName: ""
    property int textSize : globalStyle.getFontSize("TextButton_textSize",12)
    property string buttonText : ""
    property int frameStyle: TextButton.Style.Bordered
    property bool isPresss: false
    property string selectbutton : ""
    property bool isHighlight : false
    onIsHighlightChanged: {
        if(isHighlight){
        button.color = globalStyle.getColor("TextButton_color","#8372FF")
        }else{
            button.color = "transparent";
        }
    }

    onSelectbuttonChanged: {
        if(selectbutton===objectName&&objectName!==""){
            isPresss=true
        }else{
            isPresss=false
        }
    }

    color: "transparent"
    border.width: frameStyle===TextButton.Style.Bordered?1:0
    border.color : globalStyle.getColor("TextButton_borderColor","#8372FF")
    Text{
        id:button_text
        anchors.fill: parent
        text:buttonText
        color:frameStyle===TextButton.Style.Bordered?(button.color===globalStyle.getColor("TextButton_color","transparent"))?
                  globalStyle.getColor("TextButton_text_normalColor","#8372FF"):
                  globalStyle.getColor("TextButton_text_haverColor","#FFFFFF"):button.isPresss?globalStyle.getColor("TextButton_text_presslColor","#8372FF"):globalStyle.getColor("TextButton_text_normalColor","#E3E3E3")
        horizontalAlignment: Text.AlignHCenter
        verticalAlignment: Text.AlignVCenter
        wrapMode:Text.Wrap
        font.pixelSize:textSize
        font.weight:button.color===globalStyle.getColor("TextButton_color","transparent")?Font.Bold:Font.Bold
    }
    MouseArea {
        id: mouseA
        anchors.fill: parent
        hoverEnabled:true
        onEntered: {
            if(frameStyle===TextButton.Style.Bordered){
                button.color = globalStyle.getColor("TextButton_color","#8372FF")
            }
        }
        onExited: {
            if(!isHighlight){
                button.color = "transparent";
            }
            button_effectItem.visible=false
        }
        onClicked: {
            eventManager.sendToWidget(button.objectName,"onClicked")
            if(frameStyle===TextButton.Style.Rimless){
                button.isPresss=true
            }

        }
        onPressed: {
            button_effectItem.visible=true
        }
        onReleased: {
            button_effectItem.visible=false
        }
    }
    Rectangle{
        id:button_effectItem
        anchors.fill: parent
        visible: false
        clip:true
        color : Qt.rgba(0.6,0.6,0.6,0.4) ;
    }
}
