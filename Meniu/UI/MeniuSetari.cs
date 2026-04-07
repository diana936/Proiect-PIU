using NivelStocareDate;
using System;

namespace Meniu.UI
{
    public class MeniuSetari
    {
        private SetariJoc _setari = SetariJoc.Instanta;

        public void Afiseaza()
        {
            bool activ = true;
            while (activ)
            {
                Console.Clear();
                Console.WriteLine("=== SETARI ===\n");
                Console.WriteLine($"1. Rezolutie:  {_setari.RezolutieCurenta}");
                Console.WriteLine($"2. Viteza joc: {_setari.VitezaJoc}");
                Console.WriteLine($"3. Sunet:      {(_setari.SunetActivat ? "Pornit" : "Oprit")}");
                Console.WriteLine("4. Cauta setare dupa cheie");
                Console.WriteLine("5. Modifica setare");
                Console.WriteLine("6. Resetare implicite");
                Console.WriteLine("0. Inapoi");
                Console.Write("\nOptiunea ta: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": SchimbaRezolutie(); break;
                    case "2": SchimbaViteza();    break;
                    case "3": ToggleSunet();      break;
                    case "4": CautaSetare();      break;
                    case "5": ModificaSetare();   break;
                    case "6": ResetareImplicite(); break;
                    case "0": activ = false;       break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private void SchimbaRezolutie()
        {
            Console.Clear();
            Console.WriteLine("Alege rezolutia:\n");
            Console.WriteLine("1. Mica  (80 x 24)");
            Console.WriteLine("2. Medie (120 x 30)");
            Console.WriteLine("3. Mare  (160 x 40)");
            Console.Write("\nOptiunea ta: ");
            switch (Console.ReadLine()?.Trim())
            {
                case "1": _setari.RezolutieCurenta = Rezolutie.Mica;  break;
                case "2": _setari.RezolutieCurenta = Rezolutie.Medie; break;
                case "3": _setari.RezolutieCurenta = Rezolutie.Mare;  break;
                default: Console.WriteLine("Optiune invalida."); break;
            }
            _setari.Salveaza();
            Console.WriteLine("Salvat!");
            Console.ReadKey(true);
        }

        private void SchimbaViteza()
        {
            Console.Write("Viteza noua (1-10): ");
            if (int.TryParse(Console.ReadLine(), out int v) && v >= 1 && v <= 10)
            {
                _setari.VitezaJoc = v;
                _setari.Salveaza();
                Console.WriteLine("Salvat!");
            }
            else Console.WriteLine("Valoare invalida.");
            Console.ReadKey(true);
        }

        private void ToggleSunet()
        {
            _setari.SunetActivat = !_setari.SunetActivat;
            _setari.Salveaza();
            Console.WriteLine($"Sunet: {(_setari.SunetActivat ? "Pornit" : "Oprit")}. Salvat!");
            Console.ReadKey(true);
        }

        // SEARCH – find a raw setting by its key in the file
        private void CautaSetare()
        {
            Console.Write("Introdu cheia (ex: VitezaJoc, SunetActivat, Rezolutie): ");
            string cheie = Console.ReadLine()?.Trim() ?? "";
            string val   = StocareSetari.Instanta.CautaDupaCheie(cheie);
            if (val != null)
                Console.WriteLine($"  {cheie} = {val}");
            else
                Console.WriteLine("  Cheia nu a fost gasita.");
            Console.ReadKey(true);
        }

        // MODIFY – change any setting by typing key + new value
        private void ModificaSetare()
        {
            Console.Write("Cheia de modificat: ");
            string cheie = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Valoarea noua: ");
            string valoare = Console.ReadLine()?.Trim() ?? "";

            bool exista = StocareSetari.Instanta.ModificaSetare(cheie, valoare);
            // Also reload so in-memory state matches the file
            StocareSetari.Instanta.Incarca(_setari);
            Console.WriteLine(exista ? "Setare modificata si salvata!" : "Setare noua adaugata si salvata!");
            Console.ReadKey(true);
        }

        private void ResetareImplicite()
        {
            _setari.ResetareImplicite();
            Console.WriteLine("Setarile au fost resetate la valorile implicite si salvate.");
            Console.ReadKey(true);
        }
    }
}
