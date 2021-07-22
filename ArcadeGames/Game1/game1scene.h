/**
* \file game1scene.h
** \brief implements game1scene
*
*implements all the methods needed for game1 from when its launched untill its over.
*
*/

#ifndef GAME1SCENE_H
#define GAME1SCENE_H

#include <QObject>
#include <QGraphicsTextItem>
#include <QWidget>
#include <endmenu.h>
#include <QGraphicsScene>
#include <QGraphicsView>
#include <QFont>
#include <needle.h>
#include <virus.h>
#include <doctor.h>
#include <health.h>
#include <score.h>
#include <QKeyEvent>
#include <QTimer>
#include <QTime>

class Game1scene : public QGraphicsScene
{
    Q_OBJECT
public:
    Game1scene(QString currentUser = nullptr);
    void keyPressEvent(QKeyEvent *event);
    void SetViruses();

public slots:
    void spawnVirus();
    void WeWin();
    void WeLose();
    void SmallDeath();
    void MediumDeath();
    void LargeDeath();
    void SmallLoss();
    void MediumLoss();
    void LargeLoss();

private:
    QGraphicsTextItem *finish_message;
    Needle *needle;
    Doctor *doc;
    Virus *virus;
    Health *health;
    Score *score;
    QTimer *timer_new_virus;
    QTime *needleCooldown;
    QVector<int> virus_distribution;
    int viruscounter;

    QString user;
};

#endif // GAME1SCENE_H
