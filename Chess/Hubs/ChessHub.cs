using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Chess.Models;
using Chess.Core;

namespace Chess.Hubs
{
    public class ChessHub : Hub
    {
        private Figure SetFigureInitialValues(Figure figure, string id, int column, int row, Color color)
        {
            figure.Id = id;
            figure.Coordinates = new Coordinates()
            {
                Row = row,
                Col = column
            };
            figure.Color = color.ToString();
            return figure;
        }

        private static Board boardModel = new Board();

        public async Task SendMessage(Move? move)
        {
            if (move == null)
            {
                this.SetInitialBoardModel();
            }
            else
            {
                this.Move(move);
            }

            await Clients.All.SendAsync("ReceiveMessage", boardModel);
        }

        private void SetInitialBoardModel()
        {
            boardModel.Figures.Clear();


            //#region Fill all figures in the board model

            int otherFiguresRowNumber = 1;
            int pawnsRowNumber = 2;
            Color color = Color.Black;

            for (int rowNumber = 1; rowNumber <= 2; rowNumber++)
            {
                if (rowNumber == 2)
                {
                    pawnsRowNumber = 7;
                    otherFiguresRowNumber = 8;
                    color = Color.White;
                }

                for (int columnNumber = 1; columnNumber <= 8; columnNumber++)
                {
                    FiguresFactory figuresFactory = new FiguresFactory();
                    var pawn = figuresFactory.getPawn();
                    var adjustedFigure = this.SetFigureInitialValues(pawn, columnNumber.ToString() + color + pawn.Type, 
                                columnNumber, pawnsRowNumber, color);
                    boardModel.Figures.Add(adjustedFigure);

                    Figure specialFigure = null;
                    switch (columnNumber)
                    {
                        case 1:
                            specialFigure = figuresFactory.getRook();
                            break;
                        case 2:
                            specialFigure = figuresFactory.getKnight();
                            break;
                        case 3:
                            specialFigure = figuresFactory.getBishop();
                            break;
                        case 4:
                            specialFigure = figuresFactory.getQueen();
                            break;
                        case 5:
                            specialFigure = figuresFactory.getKing();
                            break;
                        case 6:
                            specialFigure = figuresFactory.getBishop();
                            break;
                        case 7:
                            specialFigure = figuresFactory.getKnight(); 
                            break;
                        case 8:
                            specialFigure = figuresFactory.getRook();
                            break;
                    }

                    adjustedFigure = this.SetFigureInitialValues(specialFigure, columnNumber.ToString() + color + specialFigure.Type,
                                    columnNumber, otherFiguresRowNumber, color);

                    boardModel.Figures.Add(adjustedFigure);
                }
            }

            //#endregion

            //#region Calculate possible moves on all figures in the board

            for (int i = 0; i < boardModel.Figures.Count; i++)
            {
                switch(boardModel.Figures[i].Type)
                {
                    case "pawn":
                        boardModel.Figures[i] = Pawn.CalculatePossibleMoves(boardModel, boardModel.Figures[i]);
                        break;
                }
            }

            //#endregion
        }

        private void Move(Move move)
        {
            if (move != null)
            {
                var currentFigure = boardModel.Figures.FirstOrDefault(f => f.Id == move.FigureId);
                if (currentFigure == null)
                {
                    throw new System.Exception("There is no figure with id " + move.FigureId + " in the board model.");
                }

                switch (currentFigure.Type)
                {
                    case "pawn":

                        currentFigure = Pawn.Move(currentFigure, move.TargetRow, move.TargetCol);
                        currentFigure = Pawn.CalculatePossibleMoves(boardModel, currentFigure);
                        break;
                }
            }
        }
    }
}
