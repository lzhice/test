import QtQuick 2.12
import QtGraphicalEffects 1.12
import QtCharts 2.0
Rectangle{
    id:root
    property var valueList: []
    property int pointCount: 0
    property int tmpi: 0
    color: globalStyle.getColor("ChartView_backgroundColor","#2E2F30")
    onWidthChanged: {

    }

    ChartView {
        id:chartView
        anchors.fill:parent;
        //theme: ChartView.ChartThemeBrownSand
        antialiasing: true
        smooth: true
        legend.visible:false
        dropShadowEnabled:true
        plotArea:Qt.rect(0,parent.height/2+8, parent.width, parent.height)
        plotAreaColor:globalStyle.getColor("ChartView_plotAreaColor","transparent")
        backgroundColor :globalStyle.getColor("ChartView_backgroundColor","transparent")
        //chartType:ChartView.ChartTypeCartesian

        ValueAxis {
            id: axisX
            min: 0
            max: pointCount
            labelsVisible: false
            lineVisible: false
            gridVisible : false
            //visible : bool
            //tickCount:pointCount
        }
        ValueAxis {
            id: axisY
            min: 0
            max: 20
            labelsVisible: false
            lineVisible: false
            gridVisible : false
            // visible : bool
        }
        BarSeries{
            id: pieSeries
            labelsVisible:false
            useOpenGL:false
            barWidth:0.5
            axisY: axisY
            axisX:axisX

        }

    }

    Component.onCompleted: {
//        setClearSeries()
//        setPieSeries()
    }
    function getRandomNum(min,max){
        var rang=max-min
        var rand=Math.random();
        return(min+Math.round(rand*rang));
    }

    function setClearSeries(){
        pieSeries.clear()

    }

    function setPieSeries(size){
        var maxCount=(size/5/10).toFixed(0)
        pointCount=maxCount*10;
        pieSeries.clear()
        pieSeries.append("music", musicCtrl.getPoints(maxCount));
        pieSeries.at(0).color=globalStyle.getColor("pieSeriesColor","#8372FF")
        pieSeries.at(0).borderWidth=0
        pieSeries.at(0).borderColor="transparent"
    }
}


