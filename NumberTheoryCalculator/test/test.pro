QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

CONFIG += c++11

# You can make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    chinrem.cpp \
    fast_exp.cpp \
    main.cpp \
    mainwindow.cpp \
    mr_algo.cpp \
    prime_fact.cpp \
    totfunc.cpp

HEADERS += \
    chinrem.h \
    fast_exp.h \
    mainwindow.h \
    mr_algo.h \
    prime_fact.h \
    totfunc.h

FORMS += \
    chinrem.ui \
    fast_exp.ui \
    mainwindow.ui \
    mr_algo.ui \
    prime_fact.ui \
    totfunc.ui

TRANSLATIONS += \
    test_en_US.ts

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target
