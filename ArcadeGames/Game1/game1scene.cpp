/**
* \file game1scene.cpp
** \brief implements game1
*
*implements all the methods needed for game1 from when its launched untill its over.
*
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "game1scene.h"
#include <vector>
#include <stdlib.h>
Game1scene::Game1scene(QString currentUser)
{
    user = currentUser;
    srand(time(NULL));
    doc = new Doctor();
    doc->setPixmap((QPixmap(":/Images/doctor.png")).scaled(150,171));
    doc->setFlag(QGraphicsItem::ItemIsFocusable);
    doc->setFocus();
    addItem(doc);
    setBackgroundBrush(QBrush(QImage(":/Images/background.png").scaledToHeight(800).scaledToWidth(750)));
    setSceneRect(0, 0, 750, 800);
    score = new Score();
    addItem(score);
    health = new Health();
    health->setPos(health->x(),health->y()+25);
    addItem(health);
    needleCooldown = new QTime;
    needleCooldown->start();
    needleCooldown->setHMS(0, 0, 0, 250);

    SetViruses();
    viruscounter=0;
    timer_new_virus= new QTimer;
    connect(timer_new_virus, SIGNAL(timeout()), this,SLOT(spawnVirus()));
    timer_new_virus->start(3000);

}

/**
* \brief spawn virusses
*
* checks the virus_distribution, if we are still inbound spawn the corresponding virus in a randomn column,
* connects the newly spawned virus to the slot for their death and to the slot for their going through
* for every 5 virus killed, increase virus spawn speed by 0.5s untill we reach 0.5s per virus
*/
void Game1scene::spawnVirus()
{

    if(viruscounter>=virus_distribution.size() || health->getHealth()<=0)
    {return;}
    virus = new Virus(this, virus_distribution[viruscounter]);
    viruscounter++;
    virus->setPos(150*(1+rand()%4)+15, -120);
    this->addItem(virus);
    virus->start();
    connect(virus, SIGNAL(SmallKill()),this,SLOT(SmallDeath()));
    connect(virus, SIGNAL(MediumKill()),this,SLOT(MediumDeath()));
    connect(virus, SIGNAL(LargeKill()),this,SLOT(LargeDeath()));
    connect(virus, SIGNAL(SmallHit()),this,SLOT(SmallLoss()));
    connect(virus, SIGNAL(MediumHit()),this,SLOT(MediumLoss()));
    connect(virus, SIGNAL(LargeHit()),this,SLOT(LargeLoss()));

    if (viruscounter%5 == 0 && timer_new_virus->interval() > 500)
    {
        timer_new_virus->setInterval(timer_new_virus->interval()-500);
    }
}

/**
* \brief implements key strokes
*
* for spaces, shoot a needle,
*for left and right keys, move the doctor to the corresponding row if possible
*/
void Game1scene::keyPressEvent(QKeyEvent *event)
{
    if(event->key() == Qt::Key_Space && needleCooldown->elapsed() > 250)
    {
        needleCooldown->restart();
        needle = new Needle();
        needle->setPixmap((QPixmap(":/Images/needle.png")).scaled(33,93));
        needle->setPos(doc->x()+15, doc->y()+27);
        addItem(needle);
    }
    if(event->key() == Qt::Key_Right && doc->x() < 600) { doc->setPos(doc->x()+150, doc->y()); }
    if(event->key() == Qt::Key_Left && doc->x() > 0) { doc->setPos(doc->x()-150, doc->y()); }
    return;
}

/**
* \brief generates the size of virusses that will spawn.
*
* generates virus size randomnly until we get over 138 points,
*then choose between the preset configuration to reach 150
*/
void Game1scene::SetViruses()
{
    QVector<QVector<int>> combination = { {7,3},{3,3,3},{3,5},{7},{3,3},{7,5},{5,3,3}};
    int values[3]={3,5,7};
    int sum=0;
    while(sum<138)
    {
        int x= values[rand()%3];
         sum +=x;
         virus_distribution.push_back(x);
    }
  int x= sum%7;
  for (int i=0; i<combination[x].size();i++)
  {virus_distribution.push_back(combination[x][i]);}

}

void Game1scene::WeWin()
{

    EndMenu *end_menu= new EndMenu(nullptr, true, score->getScore(), user);
    end_menu->show();
    views().at(0)->hide();
    delete this;
}
void Game1scene::WeLose()
{
    clear();
    EndMenu *end_menu= new EndMenu(nullptr, false, score->getScore(), user);
    end_menu->show();
    views().at(0)->hide();
    delete this;
}

void Game1scene::SmallDeath()
{
    score->increase(3);
    if (score->getScore()==150)
       {WeWin();}
}

void Game1scene::MediumDeath()
{   score->increase(5);
    if (score->getScore()==150)
       {WeWin();}
}

void Game1scene::LargeDeath()
{
    score->increase(7);
    if (score->getScore()==150)
       {WeWin();}
}
void Game1scene::SmallLoss()
{
    if (health->getHealth() > 0) health->decrease();
    virus_distribution.push_back(3);
    if (health->getHealth()<=0)
       {WeLose();}
}

void Game1scene::MediumLoss()
{
    if (health->getHealth() > 0) health->decrease();
    virus_distribution.push_back(5);
    if (health->getHealth()<=0)
           {WeLose();}
}

void Game1scene::LargeLoss()
{
    if (health->getHealth() > 0) health->decrease();
    virus_distribution.push_back(7);
    if (health->getHealth()<=0)
           {WeLose();}
}
