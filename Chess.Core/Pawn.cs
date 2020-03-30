using Chess.Models;
using System.Collections.Generic;

namespace Chess.Core
{
    //TODO: Create interface IFigure with methods CalculatePossibleMoves and Move
    public class Pawn
    {
        private static List<Coordinates> AllPossibleCoordinates
        {
            get
            {
                List<Coordinates> allPossibleCoordinates = new List<Coordinates>();

                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        Coordinates coordinates = new Coordinates();
                        coordinates.Row = i;
                        coordinates.Col = j;

                        allPossibleCoordinates.Add(coordinates);
                    }
                }

                return allPossibleCoordinates;
            }
        }

        public static Figure CalculatePossibleMoves(Board board, Figure figure)
        {
            var stepSize = 1;

            figure.PossibleMoves = AllPossibleCoordinates;

            figure.PossibleMoves = figure.PossibleMoves.FindAll(possibleMove =>
                board.Figures.Exists(f =>
                    {
                        if (figure.Color == "Black")
                        {
                            if(f.Coordinates.Row == possibleMove.Row + 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (f.Coordinates.Row == possibleMove.Row - 1)
                            {
                                return false;
                            }
                        }

                        return f.Coordinates.Row == possibleMove.Row && f.Coordinates.Col == possibleMove.Col;
                    }) == false);

            figure.PossibleMoves = figure.Color == "Black"
                ? figure.PossibleMoves.FindAll(
                    possibleMove =>
                        possibleMove.Row == figure.Coordinates.Row + 1 &&

                        (
                                possibleMove.Col == figure.Coordinates.Col
                                                ||
                                (
                                    possibleMove.Col == figure.Coordinates.Col - 1 &&
                                    board.Figures.Exists(f => f.Color == "White" && f.Coordinates.Row == figure.Coordinates.Row + 1 && f.Coordinates.Col == figure.Coordinates.Col - 1)
                                )
                                                ||
                                (
                                    possibleMove.Col == figure.Coordinates.Col + 1 &&
                                    board.Figures.Exists(f => f.Color == "White" && f.Coordinates.Row == figure.Coordinates.Row + 1 && f.Coordinates.Col == figure.Coordinates.Col + 1)
                                )
                        )
                )
                : figure.PossibleMoves.FindAll(
                    possibleMove =>
                        possibleMove.Row == figure.Coordinates.Row - 1 &&

                        (
                                possibleMove.Col == figure.Coordinates.Col
                                                ||
                                (
                                    possibleMove.Col == figure.Coordinates.Col - 1 &&
                                    board.Figures.Exists(f => f.Color == "Black" && f.Coordinates.Row == figure.Coordinates.Row - 1 && f.Coordinates.Col == figure.Coordinates.Col - 1)
                                )
                                                ||
                                (
                                    possibleMove.Col == figure.Coordinates.Col + 1 &&
                                    board.Figures.Exists(f => f.Color == "Black" && f.Coordinates.Row == figure.Coordinates.Row - 1 && f.Coordinates.Col == figure.Coordinates.Col + 1)
                                )
                        )
                );            

            return figure;
        }

        public static Figure Move(Figure figure, int targetRow, int targetCol)
        {
            var isPossibleMove = figure.PossibleMoves
                .Exists(possibleMove => possibleMove.Row == targetRow && possibleMove.Col == targetCol);

            if(isPossibleMove)
            {
                figure.Coordinates.Row = targetRow;
                figure.Coordinates.Col = targetCol;
            }

            return figure;
        }
    }
}
