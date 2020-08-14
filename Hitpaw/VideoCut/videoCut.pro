QT       += core gui quick quickwidgets network websockets multimedia
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
    src/MessageTip/messagetipbox.cpp \
    src/cmenu.cpp \
    src/framelesswindow/DarkStyle.cpp \
    src/framelesswindow/dialogmanager.cpp \
    src/framelesswindow/framelesswindow.cpp \
    src/framelesswindow/shadowwidget.cpp \
    src/framelesswindow/traywidget.cpp \
    src/globalStyle.cpp \
    src/main.cpp \
    src/mainwindow.cpp \
    src/qmlwidgetcreator.cpp

HEADERS += \
    src/MessageTip/messagetipbox.h \
    src/cmenu.h \
    src/framelesswindow/DarkStyle.h \
    src/framelesswindow/dialogmanager.h \
    src/framelesswindow/framelesswindow.h \
    src/framelesswindow/shadowwidget.h \
    src/framelesswindow/traywidget.h \
    src/globalStyle.h \
    src/mainwindow.h \
    src/qmlwidgetcreator.h

FORMS += \
    src/framelesswindow/framelesswindow.ui
include(src/ImagesModule/ImagesModule.pri)
# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target

INCLUDEPATH += $$PWD/src/framelesswindow
INCLUDEPATH += $$PWD/src/MessageTip
RESOURCES += \
    src/framelesswindow/framelesswindow.qrc \
    img.qrc \
    qml.qrc

INCLUDEPATH += $$PWD/src
DEPENDPATH += $$PWD/src

win32: LIBS += -L$$PWD/lib/processingModuleProxyH/ -lProcessingModuleProxy
INCLUDEPATH += $$PWD/lib/processingModuleProxyH
DEPENDPATH += $$PWD/lib/processingModuleProxyH
win32:!win32-g++: PRE_TARGETDEPS += $$PWD/lib/processingModuleProxyH/ProcessingModuleProxy.lib

LIBS += $$PWD/third-party/ffmpeg/4.2.2-win32-shared/lib/avcodec.lib
LIBS +=$$PWD/third-party/ffmpeg/4.2.2-win32-shared/lib/avformat.lib
LIBS +=$$PWD/third-party/ffmpeg/4.2.2-win32-shared/lib/avutil.lib
LIBS +=$$PWD/third-party/ffmpeg/4.2.2-win32-shared/lib/swresample.lib
LIBS +=$$PWD/third-party/ffmpeg/4.2.2-win32-shared/lib/swscale.lib
LIBS +=$$PWD/third-party/ffmpeg/4.2.2-win32-shared/lib/avfilter.lib

win32:CONFIG(release, debug|release): LIBS += -L$$PWD/module/MediaPlayer/win32/ -lAxe
else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/module/MediaPlayer/win32/ -lAxed
else:unix: LIBS += -L$$PWD/module/MediaPlayer/win32/ -lAxe

INCLUDEPATH += $$PWD/module/MediaPlayer/win32
DEPENDPATH += $$PWD/module/MediaPlayer/win32

win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$PWD/module/MediaPlayer/win32/libAxe.a
else:win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$PWD/module/MediaPlayer/win32/libAxed.a
else:win32:!win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$PWD/module/MediaPlayer/win32/Axe.lib
else:win32:!win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$PWD/module/MediaPlayer/win32/Axed.lib
else:unix: PRE_TARGETDEPS += $$PWD/module/MediaPlayer/win32/libAxe.a
