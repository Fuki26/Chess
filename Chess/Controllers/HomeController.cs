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

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View();
        }

        public Microsoft.AspNetCore.Mvc.JsonResult MoveFigure(Move move)
        {
            var currentFigure = boardModel.Figures.FirstOrDefault((figure) =>
            {
                return figure.Id == move.TargetId;
            });

            if (currentFigure == null)
            {
                throw new Exception("Figure is not registered in the board model.");
            }

            if(currentFigure.Coordinates.Row == move.TargetRow
                && currentFigure.Coordinates.Col == move.TargetCol)
            {
                return Json(boardModel);
            }

            string moveDirection = string.Empty;

            // TODO: Here should be presented the check for knight direction
            if(currentFigure.Coordinates.Row == move.TargetRow
                || currentFigure.Coordinates.Col == move.TargetCol)
            {
                moveDirection = Utils.MoveDirection.Direct;
            }
            else
            {
                moveDirection = Utils.MoveDirection.Cross;
            }

            switch (currentFigure.Type)
            {
                case "pawn":
                    {
                        bool backControl = false;

                        if(currentFigure.Color == Color.Black.ToString())
                        {
                            if(currentFigure.Coordinates.Row > move.TargetRow)
                            {
                                backControl = true;
                            }
                        }
                        else
                        {
                            if (currentFigure.Coordinates.Row < move.TargetRow)
                            {
                                backControl = true;
                            }
                        } 

                        if (backControl)
                        {
                            // ProcessPawnTurn(currentFigure, move);
                        }
                        break;
                    }
                case "rook":
                    {
                        break;
                    }
                case "knight":
                    {
                        break;
                    }
                case "bishop":
                    {
                        break;
                    }
                case "queen":
                    {
                        break;
                    }
                case "king":
                    {
                        break;
                    }
            }

            return Json(boardModel);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Chat()
        {
            return View();
        }

    }
}
