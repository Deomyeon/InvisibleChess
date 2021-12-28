using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Position : MonoBehaviour
{
    public (int, int) current;

    public (int, int) position;
    public string option;

    public UserGame game;

    public UnityEngine.Color[] colors = new UnityEngine.Color[2];

    public void SetPosition((int, int) current, (int, int) position, string option, UserGame game)
    {
        this.current = current;
        this.position = position;
        this.option = option;
        this.game = game;

        if (option == "E" || game.board[position.Item1, position.Item2].type != InvisibleChess.PieceType.None && game.board[current.Item1, current.Item2].color != game.board[position.Item1, position.Item2].color)
        {
            GetComponent<Image>().color = colors[1];
        }
        else
        {
            GetComponent<Image>().color = colors[0];
        }

        GetComponent<Button>().onClick.RemoveListener(SelectPosition);
        GetComponent<Button>().onClick.AddListener(SelectPosition);
    }

    private void SelectPosition()
    {
        if (!game.moveLock)
        {
            game.moveLock = true;

            if (game.board[current.Item1, current.Item2].type == InvisibleChess.PieceType.Pawn && ((position.Item2 == 0 && game.board[current.Item1, current.Item2].color == Color.Black) || (position.Item2 == 7 && game.board[current.Item1, current.Item2].color == Color.White)))
            {
                game.promotionTab.SetPromotionTab(game.turnColor, (type) =>
                {
                    GameManager.Instance.CallQuery($"Move~{GameManager.Instance.Token}#{"P"}#{current.Item1}#{current.Item2}#{position.Item1}#{position.Item2}#{option}#{(int)type}#", (s) =>
                    {
                        game.moveLock = false;
                        InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                    });
                });
                game.promotionTab.OpentPromotionTab();
            }
            else
            {
                GameManager.Instance.CallQuery($"Move~{GameManager.Instance.Token}#{option}#{current.Item1}#{current.Item2}#{position.Item1}#{position.Item2}#", (s) =>
                {
                    game.moveLock = false;
                    InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                });
            }
        }
    }
}
