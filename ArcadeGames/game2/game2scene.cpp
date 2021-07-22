#include "game2scene.h"

Game2Scene::Game2Scene(QString currentUser)
{
    user = currentUser;
    setSceneRect(0, 0, 800, 800);
    no_move_counter = 0;
    pixboard= new QGraphicsPixmapItem();
    pixboard->setPixmap(QPixmap(":/othello-board.png").scaled(800,800));
    pixboard->setPos(0,0);
    this->addItem(pixboard);
    white_turn = false;
    for(int i = 0; i < 8; i++)
    {
        for(int j = 0; j < 8; j++)
        {
            game_board[i][j] = new Piece();
            game_board[i][j]->setPixmap(QPixmap(":/empty.png").scaled(90, 90));
            game_board[i][j]->setPos(100*i+5, 100*j+5);
            addItem(game_board[i][j]);
        }
    }
    game_board[3][3]->setPixmap(QPixmap(":/BlackChess.png").scaled(90, 90));
    game_board[3][3]->color = black;
    game_board[3][4]->setPixmap(QPixmap(":/WhiteChess.png").scaled(90, 90));
    game_board[3][4]->color = white;
    game_board[4][3]->setPixmap(QPixmap(":/WhiteChess.png").scaled(90, 90));
    game_board[4][3]->color = white;
    game_board[4][4]->setPixmap(QPixmap(":/BlackChess.png").scaled(90, 90));
    game_board[4][4]->color = black;

    computePossibleMoves(black,white);
}

void Game2Scene::mousePressEvent(QGraphicsSceneMouseEvent *event)
{
//    qDebug() << "x = " << event->buttonDownScenePos(event->button()).x();
    if (event->button() == Qt::LeftButton)
    {
        DisplayPiece((int) event->buttonDownScenePos(event->button()).x()/100, (int) event->buttonDownScenePos(event->button()).y()/100, white_turn);
    }
}

 void Game2Scene::DisplayPiece(int row, int column, bool isWhite)
{ 
    if(game_board[row][column]->color != empty) return;
    if(possible_moves[row][column] == false) return;

    if(white_turn)
    {
        game_board[row][column]->setPixmap(QPixmap(":/WhiteChess.png").scaled(90,90));
        game_board[row][column]->color = white;
        game_board[row][column]->setOpacity(1);
    }
    else
    {
        game_board[row][column]->setPixmap(QPixmap(":/BlackChess.png").scaled(90,90));
        game_board[row][column]->color = black;     
        game_board[row][column]->setOpacity(1);
    }

    for(int i = 0; i < 8; i++)
    {
        for(int j = 0; j < 8; j++)
        {
            if (possible_moves[i][j] and !(i == row and j == column))
            {
                game_board[i][j]->setPixmap(QPixmap(":/empty.png").scaled(90, 90));
            }
        }
    }

    flipPieces(row, column);
    white_turn = !white_turn;
    if(white_turn)
    {computePossibleMoves(white, black);}
    else
    {computePossibleMoves(black,white);}
}

 void Game2Scene::computePossibleMoves(Color movingcolor, Color othercolor)
 {
     no_move_counter++;
     for (int row=0; row<8;row++)
     {
        for (int column=0; column<8; column++)
         {
             possible_moves[row][column]=false;
             for(int deltaY = -1; deltaY <= 1; deltaY++)
             {

               for(int deltaX = -1; deltaX <= 1; deltaX++)
               {
                   if((deltaX == 0 && deltaY == 0) || row+deltaX == -1 || row+deltaX == 8 || column+deltaY == -1 || column+deltaY == 8) continue;
                   possible_moves[row][column] = (possible_moves[row][column] or CanEat(row, column, deltaX, deltaY, movingcolor, othercolor)) and (game_board[row][column]->color == empty);
                   if(possible_moves[row][column])
                   {
                       no_move_counter = 0;
                       if (white_turn)
                       {
                            game_board[row][column]->setPixmap(QPixmap(":/WhiteChess.png").scaled(90,90));
                            game_board[row][column]->setOpacity(0.5);
                       }
                       else
                       {
                           game_board[row][column]->setPixmap(QPixmap(":/BlackChess.png").scaled(90,90));
                           game_board[row][column]->setOpacity(0.5);
                       }
                       break;
                   }
               }
               if(possible_moves[row][column])
               {
                   no_move_counter = 0;
                   break;
               }
             }
         }
     }
     if (no_move_counter == 1)
     {
         white_turn = !white_turn;
         computePossibleMoves(othercolor, movingcolor);
     }
     else if (no_move_counter == 2) EndGame();

 }

 bool Game2Scene::CanEat(int row, int column, int dx, int dy, Color movingcolor, Color othercolor)
 {
     int dRow = row + dx;
     int dColumn = column + dy;
     int ate=0;

     while (dColumn != -1 && dRow != -1 && dColumn != 8 && dRow != 8)
     {
             if(game_board[dRow][dColumn]->color != othercolor)
             {
                 break;
             }
             dRow += dx;
             dColumn += dy;
             ate++;
     }
     if(dColumn == -1 || dRow == -1 || dColumn == 8 || dRow == 8) //we reached  the end of the board with no piecce of same color
     {return 0;}

     if(game_board[dRow][dColumn]->color != movingcolor || ate == 0)//we reached to the end of the oposite tyle and landed on empty
     {return 0; }

         return true;
 }

void Game2Scene::flipPieces(int row, int column)
 {
    //qDebug() << "New piece";
    if (white_turn)
    {
        for (int deltaX = -1; deltaX <= 1; deltaX++)
        {
            for (int deltaY = -1; deltaY <=1; deltaY++)
            {
                int deltaPosX = 0;
                int deltaPosY = 0;
                bool flippableRow = false;

//                qDebug() << row+deltaPosX + deltaX;
                while (row + deltaPosX != 8 && row + deltaPosX != -1  && column + deltaPosY != 8 && column + deltaPosY != -1)
                {
//                    qDebug() << "deltaX = " << deltaX << " deltaY = " << deltaY;
                    if ((row+deltaPosX+deltaX > -1) && (row+deltaPosX+deltaX < 8) && (column+deltaPosY+deltaY > -1) && (column+deltaPosY+deltaY < 8))
                    {
                        if (game_board[row+deltaPosX+deltaX][column+deltaPosY+deltaY]->color == white) break;
                        if (game_board[row+deltaPosX+deltaX][column+deltaPosY+deltaY]->color == empty) break;
                    }
                    deltaPosX += deltaX;
                    deltaPosY += deltaY;
                    flippableRow=true;
                }
                if ((row+deltaPosX+deltaX > -1) && (row+deltaPosX+deltaX < 8) && (column+deltaPosY+deltaY > -1) && (column+deltaPosY+deltaY < 8))
                {
                    if (flippableRow and game_board[row+deltaPosX+deltaX][column+deltaPosY+deltaY]->color == white)
                    {
                        //qDebug() << "Flipping row to white";
                        int currentX = row;
                        int currentY = column;
                        while (currentX != (row + deltaPosX) || currentY != (column + deltaPosY))
                        {
                            currentX += deltaX;
                            currentY += deltaY;
                            if ((currentX > -1) && (currentX < 8) && (currentY > -1) && (currentY < 8))
                            {
                                //qDebug() << "currentX = " << currentX << " currentY = " << currentY;
                                game_board[currentX][currentY]->setPixmap(QPixmap(":/WhiteChess.png").scaled(90,90));
                                game_board[currentX][currentY]->color = white;
                                game_board[currentX][currentY]->setOpacity(1);
                            }
                        }
                    }
                }
            }
        }
    }

    if (!white_turn)
    {
        for (int deltaX = -1; deltaX <= 1; deltaX++)
        {
            for (int deltaY = -1; deltaY <=1; deltaY++)
            {
                int deltaPosX = 0;
                int deltaPosY = 0;
                bool flippableRow = false;
                while (row + deltaPosX != 8 && row + deltaPosX != -1 && column + deltaPosY != 8 && column + deltaPosY != -1)
                {
                    if ((row+deltaPosX+deltaX > -1) && (row+deltaPosX+deltaX < 8) && (column+deltaPosY+deltaY > -1) && (column+deltaPosY+deltaY < 8))
                    {
                        if (game_board[row+deltaPosX+deltaX][column+deltaPosY+deltaY]->color == black) break;
                        if (game_board[row+deltaPosX+deltaX][column+deltaPosY+deltaY]->color == empty) break;
                    }
                    deltaPosX += deltaX;
                    deltaPosY += deltaY;
                    flippableRow=true;
                }

                if ((row+deltaPosX+deltaX > -1) && (row+deltaPosX+deltaX < 8) && (column+deltaPosY+deltaY > -1) && (column+deltaPosY+deltaY < 8))
                {
                    if (flippableRow and game_board[row+deltaPosX+deltaX][column+deltaPosY+deltaY]->color ==black )
                    {
                        //qDebug() << "Flipping row to black";
                        int currentX = row;
                        int currentY = column;
                        while (currentX != (row + deltaPosX) || currentY != (column + deltaPosY))
                        {
                            currentX += deltaX;
                            currentY += deltaY;
                            game_board[currentX][currentY]->setPixmap(QPixmap(":/BlackChess.png").scaled(90,90));
                            game_board[currentX][currentY]->color = black;
                            game_board[currentX][currentY]->setOpacity(1);
                        }
                    }
                }
            }
        }
    }
 }

void Game2Scene::EndGame()
{
    int white_score = 0, black_score = 0;
    for (int i=0; i<8; i++)
    {
        for (int j=0; j<8; j++)
        {
            if (game_board[i][j]->color == black){black_score++;}
            else if (game_board[i][j]->color == white){white_score++;}
        }
    }

    Game2EndMenu *end_menu= new Game2EndMenu(nullptr, white_score, black_score, user);
    end_menu->show();
    views().at(0)->hide();
    delete this;
}
