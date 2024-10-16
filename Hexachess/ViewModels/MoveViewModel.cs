using System.ComponentModel.DataAnnotations;
using Models;

namespace Hexachess.Models
{
    public class MoveViewModel
    {
        [Required] [Range(-5, 5, ErrorMessage = "Position not allowed")]
        public string OldX;

        [Required] [Range(-5, 5, ErrorMessage = "Position not allowed")]
        public int OldY;

        [Required] [Range(-5, 5, ErrorMessage = "Position not allowed")]
        public string NewX;

        [Required] [Range(-5, 5, ErrorMessage = "Position not allowed")]
        public int NewY;

        [Required] public int Player;

        public MoveViewModel()
        {
            
        }

        public MoveViewModel(Move move)
        {
            OldX = getX(move.OldX);
            OldY = getY(move.OldX, move.OldY);
            NewX = getX(move.NewX);
            NewY = getY(move.NewX, move.NewY);
            Player = move.Player;
        }

        private string getX(decimal x)
        {
            switch (x)
            {
                case -5:
                    return "a";
                case -4:
                    return "b";
                case -3:
                    return "c";
                case -2:
                    return "d";
                case -1:
                    return "e";
                case 0:
                    return "f";
                case 1:
                    return "g";
                case 2:
                    return "h";
                case 3:
                    return "i";
                case 4:
                    return "j";
                case 5:
                    return "a";
                default:
                    return "";
            }
        }

        private int getY(decimal x, decimal y)
        {
            switch (x)
            {
                case -5:
                    return (int)(y + (decimal)3.5);
                case -4:
                    return (int)(y + 4);
                case -3:
                    return (int)(y + (decimal)4.5);
                case -2:
                    return (int)(y + 5);
                case -1:
                    return (int)(y + (decimal)5.5);
                case 0:
                    return (int)(y + 6);
                case 1:
                    return (int)(y + (decimal)5.5);
                case 2:
                    return (int)(y + 5);
                case 3:
                    return (int)(y + (decimal)4.5);
                case 4:
                    return (int)(y + 4);
                case 5:
                    return (int)(y + (decimal)3.5);
                default:
                    return 0;
            }
        }
    }
}