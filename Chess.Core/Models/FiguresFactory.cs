namespace Chess.Models
{
    public class FiguresFactory
    {
        public Figure getRook()
        {
            Figure figure = new Figure();
            figure.Type = Type.rook.ToString();
            return figure;
        }

        public Figure getKnight()
        {
            Figure figure = new Figure();
            figure.Type = Type.knight.ToString();
            return figure;
        }

        public Figure getBishop()
        {
            Figure figure = new Figure();
            figure.Type = Type.bishop.ToString();
            return figure;
        }

        public Figure getQueen()
        {
            Figure figure = new Figure();
            figure.Type = Type.queen.ToString();
            return figure;
        }

        public Figure getKing()
        {
            Figure figure = new Figure();
            figure.Type = Type.king.ToString();
            return figure;
        }

        public Figure getPawn()
        {
            Figure figure = new Figure();
            figure.Type = Type.pawn.ToString();
            return figure;
        }
    }
}
