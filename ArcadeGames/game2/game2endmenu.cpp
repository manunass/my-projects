#include "game2endmenu.h"

Game2EndMenu::Game2EndMenu(QWidget *parent,int white_score, int black_score, QString currentUser) : QWidget(parent)
{
    user = currentUser;
    GridL = new QGridLayout;
    QString scores = QString::number(white_score) + " to " + QString::number(black_score);
    if(white_score> black_score) resultL = new QLabel("White wins with a score of " + scores + "!");
    else resultL = new QLabel("Black wins with a score of " + scores + "!");
    replaygameB = new QPushButton("Replay");
    exitB = new QPushButton("Close");
    if (saveHighScore(white_score, black_score)) qDebug() << "New high score";

    QObject::connect(replaygameB, SIGNAL(clicked(bool)), this, SLOT(startGame()));
    QObject::connect(exitB, SIGNAL(clicked(bool)), this, SLOT(endAll()));

    SetGridLayout();
}

void Game2EndMenu::SetGridLayout()
{

    GridL->addWidget(resultL, 0, 1);
    GridL->addWidget(replaygameB, 1, 0);
    GridL->addWidget(exitB, 1, 2);
    setLayout(GridL);

    return;
}

bool Game2EndMenu::saveHighScore(int white_score, int black_score)
{
    if (user == nullptr) return false;
    bool newHigh = false;
    int score = std::max(white_score, black_score);
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
            if(currentLineSplit.at(6) == "na" or currentLineSplit.at(6).toInt() < score)
            {
                newHigh = true;
                out << currentLineSplit.at(0) << " " << currentLineSplit.at(1) << " " << currentLineSplit.at(2) << " " << currentLineSplit.at(3) <<
                     " " << currentLineSplit.at(4) << " " << currentLineSplit.at(5) << " " << score << " " << currentLineSplit.at(7) << "\n";
            }
        }
        else out << currentLine << "\n";
    }

    users.remove();
    newUsers.rename("../KillCovid/users.txt");
    return newHigh;
}

void Game2EndMenu::startGame()
{
    Game2Scene *scene1 = new Game2Scene();
    QGraphicsView *view = new QGraphicsView();
    view->setScene(scene1);
    view->setFixedSize(750, 800);
    view->setHorizontalScrollBarPolicy((Qt::ScrollBarAlwaysOff));
    view->setVerticalScrollBarPolicy((Qt::ScrollBarAlwaysOff));
    view->show();
    close();
    delete this;
}

void Game2EndMenu::endAll()
{
    close();
    delete this;
}

