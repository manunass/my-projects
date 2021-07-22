/**
* \file game1menu.h
** \brief implements the game1menu UI and methods
*
*/

#ifndef GAME1MENU_H
#define GAME1MENU_H
#include <QGraphicsScene>
#include <QGraphicsItem>
#include <QtWidgets>
#include <QWidget>
#include <QMessageBox>
#include <virus.h>
#include <game1scene.h>

class game1menu : public QWidget
{
    Q_OBJECT
public:
    explicit game1menu(QWidget *parent = nullptr, QString currentUser = nullptr);
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

#endif // GAME1MENU_H
