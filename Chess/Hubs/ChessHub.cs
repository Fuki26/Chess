using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Chess.Models;



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

        public static Board boardModel = new Board();

        public async Task SendMessage(Move? move)
        {
            if (move == null)
            {
                this.SetInitialBoardModel();
            }
            else
            {
                if (move != null)
                {
                    var movedFigure = boardModel.Figures.FirstOrDefault(f => f.Id == move.FigureId);
                    if (movedFigure == null)
                    {
                        throw new System.Exception("There is no figure with id " + move.FigureId + " in the board model.");
                    }

                    movedFigure.Coordinates.Row = move.TargetRow;
                    movedFigure.Coordinates.Col = move.TargetCol;
                }
            }

            await Clients.All.SendAsync("ReceiveMessage", boardModel);
        }

        private void SetInitialBoardModel()
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
        }
    }
}
