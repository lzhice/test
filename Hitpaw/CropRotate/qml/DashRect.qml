import QtQuick 2.4

Canvas {
    id: canvas
    property real start_x: 0
    property real start_y: 0
    property real end_x: width
    property real end_y: height
    property bool dashed: true
    property real dash_length: 2
    property real dash_space: 2
    property real line_width: 2
    property real stipple_length: (dash_length + dash_space) > 0 ? (dash_length + dash_space) : 4
    property color draw_color: "white"
    onPaint: {
        // Get the drawing context
        var ctx = canvas.getContext('2d')
        // set line color
        ctx.strokeStyle = draw_color;
        ctx.lineWidth = line_width;
        ctx.beginPath();

        if (!dashed)
        {
            ctx.moveTo(start_x,start_y);
            ctx.lineTo(end_x,end_y);
        }
        else
        {
            var dashLen = stipple_length;
            var dX = end_x - start_x;
            var dY = end_y - start_y;
            var dashes = Math.floor(Math.sqrt(dX * dX + dY * dY) / dashLen);
            if (dashes == 0)
            {
                dashes = 1;
            }
            var dash_to_length = dash_length/dashLen
            var space_to_length = 1 - dash_to_length
            var dashX = dX / dashes;
            var dashY = dY / dashes;
            var x1 = start_x;
            var y1 = start_y;

            ctx.moveTo(x1,y1);

            var q = 0;
            while (q++ < dashes) {
                x1 += dashX*dash_to_length;
                y1 += dashY*dash_to_length;
                ctx.lineTo(x1, y1);
                x1 += dashX*space_to_length;
                y1 += dashY*space_to_length;
                ctx.moveTo(x1, y1);

            }

        }

        ctx.stroke();

    }
}
