#ifndef JSONHELP_H
#define JSONHELP_H

#include <QVariantMap>
struct JsonHelp
{
    static QString getJsonString(const QVariantMap &varMap);
    static QVariantMap getVarMap(const QString &strJson);
};

#endif // JSONHELP_H