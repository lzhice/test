#ifndef GRAPHICVIEW_H
#define GRAPHICVIEW_H

#include <QWheelEvent>
#include <QGraphicsView>
#include <QGLWidget>
#include <QMetaObject>
class GraphicsView : public QGraphicsView
{
    Q_OBJECT
public:
    explicit GraphicsView(QGraphicsScene *scene, QWidget *parent = 0);
    void zoomFit();
protected:
    //void wheelEvent(QWheelEvent *event);
    void paintEvent(QPaintEvent *event);
    void resizeEvent(QResizeEvent *event);

    void mousePressEvent(QMouseEvent *event) ;
    //void mouseMoveEvent(QMouseEvent *event) ;
    void mouseReleaseEvent(QMouseEvent *event) ;
    void toImage(const QString& path);
protected slots:
    void onRubberBandChanged(QRect viewportRect, QPointF fromScenePoint, QPointF toScenePoint);
private:
    bool m_isScale=false;
};

#endif // GRAPHICVIEW_H
