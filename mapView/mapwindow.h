#ifndef MAPWINDOW_H
#define MAPWINDOW_H

#include <QWidget>
#include <QGraphicsView>
class LayerScene;
class MapWindow : public QWidget
{
    Q_OBJECT

public:
    MapWindow(QWidget *parent = nullptr);
    ~MapWindow();
private:
    QGraphicsScene *m_mapScene;
    QGraphicsView *m_mapView;
};
#endif // MAPWINDOW_H
