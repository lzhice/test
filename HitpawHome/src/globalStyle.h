#ifndef GlobalStyle_H
#define GlobalStyle_H
#include <QColor>
#include <QObject>
#include <QHash>
class GlobalStyle : public QObject
{
    Q_OBJECT
public:
    static GlobalStyle * getInstance(){static GlobalStyle _this;return &_this;}
    Q_INVOKABLE QColor getColor(const QString& name, const QColor& color=QColor() ){return m_colorTbl.contains(name)?m_colorTbl[name]:color;}
    Q_INVOKABLE void setColor(const QString & name,const QColor& color){m_colorTbl.insert(name,color);}

    Q_INVOKABLE int getFontSize(const QString& name,int val=0){return m_fontSizeTbl.contains(name)?m_fontSizeTbl[name]:val;}
    Q_INVOKABLE void setFontSize(const QString & name,int val){m_fontSizeTbl.insert(name,val);}

    Q_INVOKABLE QString getImgPath(const QString& name,const QString& path=QString("")){return m_imgPathTbl.contains(name)?m_imgPathTbl[name]:path;}
    Q_INVOKABLE void setImgPath(const QString & name,const QString& path){m_imgPathTbl.insert(name,path);}

private:
    explicit GlobalStyle(QObject *parent = nullptr);
    QHash<QString,QColor> m_colorTbl;
    QHash<QString,int> m_fontSizeTbl;
    QHash<QString,QString> m_imgPathTbl;
};
#endif // GlobalStyle_H
