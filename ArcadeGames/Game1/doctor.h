/**
* \file doctor.h
* \brief contains doctor initialisation
*/

#ifndef ARROW_H
#define ARROW_H
#include <QWidget>
#include <QGraphicsPixmapItem>

class Doctor: public QObject, public QGraphicsPixmapItem
{
    Q_OBJECT
public:
    explicit Doctor(QObject *parent = nullptr);
};

#endif // ARROW_H
