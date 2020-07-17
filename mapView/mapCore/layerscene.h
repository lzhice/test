#ifndef LAYERSCENE_H
#define LAYERSCENE_H

#include <QGraphicsScene>
#include "imageitem.h"
class LayerScene : public QGraphicsScene
{
public:
    LayerScene(QObject *parent = nullptr);
    void setBackground(ImageMapItem * backgroundItem);
    ImageMapItem * getBackground(){return m_backgroundItem;}
    void reSize();
private:
    ImageMapItem * m_backgroundItem=NULL;
};

#endif // LAYERSCENE_H
