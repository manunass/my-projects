/**
* \file virus.h
* \brief implements the virusses
*
*/


#ifndef VIRUS_H
#define VIRUS_H

#include <QObject>
#include <QGraphicsPixmapItem>
#include <QTimer>
#include <QGraphicsScene>

enum VirusSize {small, medium, large};


class Virus : public QObject, public QGraphicsPixmapItem
{
    Q_OBJECT
private:
    VirusSize size;
    QTimer *timer;
    int speed;
    int value;
    bool dead;

 public:
    explicit Virus(QObject *parent = nullptr, int points = 5);

public slots:
    void fall_down();
    void start();
    void Kill();
    void Hit();

signals:
    void SmallKill();
    void MediumKill();
    void LargeKill();
    void SmallHit();
    void MediumHit();
    void LargeHit();
};

#endif // VIRUS_H
