#pragma once
#include "chinrem.h"
#include "ui_chinrem.h"
#include "mainwindow.h"
using namespace std;

chinRem::chinRem(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::chinRem)
{
    ui->setupUi(this);
}

chinRem::~chinRem()
{
    delete ui;
}


long long chinRem:: bezouti (long long m, long long n, int* a,int* b)
{
    if(m<=0 || n<0)
    {
        return -1;
    }
    if (n==0)
    {
        *a=1;    //the base case when n is 0 and m >0
        *b=0;    // the gcd is m and the coeficeints are 0 and 1
        return m;
    }
    long long q = m/n;  //we compute q and r in  m = q.n +r
    long long r= m%n;

   long long gcd= bezouti(n,r,a,b);
    int c= *b;
    *b= *a - (*b * q);
    *a=c;

    return gcd;
}



long long chinRem::GCD(long long m, long long n, int* a,int* b)
{       //note that n will always be the smallest element thanks to this function
    if (m>=n) {return bezouti(m,n,a,b);}
    else{return bezouti(n,m,b,a);}
}

QVector<long long> chinRem::CRM(QVector<QVector<long long>> inputs)
{
    for(int i=this->ui->numberofequations->value()-1; i>0; i--)
    {
        long long x1=inputs[0][0], m1=inputs[0][1], x2=inputs[i][0], m2=inputs[i][1];
        if(x1<0){x1+=m1;}
        if(x2<0){x2+=m2;}
        int *a1=new int, *a2= new int;

        long long m= m1*m2;
        long long x= (*a1 *m1* x2+ *a2 *m2*x1)%m;

        inputs[0][0]=x;
        inputs[0][1]=m;

    }

    return inputs[0];
}


void chinRem::on_GoBack_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void chinRem::on_exit_clicked()
{
    this->hide();
    MainWindow *mainWin = (MainWindow*) this->parent();
    mainWin->show();
}

void chinRem::on_clear_clicked()
{
    this->ui->x1->clear();
    this->ui->x2->clear();
    this->ui->x3->clear();
    this->ui->x4->clear();
    this->ui->x5->clear();
    this->ui->m1->clear();
    this->ui->m2->clear();
    this->ui->m3->clear();
    this->ui->m4->clear();
    this->ui->m5->clear();
    this->ui->output->clear();

}

void chinRem:: on_Compute_clicked()
{
    QVector<QVector<long long>> input ;
    input.resize(5);
    input[0].append(this->ui->x1->text().toLongLong());
    input[0].append(this->ui->m1->text().toLongLong());

    input[1].append(this->ui->x2->text().toLongLong());
    input[1].append(this->ui->m2->text().toLongLong());

    input[2].append(this->ui->x3->text().toLongLong());
    input[2].append(this->ui->m3->text().toLongLong());

    input[3].append(this->ui->x4->text().toLongLong());
    input[3].append(this->ui->m4->text().toLongLong());

    input[4].append(this->ui->x5->text().toLongLong());
    input[4].append(this->ui->m5->text().toLongLong());
    input.resize(this->ui->numberofequations->value());

   QVector <long long> output= CRM(input);
   QString line=  ("x=");
   line= line+ QString::number(output[0]) + " mod" + QString::number(output[1]);
   this->ui->output->setText(line);
   input.clear();
}

