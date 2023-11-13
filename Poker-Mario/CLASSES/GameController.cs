using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario_PokerChulo.CLASSES
{
    public class GameController
    {
        private int dineroEnJuego = 0;
        private int dineroParaContinuar = 0;

        public int s1;
        public int s2;
        public int s3;
        public int s4;

        public int DineroEnJuego { get { return dineroEnJuego; } set {  dineroEnJuego = value; } }
        public int DineroParaContinuar { get { return dineroParaContinuar; } set { dineroParaContinuar = value; } }

        public void SubirApuestaTecla(ClJugador jugador)
        {
            int maxSubida= MaximaSubida(jugador);            

            Console.SetCursorPosition(10, 24);
            Console.Write(s1 + "$[1]    ");
            if (maxSubida <= 2)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(s2 + "$[2]    ");
            if (maxSubida <= 3)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(s3 + "$[3]    ");
            if (maxSubida <= 4)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(s4 + "$[4]");

            while(true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        SubirApuesta(jugador, 5);
                        return;
                    case ConsoleKey.D2:
                        if (maxSubida >= 2)
                        {
                            SubirApuesta(jugador, 10);
                            return;
                        }
                        break;
                    case ConsoleKey.D3:
                        if (maxSubida >= 3)
                        {
                            SubirApuesta(jugador, 15);
                            return;
                        }
                        break;
                    case ConsoleKey.D4:
                        if (maxSubida >= 4)
                        {
                            SubirApuesta(jugador, 20);
                            return;
                        }
                    break;
                }
            }
        }

        public void UpdateSubidas()
        {
            s1 = 5 + dineroParaContinuar;
            s2 = 10 + dineroParaContinuar;
            s3 = 15 + dineroParaContinuar;
            s4 = 20 + dineroParaContinuar;
        }

        public int MaximaSubida(ClJugador jugador)
        {
            UpdateSubidas();
            if (jugador.Money - s1 < 0)
                return 0;
            else if (jugador.Money - s2 < 0)
                return 1;
            else if (jugador.Money - s3 < 0)
                return 2;
            else if (jugador.Money - s4 < 0)
                return 3;
            else
                return 4;
        }

        public void SubirApuesta(ClJugador jugador, int amount)
        {
            amount += dineroParaContinuar;
            jugador.subidoEnRonda = true;

            jugador.DineroApostado += amount;
            jugador.Money -= amount;
            dineroEnJuego += amount;
            dineroParaContinuar = jugador.DineroApostado;
        }

        public void IgualarApuesta(ClJugador jugador)
        {
            int amount = dineroParaContinuar - jugador.DineroApostado;

            jugador.DineroApostado += amount;
            jugador.Money -= amount;
            dineroEnJuego += amount;
        }

        public void TodoAUNO(ClJugador jugador)
        {
            jugador.DineroApostado = jugador.Money;
            dineroEnJuego += jugador.Money;
            jugador.Money = 0;
            jugador.allIn = true;
        }

        public void Abandonar(ClJugador jugador)
        {
            jugador.abandonado = true;
        }

        public void MostrarPantallaInicio()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("                    _____   _____   __   ___  _____   _______       \t __   __    _____    _______   --   _____  ");
            Console.WriteLine("                   |  _  | |  _  | |  | /  / |   __| |  __   \\     \t|  \\_/  |  /     \\  |       \\  __  /  _  \\");
            Console.WriteLine("                   | |_| | | | | | |  |/  /  |  |_   | |__| _/      \t|       | /       \\ |      _/ |  | | | | |");
            Console.WriteLine("                   |  ___| | | | | |     |   |   _|  |     |        \t|   _   | |   _   | |     |   |  | | | | |");
            Console.WriteLine("                   | |     | |_| | |  |\\  \\  |  |__  |  |\\  \\   \t|  | |  | |  | |  | |  |\\  \\  |  | | |_| |");
            Console.WriteLine("                   |_|     |_____| |__| \\__\\ |_____| |__| \\__\\  \t|__| |__| |__| |__| |__| \\__\\ |__| \\_____/");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------------------PRESS ANY KEY TO START------------------------------------------------------------");
        }
    }
}
