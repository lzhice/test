import QtQuick 2.12

Text {
    id: resizable
    font.bold: true
    color: "white"
    FontMetrics {
        id: textFontMetrics
        font.family: resizable.font.family
    }
    property double scale:0.9
    property double heightFactor: height*scale / textFontMetrics.height
    property double widthFactor: width*scale / textFontMetrics.boundingRect(text).width
    property double advanceWidthFactor: width*scale / textFontMetrics.advanceWidth(text)
    property double smallestFactor: Math.min(heightFactor, widthFactor, advanceWidthFactor)

    verticalAlignment: Text.AlignVCenter
    horizontalAlignment: Text.AlignHCenter

    font.pointSize: textFontMetrics.font.pointSize * (smallestFactor > 0 ? smallestFactor : (heightFactor > 0 ? heightFactor : 1))
}
