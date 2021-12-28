using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvisibleChess
{
    public enum PieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King,
        Blind
    }

    class Piece
    {
        public PieceType type;
        public Color color;

        public int moveCount;

        public Piece()
        {
            type = PieceType.None;
            moveCount = 0;
        }

        ~Piece()
        {

        }

        public virtual HashSet<(int, int, string)> MoveRange(ChessPiece[,] board, int x, int y)
        {
            return null;
        }
    }
}
