#ifndef PLAYER_H
#define PLAYER_H
#include <piece.h>

class Player
{
private:
    int score;
    Color color;

public: 
    Player();

    void Add_piece(int row, int column);
};

#endif // PLAYER_H
