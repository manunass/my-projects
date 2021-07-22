#pragma once
#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "prime_fact.h"
#include "chinrem.h"
#include "fast_exp.h"
#include "mr_algo.h"
#include "totfunc.h"
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::on_pushButton_clicked()
{
    prime_Fact primeF(this);
    primeF.setModal(true);
    primeF.exec();
}


void MainWindow::on_pushButton_2_clicked()
{
    totFunc totient(this);
    totient.setModal(true);
    totient.exec();
}


void MainWindow::on_pushButton_3_clicked()
{
    MR_Algo milR(this);
    milR.setModal(true);
    milR.exec();
}

void MainWindow::on_pushButton_4_clicked()
{
    fast_Exp fastEx(this);
    fastEx.setModal(true);
    fastEx.exec();
}

void MainWindow::on_pushButton_5_clicked()
{
    chinRem chiR(this);
    chiR.setModal(true);
    chiR.exec();
}

void MainWindow::on_pushButton_7_clicked()
{
    QMessageBox::StandardButton reply = QMessageBox::question(this, "Number Theory Project", "Are you sure you want to Quit?", QMessageBox::Yes | QMessageBox::No);
    if (reply == QMessageBox::Yes){
        QApplication::quit();
    }
    else {
        this->hide();
    }
}

void MainWindow::on_pushButton_6_clicked()
{
    QMessageBox::StandardButton reply = QMessageBox::question(this, "Number Theory Project", "Are you sure you want to Quit?", QMessageBox::Yes | QMessageBox::No);
    if (reply == QMessageBox::Yes){
        QApplication::quit();
    }
    else {
        this->hide();
    }
}
