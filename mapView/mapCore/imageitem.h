#ifndef ImageMapItem_H
#define ImageMapItem_H
#include <QImageReader>
#include <QGraphicsItem>
#include <QSize>
class ImageItem : public QGraphicsItem
{
public:
    explicit ImageItem(const QString& file="", QGraphicsItem *parent = nullptr);
    explicit ImageItem(const QByteArray &bytes, QGraphicsItem *parent = nullptr);
    void setScene(QGraphicsScene *pScene);
    void setImageBytes(const QByteArray &bytes);

    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
    QRectF boundingRect() const override;
    QByteArray getRawData();
private:

    QByteArray m_bytes;
    QSizeF m_size;
    QSize m_scaledSize=QSize(-1,-1);
    QImage m_cacheImage;
};

class ImageMapItem : public QGraphicsItem
{
public:
    explicit ImageMapItem (const QString& file="");
    explicit ImageMapItem(const QByteArray &bytes);
    void setImageBytes(const QByteArray &bytes);
    void setImageBytes(const QString file);
    void paint(QPainter *painter, const QStyleOptionGraphicsItem *option, QWidget *widget) override;
    QRectF boundingRect() const override;
    QByteArray getRawData();
    static ImageMapItem* createItem(const QByteArray &bytes);
private:
    QSizeF m_size;
    QHash<QPair<quint32,quint32>,ImageItem *> m_ImageItemGridTbl;
    ImageMapItem(QSizeF size,QHash<QPair<quint32,quint32>,QByteArray> dataBytesTbl);
};
#endif // ImageMapItem_H
