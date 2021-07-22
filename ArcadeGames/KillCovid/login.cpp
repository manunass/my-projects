/**
* \file login.cpp
* \brief contains login class
*
* implements all the ncesseary member variables and methods for the login window,
*
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "login.h"
#include "signupform.h"
#include "mainmenu.h"


Login::Login(QWidget *parent) : QWidget(parent)
{

    setWindowTitle("Login");
    UsernameE = new QLineEdit();
    PasswordE = new QLineEdit ();
    LoginB = new QPushButton("Login");
    SignupB = new QPushButton ("Signup");
    GuestloginB = new QPushButton("Continue as guest");
    UsernameL = new QLabel ("Username");
    PasswordL = new QLabel("Password");
    PasswordE->setEchoMode(QLineEdit::Password);

    errorL = new QLabel("Invalid Username and Password combination");
    errorL->setStyleSheet("QLabel { color: red }");
    errorL->setVisible(false);

    GridL = new QGridLayout();
    SetGridLayout();

    QObject::connect(SignupB, SIGNAL(clicked()), this, SLOT(Signup()));
    QObject::connect(LoginB, SIGNAL(clicked()), this, SLOT(LoginAttempt()));
    QObject::connect(GuestloginB, SIGNAL(clicked()), this, SLOT(LoginAsGuest()));

}
void Login::SetGridLayout()
{
    GridL->addWidget(UsernameL,0,0);
    GridL->addWidget(PasswordL,1,0);
    GridL->addWidget(UsernameE,0,1);
    GridL->addWidget(PasswordE,1,1);
    GridL->addWidget(errorL,3,0,1,2);
    GridL->addWidget(LoginB,2,1);

    GridL->addWidget(SignupB,4,0);
    GridL->addWidget(GuestloginB,4,1);

    setLayout(GridL);

    return;
}

/**
* \brief searches for a matching username/ password combination
*
* \return true if found, false otherwise
*/
bool Login::CheckUserPass()
{

    QFile users(":/users.txt");

    if (!users.open(QIODevice::ReadOnly))
    {
        QString errMsg = users.errorString();
        QFileDevice::FileError err = users.error();
        qDebug() << "Reading error: " << errMsg;
        return false;
    }

    QTextStream in(&users);

    in.readLine();      //To skip the first line (else you can login using "username" and "password")
    while (!in.atEnd())
    {
        QString Line1,Line2;
        Line1.append(in.readLine());
        Line2 = Line1.split(" ").at(0);;
        //qDebug() << Line2;
        if(UsernameE->text() == Line2)
        {
            int i=Line1.lastIndexOf(" ");
            Line1=Line1.mid(i+1,Line1.size()-i);
            qDebug() << Line1;
            if(Line1==PasswordE->text())
            {return true;}
        }

    }
    return false;
}

/**
* \brief validate login attempt
*
* calls CheckUserPass, if valid close window opens game menu, else display erroo message
*/
void Login::LoginAttempt()
{
    if (this->CheckUserPass())
    {
    this->close();
    MainMenu *mainMenu = new MainMenu(nullptr, UsernameE->text());
    mainMenu->show();
    //delete this;
    return;
    }
    errorL->setVisible(true);
}


void Login::Signup()
{
    SignupForm * SignupWindow = new SignupForm();
    SignupWindow->show();
}

void Login::LoginAsGuest()
{
    this->close();
    MainMenu *mainMenu = new MainMenu();
    mainMenu->show();
    //delete this;
}
