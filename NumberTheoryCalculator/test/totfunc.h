#ifndef TOTFUNC_H
#define TOTFUNC_H

#include <QDialog>

namespace Ui {
class totFunc;
}

class totFunc : public QDialog
{
    Q_OBJECT

    int totientFunction(int x);

public:
    explicit totFunc(QWidget *parent = nullptr);
    ~totFunc();

private slots:

    void on_Compute_clicked();

    void on_Clear_clicked();

    void on_GoBack_clicked();

    void on_exit_clicked();

private:
    Ui::totFunc *ui;
};

#endif // TOTFUNC_H
