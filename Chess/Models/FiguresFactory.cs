namespace Chess.Models
{
    public class FiguresFactory
    {
        public Figure getRook()
        {
            Figure figure = new Figure();
            figure.Type = Type.rook;
            return figure;
        }

        public Figure getKnight()
        {
            Figure figure = new Figure();
            figure.Type = Type.knight;
            return figure;
        }

        public Figure getBishop()
        {
            Figure figure = new Figure();
            figure.Type = Type.bishop;
            return figure;
        }

        public Figure getQueen()
        {
            Figure figure = new Figure();
            figure.Type = Type.queen;
            return figure;
        }

        public Figure getKing()
        {
            Figure figure = new Figure();
            figure.Type = Type.king;
            return figure;
        }

        public Figure getPawn()
        {
            Figure figure = new Figure();
            figure.Type = Type.pawn;
            return figure;
        }
    }
}
