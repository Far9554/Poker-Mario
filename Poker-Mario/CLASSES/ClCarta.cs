using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario_PokerChulo.CLASSES
{
    public class ClCarta
    {
        private Random r = new Random();
        private int numero = 0;
        private Simbol simbolo = 0;
        private string simbolos = "♥♦♠♣";
        private string enumeracion = "A23456789JQK";

        public enum Simbol
        {
            Corazon = 0,
            Diamante = 1,
            Pica = 2,
            Trebol = 3
        }

        public int Numero
        {
            get
            {
                return numero;
            }
            set
            {
                if (value >= 1 && value <= 13)
                    this.numero = value;
            }
        }
        public Simbol Simbolo { get { return simbolo; } set { simbolo = value; } }
        public string Simbolos { get { return simbolos; } }
        public string Enumeracion { get { return enumeracion; } }

        public ClCarta()
        {
            Numero = r.Next(1, 14);
            Simbolo = (Simbol)r.Next(0, 4);
        }

        public ClCarta(int value, int simbolo)
        {
            this.Numero = value;
            this.Simbolo = (Simbol)simbolo;
        }
    }
}
