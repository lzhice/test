QT       += core gui quick quickwidgets network websockets
greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

CONFIG += c++11

# The following define makes your compiler emit warnings if you use
# any Qt feature that has been marked deprecated (the exact warnings
# depend on your compiler). Please consult the documentation of the
# deprecated API in order to know how to port your code away from it.
DEFINES += QT_DEPRECATED_WARNINGS

# You can also make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
# You can also select to disable deprecated APIs only up to a certain version of Qt.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    framelesswindow/DarkStyle.cpp \
    framelesswindow/framelesswindow.cpp \
    framelesswindow/traywidget.cpp \
    globalStyle.cpp \
    main.cpp \
    mainwindow.cpp \
    qmlwidgetcreator.cpp

HEADERS += \
    framelesswindow/DarkStyle.h \
    framelesswindow/framelesswindow.h \
    framelesswindow/traywidget.h \
    globalStyle.h \
    mainwindow.h \
    qmlwidgetcreator.h

FORMS += \
    framelesswindow/framelesswindow.ui

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target

INCLUDEPATH += $$PWD/framelesswindow

RESOURCES += \
    framelesswindow.qrc \
    img.qrc \
    qml.qrc

win32:CONFIG(release, debug|release): LIBS += -L$$PWD/lib/ -lProcessingModuleProxy
else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/lib/ -lProcessingModuleProxyd

INCLUDEPATH += $$PWD/lib/processingModuleProxyH
DEPENDPATH += $$PWD/lib/processingModuleProxyH

win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$PWD/lib/libProcessingModuleProxy.a
else:win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$PWD/lib/libProcessingModuleProxyd.a
else:win32:!win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$PWD/lib/ProcessingModuleProxy.lib
else:win32:!win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$PWD/lib/ProcessingModuleProxyd.lib
