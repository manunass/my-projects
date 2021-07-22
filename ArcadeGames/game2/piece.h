#ifndef PIECE_H
#define PIECE_H
#include <QGraphicsPixmapItem>
enum Color {white, black, empty};

class Piece: public QGraphicsPixmapItem
{

private:

public:
    Piece(Color piececolor=empty);
    Color color;
};

#endif // PIECE_H
