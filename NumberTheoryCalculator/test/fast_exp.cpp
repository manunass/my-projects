#pragma once
#include "fast_exp.h"
#include "ui_fast_exp.h"
#include "mainwindow.h"
#include <string>
using namespace std;

fast_Exp::fast_Exp(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::fast_Exp)
{
    ui->setupUi(this);
}

long long fast_Exp:: exp(long long x, long long n) {
        if(n == 0){return 1;}
        long long res = exp(x, n/2);
        res = res * res;
        return (n & 1 ? res * x : res);
    }

long long fast_Exp::modularExp(long long x, long long n, const int &mod){
        if(n == 0){return 1;}
        long long res = modularExp(x, n / 2, mod);
        res = (res * res) % mod;
        if(n % 2 == 0) {return res;}
        return (res * x) % mod;
    }

fast_Exp::~fast_Exp()
{
    delete ui;
}

void fast_Exp::on_GoBack_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void fast_Exp::on_exit_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void fast_Exp::on_Compute_clicked()
{
    long long x = this->ui->numberInput->toPlainText().toLongLong();
    long long n = this->ui->exponent->toPlainText().toLongLong();
    if(this->ui->Modulo->text().size() == 0)
        this->ui->result->setText(QString(to_string(exp(x, n)).c_str()));
    else{
        int mod = this->ui->Modulo->text().toInt();
        this->ui->result->setText(QString(to_string(modularExp(x, n, mod)).c_str()));
    }
}

void fast_Exp::on_Clear_clicked()
{
    this->ui->numberInput->clear();
    this->ui->exponent->clear();
    this->ui->Modulo->clear();
    this->ui->result->clear();
}
