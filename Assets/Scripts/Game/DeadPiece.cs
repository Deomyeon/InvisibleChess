using InvisibleChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadPiece : MonoBehaviour
{
    public Image Queen;
    public Image Rook;
    public Image Bishop;
    public Image Knight;
    public Image Pawn;

    [Space(20)]

    public Sprite whiteQueen;
    public Sprite whiteRook;
    public Sprite whiteBishop;
    public Sprite whiteKnight;
    public Sprite whitePawn;

    [Space(20)]

    public Sprite blackQueen;
    public Sprite blackRook;
    public Sprite blackBishop;
    public Sprite blackKnight;
    public Sprite blackPawn;

    [Space(20)]

    public Text QueenText;
    public Text RookText;
    public Text BishopText;
    public Text KnightText;
    public Text PawnText;

    private int QueenCount;
    private int RookCount;
    private int BishopCount;
    private int KnightCount;
    private int PawnCount;

    public void SetColor(Color color)
    {
        if (color == Color.White)
        {
            Queen.sprite = whiteQueen;
            Rook.sprite = whiteRook;
            Bishop.sprite = whiteBishop;
            Knight.sprite = whiteKnight;
            Pawn.sprite = whitePawn;
        }
        else
        {
            Queen.sprite = blackQueen;
            Rook.sprite = blackRook;
            Bishop.sprite = blackBishop;
            Knight.sprite = blackKnight;
            Pawn.sprite = blackPawn;
        }

        QueenCount = 0;
        RookCount = 0;
        BishopCount = 0;
        KnightCount = 0;
        PawnCount = 0;

        QueenText.text = QueenCount.ToString();
        RookText.text = RookCount.ToString();
        BishopText.text = BishopCount.ToString();
        KnightText.text = KnightCount.ToString();
        PawnText.text = PawnCount.ToString();
    }

    public void AddCount(PieceType type)
    {
        switch (type)
        {
            case PieceType.Queen:
                QueenCount++;
                QueenText.text = QueenCount.ToString();
                break;
            case PieceType.Rook:
                RookCount++;
                RookText.text = RookCount.ToString();
                break;
            case PieceType.Bishop:
                BishopCount++;
                BishopText.text = BishopCount.ToString();
                break;
            case PieceType.Knight:
                KnightCount++;
                KnightText.text = KnightCount.ToString();
                break;
            case PieceType.Pawn:
                PawnCount++;
                PawnText.text = PawnCount.ToString();
                break;
        }
    }
}
