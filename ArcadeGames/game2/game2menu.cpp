#include "game2menu.h"

game2menu::game2menu(QWidget *parent, QString currentUser) : QWidget(parent)
{
    user = currentUser;
    QCoreApplication::setApplicationName("Reversi");
    GridL = new QGridLayout();
        informationB = new QPushButton("Info");
        QObject::connect(informationB, SIGNAL(clicked(bool)), this, SLOT(displayInfo()));
        startgameB = new QPushButton("Start");
        returnB = new QPushButton("Exit");
        QObject::connect(returnB, SIGNAL(clicked(bool)), this, SLOT(endAll()));

        QObject::connect(startgameB, SIGNAL(clicked(bool)), this, SLOT(startGame()));


     SetGridLayout();
}

void game2menu::SetGridLayout()
{

    GridL->addWidget(informationB,1,2);
    GridL->addWidget(startgameB,2,2);
    GridL->addWidget(returnB,0,0);
    //GridL->addItem(virus);
    setLayout(GridL);

    return;
}

void game2menu::startGame()
{
    Game2Scene *scene1 = new Game2Scene(user);
    QGraphicsView *view = new QGraphicsView();
    view->setScene(scene1);
    view->setFixedSize(800, 800);
    view->setHorizontalScrollBarPolicy((Qt::ScrollBarAlwaysOff));
    view->setVerticalScrollBarPolicy((Qt::ScrollBarAlwaysOff));
    view->show();
    close();
    delete this;
}

void game2menu::endAll()
{
    close();
    delete this;
}

void game2menu::displayInfo()
{
    QMessageBox *info = new QMessageBox();
    info->setWindowTitle("Information");
    info->setText("Play the classic game, Reversi, against a friend of yours!\nFlip the other player's pieces to your color by placing your pieces on both ends of a line\nConquer more pieces than the opponent to win");
    info->setDetailedText("Black always begins\nYour possible moves are highlighted\nClick on a square to place a piece\nIf you have no possible moves, your turn is skipped\nThe game ends once both players don't have any possible moves left\nThe player with the highest number of pieces on the board wins");
    info->show();
}
