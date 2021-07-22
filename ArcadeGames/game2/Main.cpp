#include <QApplication>
#include <QPushButton>
#include <game2menu.h>

using namespace std;

int main(int argc, char **argv)
{
    QApplication app (argc, argv);

    game2menu menu;
    menu.show();
    return app.exec();
}
