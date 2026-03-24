using System;
using LibrarieModele;

namespace Meniu.UI
{
    public class MeniuInregistrari
    {
        public void Afiseaza()
        {
            bool activ = true;
            while (activ)
            {
                Console.Clear();
                Console.WriteLine("=== LEADERBOARD ===\n");
                Console.WriteLine("1. Snake");
                Console.WriteLine("2. X si 0");
                Console.WriteLine("3. Connect 4");
                Console.WriteLine("0. Inapoi");
                Console.Write("\nOptiunea ta: ");

                string optiune = Console.ReadLine() ?? "";
                switch (optiune)
                {
                    case "1":
                        AfiseazaLeaderboard(TipJoc.Snake);
                        break;
                    case "2":
                        AfiseazaLeaderboard(TipJoc.XSiZero);
                        break;
                    case "3":
                        AfiseazaLeaderboard(TipJoc.Connect4);
                        break;
                    case "0":
                        activ = false;
                        break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }
            }
        }

        private void AfiseazaLeaderboard(TipJoc tipJoc)
        {
            Console.Clear();
            Console.WriteLine($"=== LEADERBOARD {tipJoc.ToString().ToUpper()} ===\n");
            Console.WriteLine($"{"Loc",-5} {"Nume",-15} {"Scor",5} {"Status",-12}");
            Console.WriteLine(new string('-', 45));

            var lista = GestiuneInregistrari.Instanta.ObtineLeaderboard(tipJoc);

            if (lista.Count == 0)
            {
                Console.WriteLine("Nu exista inregistrari inca.");
            }
            else
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    string loc = i == 0 ? "I" : i == 1 ? "II" : i == 2 ? "III" : $"#{i + 1}";
                    Console.WriteLine($"{loc,-5} {lista[i]}");
                }
            }

            Console.WriteLine("\nApasa orice tasta pentru a continua...");
            Console.ReadKey(true);
        }
    }
}
