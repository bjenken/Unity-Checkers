using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite;
    public bool isKing;

    public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2, bool hasKilled)
    {
        //Are we moving on top of another piece?
        if (board[x2, y2] != null)
            return false;
        int tilesJumpedX = Mathf.Abs(x2 - x1);
        int tilesJumpedY = y2 - y1;
        if (isWhite || isKing)
        {
            if (tilesJumpedX == 1) //normal move
            {
                if (tilesJumpedY == 1 && !hasKilled)
                    return true;
            }
            else if (tilesJumpedX == 2)//jumping another piece
            {
                if (tilesJumpedY == 2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; //get jumped piece

                    if (p != null && p.isWhite != isWhite)
                        return true;    
                }
            }
        }
        if (!isWhite || isKing)
        {
            if (tilesJumpedX == 1) //normal move
            {
                if (tilesJumpedY == -1 && !hasKilled)
                    return true;
            }
            else if (tilesJumpedX == 2)//jumping another piece
            {
                if (tilesJumpedY == -2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; //get jumped piece
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }

        return false;
    }

    public bool CanStillKill(Piece[,] board, int x, int y, int boardSize)
    {
        if (this.isWhite || this.isKing)
        {
            //make sure we have room at the end to jump
            if (y < boardSize - 2)
            {
                //check right jump
                if (x < boardSize - 2)
                {
                    if (board[x + 2, y + 2] == null && board[x + 1, y + 1] != null)
                    {
                        Piece p = board[x + 1, y + 1];
                        if (p != null && (p.isWhite != this.isWhite))
                            return true; //we can kill this piece
                    }
                }
                //check left jump
                if (x >= 2)
                {
                    if (board[x - 2, y + 2] == null && board[x - 1, y + 1] != null)
                    {
                        Piece p = board[x - 1, y + 1];
                        if (p != null && (p.isWhite != this.isWhite))
                            return true; //we can kill this piece
                    }
                }
            }
        }
        if (!this.isWhite || isKing)
        {
                //make sure we have room at the end to jump
                if (y >= 2)
                {
                    //check right jump
                    if (x < boardSize - 2)
                    {
                        if (board[x + 2, y - 2] == null && board[x + 1, y - 1] != null)
                        {
                            Piece p = board[x + 1, y - 1];
                            if (p != null && (p.isWhite != this.isWhite))
                                return true; //we can kill this piece
                        }
                    }
                    //check left jump
                    if (x >= 2)
                    {
                        if (board[x - 2, y - 2] == null && board[x - 1, y - 1] != null)
                        {
                            Piece p = board[x - 1, y - 1];
                            if (p != null && (p.isWhite != this.isWhite))
                                return true; //we can kill this piece
                        }
                    }
                }
         }

        return false;
    }
}
