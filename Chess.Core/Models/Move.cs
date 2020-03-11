namespace Chess.Models
{
    public class Move
    {
        public string FigureId { get; set; }
        public string TargetId { get; set; }
        public int TargetRow { get; set; }
        public int TargetCol { get; set; }
    }
}
