/****************************************************************************
** Meta object code from reading C++ file 'movewidget.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.12.8)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../movewidget.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'movewidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.12.8. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_MoveWidget_t {
    QByteArrayData data[7];
    char stringdata0[65];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_MoveWidget_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_MoveWidget_t qt_meta_stringdata_MoveWidget = {
    {
QT_MOC_LITERAL(0, 0, 10), // "MoveWidget"
QT_MOC_LITERAL(1, 11, 10), // "sigVisible"
QT_MOC_LITERAL(2, 22, 0), // ""
QT_MOC_LITERAL(3, 23, 1), // "v"
QT_MOC_LITERAL(4, 25, 11), // "slotVisible"
QT_MOC_LITERAL(5, 37, 13), // "arrows_update"
QT_MOC_LITERAL(6, 51, 13) // "onDelayUpdate"

    },
    "MoveWidget\0sigVisible\0\0v\0slotVisible\0"
    "arrows_update\0onDelayUpdate"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_MoveWidget[] = {

 // content:
       8,       // revision
       0,       // classname
       0,    0, // classinfo
       5,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: name, argc, parameters, tag, flags
       1,    1,   39,    2, 0x06 /* Public */,

 // slots: name, argc, parameters, tag, flags
       4,    1,   42,    2, 0x0a /* Public */,
       4,    0,   45,    2, 0x2a /* Public | MethodCloned */,
       5,    0,   46,    2, 0x09 /* Protected */,
       6,    0,   47,    2, 0x09 /* Protected */,

 // signals: parameters
    QMetaType::Void, QMetaType::Bool,    3,

 // slots: parameters
    QMetaType::Void, QMetaType::Bool,    3,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,

       0        // eod
};

void MoveWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        auto *_t = static_cast<MoveWidget *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->sigVisible((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 1: _t->slotVisible((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 2: _t->slotVisible(); break;
        case 3: _t->arrows_update(); break;
        case 4: _t->onDelayUpdate(); break;
        default: ;
        }
    } else if (_c == QMetaObject::IndexOfMethod) {
        int *result = reinterpret_cast<int *>(_a[0]);
        {
            using _t = void (MoveWidget::*)(bool );
            if (*reinterpret_cast<_t *>(_a[1]) == static_cast<_t>(&MoveWidget::sigVisible)) {
                *result = 0;
                return;
            }
        }
    }
}

QT_INIT_METAOBJECT const QMetaObject MoveWidget::staticMetaObject = { {
    &MarginWidget::staticMetaObject,
    qt_meta_stringdata_MoveWidget.data,
    qt_meta_data_MoveWidget,
    qt_static_metacall,
    nullptr,
    nullptr
} };


const QMetaObject *MoveWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *MoveWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_MoveWidget.stringdata0))
        return static_cast<void*>(this);
    return MarginWidget::qt_metacast(_clname);
}

int MoveWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = MarginWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 5)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 5;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 5)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 5;
    }
    return _id;
}

// SIGNAL 0
void MoveWidget::sigVisible(bool _t1)
{
    void *_a[] = { nullptr, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
