/**
* \file login.cpp
* \brief contains Student class definition
*
*
* implements UI, method to check password strength,
* method to check if info is valid and username is unique,
* method to register a valid user.
* \author Nicolas Sassine & Emmanuel Nassour
*/

#include "signupform.h"

SignupForm::SignupForm(QWidget *parent) : QWidget(parent)
{
    setWindowTitle("Sign Up");
    GridL = new QGridLayout();
    firstL = new QLabel("First Name");
    firstE = new QLineEdit();
    lastL = new QLabel("Last Name");
    lastE = new QLineEdit();
    userL = new QLabel("Username");
    userE = new QLineEdit();
    userE->setPlaceholderText("Should be unique");
    dobL = new QLabel("Date of Birth");
    dobD = new QCalendarWidget();
    passwordL = new QLabel("Password");
    passwordE = new QLineEdit();
    passwordinstL = new QLabel("Password should contain at least:");
    passwordcharsL = new QLabel("- 8 characters");
    passwordnumberL = new QLabel("- 1 number");
    passwordupperL = new QLabel("- 1 uppercase letter");
    passwordlowerL = new QLabel("- 1 lowercase letter");
    passwordE->setEchoMode(QLineEdit::Password);
    genderL = new QLabel("Gender");
    maleR = new QRadioButton("Male");
    femaleR = new QRadioButton("Female");
    cancelB = new QPushButton("Cancel");
    QObject::connect(cancelB, SIGNAL(clicked(bool)), this, SLOT(cancel()));

    proceedB = new QPushButton("Proceed");
    QObject::connect(proceedB, SIGNAL(clicked(bool)), this, SLOT(proceed()));

    errorL = new QLabel("I am error");
    errorL->setStyleSheet("QLabel { color: red }");
    errorL->setVisible(false);
    setGridLayout();
}

void SignupForm::setGridLayout()
{
    GridL->addWidget(firstL, 0, 0);
    GridL->addWidget(firstE, 1, 0);
    GridL->addItem(new QSpacerItem(50,10), 0, 1);
    GridL->addWidget(lastL, 0, 2);
    GridL->addWidget(lastE, 1, 2);
    GridL->addItem(new QSpacerItem(10,25), 2, 0);
    GridL->addWidget(dobL, 3, 0);
    GridL->addWidget(dobD, 4, 0);
    GridL->addWidget(genderL, 3, 2);

    QGroupBox *genderG = new QGroupBox();
    QVBoxLayout *GenderL = new QVBoxLayout();
    GenderL->addWidget(maleR, 0);
    GenderL->addWidget(femaleR, 1);
    genderG->setLayout(GenderL);

    GridL->addWidget(genderG, 4, 2);
    GridL->addItem(new QSpacerItem(10,25), 5, 0);
    GridL->addWidget(userL, 6, 0);
    GridL->addWidget(userE, 7, 0);
    GridL->addWidget(errorL, 7, 2);
    GridL->addItem(new QSpacerItem(10,25), 8, 0);
    GridL->addWidget(passwordL, 9, 0);
    GridL->addWidget(passwordinstL, 9, 2);
    GridL->addWidget(passwordE, 10, 0, Qt::AlignTop);

    QGroupBox *passwordInstructionsG = new QGroupBox();
    QVBoxLayout *PasswordL = new QVBoxLayout();
    PasswordL->addWidget(passwordcharsL, 0);
    PasswordL->addWidget(passwordupperL, 1);
    PasswordL->addWidget(passwordlowerL, 2);
    PasswordL->addWidget(passwordnumberL, 3);
    passwordInstructionsG->setLayout(PasswordL);

    GridL->addWidget(passwordInstructionsG, 10, 2);
    GridL->addItem(new QSpacerItem(10,25), 11, 0);
    GridL->addWidget(cancelB, 12, 0);
    GridL->addWidget(proceedB, 12, 2);
    setLayout(GridL);
    return;
}

void SignupForm::cancel()
{
    close();
}
/**
* \brief if valid, register user
* uses all the data from the UI
*
* calls checkValidity, if valid registers user, else display error message
* \return double value of distance
*/
void SignupForm::proceed()
{
    if (checkValidity())
    {
        QFile users(":/users.txt");
        qDebug() << "Changed permissions: " << users.setPermissions(QFileDevice::WriteOther) << " | File permissions: " << users.permissions();
        if (users.isOpen()) users.close();
        if (!users.open(QIODevice::WriteOnly | QIODevice::Append))
        {
            QString errMsg = users.errorString();
            QFileDevice::FileError err = users.error();
            qDebug() << "Writing error: error code " <<  err;
            return;
        }

        QTextStream out(&users);

        out << "test";

        QString info;
        info = userE->text() + " " + firstE->text() + " " + lastE->text() + " " + dobD->selectedDate().toString(Qt::ISODate) + " ";
        if (maleR->isChecked()) { info.append("male na na "); }
        else { info.append("female na na "); }
        info.append(passwordE->text());

        out << info << endl;
        users.close();

        close();
        delete this;
    }
    return;
}


/**
* \brief check user data validity
* check if all ghe fields are filled corectly,
* calls checkUser and checkPassword, if boat are true, returns true
*
* \return true if user data is valid, usernme unique, and password strong, false otherwise
*/
bool SignupForm::checkValidity()
{
    if (firstE->text().isEmpty() || lastE->text().isEmpty() || userE->text().isEmpty() || passwordE->text().isEmpty() || (!maleR->isChecked() && !femaleR->isChecked()))
    {
        errorL->setText("All fields must be filled");
        errorL->setVisible(true);
        return false;
    }

    if (firstE->text().contains(" ") || lastE->text().contains(" ") || userE->text().contains(" ") || passwordE->text().contains(" "))
    {
        errorL->setText("No text field can contain a space");
        errorL->setVisible(true);
        return false;
    }
    if (!checkUser())
    {
        errorL->setText("Username has already been taken");
        errorL->setVisible(true);
        return false;
    }
    if (!checkPassword())
    {
        errorL->setText("Password is not valid");
        errorL->setVisible(true);
        return false;
    }
    errorL->setVisible(false);
    return true;
}


/**
* \brief checks username
* uses UsernameE input and checks if its a new username.
*
* \return true if unique, false if not
*/
bool SignupForm::checkUser()
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
    QStringList userList;
    in.readLine();
    while (!in.atEnd())
    {
        userList.append(in.readLine().split(" ").at(0));
    }

    qDebug() << userList;

    users.close();

    return !userList.contains(userE->text());
}

/**
* \brief checks password
* uses PasswordE input and checks if it has 8+ characters,
* has one lowercase letter, one upercase letter and at least one number
* \return true if all checked, false if not
*/
bool SignupForm::checkPassword()
{
    QString password = passwordE->text();

    if (password.length() < 8)
    {
        passwordcharsL->setStyleSheet("QLabel { color: red }");
        passwordupperL->setStyleSheet("QLabel { color: black }");
        passwordlowerL->setStyleSheet("QLabel { color: black }");
        passwordnumberL->setStyleSheet("QLabel { color: black }");
        return false;
    }

    if (password.toLower() == password)
    {
        passwordcharsL->setStyleSheet("QLabel { color: black }");
        passwordupperL->setStyleSheet("QLabel { color: red }");
        passwordlowerL->setStyleSheet("QLabel { color: black }");
        passwordnumberL->setStyleSheet("QLabel { color: black }");
        return false;
    }

    if (password.toUpper() == password)
    {
        passwordcharsL->setStyleSheet("QLabel { color: black }");
        passwordupperL->setStyleSheet("QLabel { color: black }");
        passwordlowerL->setStyleSheet("QLabel { color: red }");
        passwordnumberL->setStyleSheet("QLabel { color: black }");
        return false;
    }

    QRegExp number("\\d");
    if(!password.contains(number))
    {
        passwordcharsL->setStyleSheet("QLabel { color: black }");
        passwordupperL->setStyleSheet("QLabel { color: black }");
        passwordlowerL->setStyleSheet("QLabel { color: black }");
        passwordnumberL->setStyleSheet("QLabel { color: red }");
        return false;
    }

    passwordcharsL->setStyleSheet("QLabel { color: black }");
    passwordupperL->setStyleSheet("QLabel { color: black }");
    passwordlowerL->setStyleSheet("QLabel { color: black }");
    passwordnumberL->setStyleSheet("QLabel { color: black }");
    return true;
}
