using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvisibleChess
{
    class King : Piece
    {
        public King(Color color)
        {
            type = PieceType.King;
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

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1) continue;

                    if (x + i - 1 >= 0 && y + j - 1 >= 0 && x + i - 1 < 8 && y + j - 1 < 8 && (board[x + i - 1, y + j - 1].type == PieceType.None || board[x + i - 1, y + j - 1].color != color))
                    {
                        result.Add((x + i - 1, y + j - 1, "D"));
                    }
                }
            }

            if (moveCount == 0)
            {
                ChessPiece temp;
                temp = board[x - 4, y];
                if (temp.type == PieceType.Rook && temp.color == color && temp.moveCount == 0)
                {
                    if (board[x - 1, y].type == PieceType.None && board[x - 2, y].type == PieceType.None && board[x - 3, y].type == PieceType.None)
                    {
                        result.Add((x - 2, y, "CR"));
                    }
                }
                temp = board[x + 3, y];
                if (temp.type == PieceType.Rook && temp.color == color && temp.moveCount == 0)
                {
                    if (board[x + 1, y].type == PieceType.None && board[x + 2, y].type == PieceType.None)
                    {
                        result.Add((x + 2, y, "CL"));
                    }
                }
            }

            return result;
        }
    }
}
