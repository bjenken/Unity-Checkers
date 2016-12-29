using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite;
    public bool isKing;

    public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2)
    {
        //Are we moving on top of another piece?
        if (board[x2, y2] != null)
            return false;
        int tilesJumpedX = Mathf.Abs(x1 - x2);
        int tilesJumpedY = y2 - y1;
        if (isWhite || isKing)
        {
            if (tilesJumpedX == 1) //normal move
            {
                if (tilesJumpedY == 1)
                    return true;
            }
            else if (tilesJumpedX == 2)//jumping another piece
            {
                if (tilesJumpedY == 2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; //get jumped piece
                    if (p != null && p.isWhite == false)
                        return true;    
                }
            }
        }
        if (!isWhite || isKing)
        {
            if (tilesJumpedX == 1) //normal move
            {
                if (tilesJumpedY == -1)
                    return true;
            }
            else if (tilesJumpedX == 2)//jumping another piece
            {
                if (tilesJumpedY == -2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; //get jumped piece
                    if (p != null && p.isWhite == true)
                        return true;
                }
            }
        }

        return false;
    }
}
