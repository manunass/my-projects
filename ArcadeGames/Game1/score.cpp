/**
* \file score.cpp
* \brief implements score class
* implements the methods necessary for the score to be displayed and updated
* \author Nicolas Sassine & Emmanuel Nassour
*/


#include "score.h"
#include <QFont>

Score::Score(QGraphicsItem *parent): QGraphicsTextItem(parent){
    // initialize the score to 0
    score = 0;

    // draw the text
    setPlainText(QString("Score: ") + QString::number(score)); // Score: 0
    setDefaultTextColor(Qt::yellow);
    setFont(QFont("times",16));
}

void Score::increase(int x){
    score+= x;
    setPlainText(QString("Score: ") + QString::number(score)); // Score: 1
}

int Score::getScore(){
    return score;
}
