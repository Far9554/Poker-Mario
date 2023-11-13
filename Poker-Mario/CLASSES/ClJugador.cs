using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario_PokerChulo.CLASSES
{
    public class ClJugador
    {
        private string nombre;
        private List<ClCarta> llCartas = new List<ClCarta>();
        private int money;
        private int dineroApostado;
        public bool abandonado;
        public bool subidoEnRonda;
        public bool allIn = false;
        public bool eliminado;
        public int nivelAgresivo;

        public List<ClCarta> Cartas { get { return llCartas; } set { llCartas = value; } }
        public string Nombre { get { return nombre; } set { nombre = value; } }
        public int Money { get { return money; } set { money = value; } }
        public int DineroApostado { get { return dineroApostado; } set { dineroApostado = value; } }

        public int mejorCombinacion;

        public ClJugador(int nNombre, int nmoney)
        {
            Nombre = "Jugador " + nNombre;
            Money = nmoney;
        }
    }
}
