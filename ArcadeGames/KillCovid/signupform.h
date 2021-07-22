/**
* \file SignupForm.h
* \brief SignupForm class
*
*
* all the member variables of the class implementing the signupwindow
*/

#ifndef LOGINSCREEN_H
#define LOGINSCREEN_H

#include <QWidget>
#include <QLabel>
#include <QLineEdit>
#include <QSpinBox>
#include <QRadioButton>
#include <QPushButton>
#include <QTextEdit>
#include <QVBoxLayout>
#include <QGridLayout>
#include <QGroupBox>
#include <QCalendarWidget>
#include <string>
#include <QFile>
#include <QTextStream>
#include <iostream>
#include <QDebug>
#include <QDir>

class SignupForm : public QWidget
{
    Q_OBJECT
public:
    explicit SignupForm(QWidget *parent = nullptr);
    void setGridLayout();
    bool checkUser();
    bool checkPassword();
    bool checkValidity();
    bool passHasNumber();

public slots:
    void proceed();
    void cancel();

private:
    QLabel *firstL;
    QLineEdit *firstE;
    QLabel *lastL;
    QLineEdit *lastE;
    QLabel *userL;
    QLineEdit *userE;
    QLabel *dobL;
    QCalendarWidget *dobD;
    QLabel *passwordL;
    QLineEdit *passwordE;
    QLabel *passwordinstL;
    QLabel *passwordcharsL;
    QLabel *passwordnumberL;
    QLabel *passwordupperL;
    QLabel *passwordlowerL;
    QLabel *genderL;
    QRadioButton *maleR;
    QRadioButton *femaleR;
    QLabel *errorL;
    QPushButton *proceedB;
    QPushButton *cancelB;
    QGridLayout *GridL;

signals:

};

#endif // LOGINSCREEN_H
