/**
* \file needle.h
* \brief implements needle class
*/


#ifndef NEEDLE_H
#define NEEDLE_H

#include <QObject>
#include <QGraphicsPixmapItem>
#include <QGraphicsScene>
#include <QTimer>

class Needle : public QObject, public QGraphicsPixmapItem
{
    Q_OBJECT
public:
    explicit Needle(QObject *parent = nullptr);

public slots:
    void moveUp();

private:
    QTimer *timer;

signals:

};

#endif // NEEDLE_H
