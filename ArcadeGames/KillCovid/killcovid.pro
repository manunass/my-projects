QT += widgets

INCLUDEPATH  += \
    ../Game1 \
    ../game2

SOURCES += \
    login.cpp \
    main.cpp \
    mainmenu.cpp \
    signupform.cpp \
    ../Game1/doctor.cpp \
    ../Game1/endmenu.cpp \
    ../Game1/game1menu.cpp \
    ../Game1/game1scene.cpp \
    ../Game1/health.cpp \
    ../Game1/needle.cpp \
    ../Game1/score.cpp \
    ../Game1/virus.cpp \
    ../game2/game2endmenu.cpp \
    ../game2/game2menu.cpp \
    ../game2/game2scene.cpp \
    ../game2/piece.cpp \
    ../game2/player.cpp

HEADERS += \
    login.h \
    mainmenu.h \
    signupform.h \
    ../Game1/doctor.h \
    ../Game1/endmenu.h \
    ../Game1/game1menu.h \
    ../Game1/game1scene.h \
    ../Game1/health.h \
    ../Game1/needle.h \
    ../Game1/score.h \
    ../Game1/virus.h \
    ../game2/game2endmenu.h \
    ../game2/game2menu.h \
    ../game2/game2scene.h \
    ../game2/piece.h \
    ../game2/player.h

RESOURCES += \
    ../Game1/game1resources.qrc \
    ../game2/game2resources.qrc \
    mainresources.qrc
