using Mario_PokerChulo.CLASSES;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mario_PokerChulo
{
    internal class Program
    {
        static int idPlayer = 3;
        static int QttJugadores = 6;
        static int StartMoney = 100;

        static List<ClJugador> jugadores = new List<ClJugador>();
        static List<ClCarta> mesero = new List<ClCarta>();
        static ClBaraja baraja;
        static ClInterfaz Interfaz = new ClInterfaz();
        static GameController controller = new GameController();

        static void Main(string[] args)
        {
            StartWindow();

            controller.MostrarPantallaInicio();
            Console.ReadLine();

            IniciarJuego();


            while (true)
            {
                IniciarMesa();
                int ronda = 0;

                //---Ronda---(Hasta que quede 1)---///
                while (true)
                {
                    //---TURNO---(3 veces)---///
                    while (true)
                    {
                        Console.Clear();

                        DibujarCartasMesero();
                        DibujarCartasJugadores(true);

                        if (VerSiQuedaUnJugador())
                            break;

                        for (int i = 0; i < QttJugadores; i++)
                        {
                            if (!jugadores[i].abandonado && !jugadores[i].eliminado && !jugadores[i].allIn)
                            {

                                Thread.Sleep(200);
                                if (idPlayer == i)
                                {
                                    DibujarInteraccionesJugador(jugadores[i]);
                                    DetectarInterracionJugador(jugadores[i]);
                                }
                                else
                                    TurnoIA(jugadores[i]);

                                DibujarCartasJugadores(true);
                                Thread.Sleep(200);
                            }
                        }

                        bool listoContinuar = true;
                        for (int i = 0; i < jugadores.Count; i++)
                            if (jugadores[i].DineroApostado != controller.DineroParaContinuar && !jugadores[i].abandonado && !jugadores[i].eliminado && !jugadores[i].allIn)
                                listoContinuar = false;

                        if (listoContinuar)
                            break;
                    }

                    for (int i = 0; i < QttJugadores; i++)
                    {
                        jugadores[i].DineroApostado = 0;
                        jugadores[i].subidoEnRonda = false;
                    }

                    controller.DineroParaContinuar = 0;

                    ronda++;
                    if (ronda == 3)
                        break;

                    mesero.Add(baraja.Robar());
                }

                Console.Clear();
                DibujarCartasMesero();
                DibujarCartasJugadores(false);
                MostrarCombinaciones();

                BuscarGanadorRonda();

                Console.ReadLine();
                //Thread.Sleep(5000);
                EliminarJugadores();

                if (VerSiQuedaUnJugadorVivo())
                    break;
            }


            Console.ReadLine();
        }

        static void StartWindow()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = 150;
            Console.WindowHeight = 30;
            Console.SetBufferSize(150, 30);
            Console.SetWindowPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();
        }
        static void IniciarJuego()
        {
            baraja = new ClBaraja();

            for (int i = 0; i < QttJugadores; i++)
                jugadores.Add(new ClJugador(i + 1, StartMoney));
        }
        static void IniciarMesa()
        {
            baraja.GenerarBarajaNueva();
            baraja.BarajarBaraja();

            controller.DineroEnJuego = 0;

            //LimpiarListas
            for (int i = 0; i < QttJugadores; i++)
            {
                jugadores[i].Cartas.Clear();
                jugadores[i].abandonado = false;
                jugadores[i].allIn = false;
            }

            mesero.Clear();

            //Dar 2 cartas por jugador
            for (int i = 0; i < QttJugadores; i++)
            {
                if (!jugadores[i].eliminado)
                {
                    jugadores[i].Cartas.Add(baraja.Robar());
                    jugadores[i].Cartas.Add(baraja.Robar());
                }
            }

            // Dar 3 cartas mesero
            for (int i = 0; i < 3; i++)
                mesero.Add(baraja.Robar());
        }

        static void DibujarCartasMesero()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < mesero.Count; i++)
            {
                int startX = 14 * i + 10;
                int startY = 1;

                ClCarta carta = new ClCarta(mesero[i].Numero, (int)mesero[i].Simbolo);
                int c = 0;

                if (carta.Simbolo == ClCarta.Simbol.Corazon || carta.Simbolo == ClCarta.Simbol.Diamante)
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                else
                    Console.ForegroundColor = ConsoleColor.Black;

                for (int y = startY; y <= startY + 8; y++)
                {
                    for (int x = startX; x <= startX + 10; x++)
                    {
                        Console.SetCursorPosition(x, y);
                        if (Interfaz.Big[c] == '#')
                            Console.Write(carta.Enumeracion[carta.Numero - 1]);
                        else if (Interfaz.Big[c] == '@')
                            Console.Write(carta.Simbolos[(int)carta.Simbolo]);
                        else
                            Console.Write(Interfaz.Big[c]);

                        c++;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("Dinero en juego: " + controller.DineroEnJuego + "$");
        }
        static void DibujarCartasJugadores(bool hideIA)
        {
            for (int i = 0; i < QttJugadores; i++)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(i * 20 + 10, 12);

                if (jugadores[i].abandonado)
                    Console.WriteLine("Abandonado");
                else if (jugadores[i].allIn)
                    Console.WriteLine("Todo a uno");
                else if (jugadores[i].DineroApostado > 0)
                    Console.WriteLine(jugadores[i].DineroApostado + "$");

                if (jugadores[i].abandonado || jugadores[i].eliminado)
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                if (!jugadores[i].eliminado)
                    DibujarCartas(i, hideIA);

                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(i * 20 + 10, 18);
                Console.Write(jugadores[i].Nombre);
                Console.SetCursorPosition(i * 20 + 10, 19);

                if (jugadores[i].eliminado)
                    Console.WriteLine("Eliminado");
                else
                    Console.Write(jugadores[i].Money + "$");
            }
        }

        static void DibujarCartas(int id, bool hideIA)
        {
            for (int j = 0; j < 2; j++)
            {
                int startX = 8 * j + (id * 20) + 10;
                int startY = 13;
                string dibujoCarta;
                int c = 0;

                ClCarta carta = new ClCarta(jugadores[id].Cartas[j].Numero, (int)jugadores[id].Cartas[j].Simbolo);
                if (id != idPlayer && hideIA)
                    dibujoCarta = Interfaz.SmallHide;
                else
                {
                    dibujoCarta = Interfaz.Small;

                    if (carta.Simbolo == ClCarta.Simbol.Corazon || carta.Simbolo == ClCarta.Simbol.Diamante)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    else
                        Console.ForegroundColor = ConsoleColor.Black;
                }


                for (int y = startY; y <= startY + 4; y++)
                {
                    for (int x = startX; x <= startX + 6; x++)
                    {
                        Console.SetCursorPosition(x, y);
                        if (dibujoCarta[c] == '#')
                            Console.Write(carta.Enumeracion[carta.Numero - 1]);
                        else if (dibujoCarta[c] == '@')
                            Console.Write(carta.Simbolos[(int)carta.Simbolo]);
                        else
                            Console.Write(dibujoCarta[c]);

                        c++;
                    }
                }
            }
        }

        static void DibujarInteraccionesJugador(ClJugador jugador)
        {
            Console.SetCursorPosition(10, 22);

            //-- SEGUIR --//
            if (jugador.DineroApostado < controller.DineroParaContinuar)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Black;

            Console.Write("Seguir [1]\t");

            //-- SUBIR --//
            if (jugador.Money == 0 || controller.MaximaSubida(jugador) == 0)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Black;

            Console.Write("Subir apuesta [2]\t");

            //-- IGUALAR --//
            if (jugador.DineroApostado >= controller.DineroParaContinuar || jugador.Money < controller.DineroParaContinuar - jugador.DineroApostado)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Black;

            Console.Write("Igualar apuesta [3]\t");

            //-- IGUALAR --//
            if (jugador.allIn)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Black;

            Console.Write("Todo a uno [4]\t");

            //-- ABANDONAR --//
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Abandonar [5]");
        }
        static void DetectarInterracionJugador(ClJugador jugador)
        {
            while (true)
            {
                Console.SetCursorPosition(0, 26);
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    //SEGUIR
                    case ConsoleKey.D1:
                        if (jugador.DineroApostado >= controller.DineroParaContinuar)
                            return;
                        break;
                    //SUBIR APUESTA
                    case ConsoleKey.D2:
                        if (jugador.Money != 0 && controller.MaximaSubida(jugador) > 0)
                        {
                            controller.SubirApuestaTecla(jugador);
                            return;
                        }
                        break;
                    //IGUALAR APUESTA
                    case ConsoleKey.D3:
                        if (jugador.DineroApostado < controller.DineroParaContinuar && jugador.Money > controller.DineroParaContinuar - jugador.DineroApostado)
                        {
                            controller.IgualarApuesta(jugador);
                            return;
                        }
                        break;
                    //TODO A UNO
                    case ConsoleKey.D4:
                        controller.TodoAUNO(jugador);
                        return;
                    //ABANDONAR
                    case ConsoleKey.D5:
                        controller.Abandonar(jugador);
                        return;
                }
            }
        }

        static void TurnoIA(ClJugador jugador)
        {
            if (jugador.abandonado || jugador.eliminado)
                return;

            Random r = new Random();
            if (jugador.DineroApostado < controller.DineroParaContinuar)
            {
                if (controller.DineroParaContinuar - jugador.DineroApostado > jugador.Money)
                {
                    if (r.Next(0, 3) == 0)
                        controller.TodoAUNO(jugador);
                    else
                        controller.Abandonar(jugador);
                }
                else if (r.Next(0, 10) == 0)
                    controller.Abandonar(jugador);
                else if (r.Next(0, 5) == 0 && !jugador.subidoEnRonda && jugador.Money > 0)
                    controller.SubirApuesta(jugador, r.Next(1, 4) * 5);
                else
                    controller.IgualarApuesta(jugador);
            }
            else if (r.Next(0, 3) == 0 && !jugador.subidoEnRonda && jugador.Money > 0 && controller.MaximaSubida(jugador) > 0)
                controller.SubirApuesta(jugador, r.Next(1, controller.MaximaSubida(jugador)) * 5);

        }

        static int ConvinacionActual(ClCarta[] cartas)
        {
            for (int j = 9; j >= 0; j--)
                if (Comprobacion(j, cartas))
                    return j;

            return 0;
        }

        static void MostrarCombinaciones()
        {
            for (int i = 0; i < QttJugadores; i++)
            {
                if (!jugadores[i].abandonado && !jugadores[i].eliminado)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(i * 20 + 10, 12);

                    ClCarta[] cartasTotales = mesero.Concat(jugadores[i].Cartas).ToArray();
                    Comprobaciones.Comprobacion.OrdenarCartas(cartasTotales);

                    bool ok = false;
                    for (int j = 9; j >= 0; j--)
                    {
                        if (Comprobacion(j, cartasTotales))
                        {
                            Console.Write(Comprobaciones.Comprobacion.Combinaciones[j]);
                            jugadores[i].mejorCombinacion = j;
                            ok = true;
                            break;
                        }
                    }

                    if (!ok)
                    {
                        Console.Write(Comprobaciones.Comprobacion.Combinaciones[0]);
                        jugadores[i].mejorCombinacion = 0;
                    }
                }
            }
        }

        static bool Comprobacion(int i, ClCarta[] cartas)
        {
            switch (i)
            {
                //Pareja
                case 1:
                    return Comprobaciones.Comprobacion.ComprobarPareja(cartas);
                //Doble Pareja
                case 2:
                    return Comprobaciones.Comprobacion.ComprobarDoblePareja(cartas);
                //Trio
                case 3:
                    return Comprobaciones.Comprobacion.ComprobarTrio(cartas);
                //Escalera
                case 4:
                    return Comprobaciones.Comprobacion.ComprobarEscaleras(cartas);
                //Color
                case 5:
                    return Comprobaciones.Comprobacion.ComprobarColor(cartas);
                //Full
                case 6:
                    return Comprobaciones.Comprobacion.ComprobarFull(cartas);
                //Poker
                case 7:
                    return Comprobaciones.Comprobacion.ComprobarPoker(cartas);
                //Escalera de Color
                case 8:
                    return Comprobaciones.Comprobacion.ComprobarEscaleraColor(cartas);
            }

            return false;
        }

        static void EliminarJugadores()
        {
            for (int i = 0; i < QttJugadores; i++)
            {
                if (jugadores[i].Money <= 0)
                    jugadores[i].eliminado = true;
            }
        }

        static void BuscarGanadorRonda()
        {
            List<ClJugador> Ganadores = new List<ClJugador>();

            //No incluimos a los que han abandonado y han sido eliminados
            for (int i = 0; i < jugadores.Count; i++)
                if (!jugadores[i].abandonado && !jugadores[i].eliminado)
                    Ganadores.Add(jugadores[i]);

            //Eliminamos los que tienen combinaciones bajas
            for (int i = 0; i < jugadores.Count; i++)
                if (jugadores[i].mejorCombinacion < Ganadores.Max(x => x.mejorCombinacion))
                    Ganadores.Remove(jugadores[i]);

            ClJugador Ganador;
            Ganador = Ganadores[0];

            if (Ganadores.Count > 1)
            {
                for (int i = 1; i < Ganadores.Count; i++)
                {
                    if (Comprobaciones.Comprobacion.DarCartaAlta(Ganadores[i].Cartas.ToArray()) >
                        Comprobaciones.Comprobacion.DarCartaAlta(Ganador.Cartas.ToArray()))
                        Ganador = Ganadores[i];
                }
            }

            Console.SetCursorPosition(0, 24);
            Console.WriteLine("         ------------------------------------------------");
            Console.WriteLine("                 HA GANADO EL " + Ganador.Nombre);
            Console.WriteLine("         ------------------------------------------------");

            Ganador.Money += controller.DineroEnJuego;
        }

        static bool VerSiQuedaUnJugador()
        {
            int QttJugando = 0;

            for (int i = 0; i < QttJugadores; i++)
                if (!jugadores[i].abandonado && !jugadores[i].eliminado)
                    QttJugando++;

            return QttJugando == 1;
        }

        static bool VerSiQuedaUnJugadorVivo()
        {
            int QttVivos = 0;

            for (int i = 0; i < QttJugadores; i++)
                if (!jugadores[i].eliminado)
                    QttVivos++;

            return QttVivos == 1;
        }
    }
}
