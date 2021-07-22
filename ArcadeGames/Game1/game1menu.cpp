/**
* \file game1menu.cpp
** \brief implements the game1menu UI and methods
*
*implements UI for game1menu, can start a new game, viewprofile or return to mainmenu
*
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "game1menu.h"

game1menu::game1menu(QWidget *parent, QString currentUser) : QWidget(parent)
{
    user = currentUser;
    QCoreApplication::setApplicationName("Kill Covid");
    GridL = new QGridLayout();
    informationB = new QPushButton("Info");
    QObject::connect(informationB, SIGNAL(clicked(bool)), this, SLOT(displayInfo()));
    startgameB = new QPushButton();
    returnB = new QPushButton("Exit");
    QObject::connect(returnB, SIGNAL(clicked(bool)), this, SLOT(endAll()));

    QPixmap pixmap(":/Images/image001.png");
    QIcon ButtonIcon(pixmap);

    startgameB->setIcon(ButtonIcon);
    startgameB->setIconSize(pixmap.rect().size());
    QObject::connect(startgameB, SIGNAL(clicked(bool)), this, SLOT(startGame()));


 SetGridLayout();

}
void game1menu::SetGridLayout()
{

    GridL->addWidget(informationB,1,2);
    GridL->addWidget(startgameB,2,2);
    GridL->addWidget(returnB,0,0);
    //GridL->addItem(virus);
    setLayout(GridL);

    return;
}

void game1menu::startGame()
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

void game1menu::endAll()
{
    close();
    delete this;
}

void game1menu::displayInfo()
{
    QMessageBox *info = new QMessageBox();
    info->setWindowTitle("Information");
    info->setText("You are a doctor who entered a patient's bloodstream\nShoot the invasive Coronaviruses using your trusty needles\nBe careful not to let too many through!");
    info->setDetailedText("Use the left and right arrows to move\nUse the space bar to shoot\nWhite viruses are worth 3 points, red are worth 5 and black are worth 7\nYou win once you score 150 points\nYou lose if you let 3 viruses live");
    info->show();
}
