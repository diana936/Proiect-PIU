using System;
using System.Collections.Generic;
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
                Console.WriteLine("4. Cauta dupa nume jucator");
                Console.WriteLine("5. Modifica scor");
                Console.WriteLine("6. Sterge inregistrare");
                Console.WriteLine("0. Inapoi");
                Console.Write("\nOptiunea ta: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": AfiseazaLeaderboard(TipJoc.Snake);    break;
                    case "2": AfiseazaLeaderboard(TipJoc.XSiZero);  break;
                    case "3": AfiseazaLeaderboard(TipJoc.Connect4); break;
                    case "4": CautaDupaNumeJucator();               break;
                    case "5": ModificaScor();                       break;
                    case "6": StergeInregistrare();                 break;
                    case "0": activ = false;                        break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        // DISPLAY leaderboard for one game type
        private void AfiseazaLeaderboard(TipJoc tipJoc)
        {
            Console.Clear();
            Console.WriteLine($"=== LEADERBOARD {tipJoc.ToString().ToUpper()} ===\n");
            Console.WriteLine($"{"Loc",-5} {"Nume",-15} {"Scor",5} {"Status",-12}");
            Console.WriteLine(new string('-', 45));

            var lista = GestiuneInregistrari.Instanta.ObtineLeaderboard(tipJoc);

            if (lista.Count == 0)
                Console.WriteLine("Nu exista inregistrari inca.");
            else
                for (int i = 0; i < lista.Count; i++)
                {
                    string loc = i == 0 ? "I" : i == 1 ? "II" : i == 2 ? "III" : $"#{i + 1}";
                    Console.WriteLine($"{loc,-5} {lista[i]}");
                }

            Console.WriteLine("\nApasa orice tasta pentru a continua...");
            Console.ReadKey(true);
        }

        // SEARCH by player name
        private void CautaDupaNumeJucator()
        {
            Console.Clear();
            Console.Write("Introdu numele (sau o parte din el): ");
            string termen = Console.ReadLine()?.Trim() ?? "";

            List<InregistrareJoc> rezultate =
                GestiuneInregistrari.Instanta.CautaDupaNumeJucator(termen);

            Console.Clear();
            Console.WriteLine($"=== REZULTATE CAUTARE: \"{termen}\" ===\n");

            if (rezultate.Count == 0)
                Console.WriteLine("Nu s-au gasit inregistrari.");
            else
                foreach (InregistrareJoc inreg in rezultate)
                    Console.WriteLine($"  [{inreg.TipulJocului}]  {inreg}");

            Console.WriteLine("\nApasa orice tasta pentru a continua...");
            Console.ReadKey(true);
        }

        // MODIFY – update a player's score for a specific game
        private void ModificaScor()
        {
            Console.Clear();
            Console.WriteLine("=== MODIFICA SCOR ===\n");
            Console.Write("Numele jucatorului (exact): ");
            string nume = Console.ReadLine()?.Trim() ?? "";

            Console.WriteLine("Tipul jocului: 1=Snake  2=XSiZero  3=Connect4");
            Console.Write("Optiunea ta: ");
            TipJoc tipJoc;
            switch (Console.ReadLine()?.Trim())
            {
                case "1": tipJoc = TipJoc.Snake;    break;
                case "2": tipJoc = TipJoc.XSiZero;  break;
                case "3": tipJoc = TipJoc.Connect4; break;
                default:
                    Console.WriteLine("Tip invalid.");
                    Console.ReadKey(true);
                    return;
            }

            Console.Write("Scorul nou: ");
            if (!int.TryParse(Console.ReadLine(), out int scorNou))
            {
                Console.WriteLine("Scor invalid.");
                Console.ReadKey(true);
                return;
            }

            bool ok = GestiuneInregistrari.Instanta.ModificaScor(nume, tipJoc, scorNou);
            Console.WriteLine(ok
                ? "Scor actualizat si salvat!"
                : "Nu s-a gasit nicio inregistrare cu acele date.");
            Console.ReadKey(true);
        }

        // DELETE a record
        private void StergeInregistrare()
        {
            Console.Clear();
            Console.WriteLine("=== STERGE INREGISTRARE ===\n");
            Console.Write("Numele jucatorului (exact): ");
            string nume = Console.ReadLine()?.Trim() ?? "";

            Console.WriteLine("Tipul jocului: 1=Snake  2=XSiZero  3=Connect4");
            Console.Write("Optiunea ta: ");
            TipJoc tipJoc;
            switch (Console.ReadLine()?.Trim())
            {
                case "1": tipJoc = TipJoc.Snake;    break;
                case "2": tipJoc = TipJoc.XSiZero;  break;
                case "3": tipJoc = TipJoc.Connect4; break;
                default:
                    Console.WriteLine("Tip invalid.");
                    Console.ReadKey(true);
                    return;
            }

            bool ok = GestiuneInregistrari.Instanta.Sterge(nume, tipJoc);
            Console.WriteLine(ok
                ? "Inregistrare stearsa si fisier actualizat!"
                : "Nu s-a gasit nicio inregistrare cu acele date.");
            Console.ReadKey(true);
        }
    }
}
