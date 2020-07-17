#ifndef PIXMANAGER_H
#define PIXMANAGER_H
#include <QPixmap>
#include <QHash>
#include <QSvgRenderer>
class PixManager
{

public:
    static PixManager &getInstance(){
        static PixManager _this;
        return _this;
    }

    QPixmap *getPixmap(const QString& name);
    void setPixmap(const QString& name,const QPixmap& pix);

    QSvgRenderer *getSvgRender(const QString& name);

    void setSvgRender(const QString& name,QByteArray bytes);
    void setSvgRender(const QString& name,const QString& file);
private:
    QHash<QString,QPixmap> m_pixTbl;
    QHash<QString,QSvgRenderer*> m_svgTbl;
    PixManager(){}
    QPixmap Svg2Pixmap(QSvgRenderer *pSvgRenderer, QSize size=QSize(100,100));
};


#endif // PIXMANAGER_H
