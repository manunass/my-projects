#ifndef FAST_EXP_H
#define FAST_EXP_H

#include <QDialog>

namespace Ui {
class fast_Exp;
}

class fast_Exp : public QDialog
{
    Q_OBJECT

    long long exp(long long x, long long n);
    long long modularExp(long long x, long long n, const int &mod);

public:
    explicit fast_Exp(QWidget *parent = nullptr);
    ~fast_Exp();

private slots:
    void on_GoBack_clicked();

    void on_exit_clicked();

    void on_Compute_clicked();

    void on_Clear_clicked();

private:
    Ui::fast_Exp *ui;
};

#endif // FAST_EXP_H
