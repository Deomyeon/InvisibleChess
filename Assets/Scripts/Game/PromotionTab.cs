using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionTab : MonoBehaviour
{
    public System.Action<InvisibleChess.PieceType> action;

    public Image knightImage;
    public Image bishopImage;
    public Image rookImage;
    public Image queenImage;

    [Space(20)]

    public Sprite whiteKnight;
    public Sprite blackKnight;
    public Sprite whiteBishop;
    public Sprite blackBishop;
    public Sprite whiteRook;
    public Sprite blackRook;
    public Sprite whiteQueen;
    public Sprite blackQueen;


    public void OpentPromotionTab()
    {
        gameObject.SetActive(true);
    }
    public void ClosePromotionTab()
    {
        action = null;
        gameObject.SetActive(false);
    }

    public void SetPromotionTab(Color color, System.Action<InvisibleChess.PieceType> action)
    {
        if (color == Color.White)
        {
            knightImage.sprite = whiteKnight;
            bishopImage.sprite = whiteBishop;
            rookImage.sprite = whiteRook;
            queenImage.sprite = whiteQueen;
        }
        else
        {
            knightImage.sprite = blackKnight;
            bishopImage.sprite = blackBishop;
            rookImage.sprite = blackRook;
            queenImage.sprite = blackQueen;
        }
        this.action = action;
    }

    public void SelectKnight()
    {
        action(InvisibleChess.PieceType.Knight);
        ClosePromotionTab();
    }
    public void SelectBishop()
    {
        action(InvisibleChess.PieceType.Bishop);
        ClosePromotionTab();
    }
    public void SelectRook()
    {
        action(InvisibleChess.PieceType.Rook);
        ClosePromotionTab();
    }
    public void SelectQueen()
    {
        action(InvisibleChess.PieceType.Queen);
        ClosePromotionTab();
    }
}
