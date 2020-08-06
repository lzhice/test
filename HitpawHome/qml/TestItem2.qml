import QtQuick 2.4
import QtQuick.Layouts 1.2
import QtQuick.Controls 2.4
import QtQuick.Controls.Styles 1.4

Item {
    visible: true
    property int dpi: 1
    property var listModel:[{ name: "Apple" },{ name: "Apple2" },{ name: "Apple3" },{ name: "Apple4" },{ name: "Apple5" }]
    property int _listCount: list.count
    property int num: 0
    ScrollView {
        anchors.fill: parent
        ListView {
            id:list
            clip:true
            orientation:  ListView.Vertical
            snapMode   :  ListView.SnapToItem       //停靠在列表的最开始
            cacheBuffer:  20
            anchors.fill: parent
            model: 5
            highlightFollowsCurrentItem :true
            onCurrentIndexChanged: {
                //console.log("current index = ",currentIndex)
            }
            delegate: Rectangle{
                id:delegate_list
                // color: "red"
                height: 32
                width: parent.width
                signal signalShowMenu(var id,int x,int y)
                Text{
                    id:button_text
                    anchors.fill: parent
                    text:" "+listModel[index].name+ " "
                    color:"black"
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                    wrapMode:Text.Wrap
                    font.pixelSize:12
                }
                MouseArea{
                    id:mouse_delegate

                    acceptedButtons: Qt.RightButton|Qt.LeftButton
                    hoverEnabled: true
                    propagateComposedEvents: true
                    enabled:true
                    anchors.fill: parent
                    onEntered:{
                        delegate_list.color = "#DCDCDC"
                        //  //console.log("in")
                    }
                    onExited:{
                        delegate_list.color = "white"
                        //  //console.log("out")
                    }
                    onClicked: {
                        mouse.accepted = false;
                        doAdd();
                       // list.currentIndex=1
                        //console.log("item click.");
                    }
                    onDoubleClicked: {
                        mouse.accepted = false;
                        //console.log("item double click.");
                    }
                }
            }
        }
    }
    function doAdd(){
        var pos=list.contentY
        listModel[_listCount]={ name: num++ }
        list.model=_listCount+1
        list.contentY=pos
    }
}
