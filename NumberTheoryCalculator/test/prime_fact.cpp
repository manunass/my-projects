#pragma once
#include "prime_fact.h"
#include "mainwindow.h"
#include "ui_prime_fact.h"
#include <string>
using namespace std;

prime_Fact::prime_Fact(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::prime_Fact)
{
    ui->setupUi(this);
}

vector<pair<int, int>> prime_Fact::primeFact(int x) {
    vector<pair<int, int>> pf;
    for(int i = 2; i * i <= x; i++){
        if(x % i == 0){
            int power = 0;
            while(x % i == 0){
                x /= i; power++;
            }
            pf.push_back({i, power});
        }
    }
    if(x != 1){pf.push_back({x, 1});}
    return pf;
}

prime_Fact::~prime_Fact()
{
    delete ui;
}

void prime_Fact::on_Compute_clicked()
{
    int x = this->ui->Input->toPlainText().toInt();
    vector<pair<int, int>> pf = primeFact(x);
    string res = ""; int n = (int)pf.size();
    for(int i = 0; i < n; i++){
        res += to_string(pf[i].first) + "^" + to_string(pf[i].second);
        if(i != n - 1){res += " * ";}
    }
    this->ui->Output->setText(QString(res.c_str()));
}

void prime_Fact::on_Clear_clicked()
{
    this->ui->Input->clear();
    this->ui->Output->clear();
}

void prime_Fact::on_GoBack_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void prime_Fact::on_exit_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}
