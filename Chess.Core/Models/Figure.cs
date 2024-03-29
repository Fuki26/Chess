﻿using System.Collections.Generic;

namespace Chess.Models
{
    public class Figure
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public Coordinates Coordinates { get; set; }
        public List<Coordinates> PossibleMoves { get; set; }
        public List<Coordinates> PossibleAttackMoves { get; set; }
        
        public bool IsFirstMove { get; set; }

        public bool IsAlive { get; set; }
    }
}
