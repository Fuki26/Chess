using System.Collections.Generic;

namespace Chess.Models
{
    public class Board
    {
        private Color _turnColor = new Color();

        public Color TurnType 
        {
            get
            {
                return this._turnColor;
            }
            set
            {
                this._turnColor = value;
            }
        
        }


        private List<Figure> _figures = new List<Figure>();
        public List<Figure> Figures
        {
            get
            {
                return this._figures;
            }

            set
            {
                this._figures = value;
            }
        }
    }
}