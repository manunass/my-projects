/**
* \file health.cpp
** \brief implements health
*
* implements method to display and update health
*
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "health.h"
#include <QFont>

Health::Health(QGraphicsItem *parent): QGraphicsTextItem(parent){
    // initialize the score to 0
    health = 3;

    setPlainText(QString("Health: ") + QString::number(health)); // Health: 3
    setDefaultTextColor(Qt::green);
    setFont(QFont("times",16));
}

void Health::decrease(){
    health--;
    setPlainText(QString("Health: ") + QString::number(health)); // Health: 2
}

int Health::getHealth(){
    return health;
}
