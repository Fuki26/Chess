using System.Collections.Generic;

namespace Chess.Models
{
    public class Figure
    {
        public Type Type { get; set; }
        public Color Color { get; set; }
        public Coordinates Coordinates { get; set; }
        public List<Coordinates> PossibleMoves { get; set; }
        public List<Coordinates> PossibleAttackMoves { get; set; }
        public string Id { get; set; }
        public bool IsFirstMove { get; set; }
    }
}
