/**
* \file doctor.cpp
* \brief contains doctor initialisation
* \author Nicolas Sassine & Emmanuel Nassour
*/
#include "doctor.h"

Doctor::Doctor(QObject *parent) : QObject(parent)
{
    setPos(300, 629);
}
