using Models.Utils;

namespace Models
{
    public class Move : DatabaseItem
    {
        [Property] public int GameId { get; set; }
        [Property] public decimal OldX { get; set; }
        [Property] public decimal OldY { get; set; }
        [Property] public decimal NewX { get; set; }
        [Property] public decimal NewY { get; set; }
        [Property] public int Player { get; set; }
        
        public int Score { get; set; }
        
        public Move()
        {
        }

        public Move(Hexagon oldPosition, Hexagon newPosition)
        {
            OldX = oldPosition.X;
            OldY = oldPosition.Y;
            NewX = newPosition.X;
            NewY = newPosition.Y;
            Score = newPosition.Piece.Score - oldPosition.Piece.Score;
        }
        
        public Move(decimal oldX, decimal oldY, decimal newX, decimal newY, int gameid = 0, int player = -1)
        {
            OldX = oldX;
            OldY = oldY;
            NewX = newX;
            NewY = newY;
            GameId = gameid;
            Player = player;
        }

        public void ConvertDecimals()
        {
            OldX /= 10;
            OldY /= 10;
            NewX /= 10;
            NewY /= 10;
        }
    }
}