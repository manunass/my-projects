#ifndef GAME2ENDMENU_H
#define GAME2ENDMENU_H

#include <QWidget>
#include <QtWidgets>
#include <game2scene.h>


class Game2EndMenu : public QWidget
{
    Q_OBJECT
public:
    explicit Game2EndMenu(QWidget *parent = nullptr, int white_score=0, int black_score=0, QString currentUser = nullptr);

    void SetGridLayout();
    bool saveHighScore(int white_score, int black_score);

    public slots:
        void startGame();
        void endAll();

    private:
        QLabel *resultL;
        QPushButton *exitB;
        QPushButton *replaygameB;
        QGridLayout *GridL;

        QString user;

    signals:

    };

#endif // GAME2ENDMENU_H
