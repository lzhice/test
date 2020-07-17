#include "pixmanager.h"
#include <QPainter>
#include <QDebug>
QPixmap *PixManager::getPixmap(const QString &name)
{
    if(m_pixTbl.contains(name)){
        return  &m_pixTbl[name];
    }return NULL;
}

void PixManager::setPixmap(const QString &name, const QPixmap &pix)
{
    m_pixTbl.insert(name,pix);
}

QSvgRenderer *PixManager::getSvgRender(const QString &name)
{
    if(m_svgTbl.contains(name)){
        return  m_svgTbl[name];
    }return NULL;
}

void PixManager::setSvgRender(const QString &name, QByteArray bytes)
{
    QSvgRenderer * pSvgRenderer=NULL;
    if(m_svgTbl.contains(name)){
        pSvgRenderer=  m_svgTbl[name];
    }if(!pSvgRenderer){
        pSvgRenderer= new QSvgRenderer;
    }

    pSvgRenderer->load(bytes);
    m_svgTbl.insert(name,pSvgRenderer);
    setPixmap(name,Svg2Pixmap(pSvgRenderer));
}

void PixManager::setSvgRender(const QString &name, const QString &file)
{
    QSvgRenderer * pSvgRenderer=NULL;
    if(m_svgTbl.contains(name)){
        pSvgRenderer=  m_svgTbl[name];
    }if(!pSvgRenderer){
        pSvgRenderer= new QSvgRenderer;
    }
    pSvgRenderer->load(file);
    m_svgTbl.insert(name,pSvgRenderer);
    setPixmap(name,Svg2Pixmap(pSvgRenderer));
}

QPixmap PixManager::Svg2Pixmap(QSvgRenderer * pSvgRenderer, QSize size)
{
    QRectF rect(0.0f, 0.0f, size.width(), size.height());
    QPixmap pix(rect.width(), rect.height());
    pix.fill(QColor(0,0,0,0));
    QPainter painter;
//      painter.setCompositionMode(QPainter::CompositionMode_Destination);
    painter.begin(&pix);
    pSvgRenderer->render(&painter, rect);
    painter.end();
    return  pix;
}

