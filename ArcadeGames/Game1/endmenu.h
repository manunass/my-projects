/**
* \endmenu.h
* \brief variables and method for the end menu
*/

#ifndef ENDMENU_H
#define ENDMENU_H

#include <QWidget>
#include <QtWidgets>
#include <game1scene.h>

class EndMenu : public QWidget
{
    Q_OBJECT
public:
    explicit EndMenu(QWidget *parent = nullptr, bool isWin = false, int currentScore = 0, QString currentUser = nullptr);
    void SetGridLayout();
    bool saveHighScore();

public slots:
    void startGame();
    void endAll();

private:
    QLabel *resultL;
    QPushButton *exitB;
    QPushButton *replaygameB;
    QLabel *scoreL;
    QGridLayout *GridL;

    int score;
    QString user;

signals:

};

#endif // ENDMENU_H
