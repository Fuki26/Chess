using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Chess.Models;
using Type = Chess.Models.Type;

namespace Chess.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private static List<Coordinates> AllChessCoordinates
        {
            get
            {
                List<Coordinates> coordinates = new List<Coordinates>();
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        coordinates.Add(new Coordinates() { Row = i, Col = j });
                    }
                }

                return coordinates;
            }
        }

        public static Board boardModel = new Board();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private Figure SetFigureInitialValues(Figure figure, string id, int column, int row, Color color)
        {
            figure.Id = id;
            figure.Coordinates = new Coordinates()
            {
                Row = row,
                Col = column
            };
            figure.Color = color;
            return figure;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            boardModel.Figures.Clear();
            int otherFiguresRowNumber = 1;
            int pawnsRowNumber = 2;
            Color color = Color.Black;
            // TODO: when the user choose type of color here it should be the oposite color

            for (int j = 0; j < 2; j++)
            {
                if (j == 1)
                {
                    pawnsRowNumber = 7;
                    otherFiguresRowNumber = 8;
                    color = Color.White;
                }

                for (int i = 1; i <= 8; i++)
                {
                    // pawn
                    FiguresFactory figuresFactory = new FiguresFactory();
                    var pawn = figuresFactory.getPawn();
                    var adjustedFigure = this.SetFigureInitialValues(pawn, i.ToString() + color + pawn.Type, i, pawnsRowNumber, color);
                    boardModel.Figures.Add(adjustedFigure);

                    switch (i)
                    {
                        case 1://rook
                            {
                                var rook = figuresFactory.getRook();
                                adjustedFigure = this.SetFigureInitialValues(rook, i.ToString() + color + rook.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 2://knight
                            {
                                var knight = figuresFactory.getKnight();
                                adjustedFigure = this.SetFigureInitialValues(knight, i.ToString() + color + knight.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 3://bishop
                            {
                                var bishop = figuresFactory.getBishop();
                                adjustedFigure = this.SetFigureInitialValues(bishop, i.ToString() + color + bishop.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 4://queen
                            {
                                var queen = figuresFactory.getQueen();
                                adjustedFigure = this.SetFigureInitialValues(queen, i.ToString() + color + queen.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 5://king
                            {
                                var king = figuresFactory.getKing();
                                adjustedFigure = this.SetFigureInitialValues(king, i.ToString() + color + king.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 6://bishop
                            {
                                var bishop = figuresFactory.getBishop();
                                adjustedFigure = this.SetFigureInitialValues(bishop, i.ToString() + color + bishop.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 7://knight
                            {
                                var knight = figuresFactory.getKnight();
                                adjustedFigure = this.SetFigureInitialValues(knight, i.ToString() + color + knight.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                        case 8://rook
                            {
                                var rook = figuresFactory.getRook();
                                adjustedFigure = this.SetFigureInitialValues(rook, i.ToString() + color + rook.Type, i, otherFiguresRowNumber, color);
                                break;
                            }
                    }

                    boardModel.Figures.Add(adjustedFigure);
                }
            }

            boardModel.Figures = this.CalculatePossibleMoves(boardModel.Figures, Color.White);
            return View(boardModel);
        }

        private List<Figure> CalculatePossibleMoves(List<Figure> figures, Color color)
        {
            List<Coordinates> possibleCoordinates = new List<Coordinates>();
            for (int i = 0; i < figures.Count; i++)
            {
                var currentFigure = figures[i];
                    
                if(currentFigure.Type == Type.pawn)
                {
                    // Empty board and back control handled
                    if (currentFigure.Color == Color.Black)
                    {
                        possibleCoordinates = AllChessCoordinates.FindAll(coordinate => coordinate.Row > currentFigure.Coordinates.Row);
                    } 
                    else
                    {
                        possibleCoordinates = AllChessCoordinates.FindAll(coordinate => coordinate.Row < currentFigure.Coordinates.Row);
                    }

                    possibleCoordinates = possibleCoordinates.FindAll(coordinate => {
                        return !figures.Any(figure => figure.Coordinates.Row == coordinate.Row && figure.Coordinates.Col == coordinate.Col);
                    });

                    currentFigure.PossibleMoves = possibleCoordinates;
                }
            }

            return figures;
        }

        private string ProcessTurn(Move move, Figure figure, int? stepSize, string Direction)
        {

            if (figure.Coordinates.Row == move.Row && figure.Coordinates.Col == move.Col)
            {
                // If the movement is with the same coordinates as the present location of the figure
                // it does not make sense to move
                return false.ToString();
            }

            string removeID = "";
            List<Coordinates> coordinates = new List<Coordinates>();
            var item = new Coordinates();
            int x = figure.Coordinates.Row;
            int y = figure.Coordinates.Col;

            switch (Direction)
            {
                case Utils.Direction.Direct:
                {
                    if (stepSize == null)
                    {
                        // Take step size from the difference in the coordinates of the present coordinates of the figure
                        // and the movement coordinates
                        stepSize = Math.Abs(move.Col - figure.Coordinates.Col) != 0
                            ? Math.Abs(move.Col - figure.Coordinates.Col)
                            : Math.Abs(move.Row - figure.Coordinates.Row);
                    }
                    else
                    {
                        // Take step size from the difference in the present X coordinate of the figure and the movement X coordinate
                        // or present Y coordinate of the figure and the movement Y coordinate
                        // if some of that coordinates differentiate from the specified step size break the operation
                        stepSize = Math.Abs(figure.Coordinates.Row - move.Row) == stepSize || Math.Abs(figure.Coordinates.Row - move.Row) == 1
                            ? Math.Abs(figure.Coordinates.Row - move.Row)
                            : Math.Abs(figure.Coordinates.Col - move.Col) == stepSize || Math.Abs(figure.Coordinates.Col - move.Col) == 1
                                ? Math.Abs(figure.Coordinates.Col - move.Col)
                                : 0;

                        if (stepSize == 0)
                        {
                            break;
                        }
                    }

                    if (move.Row >= figure.Coordinates.Row && x + stepSize <= 8 && figure.Coordinates.Col == move.Col)//x+ 
                    {
                        coordinates.Add(new Coordinates() { Col = y, Row = x + stepSize.Value });
                    }
                    if (move.Row < figure.Coordinates.Row && x - stepSize >= 1 && figure.Coordinates.Col == move.Col)//x-
                    {
                        coordinates.Add(new Coordinates() { Col = y, Row = x - stepSize.Value });
                    }
                    if (move.Col >= figure.Coordinates.Col && y + stepSize <= 8 && figure.Coordinates.Row == move.Row)//y+
                    {
                        coordinates.Add(new Coordinates() { Col = y + stepSize.Value, Row = x });
                    }
                    if (move.Col <= figure.Coordinates.Col && y - stepSize >= 1 && figure.Coordinates.Row == move.Row)//y-
                    {
                        coordinates.Add(new Coordinates() { Col = y - stepSize.Value, Row = x });
                    }

                    break;
                }
                case "cross":
                {
                    if (figure.Coordinates.Row == move.Row || figure.Coordinates.Col == move.Col)
                    {
                        // If the movement of the figure is on the same row or on the same line abort the operation
                        break;
                    }


                    if (stepSize == null)
                    {
                        stepSize = Math.Abs(move.Col - figure.Coordinates.Col);
                    }
                    else
                    {
                        if (Math.Abs(figure.Coordinates.Row - move.Row) != 1 || Math.Abs(figure.Coordinates.Col - move.Col) != 1)
                        {
                            break;
                        }
                    }


                    if (move.Row > figure.Coordinates.Row && move.Col > figure.Coordinates.Col && x + stepSize <= 8 && y + stepSize <= 8)//x+ y+
                    {
                        coordinates.Add(new Coordinates() { Col = y + stepSize.Value, Row = x + stepSize.Value });
                    }
                    if (move.Row < figure.Coordinates.Row && move.Col < figure.Coordinates.Col && x - stepSize.Value >= 1 && y - stepSize.Value >= 1)//x- y-
                    {
                        coordinates.Add(new Coordinates() { Col = y - stepSize.Value, Row = x - stepSize.Value });
                    }
                    if (move.Row > figure.Coordinates.Row && move.Col < figure.Coordinates.Col && x + stepSize.Value <= 8 && y - stepSize.Value >= 1)//x+ y-
                    {
                        coordinates.Add(new Coordinates() { Col = y - stepSize.Value, Row = x + stepSize.Value });
                    }
                    if (move.Row < figure.Coordinates.Row && move.Col > figure.Coordinates.Col && x - stepSize.Value >= 1 && y + stepSize.Value <= 8)//x- y+
                    {
                        coordinates.Add(new Coordinates() { Col = y + stepSize.Value, Row = x - stepSize.Value });
                    }

                    break;
                }
                case "L":
                {
                    if (move.Col == y + 1 && move.Row == x + 2 && x + 2 <= 8 && y + 1 <= 8)//y+1 x+2
                    {
                        coordinates.Add(new Coordinates() { Col = y + 1, Row = x + 2 });
                    }
                    else if (move.Col == y + 1 && move.Row == x - 2 && x - 2 >= 1 && y + 1 <= 8)//y+1 x-2
                    {
                        coordinates.Add(new Coordinates() { Col = y + 1, Row = x - 2 });
                    }
                    else if (move.Col == y + 2 && move.Row == x + 1 && x + 1 <= 8 && y + 2 <= 8)//y+2 x+1
                    {
                        coordinates.Add(new Coordinates() { Col = y + 2, Row = x + 1 });
                    }
                    else if (move.Col == y + 2 && move.Row == x - 1 && x - 1 >= 1 && y + 2 <= 8)//y+2 x-1
                    {
                        coordinates.Add(new Coordinates() { Col = y + 2, Row = x - 1 });
                    }
                    else if (move.Col == y - 1 && move.Row == x + 2 && x + 2 <= 8 && y - 1 >= 1)//y-1 x+2
                    {
                        coordinates.Add(new Coordinates() { Col = y - 1, Row = x + 2 });
                    }
                    else if (move.Col == y - 1 && move.Row == x - 2 && x - 2 >= 1 && y - 1 >= 1)//y-1 x-2
                    {
                        coordinates.Add(new Coordinates() { Col = y - 1, Row = x - 2 });
                    }
                    else if (move.Col == y - 2 && move.Row == x + 1 && x + 1 <= 8 && y - 2 <= 8)//y-2 x+1
                    {
                        coordinates.Add(new Coordinates() { Col = y - 2, Row = x + 1 });
                    }
                    else if (move.Col == y - 2 && move.Row == x - 1 && x - 1 >= 1 && y - 2 <= 8)//y-2 x-1
                    {
                        coordinates.Add(new Coordinates() { Col = y - 2, Row = x - 1 });
                    }
                    break;
                }
                default:
                    break;
            }

            List<Figure> droppedFigures = new List<Figure>();
            foreach (var direct in coordinates)
            {
                foreach (var pc in boardModel.Figures)
                {
                    if (direct.Row == pc.Coordinates.Row && direct.Col == pc.Coordinates.Col)
                    {
                        droppedFigures.Add(pc);
                    }
                }
            }

            if (coordinates.Count > 0)
            {
                if (droppedFigures.Count > 0 && droppedFigures[0].Color != figure.Color)
                {
                    if (Direction == Utils.Direction.Direct && figure.Type == Type.pawn)
                    { }
                    else
                    {
                        boardModel.Figures.RemoveAll(a => a.Id == droppedFigures[0].Id);
                        removeID = droppedFigures[0].Id;
                    }
                }

                // TODO: here the item for the turn should be preserved in the database
                // and should be shown in the box for turns history
                figure.Coordinates = new Coordinates()
                {
                    Row = move.Row,
                    Col = move.Col
                };

                figure.IsFirstMove = true;
            }

            return true + ";" + removeID;
        }

        public Microsoft.AspNetCore.Mvc.JsonResult MoveFigure(Move move)
        {
            var currentFigure = (
                            from figure in boardModel.Figures
                            where figure.Id == move.Id
                            select figure
                         )
                         .FirstOrDefault();

            string Direction = (currentFigure.Coordinates.Row == move.Row || currentFigure.Coordinates.Col == move.Col)
                ? Utils.Direction.Direct
                : Utils.Direction.Cross;

            string result = "";
            switch (currentFigure.Type)
            {
                case Type.pawn:
                    {
                        // Only in the very first turn the pawn can do double turn(two steps instead of one)
                        int stepSize = currentFigure.IsFirstMove
                            ? 1
                            : 2;
                        bool backControl = currentFigure.Color == Color.Black
                            ? (currentFigure.Coordinates.Row - move.Row > 0
                                ? false
                                : true)
                            : (currentFigure.Coordinates.Row - move.Row < 0
                                ? false
                                : true);

                        result = backControl == true
                            ? ProcessTurn(move, currentFigure, stepSize, Direction)
                            : "false;";
                        break;
                    }
                case Type.rook:
                    {
                        result = ProcessTurn(move, currentFigure, null, Utils.Direction.Direct);
                        break;
                    }
                case Type.knight:
                    {
                        result = ProcessTurn(move, currentFigure, null, Utils.Direction.L);
                        break;
                    }
                case Type.bishop:
                    {
                        result = ProcessTurn(move, currentFigure, null, Utils.Direction.Cross);
                        break;
                    }
                case Type.queen:
                    {
                        result = ProcessTurn(move, currentFigure, null, Direction);
                        break;
                    }
                case Type.king:
                    {
                        result = ProcessTurn(move, currentFigure, 1, Direction);
                        break;
                    }
            }

            boardModel.Figures = this.CalculatePossibleMoves(boardModel.Figures, Color.White);
            return Json(result);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Chat()
        {
            return View();
        }

    }
}
