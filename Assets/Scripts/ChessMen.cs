using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ChessMen : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject moveDot;

    // Positions
    private int xBoard = -1;
    private int yBoard = -1;

    // variable to keep track of "black" player or "white" player
    private string player;

    // References for all the sprites that the chesspiece can be
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;

            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;

           


        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }
    public int GetXBoard()
    { 
        return xBoard; 
    }
    public int GetYBoard() 
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }
    private void OnMouseUp()
    {
        if(!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMoveDot();

            InitiateMoveDot();
        }
        
    }
    public void DestroyMoveDot()
    {
        GameObject[] moveDot = GameObject.FindGameObjectsWithTag("MoveDot"); 
        for (int i = 0; i < moveDot.Length; i++) 
        {
            Destroy(moveDot[i]);
        }
    }
    
    public void InitiateMoveDot()
    {
        switch(this.name)
        {
            case "black_queen":
            case "white_queen":

                LineMoveDot(1, 0);
                LineMoveDot(0, 1);
                LineMoveDot(1, 1);
                LineMoveDot(-1, 0);
                LineMoveDot(0, -1);
                LineMoveDot(-1, -1);
                LineMoveDot(-1, 1);
                LineMoveDot(1, -1);
                break;

            case "black_Knight":
            case "white_Knight":

                LMoveDot();
                break;

            case "black_bishop":
            case "white_bishop":

                LineMoveDot(1, 1);
                LineMoveDot(1, -1);
                LineMoveDot(-1, 1);
                LineMoveDot(-1, -1);
                break;

            case "black_king":
            case "white_king":

                SurroundMoveDot();
                break;

            case "black_rook":
            case "white_rook":

                LineMoveDot(1, 0);
                LineMoveDot(0, 1);
                LineMoveDot(-1, 0);
                LineMoveDot(0, -1);
                break;

            case "black_pawn":

                PawnMoveDot(xBoard, yBoard - 1);
                break;

            case "white_pawn":

                PawnMoveDot(xBoard, yBoard + 1);
                break;

        }
    }
    public void LineMoveDot(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MoveDotSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }
        if (sc.PositionOnBoard(x, y)&& sc.GetPosition(x, y).GetComponent<ChessMen>().player != player)
        {
            MoveDotAttackSpawn(x, y);
        }
    }
    public void LMoveDot()
    {
        PointMoveDot(xBoard + 1, yBoard + 2);
        PointMoveDot(xBoard - 1, yBoard + 2);
        PointMoveDot(xBoard + 2, yBoard + 1);
        PointMoveDot(xBoard + 2, yBoard - 1);
        PointMoveDot(xBoard + 1, yBoard - 2);
        PointMoveDot(xBoard - 1, yBoard - 2);
        PointMoveDot(xBoard - 2, yBoard + 1);
        PointMoveDot(xBoard - 2, yBoard - 1);

    }
    public void SurroundMoveDot()
    {
        PointMoveDot(xBoard, yBoard + 1);
        PointMoveDot(xBoard, yBoard - 1);
        PointMoveDot(xBoard - 1, yBoard - 1);
        PointMoveDot(xBoard - 1, yBoard - 0);
        PointMoveDot(xBoard - 1, yBoard + 1);
        PointMoveDot(xBoard + 1, yBoard - 1);
        PointMoveDot(xBoard + 1, yBoard - 0);
        PointMoveDot(xBoard + 1, yBoard + 1);

    }
    public void PointMoveDot(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if(cp == null)
            {
                MoveDotSpawn(x, y);
            }
            else if (cp.GetComponent<ChessMen>().player != player) 
            {
                MoveDotAttackSpawn(x, y);
            }
        }
    }
    public void PawnMoveDot(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x,y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MoveDotSpawn(x, y);
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null &&
                sc.GetPosition(x + 1, y).GetComponent<ChessMen>().player != player)
            {
                MoveDotAttackSpawn(x + 1, y);
            }
            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null &&
               sc.GetPosition(x - 1, y).GetComponent<ChessMen>().player != player)
            {
                MoveDotAttackSpawn(x - 1, y);
            }
        }
    }
    public void MoveDotSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(moveDot, new Vector3(x, y, -3.0f), Quaternion.identity);

        MoveDot mpScript = mp.GetComponent<MoveDot>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    public void MoveDotAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(moveDot, new Vector3(x, y, -3.0f), Quaternion.identity);

        MoveDot mpScript = mp.GetComponent<MoveDot>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
