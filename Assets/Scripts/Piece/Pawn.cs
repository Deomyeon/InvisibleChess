using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvisibleChess
{
    class Pawn : Piece
    {
        public Pawn(Color color)
        {
            type = PieceType.Pawn;
            this.color = color;
            moveCount = 0;
        }

        public override HashSet<(int, int, string)> MoveRange(ChessPiece[,] board, int x, int y)
        {
            HashSet<(int, int, string)> result = new HashSet<(int, int, string)>();

            if (!(x >= 0 && y >= 0 && x < 8 && y < 8))
            {
                return result;
            }

            int i = (color == Color.White) ? 1 : -1;

            if (y + i * 2 >= 0 && y + i * 2 < 8 && board[x, y + i * 2].type == PieceType.None && board[x, y + i].type == PieceType.None && moveCount == 0)
            {
                result.Add((x, y + i * 2, "D"));
            }
            if (y + i >= 0 && y + i < 8 && board[x, y + i].type == PieceType.None)
            {
                result.Add((x, y + i, "D"));
            }


            if (x + 1 >= 0 && x + 1 < 8 && y + i >= 0 && y + i < 8 && board[x + 1, y + i].type != PieceType.None && board[x + 1, y + i].color != color)
            {
                result.Add((x + 1, y + i, "D"));
            }
            if (x - 1 >= 0 && x - 1 < 8 && y + i >= 0 && y + i < 8 && board[x - 1, y + i].type != PieceType.None && board[x - 1, y + i].color != color)
            {
                result.Add((x - 1, y + i, "D"));
            }

            int j = (color == Color.White) ? 4 : 3;

            if (x + 1 >= 0 && x + 1 < 8)
            {
                ChessPiece temp = board[x + 1, y];
                if (temp.type == PieceType.Pawn && temp.color != color && temp.moveCount == 1 && y == j)
                {
                    if (y + i >= 0 && y + i < 8 && (board[x + 1, y + i].type == PieceType.None || board[x + 1, y + i].color != color))
                    {
                        result.Add((x + 1, y + i, "E"));
                    }
                }
            }
            if (x - 1 >= 0 && x - 1 < 8)
            {
                ChessPiece temp = board[x - 1, y];
                if (temp.type == PieceType.Pawn && temp.color != color && temp.moveCount == 1 && y == j)
                {
                    if (y + i >= 0 && y + i < 8 && (board[x - 1, y + i].type == PieceType.None || board[x - 1, y + i].color != color))
                    {
                        result.Add((x - 1, y + i, "E"));
                    }
                }
            }



            return result;
        }
    }
}
