#pragma once
#include "totfunc.h"
#include "ui_totfunc.h"
#include "mainwindow.h"
#include <string>
using namespace std;

totFunc::totFunc(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::totFunc)
{
    ui->setupUi(this);
}

int totFunc::totientFunction(int n) {
    int totient = n;
    for(int i = 2; i * i <= n; i++){
        if(n % i == 0){
            while (n % i == 0){ n /= i; }
            totient -= totient / i;
        }
    }
    if(n > 1){totient -= totient / n;}
    return totient;
}

totFunc::~totFunc()
{
    delete ui;
}

void totFunc::on_Compute_clicked()
{
    int n = this->ui->Input->toPlainText().toInt();
    int totient = totientFunction(n);
    this->ui->Output->setText(QString(to_string(totient).c_str()));
}

void totFunc::on_Clear_clicked()
{
    this->ui->Input->clear();
    this->ui->Output->clear();
}

void totFunc::on_GoBack_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void totFunc::on_exit_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}
