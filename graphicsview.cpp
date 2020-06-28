#include "graphicsview.h"
#include <QDebug>
#include<QScreen>
#include <QGuiApplication>
#include "layerscene.h"
GraphicsView::GraphicsView(QGraphicsScene *scene, QWidget *parent): QGraphicsView(parent)
{
    setScene(scene);
    setDragMode(QGraphicsView::RubberBandDrag);
    //setRenderHints(QPainter::HighQualityAntialiasing | QPainter::SmoothPixmapTransform);
    setViewportUpdateMode(QGraphicsView::FullViewportUpdate);
    setRenderHint(QPainter::Antialiasing, false);
    setOptimizationFlags(QGraphicsView::DontSavePainterState);
    setTransformationAnchor(QGraphicsView::AnchorUnderMouse);
    connect(this, &GraphicsView::rubberBandChanged,this, &GraphicsView::onRubberBandChanged);


}
void GraphicsView::zoomFit()
{
    QGraphicsScene *pscene=scene();
    if(pscene){
        fitInView(pscene->sceneRect(),Qt::KeepAspectRatio);
    }
    qDebug()<<this->width()<<this->height();
}

void GraphicsView::wheelEvent(QWheelEvent *event)
{
    m_isScale=true;
    if (event->delta() > 0){
        scale(1.15, 1.15);
    }else{
        scale(0.85, 0.85);
    }
    event->accept();
}

void GraphicsView::paintEvent(QPaintEvent *event)
{
    QGraphicsView::paintEvent(event);
}

void GraphicsView::resizeEvent(QResizeEvent *event)
{
    if(!m_isScale)zoomFit();
    QGraphicsView::resizeEvent(event);
}
#include <QFileDialog>
static QByteArray loadRawData(const QString &filePath)
{
    QFile file(filePath);
    if (file.open(QIODevice::ReadOnly)) {
        return file.readAll();
    }
    return QByteArray();
}

static void writeRawData(const QString &filePath,const QByteArray & bytes){
    QFile file(filePath);
    if (file.open(QIODevice::WriteOnly|QIODevice::Truncate)) {
        QDataStream stream(&file);
        stream<<bytes;
    }
}

void GraphicsView::mousePressEvent(QMouseEvent *event)
{
    QGraphicsView::mousePressEvent(event);
    if(event->button() == Qt::RightButton){
//        if(RubberBandDrag==dragMode()){
//            setDragMode(ScrollHandDrag);
//        }
        //setDragMode(ScrollHandDrag);
        QString file = QFileDialog::getOpenFileName(this, tr("选择文件"),"");
        toImage(file);
//        QGraphicsScene *pLayerScene=dynamic_cast<QGraphicsScene *>(scene());
//        if(pLayerScene){
//            if(pLayerScene->getBackground()){
//                QString file = QFileDialog::getOpenFileName(this, tr("选择文件"),"");
//                if(file!=""){
//                    //pLayerScene->getBackground()->setImageBytes(file);

//                    //loadRawData(file);
//                    //writeRawData(file,pLayerScene->getBackground()->getRawData());
//                    //pLayerScene->reSize();
//                    //zoomFit();
//                    toImage(file);
//                }

//            }
//        }

    }
}

void GraphicsView::mouseReleaseEvent(QMouseEvent *event)
{

    QGraphicsView::mouseReleaseEvent(event);
    if(event->button() == Qt::RightButton){
        setDragMode(RubberBandDrag);
    }/*else{
        setDragMode(ScrollHandDrag);
    }*/

}

void GraphicsView::toImage(const QString &path)
{
    QGraphicsScene *scene=this->scene();
    scene->clearSelection();
    scene->setSceneRect(scene->itemsBoundingRect());
    const QSize imgSize=scene->sceneRect().size().toSize();
    QSize imgSize1=QSize(imgSize.width()*2,imgSize.height()*2);
    QImage image(imgSize1, QImage::Format_ARGB32);
    image.fill(Qt::transparent);

    QPainter painter(&image);
    scene->render(&painter);
    image.save(path);
}

void GraphicsView::onRubberBandChanged(QRect viewportRect, QPointF fromScenePoint, QPointF toScenePoint)
{
    //qDebug()<<"onRubberBandChanged:"<<viewportRect<<fromScenePoint<<toScenePoint;
}

