/**
* \file login.h
* \brief login window class
*
*/


#ifndef LOGIN_H
#define LOGIN_H

#include <QWidget>
#include <QtWidgets>
#include <QObject>
#include <QLabel>
#include <QLineEdit>
#include <QPushButton>

class Login : public QWidget
{
    Q_OBJECT
public:
    explicit Login(QWidget *parent = nullptr);
    void SetGridLayout();
    bool CheckUserPass();

private:
    QLineEdit *UsernameE;
    QLineEdit *PasswordE;
    QPushButton *LoginB;
    QPushButton *SignupB;
    QPushButton *GuestloginB;
    QLabel *UsernameL;
    QLabel *PasswordL;
    QLabel *errorL;


    QGridLayout *GridL;

signals:


public slots:

void LoginAttempt();
void Signup();
void LoginAsGuest();




};
#endif // LOGIN_H
