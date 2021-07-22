/**
* \mainpage EECE435L project
* \author Nicolas Sassine
* \author Emmanuel Nassour
* \date 1-1-2014
*/


#include <QApplication>
#include <QPushButton>
#include <signupform.h>
#include <login.h>
#include "mainmenu.h"

int main(int argc, char **argv)
{
    QApplication app (argc, argv);
    Login login;
    SignupForm signup;
//    MainMenu mainMenu;
//    mainMenu.show();
    login.show();
    return app.exec();
}
