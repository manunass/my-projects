#ifndef PRIME_FACT_H
#define PRIME_FACT_H

#include <QDialog>
#include <vector>
using namespace std;

namespace Ui {
class prime_Fact;
}

class prime_Fact : public QDialog
{
    Q_OBJECT
    vector<pair<int, int>> primeFact(int x);

public:
    explicit prime_Fact(QWidget *parent = nullptr);
    ~prime_Fact();

private slots:

    void on_Compute_clicked();

    void on_Clear_clicked();

    void on_GoBack_clicked();

    void on_exit_clicked();

private:
    Ui::prime_Fact *ui;
};

#endif // PRIME_FACT_H
