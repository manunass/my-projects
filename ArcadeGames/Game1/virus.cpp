/**
* \file virus.cpp
* \brief implements the virusses
*
* implements three type of virusses, small medium and large each with respective color and fall speed.
* implements the falling function, the collision functionand emit their death and hits.
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "virus.h"
#include "game1scene.h"

Virus::Virus(QObject *parent, int vpoint): QObject(parent)
{
    value=vpoint;
    dead = false;

    if (vpoint== 3)
     {
        size=small;
        setPixmap(QPixmap (":/Images/white_virus.png").scaled(120,120));
    }
    else if(vpoint==5)
    {
          size=medium;
          setPixmap(QPixmap (":/Images/red_virus.png").scaled(120,120));
    }
    else if(vpoint==7)
    {
            size=large;
            setPixmap(QPixmap (":/Images/black_virus.png").scaled(120,120));
    }

     speed = vpoint;

}



/**
* \brief time the virus falling down
*
* every 20ms, calls fall_down
*/
void Virus::start()
{
    timer = new QTimer;
    connect(timer, SIGNAL(timeout()), this,SLOT(fall_down()));
    timer->start(20);
}

/**
* \brief makes virus fall
*
* if dead, shrink virus until removal.
* else if virus is in collision with a needle, call kill and remove colliding needle.
* else if we reached the bottom, call hit and delete virusses.
* else move virus down 'speed' pixels
*/
void Virus::fall_down()
{

    if (dead)
    {
        if (pixmap().width() > 0) {setPixmap(pixmap().scaled(pixmap().width()-5, pixmap().height()-5));}
        else
        {
            scene()->removeItem(this);
            delete this;
        }
        return;
    }

    QList<QGraphicsItem *> colliding_items = collidingItems();

        for (int i = 0, n = colliding_items.size(); i < n; ++i)
       {
            if(typeid(*(colliding_items[i])) == typeid(Needle))
            {
                Kill();
                scene()->removeItem(colliding_items[i]);
                delete colliding_items[i];
                return;
            }
        }


    if(this->y() > 800){
        scene()->removeItem(this);
        Hit();
        delete this;
        Hit();
    }
    else this->setPos(this->x(), this->y() + speed);
}

void Virus::Kill()
{
    dead = true;
    if(value==3)
        emit SmallKill();
    if(value==5)
        emit MediumKill();
    if(value==7)
        emit LargeKill();
}

void Virus::Hit()
{
    if(value==3)
    {emit SmallHit();}
    if(value==5)
    {emit MediumHit();}
    if(value==7)
    {emit LargeHit();}
}
