using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario_PokerChulo.CLASSES
{
    public class Comprobaciones
    {
        private static Comprobaciones instance;
        public static Comprobaciones Comprobacion
        {
            get
            {
                if (instance == null)
                    instance = new Comprobaciones();
                return instance;
            }
        }

        public string[] Combinaciones = new string[] {"Carta alta     ", "Pareja        ", "Doble pareja   ", "Trio           ", "Escalera          ", 
                                                      "Color          ", "Full          ", "Poker          ", "Escalera Color    "};
        public void OrdenarCartas(ClCarta[] cartas)
        {
            ClCarta tmp;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7 - 1; j++)
                {
                    if (cartas[j].Numero > cartas[j + 1].Numero)
                    {
                        tmp = cartas[j];
                        cartas[j] = cartas[j + 1];
                        cartas[j + 1] = tmp;
                    }
                }
            }
        }

        public int DarCartaAlta(ClCarta[] cartas)
        {
            int nAlto = 0;
            for (int s = 0; s < cartas.Length; s++)
            {
                if (cartas[s].Numero == 1)
                    return 100;
                else if (cartas[s].Numero > nAlto)
                    nAlto = cartas[s].Numero;
            }

            return nAlto;
        }

        public bool ComprobarPareja(ClCarta[] cartas)
        {
            for (int s = 1; s < cartas.Length; s++)
            {
                if (cartas[s].Numero == cartas[s - 1].Numero)
                    return true;
            }

            return false;
        }

        public bool ComprobarDoblePareja(ClCarta[] cartas)
        {
            int Pareja1 = 0;

            for (int s = 1; s < cartas.Length; s++)
            {
                if (cartas[s].Numero == cartas[s - 1].Numero)
                    Pareja1= cartas[s].Numero;
            }

            if(Pareja1 != 0 )
            {
                for (int s = 1; s < cartas.Length; s++)
                {
                    if (cartas[s].Numero == cartas[s - 1].Numero && cartas[s].Numero != Pareja1)
                        return true;
                }
            }

            return false;
        }

        public bool ComprobarTrio(ClCarta[] cartas)
        {
            for (int s = 2; s < cartas.Length; s++)
            {
                int num = cartas[s].Numero;
                if (num == cartas[s - 1].Numero && num == cartas[s - 2].Numero)
                    return true;
            }

            return false;
        }

        public bool ComprobarEscaleras(ClCarta[] cartas)
        {
            int count = 0;

            for (int s = 1; s < cartas.Length; s++)
            {
                int num = cartas[s].Numero;
                if (num == cartas[s - 1].Numero + 1)
                    count++;
                else
                    count=0;

                if (count >= 4)
                    return true;
            }

            return false;
        }

        public bool ComprobarColor(ClCarta[] cartas)
        {
            for (int i = 0; i < 4; i++)
            {
                int count = 0;

                for (int s = 0; s < cartas.Length; s++)
                {
                    if ((int)cartas[s].Simbolo == i)
                        count++;
                }

                if (count >= 5)
                    return true;
            }

            return false;
        }

        public bool ComprobarFull(ClCarta[] cartas)
        {
            int Trio = 0;

            for (int s = 2; s < cartas.Length; s++)
            {
                if (cartas[s].Numero == cartas[s - 1].Numero && cartas[s].Numero == cartas[s - 2].Numero)
                    Trio = cartas[s].Numero;
            }

            if (Trio != 0)
            {
                for (int s = 1; s < cartas.Length; s++)
                {
                    if (cartas[s].Numero == cartas[s - 1].Numero && cartas[s].Numero != Trio)
                        return true;
                }
            }

            return false;
        }

        public bool ComprobarPoker(ClCarta[] cartas)
        {
            for (int s = 3; s < cartas.Length; s++)
            {
                int num = cartas[s].Numero;
                if (num == cartas[s - 1].Numero && num == cartas[s - 2].Numero && num == cartas[s - 3].Numero)
                    return true;
            }
            return false;
        }

        public bool ComprobarEscaleraColor(ClCarta[] cartas)
        {
            for (int i = 0; i < 4; i++)
            {
                int count = 0;

                for (int s = 1; s < cartas.Length; s++)
                {
                    if ((int)cartas[s].Simbolo == i && cartas[s].Numero == cartas[s - 1].Numero + 1)
                        count++;
                    else
                        count=0;
                }

                if (count >= 5)
                    return true;
            }

            return false;
        }
    }
}
