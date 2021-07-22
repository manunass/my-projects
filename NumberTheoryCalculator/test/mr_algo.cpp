#pragma once
#include "mr_algo.h"
#include "ui_mr_algo.h"
#include "mainwindow.h"
#include <stdlib.h>
#include <string>
using namespace std;

MR_Algo::MR_Algo(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::MR_Algo)
{
    ui->setupUi(this);
}

long long MR_Algo::power(long long x, long long n, int mod){
    if(n == 0){return 1;}
    long long res = power(x, n / 2, mod);
    res = (res * res) % mod;
    if(n % 2 == 0) {return res;}
    return (res * x) % mod;
}

bool MR_Algo::millerRabinTest(int d, int n){
    // Pick a random number in [2..n-2]
    // Corner cases make sure that n > 4
    int a = 2 + rand() % (n - 4);

    // Compute a^d % n
    int x = power(a, d, n);

    if (x == 1  || x == n-1)
       return true;

    // Keep squaring x while one of the following doesn't
    // happen
    // (i)   d does not reach n-1
    // (ii)  (x^2) % n is not 1
    // (iii) (x^2) % n is not n-1
    while (d != n-1)
    {
        x = (x * x) % n;
        d *= 2;

        if (x == 1)      return false;
        if (x == n-1)    return true;
    }

    // Return composite
    return false;
}

bool MR_Algo::isPrime(int n, int k){
    // Corner cases
    if (n <= 1 || n == 4)  return false;
    if (n <= 3) return true;
    if (n % 2 == 0) return false;

    // Find r such that n = 2^d * r + 1 for some r >= 1
    int d = n - 1;
    while (d % 2 == 0) d /= 2;

    // Iterate given nber of 'k' times
    for (int i = 0; i < k; i++)
         if (!millerRabinTest(d, n))
              return false;

    return true;
}

MR_Algo::~MR_Algo()
{
    delete ui;
}

void MR_Algo::on_Compute_clicked()
{
    int n = this->ui->Input->toPlainText().toInt();
    int k = this->ui->nbrOfRounds->toPlainText().toInt();
    string res = isPrime(n, k) ? "PRIME" : "COMPOSITE";
    this->ui->result->setText(QString(res.c_str()));
}

void MR_Algo::on_Clear_clicked()
{
    this->ui->Input->clear();
    this->ui->nbrOfRounds->clear();
    this->ui->result->clear();
}

void MR_Algo::on_GoBack_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void MR_Algo::on_exit_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}
