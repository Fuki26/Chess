using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Move
    {
        public string Id { get; set; }
        public string Target { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
