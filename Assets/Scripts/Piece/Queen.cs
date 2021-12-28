using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvisibleChess
{
    class Queen : Piece
    {
        public Queen(Color color)
        {
            type = PieceType.Queen;
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


            int x1, y1;
            for (x1 = x + 1, y1 = y + 1; x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].type == PieceType.None; x1++, y1++)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));
            for (x1 = x - 1, y1 = y + 1; x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].type == PieceType.None; x1--, y1++)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));
            for (x1 = x + 1, y1 = y - 1; x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].type == PieceType.None; x1++, y1--)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));
            for (x1 = x - 1, y1 = y - 1; x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].type == PieceType.None; x1--, y1--)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));


            for (x1 = x + 1, y1 = y; x1 >= 0 && x1 < 8 && board[x1, y1].type == PieceType.None; x1++)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));
            for (x1 = x - 1, y1 = y; x1 >= 0 && x1 < 8 && board[x1, y1].type == PieceType.None; x1--)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));
            for (x1 = x, y1 = y + 1; y1 >= 0 && y1 < 8 && board[x1, y1].type == PieceType.None; y1++)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));
            for (x1 = x, y1 = y - 1; y1 >= 0 && y1 < 8 && board[x1, y1].type == PieceType.None; y1--)
            {
                result.Add((x1, y1, "D"));
            }
            if (x1 >= 0 && y1 >= 0 && x1 < 8 && y1 < 8 && board[x1, y1].color != color) result.Add((x1, y1, "D"));

            return result;
        }
    }
}
