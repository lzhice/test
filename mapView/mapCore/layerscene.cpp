#include "layerscene.h"

LayerScene::LayerScene(QObject *parent):QGraphicsScene(parent)
{

}

void LayerScene::setBackground(ImageMapItem *backgroundItem)
{
    m_backgroundItem=backgroundItem;
    this->addItem(m_backgroundItem);
}

void LayerScene::reSize()
{
    setSceneRect(m_backgroundItem->boundingRect());
}
