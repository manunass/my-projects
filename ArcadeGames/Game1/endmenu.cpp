/**
* \file endmenu.cpp
* \brief contain the endmenu UI and methods
*
* implements UI for game1 endmenu, can start a new game or return to game1menu
*
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "endmenu.h"

EndMenu::EndMenu(QWidget *parent, bool isWin, int currentScore, QString currentUser) : QWidget(parent)
{
    user = currentUser;
    score = currentScore;
    GridL = new QGridLayout;
    if (isWin) resultL = new QLabel("Congratulations, you killed COVID-19!");
    else resultL = new QLabel("Oh no, you caught COVID-19!");
    if (saveHighScore()) scoreL = new QLabel("Score: " + QString::number(score) + " | New High!");
    else scoreL = new QLabel("Score: " + QString::number(score) + " | You can do better");
    replaygameB = new QPushButton("Replay");
    exitB = new QPushButton("Close");

    QObject::connect(replaygameB, SIGNAL(clicked(bool)), this, SLOT(startGame()));
    QObject::connect(exitB, SIGNAL(clicked(bool)), this, SLOT(endAll()));

    SetGridLayout();
}

void EndMenu::SetGridLayout()
{

    GridL->addWidget(resultL, 0, 1);
    GridL -> addWidget(scoreL, 1, 1);
    GridL->addWidget(replaygameB, 1, 0);
    GridL->addWidget(exitB, 1, 2);
    setLayout(GridL);

    return;
}

bool EndMenu::saveHighScore()
{
    if (user == nullptr) return false;
    bool newHigh = false;
    QFile users(":/users.txt");

    if (!users.open(QIODevice::ReadOnly))
    {
        QString errMsg = users.errorString();
        QFileDevice::FileError err = users.error();
        qDebug() << "Read error: " << err;
        return false;
    }

    QFile newUsers("../KillCovid/TEMPusers.txt");

    if (!newUsers.open(QIODevice::WriteOnly))
    {
        QString errMsg = users.errorString();
        QFileDevice::FileError err = users.error();
        qDebug() << "Write error: " << err;
    }

    QTextStream in(&users);
    QTextStream out(&newUsers);
    while (!in.atEnd())
    {
        QString currentLine = in.readLine();
        QStringList currentLineSplit = currentLine.split(" ");

        if (currentLineSplit.at(0) == user)
        {
            //qDebug() << currentLine;
            if(currentLineSplit.at(5) == "na" or currentLineSplit.at(5).toInt() < score)
            {
                newHigh = true;
                out << currentLineSplit.at(0) << " " << currentLineSplit.at(1) << " " << currentLineSplit.at(2) << " " << currentLineSplit.at(3) <<
                     " " << currentLineSplit.at(4) << " " << score << " " << currentLineSplit.at(6) << " " << currentLineSplit.at(7) << "\n";
            }
        }
        else out << currentLine << "\n";
    }

    users.remove();
    newUsers.rename("../KillCovid/users.txt");
    return newHigh;
}

void EndMenu::startGame()
{
    Game1scene *scene1 = new Game1scene(user);
    QGraphicsView *view = new QGraphicsView();
    view->setScene(scene1);
    view->setFixedSize(750, 800);
    view->setHorizontalScrollBarPolicy((Qt::ScrollBarAlwaysOff));
    view->setVerticalScrollBarPolicy((Qt::ScrollBarAlwaysOff));
    view->show();
    close();
    delete this;
}

void EndMenu::endAll()
{
    close();
    delete this;
}
