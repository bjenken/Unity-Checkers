﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoard : MonoBehaviour {
    private static int boardSize = 8; // x == y
    public Piece[,] pieces = new Piece[boardSize, boardSize];
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

    private Vector3 boardOffset = new Vector3(-4.0f,0.25f,-4.0f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private bool isWhiteTurn;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;

    private Piece selectedPiece;
    private void Start()
    {
        isWhiteTurn = true;
        this.GenerateBoard();
    }

    private void Update()
    {
        UpdateMouseOver();

        // if it is my turn
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if (selectedPiece != null)
                UpdateHeldPiece(selectedPiece);
            if (Input.GetMouseButtonDown(0))
                SelectPiece(x, y);

            if (Input.GetMouseButtonUp(0))
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);
        }
    }
    private void UpdateMouseOver()
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out hit, 
            25.0f,
            LayerMask.GetMask("Board")))
        {
            // x and z since the board is on the "floor"
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);

        }else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }

    }

    private void SelectPiece(int x, int y)
    {
        //Out of Bounds
        if (x < 0 || x >= pieces.Length || y < 0 || y >= pieces.Length)
            return;
        Piece p = pieces[x, y];
        if (p != null)
        {
            selectedPiece = p;
            startDrag = mouseOver;
        }
    }

    private void TryMove (int x1, int y1, int x2, int y2)
    {
        //multiplayer support, might not need this
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];

        //Out of Bounds
        if (x2 < 0 || x2 >= pieces.Length || y2 < 0 || y2 >= pieces.Length)
        {
            if (selectedPiece != null)
                MovePiece(selectedPiece, x1, y1);

            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }

        //Piece has not removed, return it to its origin
        if (selectedPiece != null)
        {
            if (endDrag == startDrag)
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }
        }

        //Check if move is VALID
        if(selectedPiece.ValidMove(pieces, x1, y1, x2, y2))
        {
            //Did we jump another piece?
            if (Mathf.Abs(x1-x2) == 2)
            {
                Piece p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
                if (p != null)
                {
                    pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                    Destroy(p.gameObject);
                }
            }
            pieces[x2, y2] = selectedPiece;
            pieces[x1, y1] = null;
            MovePiece(selectedPiece, x2, y2);
            EndTurn();
            return;
        }
    }

    private void UpdateHeldPiece(Piece p)
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out hit,
            25.0f,
            LayerMask.GetMask("Board")))
        {
            p.transform.position = hit.point + Vector3.up;
        }
      
    }
    private void EndTurn()
    {
        selectedPiece = null;
        startDrag = Vector2.zero;

        isWhiteTurn = !isWhiteTurn;
        CheckVictory();
        return;
    }
    private bool CheckVictory()
    {
        return false;
    }
    private void GenerateBoard()
    {
        for (int y = 0; y < boardSize/2 - 1; y++)
        {
            bool oddRow = ((y+1) % 2 != 0);
            for (int x = 0; x < boardSize; x += 2)
            {
                //generate white pieces
                GeneratePiece((oddRow ? x : x + 1), y);
            }
        }

        for (int y = boardSize - 1; y > boardSize / 2 ; y--)
        {
            bool oddRow = ((y + 1) % 2 != 0);
            for (int x = 0; x < boardSize; x += 2)
            {
                //generate white pieces
                GeneratePiece((oddRow ? x : x + 1), y);
            }
        }
    }
    private void GeneratePiece(int x, int y)
    {
        bool isWhite = (y > boardSize/2 - 1) ? false : true;
        GameObject go = Instantiate(isWhite ? whitePiecePrefab : blackPiecePrefab) as GameObject;

        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }
    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    } 
}