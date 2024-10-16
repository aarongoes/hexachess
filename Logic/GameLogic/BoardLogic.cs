using System.Collections.Generic;
using System.Linq;
using Logic.Interfaces;
using Models;
using Models.ChessPieceObjects;
using Models.Enums;

namespace Logic
{
    public class BoardLogic : IBoardLogic
    {
        //public static readonly Dictionary<Guid, Board> Boards = new Dictionary<Guid, Board>();
        private IHexagonLogic hexagonLogic;

        public BoardLogic(IHexagonLogic hexagonLogic)
        {
            this.hexagonLogic = hexagonLogic;
        }
        
        public Board LoadBoard(Game game)
        {
            var board = new Board(game);

            createHexes(board);
            createGrid(board);
            assignPieces(board);
            setPossibleMoves(board);
            applyMoves(board);
            setAllPossibleMoves(board);
            return board;
        }
        
        public Board ApplyMove(Board board, Move move)
        {
            checkAndApplyMove(board, move);
            setAllPossibleMoves(board);
            return board;
        }
        
        private void createHexes(Board board)
        {
            var midWidth = board.Width / 2;
            var firstHex = new Hexagon(0, 0, HexagonColors.Normal);
            board.Hexes.Add(firstHex);
            var layer = 1;
            var current = firstHex;

            while (layer <= midWidth) //deze wordt uitgevoerd voor iedere laag
            {
                var newlayer = new List<Hexagon>();
                current = getNeighbours(board, current)[0];
                var count = 0;
                while (count < 6)
                {
                    var currentlayer = layer;

                    for (var i = 0; i < currentlayer; i++)
                    {
                        var neighbours = getNeighbours(board, current);
                        newlayer.Add(current);
                        switch (count)
                        {
                            case 0:
                                current = neighbours[2];
                                break;
                            case 1:
                                current = neighbours[3];
                                break;
                            case 2:
                                current = neighbours[4];
                                break;
                            case 3:
                                current = neighbours[5];
                                break;
                            case 4:
                                current = neighbours[0];
                                break;
                            case 5:
                                current = neighbours[1];
                                break;
                        }
                    }
                    count++;
                }
                layer++;
                board.Hexes.AddRange(newlayer);
            }
        }

        private List<Hexagon> getNeighbours(Board board, Hexagon hex)
        {
            var neighbours = new List<Hexagon>();


            var pairs = new List<KeyValuePair<decimal, decimal>>
            {
                new KeyValuePair<decimal, decimal>(0, 1),
                new KeyValuePair<decimal, decimal>(1, (decimal)0.5),
                new KeyValuePair<decimal, decimal>(1, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(0, -1),
                new KeyValuePair<decimal, decimal>(-1, (decimal)-0.5),
                new KeyValuePair<decimal, decimal>(-1, (decimal)0.5)
            };

            var switcher = false;
            foreach (var (key, value) in pairs)
            {
                var color = (int)hex.Color - 1;
                if (switcher)
                {
                    color += 2;
                }
                if (color > 2)
                {
                    color = 0;
                }
                if (color < 0)
                {
                    color = 2;
                }

                switcher = !switcher;
                var searchXPos = hex.X + key;
                var searchYPos = hex.Y + value;
                neighbours.Add(board.Hexes.FirstOrDefault(h => h.X == searchXPos && h.Y == searchYPos) ??
                               new Hexagon(searchXPos, searchYPos, (HexagonColors)color));
            }

            return neighbours;
        }
        
        private void createGrid(Board board) //Creates the grid with empty hexes so types can be assigned to them later.
        {
            for (decimal i = -5; i <= 5; i++)
            {
                var row = new List<Hexagon>();
                row.AddRange(board.Hexes.FindAll(h => h.X == i).OrderByDescending(h => h.Y));
                board.Grid.Add(row);
            }
        }

        public Hexagon getHexAtPosition(Board board, decimal x, decimal y)
        {
            return board.Hexes.FirstOrDefault(h => h.X == x && h.Y == y);
        }
        
        private void checkAndApplyMove(Board board, Move move) //Checks if the move is made by the player who's up, and if the new position is one of the possible positions.
        {
            if (isPlayerUp(board, move))
            {
                if (isPositionAllowed(board, move))
                {
                    movePiece(board, move);
                    board.Game.Moves.Add(move);
                    board.Succes = true;
                }
            }
        }

        private bool isPositionAllowed(Board board, Move move)
        {
            return getHexAtPosition(board, move.OldX,move.OldY).Piece.PossibleMoves
                       .FirstOrDefault(o => o.OldX == move.OldX && o.OldY == move.OldY && o.NewX == move.NewX && o.NewY == move.NewY) != null;
        }

        private bool isPlayerUp(Board board, Move move)
        {
            return move.Player == GetNextPlayer(board);
        }
        
        public int GetNextPlayer(Board board)
        {
            var player1 = 1;
            var player2 = 2;
            if (board.Game.Moves.Count != 0)
            {
                var player = getHexAtPosition(board, board.Game.Moves.Last().NewX, board.Game.Moves.Last().NewY).Piece
                                .Player;
                if (player == player1)
                {
                    return player2;
                }
            }
            return player1;
            
        }
        
        public int GetPlayerAssignedToPiecePosition(Board board, decimal x, decimal y)
        {
            return getHexAtPosition(board, x, y).Piece.Player;
        }

        private void applyMoves(Board board) //Applies the moves inside of the game object to the board.
        {
            foreach (var move in board.Game.Moves)
            {
                movePiece(board, move);
            }
        }

        private void movePiece(Board board, Move move) //Assigns the piece to the new hex and assigns an empty piece object to the old hex.
        {
            hexagonLogic.AssignPiece(getHexAtPosition(board, move.NewX, move.NewY), getHexAtPosition(board, move.OldX, move.OldY).Piece);
            hexagonLogic.RemovePiece(getHexAtPosition(board, move.OldX, move.OldY));
            getHexAtPosition(board, move.NewX, move.NewY).Piece.SetPossibleMoves(getHexAtPosition(board, move.NewX, move.NewY), board.Hexes);
        }

        private void assignPieces(Board board) //Assigns all pieces to their correct positions.
        {
            addPawnsForPlayer1(board);
            addPawnsForPlayer2(board);
            addBishopsForPlayer1(board);
            addBishopsForPlayer2(board);
            addRooksForPlayer1(board);
            addRooksForPlayer2(board);
            addKnightsForPlayer1(board);
            addKnightsForPlayer2(board);
            addKingForPlayer1(board);
            addKingForPlayer2(board);
            addQueenForPlayer1(board);
            addQueenForPlayer2(board);
        }
        
        private void setPossibleMoves(Board board) //Assigns all pieces to their correct positions.
        {
            setPossibleMovesPawnsForPlayer1(board);
            setPossibleMovesPawnsForPlayer2(board);
            setPossibleMovesBishopsForPlayer1(board);
            setPossibleMovesBishopsForPlayer2(board);
            setPossibleMovesRooksForPlayer1(board);
            setPossibleMovesRooksForPlayer2(board);
            setPossibleMovesKnightsForPlayer1(board);
            setPossibleMovesKnightsForPlayer2(board);
            setPossibleMovesKingForPlayer1(board);
            setPossibleMovesKingForPlayer2(board);
            setPossibleMovesQueenForPlayer1(board);
            setPossibleMovesQueenForPlayer2(board);
        }
        
        private void setAllPossibleMoves(Board board) //Assigns all pieces to their correct positions.
        {
            foreach (var hex in board.Hexes)
            {
                if (hex.Piece != null)
                {
                    if (hex.Piece.PieceType != ChessPieces.None)
                    {
                        hex.Piece.SetPossibleMoves(hex, board.Hexes);
                    }
                }
            }
        }

        private void addPawnsForPlayer1(Board board)
        {
            var requiredAmount = 9;
            var center = 0;
            var left = 0;
            var right = 0;
            decimal yAxis = -1;
            hexagonLogic.AssignPiece(getHexAtPosition(board, center, yAxis), new Pawn(1));
            requiredAmount--;
            var amountOfPiecesPlacedPerTime = 2;
            for (int i = 0; i <= requiredAmount; i += amountOfPiecesPlacedPerTime)
            {
                hexagonLogic.AssignPiece(getHexAtPosition(board, left, yAxis), new Pawn(1));
                hexagonLogic.AssignPiece(getHexAtPosition(board, right, yAxis), new Pawn(1));
                left--;
                right++;
                yAxis -= (decimal)0.5;
            }
        }

        private void addPawnsForPlayer2(Board board)
        {
            var requiredAmount = 9;
            var center = 0;
            var left = 0;
            var right = 0;
            decimal yAxis = 1;
            hexagonLogic.AssignPiece(getHexAtPosition(board, center, yAxis), new Pawn(2));
            requiredAmount--;
            var amountOfPiecesPlacedPerTime = 2;
            for (int i = 0; i <= requiredAmount; i += amountOfPiecesPlacedPerTime)
            {
                hexagonLogic.AssignPiece(getHexAtPosition(board, left, yAxis), new Pawn(2));
                hexagonLogic.AssignPiece(getHexAtPosition(board, right, yAxis), new Pawn(2));
                left--;
                right++;
                yAxis += (decimal)0.5;
            }
        }

        private void addBishopsForPlayer1(Board board)
        {
            var x = 0;
            var y = -3;
            for (int i = 0; i < 3; i++)
            {
                hexagonLogic.AssignPiece(getHexAtPosition(board, x, y), new Bishop(1));
                y--;
            }
        }

        private void addBishopsForPlayer2(Board board)
        {
            var x = 0;
            var y = 3;
            for (int i = 0; i < 3; i++)
            {
                hexagonLogic.AssignPiece(getHexAtPosition(board, x, y), new Bishop(2));
                y++;
            }
        }

        private void addRooksForPlayer1(Board board)
        {
            var x = 0;
            var y = (decimal)-3.5;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x + 3, y), new Rook(1));
            hexagonLogic.AssignPiece(getHexAtPosition(board, x - 3, y), new Rook(1));
        }

        private void addRooksForPlayer2(Board board)
        {
            var x = 0;
            var y = (decimal)3.5;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x + 3, y), new Rook(2));
            hexagonLogic.AssignPiece(getHexAtPosition(board, x - 3, y), new Rook(2));
        }

        private void addKnightsForPlayer1(Board board)
        {
            var x = 0;
            var y = -4;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x + 2, y), new Knight(1));
            hexagonLogic.AssignPiece(getHexAtPosition(board, x - 2, y), new Knight(1));
        }

        private void addKnightsForPlayer2(Board board)
        {
            var x = 0;
            var y = 4;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x + 2, y), new Knight(2));
            hexagonLogic.AssignPiece(getHexAtPosition(board, x - 2, y), new Knight(2));
        }

        private void addKingForPlayer1(Board board)
        {
            var x = 1;
            var y = (decimal)-4.5;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x, y), new King(1));
        }

        private void addKingForPlayer2(Board board)
        {
            var x = 1;
            var y = (decimal)4.5;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x, y), new King(2));
        }

        private void addQueenForPlayer1(Board board)
        {
            var x = -1;
            var y = (decimal)-4.5;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x, y), new Queen(1));
        }

        private void addQueenForPlayer2(Board board)
        {
            var x = -1;
            var y = (decimal)4.5;
            hexagonLogic.AssignPiece(getHexAtPosition(board, x, y), new Queen(2));
        }
        
        private void setPossibleMovesPawnsForPlayer1(Board board)
        {
            var requiredAmount = 9;
            var center = 0;
            var left = 0;
            var right = 0;
            decimal yAxis = -1;
            getHexAtPosition(board, left, yAxis).Piece.SetPossibleMoves(getHexAtPosition(board, center, yAxis), board.Hexes);
            requiredAmount--;
            var amountOfPiecesPlacedPerTime = 2;
            for (int i = 0; i <= requiredAmount; i += amountOfPiecesPlacedPerTime)
            {
                getHexAtPosition(board, left, yAxis).Piece.SetPossibleMoves(getHexAtPosition(board, left, yAxis), board.Hexes);
                getHexAtPosition(board, right, yAxis).Piece.SetPossibleMoves(getHexAtPosition(board, right, yAxis), board.Hexes);
                left--;
                right++;
                yAxis -= (decimal)0.5;
            }
        }

        private void setPossibleMovesPawnsForPlayer2(Board board)
        {
            var requiredAmount = 9;
            var center = 0;
            var left = 0;
            var right = 0;
            decimal yAxis = 1;
            getHexAtPosition(board, left, yAxis).Piece.SetPossibleMoves(getHexAtPosition(board, center, yAxis), board.Hexes);
            requiredAmount--;
            var amountOfPiecesPlacedPerTime = 2;
            for (int i = 0; i <= requiredAmount; i += amountOfPiecesPlacedPerTime)
            {
                getHexAtPosition(board, left, yAxis).Piece.SetPossibleMoves(getHexAtPosition(board, left, yAxis), board.Hexes);
                getHexAtPosition(board, right, yAxis).Piece.SetPossibleMoves(getHexAtPosition(board, right, yAxis), board.Hexes);
                left--;
                right++;
                yAxis += (decimal)0.5;
            }
        }

        private void setPossibleMovesBishopsForPlayer1(Board board)
        {
            var x = 0;
            var y = -3;
            for (int i = 0; i < 3; i++)
            {
                getHexAtPosition(board, x, y).Piece.SetPossibleMoves(getHexAtPosition(board, x, y), board.Hexes);
                y--;
            }
        }

        private void setPossibleMovesBishopsForPlayer2(Board board)
        {
            var x = 0;
            var y = 3;
            for (int i = 0; i < 3; i++)
            {
                getHexAtPosition(board, x, y).Piece.SetPossibleMoves(getHexAtPosition(board, x, y), board.Hexes);
                y++;
            }
        }

        private void setPossibleMovesRooksForPlayer1(Board board)
        {
            var x = 0;
            var y = (decimal)-3.5;
            getHexAtPosition(board, x + 3, y).Piece.SetPossibleMoves(getHexAtPosition(board, x + 3, y), board.Hexes);
            getHexAtPosition(board, x - 3, y).Piece.SetPossibleMoves(getHexAtPosition(board, x - 3, y), board.Hexes);
        }

        private void setPossibleMovesRooksForPlayer2(Board board)
        {
            var x = 0;
            var y = (decimal)3.5;
            getHexAtPosition(board, x + 3, y).Piece.SetPossibleMoves(getHexAtPosition(board, x + 3, y), board.Hexes);
            getHexAtPosition(board, x - 3, y).Piece.SetPossibleMoves(getHexAtPosition(board, x - 3, y), board.Hexes);
        }

        private void setPossibleMovesKnightsForPlayer1(Board board)
        {
            var x = 0;
            var y = -4;
            getHexAtPosition(board, x + 2, y).Piece.SetPossibleMoves(getHexAtPosition(board, x + 2, y), board.Hexes);
            getHexAtPosition(board, x - 2, y).Piece.SetPossibleMoves(getHexAtPosition(board, x - 2, y), board.Hexes);
        }

        private void setPossibleMovesKnightsForPlayer2(Board board)
        {
            var x = 0;
            var y = 4;
            getHexAtPosition(board, x + 2, y).Piece.SetPossibleMoves(getHexAtPosition(board, x + 2, y), board.Hexes);
            getHexAtPosition(board, x - 2, y).Piece.SetPossibleMoves(getHexAtPosition(board, x - 2, y), board.Hexes);
        }

        private void setPossibleMovesKingForPlayer1(Board board)
        {
            var x = 1;
            var y = (decimal)-4.5;
            getHexAtPosition(board, x, y).Piece.SetPossibleMoves(getHexAtPosition(board, x, y), board.Hexes);
        }

        private void setPossibleMovesKingForPlayer2(Board board)
        {
            var x = 1;
            var y = (decimal)4.5;
            getHexAtPosition(board, x, y).Piece.SetPossibleMoves(getHexAtPosition(board, x, y), board.Hexes);
        }

        private void setPossibleMovesQueenForPlayer1(Board board)
        {
            var x = -1;
            var y = (decimal)-4.5;
            getHexAtPosition(board, x, y).Piece.SetPossibleMoves(getHexAtPosition(board, x, y), board.Hexes);
        }

        private void setPossibleMovesQueenForPlayer2(Board board)
        {
            var x = -1;
            var y = (decimal)4.5;
            getHexAtPosition(board, x, y).Piece.SetPossibleMoves(getHexAtPosition(board, x, y), board.Hexes);
        }
    }
}