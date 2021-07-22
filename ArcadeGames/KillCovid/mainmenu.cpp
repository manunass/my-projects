/**
* \file mainmenu.cpp
* \brief contains mainmenu class definition
*
*this class initialise the main menu and redirects you to the menus of game 1 or 2
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "mainmenu.h"

MainMenu::MainMenu(QWidget *parent, QString currentUser) : QWidget(parent)
{
    user = currentUser;
    setWindowTitle("Menu");
    Game1B = new QPushButton("Play Kill Covid");
    QObject::connect(Game1B, SIGNAL(clicked()), this, SLOT(playGame1()));
    Game2B = new QPushButton("Play Reversi");
    QObject::connect(Game2B, SIGNAL(clicked()), this, SLOT(playGame2()));

    ProfileB = new QPushButton("View Profile");
    QObject::connect(ProfileB, SIGNAL(clicked()), this, SLOT(showProfile()));

    GridL = new QGridLayout();
    SetGridLayout();
}
 void MainMenu::SetGridLayout()
 {
     GridL->addWidget(Game1B,1,2);
     GridL->addWidget(Game2B,2,2);
     if (user != nullptr) GridL->addWidget(ProfileB,0,3);

     setLayout(GridL);
 }

 void MainMenu::playGame1()
 {
     game1menu *game1 = new game1menu(nullptr, user);
     game1->show();
     close();
     delete this;
 }

 void MainMenu::playGame2()
 {
     game2menu *game2 = new game2menu(nullptr, user);
     game2->show();
     close();
     delete this;
 }

 void MainMenu::showProfile()
 {
    QMessageBox *profile = new QMessageBox();
    profile->setWindowTitle(user + "'s Profile");

    QString userInfo;

    QFile users(":/users.txt");
    if (!users.open(QIODevice::ReadOnly))
    {
        QString errMsg = users.errorString();
        QFileDevice::FileError err = users.error();
        qDebug() << "Reading error: " << errMsg;
        return;
    }

    QTextStream in(&users);

    in.readLine();      //Skip the first line

    while (!in.atEnd())
    {
        QStringList currentLine = in.readLine().split(" ");

        if (currentLine.at(0) == user)
        {
            userInfo.append("Name: " + (QString) currentLine.at(1) + " " + (QString) currentLine.at(2) + "\n");
            userInfo.append("Date of Birth: " + (QString) currentLine.at(3) + "\n");
            userInfo.append("Gender: " + (QString) currentLine.at(4) + "\n");
            if (currentLine.at(5) != "na") userInfo.append("High Score for Kill Covid: " + (QString) currentLine.at(5) + "\n");
            if (currentLine.at(6) != "na") userInfo.append("High Score for Reversi: " + (QString) currentLine.at(6));
            break;
        }
    }

    profile->setText(userInfo);
    profile->show();
 }
