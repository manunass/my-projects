#ifndef GAME2MENU_H
#define GAME2MENU_H

#include <QGraphicsScene>
#include <QGraphicsItem>
#include <QtWidgets>
#include <QWidget>
#include <game2scene.h>

class game2menu : public QWidget
{
    Q_OBJECT
public:
    explicit game2menu(QWidget *parent = nullptr, QString currentUser = nullptr);
    void SetGridLayout();

public slots:
    void startGame();
    void endAll();
    void displayInfo();

private:
    QPushButton *informationB;
    QPushButton *startgameB;
    QPushButton *returnB;
    QGridLayout *GridL;

    QString user;


signals:

};

#endif // GAME2MENU_H
