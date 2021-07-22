/**
* \file needle.cpp
* \brief implements needle class
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "needle.h"
#include "score.h"
#include "doctor.h"
#include "game1scene.h"

extern Game1scene scene1;

Needle::Needle(QObject *parent) : QObject(parent)
{
    timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(moveUp()));
    timer->start(20);
}

void Needle::moveUp()
{
    setPos(x(), y()-20);
    if (y() < -93)
    {
        scene()->removeItem(this);
        delete this;
    }
}
