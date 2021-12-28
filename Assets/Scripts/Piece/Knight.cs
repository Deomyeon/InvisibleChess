using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvisibleChess
{
    class Knight : Piece
    {
        public Knight(Color color)
        {
            type = PieceType.Knight;
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

            (int, int)[] range = new (int, int)[8] { (x + 1, y + 2), (x + 1, y - 2), (x - 2, y + 1), (x - 2, y - 1), (x + 2, y + 1), (x + 2, y - 1), (x - 1, y + 2), (x - 1, y - 2) };
            

            for (int i = 0; i < range.Length; i++)
            {
                int destX = range[i].Item1;
                int destY = range[i].Item2;

                if (destX >= 0 && destY >= 0 && destX < 8 && destY < 8)
                {
                    if (board[destX, destY].type == PieceType.None || board[destX, destY].color != color)
                    {
                        result.Add((destX, destY, "D"));
                    }
                }
            }

            return result;
        }
    }
}
