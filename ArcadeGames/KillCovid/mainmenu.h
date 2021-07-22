/**
* \file mainmenu.h
* \brief mainmenu class
*
* Longer description goes here.
*/

#ifndef MAINMENU_H
#define MAINMENU_H

#include <QtWidgets>
#include <QObject>
#include <game1menu.h>
#include <game2menu.h>
#include <QMessageBox>

class MainMenu : public QWidget
{
    Q_OBJECT
public:
    explicit MainMenu(QWidget *parent = nullptr, QString currentUser = nullptr);
    void SetGridLayout();

public slots:
    void playGame1();
    void playGame2();
    void showProfile();

private:

    QPushButton *Game1B;
    QPushButton *Game2B;
    QPushButton *ProfileB;

    QGridLayout *GridL;

    QString user;

signals:

};

#endif // MAINMENU_H
