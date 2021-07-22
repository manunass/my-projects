#ifndef CHINREM_H
#define CHINREM_H

#include <QDialog>
#include <vector>
#include <string>
namespace Ui {
class chinRem;
}

class chinRem : public QDialog
{
    Q_OBJECT
    long long bezouti(long long m, long long n, int* a,int* b);
    long long GCD(long long m, long long n, int* a,int* b);
    QVector<long long> CRM(QVector<QVector<long long>> inputs);
public:
    explicit chinRem(QWidget *parent = nullptr);
    ~chinRem();

private slots:
    void on_GoBack_clicked();
    void on_exit_clicked();
    void on_Compute_clicked();
    void on_clear_clicked();

private:
    Ui::chinRem *ui;

};

#endif // CHINREM_H
