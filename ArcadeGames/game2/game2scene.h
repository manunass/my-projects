#ifndef GAME2SCENE_H
#define GAME2SCENE_H

#include <QObject>
#include <QWidget>
#include <QGraphicsScene>
#include <QGraphicsPixmapItem>
#include <QGraphicsView>
#include <player.h>
#include <QMouseEvent>
#include <QDebug>
#include <QGraphicsSceneMouseEvent>
#include <game2endmenu.h>

class Game2Scene : public QGraphicsScene
{
    Q_OBJECT
public:
    Game2Scene(QString currentUser = nullptr);
    void DisplayPiece(int row, int column, bool isWhite);
    void mousePressEvent(QGraphicsSceneMouseEvent *event);
    void computePossibleMoves(Color movingcolor, Color othercolor);
    bool CanEat(int row, int column, int dx,int dy,Color movingcolor, Color othercolor);
    void flipPieces(int row, int column);
    void EndGame();

private:
    QGraphicsPixmapItem* pixboard;
    Piece *game_board[8][8];
    Player * white_player;
    Player *black_player;
    bool white_turn;
    bool possible_moves[8][8];
    int no_move_counter;

    QString user;

signals:

};

#endif // GAME2SCENE_H
