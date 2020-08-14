import QtQuick 2.12
import QtGraphicalEffects 1.12
import QtQuick.Controls 2.5
Rectangle{
    property string selectbuttonText: ""
    property color splitLineColor: "#000000"

    onSelectbuttonTextChanged: {
        //console.log("onSelectbuttonTextChanged-----------------------------:",selectbuttonText)
    }

    id:root
    color : "#2E2F30"
    radius: 10
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
        anchors.topMargin: 20
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 15
        Text{
            id:textTitle
            anchors.fill: parent
            text: qsTr("Scale")
            color:globalStyle.getColor("TextButton_color","#979798")
            horizontalAlignment: Text.AlignLeft
            verticalAlignment: Text.AlignVCenter
            wrapMode:Text.Wrap
            font.pixelSize:globalStyle.getFontSize("TextButton_textSize",12)
        }
    }

    Item {
        id: row4
        anchors.top: row3.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 10
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 56
        Flow{
            anchors.fill: parent
            spacing: 14.5;
            Rectangle{
                width: 54
                height: 34
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_Original
                    objectName: "scale_Original"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("Original")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        if(isPresss){
                            selectbuttonText=objectName
                        }else{
                        }
                    }

                }
            }
            Rectangle{
                width: 34
                height: 34
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_1_1
                    objectName: "scale_1_1"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("1:1")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        //console.log("onIsPresssChanged-----------------------------:",objectName,isPresss)
                        if(isPresss){
                            selectbuttonText=objectName
                        }
                    }
                }
            }
            Rectangle{
                width: 34
                height: 50
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_4_3
                    objectName: "scale_4_3"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("4:3")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        //console.log("onIsPresssChanged-----------------------------:",objectName,isPresss)
                        if(isPresss){
                            selectbuttonText=objectName
                        }
                    }
                }
            }
            Rectangle{
                width: 34
                height: 56
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_9_16
                    objectName: "scale_9_16"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("9:16")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        //console.log("onIsPresssChanged-----------------------------:",objectName,isPresss)
                        if(isPresss){
                            selectbuttonText=objectName
                        }
                    }
                }
            }

        }
    }

    Item {
        id: row5
        anchors.top: row4.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 10
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 34
        Flow{
            anchors.fill: parent
            spacing: 14.5;
            Rectangle{
                width: 55
                height: 34
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_free
                    objectName: "scale_free"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("Free")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        //console.log("onIsPresssChanged-----------------------------:",objectName,isPresss)
                        if(isPresss){
                            selectbuttonText=objectName
                        }
                    }
                }
            }
            Rectangle{
                width: 56
                height: 34
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_3_4
                    objectName: "scale_3_4"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("3:4")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        //console.log("onIsPresssChanged-----------------------------:",objectName,isPresss)
                        if(isPresss){
                            selectbuttonText=objectName
                        }
                    }
                }
            }
            Rectangle{
                width: 60
                height: 34
                color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#3B3C3D")
                border.color: globalStyle.getColor("VideoRotateAndSizeSetView_TextButton_color","#454649")
                border.width: 1
                TextButton{
                    id:scale_16_9
                    objectName: "scale_16_9"
                    anchors.fill: parent
                    textSize : globalStyle.getFontSize("ScaleButton_textSize",10)
                    buttonText : " "+qsTr("16:9")+" "
                    frameStyle:TextButton.Style.Rimless
                    selectbutton: selectbuttonText
                    onIsPresssChanged: {
                        //console.log("onIsPresssChanged-----------------------------:",objectName,isPresss)
                        if(isPresss){
                            selectbuttonText=objectName
                        }
                    }
                }
            }

        }
    }
    Item {
        id: row6
        anchors.top: row5.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 20
        anchors.leftMargin: 0
        anchors.rightMargin: 0
        height: 1
        Rectangle{
            anchors.fill: parent
            color: splitLineColor
        }
    }

    Item {
        id: row7
        anchors.top: row6.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 20
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 15
        Text{
            id:textTitle2
            anchors.fill: parent
            text: " "+qsTr("Rotate & Mirror")+" "
            color:globalStyle.getColor("TextButton_color","#979798")
            horizontalAlignment: Text.AlignLeft
            verticalAlignment: Text.AlignVCenter
            wrapMode:Text.Wrap
            font.pixelSize:globalStyle.getFontSize("TextButton_textSize",12)
        }
    }
    Item {
        id: row8
        anchors.top: row7.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.topMargin: 13
        anchors.leftMargin: 20
        anchors.rightMargin: 20
        height: 56
        Flow{
            anchors.fill: parent
            spacing: 2.5;

            ImageButton{
                objectName: "RotateLeft"
                id:imageButton1
                width: 48
                height: 24
                imageSrc: "qrc:/img/drawable-xxxhdpi_route-right.png"
            }
            ImageButton{
                objectName: "RotateRight"
                width: 48
                height: 24
                imageSrc: "qrc:/img/drawable-xxxhdpi_route-right copy.png"
            }
            ImageButton{
                objectName: "Mirror_H"
                width: 48
                height: 24
                imageSrc: "qrc:/img/drawable-xxxhdpi_scale.png"
            }
            ImageButton{
                objectName: "Mirror_V"
                width: 48
                height: 24
                imageSrc: "qrc:/img/drawable-xxxhdpi_scale copy.png"
            }
        }
    }
    Item {
        id: row9
        anchors.bottom: parent.bottom
        anchors.bottomMargin: 38
        anchors.left: parent.left
        anchors.leftMargin: 20
        width: 200
        height: 30
        TextButton{
            id:buttonRight
            objectName: "buttonExport"
            anchors.fill: parent
            textSize : globalStyle.getFontSize("TextButton_textSize",12)
            buttonText : " "+qsTr("Export")+" "
        }
    }
}
