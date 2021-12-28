using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InvisibleChess;
using UnityEngine.UI;
using System.Linq;

public class ChessPiece : MonoBehaviour
{
    public int x;
    public int y;

    public Image image;
         
    public PieceType type;
    public Color color;
    public int moveCount;

    public UserGame game;


    public void SetData(int x, int y, PieceType type, Color color, int moveCount, Sprite sprite, UserGame game)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.color = color;
        this.moveCount = moveCount;

        if (image != null)
            image.sprite = sprite;

        this.game = game;

        try
        {
            if (GetComponent<Button>() != null)
            {
                GetComponent<Button>().onClick.RemoveListener(SelectPiece);
                GetComponent<Button>().onClick.AddListener(SelectPiece);
            }
        }
        catch
        {

        }
    }

    private void SelectPiece()
    {
        try
        {
            for (int i = 0; i < game.positionsParent.transform.childCount; i++)
            {
                game.positionsParent.transform.GetChild(i).gameObject.SetActive(false);
            }

            if (color == ((GameManager.Instance.roomRole == RoomRole.White) ? Color.White : Color.Black) && color == game.turnColor)
            {
                Piece piece = null;

                switch (type)
                {
                    case PieceType.Pawn:
                        piece = new Pawn(color);
                        break;
                    case PieceType.Knight:
                        piece = new Knight(color);
                        break;
                    case PieceType.Bishop:
                        piece = new Bishop(color);
                        break;
                    case PieceType.Rook:
                        piece = new Rook(color);
                        break;
                    case PieceType.Queen:
                        piece = new Queen(color);
                        break;
                    case PieceType.King:
                        piece = new King(color);
                        break;
                }

                piece.moveCount = this.moveCount;

                HashSet<(int, int, string)> range = piece.MoveRange(game.board, x, y);

                {
                    List<(int, int, string)> l = new List<(int, int, string)>();

                    foreach (var item in range)
                    {
                        if (game.board[item.Item1, item.Item2].type == PieceType.Blind)
                        {
                            l.Add(item);
                        }
                    }

                    foreach (var item in l)
                    {
                        range.Remove(item);
                    }
                }

                {
                    List<(int, int, string)> s = range.ToList();
                    for (int i = 0; i < s.Count; i++)
                    {
                        game.positionsParent.transform.GetChild(i).gameObject.SetActive(true);
                        Position pos = game.positionsParent.transform.GetChild(i).GetComponent<Position>();
                        pos.SetPosition((x, y), (s[i].Item1, s[i].Item2), s[i].Item3, game);

                        pos.transform.localPosition = new Vector3(game.boardX[(GameManager.Instance.roomRole != RoomRole.White) ? 7 - s[i].Item1 : s[i].Item1], game.boardY[(GameManager.Instance.roomRole == RoomRole.White) ? 7 - s[i].Item2 : s[i].Item2], 0);
                    }
                }
            }
        }
        catch
        {

        }
    }
}
