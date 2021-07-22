#ifndef MR_ALGO_H
#define MR_ALGO_H

#include <QDialog>

namespace Ui {
class MR_Algo;
}

class MR_Algo : public QDialog
{
    Q_OBJECT

    long long power(long long x, long long n, int mod);
    bool millerRabinTest(int d, int n);
    bool isPrime(int n, int k);

public:
    explicit MR_Algo(QWidget *parent = nullptr);
    ~MR_Algo();

private slots:
    void on_Compute_clicked();

    void on_Clear_clicked();

    void on_GoBack_clicked();

    void on_exit_clicked();

private:
    Ui::MR_Algo *ui;
};

#endif // MR_ALGO_H
