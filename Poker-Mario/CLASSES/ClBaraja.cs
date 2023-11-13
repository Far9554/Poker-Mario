using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario_PokerChulo.CLASSES
{
    public class ClBaraja
    {
        private List<ClCarta> llCartas = new List<ClCarta>();
        private Random rnd = new Random();

        public List<ClCarta> Cartas { get { return llCartas; } set { llCartas = value; } }

        public ClBaraja() { }

        public void GenerarBarajaNueva()
        {
            llCartas.Clear();
            for (int simbol = 0; simbol < 4; simbol++)
            {
                for (int number = 1; number <= 12; number++)
                {
                    llCartas.Add(new ClCarta(number, simbol));
                }
            }
        }

        public void BarajarBaraja()
        {
            int n = Cartas.Count;

            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                ClCarta value = Cartas[k];
                Cartas[k] = Cartas[n];
                Cartas[n] = value;
            }
        }

        public ClCarta Robar()
        {
            ClCarta c = new ClCarta(Cartas[0].Numero, (int)Cartas[0].Simbolo);
            Cartas.Remove(Cartas[0]);

            return c;
        }
    }
}
