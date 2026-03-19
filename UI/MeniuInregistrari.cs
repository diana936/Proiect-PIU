using Meniu.Modeluri;
using System;

namespace Meniu.UI
{
    public class MeniuInregistrari
    {
        private GestiuneInregistrari _gestiune;

        public MeniuInregistrari()
        {
            _gestiune = new GestiuneInregistrari();
        }

        public void Afiseaza()
        {
            bool activ = true;
            while (activ)
            {
                Randeaza();
                activ = GestioneazaInput();
            }
        }

        private void Randeaza()
        {
            Console.Clear();
            Console.WriteLine("=== INREGISTRARI ===\n");
            Console.WriteLine("1. Adauga inregistrare");
            Console.WriteLine("2. Afiseaza toate");
            Console.WriteLine("3. Cauta dupa nume jucator");
            Console.WriteLine("4. Cauta dupa tip joc");
            Console.WriteLine("5. Cauta dupa scor minim");
            Console.WriteLine("6. Afiseaza doar victorii");
            Console.WriteLine("0. Inapoi la meniu");
        }

        private bool GestioneazaInput()
        {
            Console.Write("\nOptiunea ta: ");
            string optiune = Console.ReadLine();

            switch (optiune)
            {
                case "1":
                    AdaugaInregistrare();
                    break;
                case "2":
                    AfiseazaToate();
                    break;
                case "3":
                    CautaDupaNumeJucator();
                    break;
                case "4":
                    CautaDupaTipJoc();
                    break;
                case "5":
                    CautaDupaScorMinim();
                    break;
                case "6":
                    AfiseazaVictorii();
                    break;
                case "0":
                    return false;
                default:
                    Console.WriteLine("Optiune invalida.");
                    break;
            }

            Console.WriteLine("\nApasa orice tasta pentru a continua...");
            Console.ReadKey(true);
            return true;
        }

        private void AdaugaInregistrare()
        {
            Console.Clear();
            Console.WriteLine("=== ADAUGA INREGISTRARE ===\n");

            Console.Write("Nume jucator: ");
            string numeJucator = Console.ReadLine();

            Console.WriteLine("Tip joc (1-Snake, 2-X si 0, 3-Connect 4): ");
            string tipJocInput = Console.ReadLine();
            string tipJoc;
            switch (tipJocInput)
            {
                case "1": tipJoc = "Snake"; break;
                case "2": tipJoc = "X si 0"; break;
                case "3": tipJoc = "Connect 4"; break;
                default: tipJoc = "Necunoscut"; break;
            }

            Console.Write("Scor obtinut: ");
            int scor = int.Parse(Console.ReadLine());

            Console.Write("Numar de mutari: ");
            int nrMutari = int.Parse(Console.ReadLine());

            Console.Write("A castigat? (1-Da, 0-Nu): ");
            bool aCastigat = Console.ReadLine() == "1";

            InregistrareJoc inregistrare = new InregistrareJoc(numeJucator, tipJoc, scor, nrMutari, aCastigat);

            if (_gestiune.Adauga(inregistrare))
                Console.WriteLine("\nInregistrare adaugata cu succes!");
            else
                Console.WriteLine("\nEroare: vectorul este plin.");
        }

        private void AfiseazaToate()
        {
            Console.Clear();
            Console.WriteLine("=== TOATE INREGISTRARILE ===\n");
            _gestiune.AfiseazaToate();
        }

        private void CautaDupaNumeJucator()
        {
            Console.Clear();
            Console.WriteLine("=== CAUTA DUPA NUME JUCATOR ===\n");
            Console.Write("Introdu numele: ");
            string nume = Console.ReadLine();

            InregistrareJoc[] rezultate = _gestiune.CautaDupaNumeJucator(nume);
            AfiseazaRezultate(rezultate);
        }

        private void CautaDupaTipJoc()
        {
            Console.Clear();
            Console.WriteLine("=== CAUTA DUPA TIP JOC ===\n");
            Console.WriteLine("Tip joc (Snake / X si 0 / Connect 4): ");
            string tipJoc = Console.ReadLine();

            InregistrareJoc[] rezultate = _gestiune.CautaDupaTipJoc(tipJoc);
            AfiseazaRezultate(rezultate);
        }

        private void CautaDupaScorMinim()
        {
            Console.Clear();
            Console.WriteLine("=== CAUTA DUPA SCOR MINIM ===\n");
            Console.Write("Scor minim: ");
            int scorMinim = int.Parse(Console.ReadLine());

            InregistrareJoc[] rezultate = _gestiune.CautaDupaScorMinim(scorMinim);
            AfiseazaRezultate(rezultate);
        }

        private void AfiseazaVictorii()
        {
            Console.Clear();
            Console.WriteLine("=== DOAR VICTORII ===\n");
            InregistrareJoc[] rezultate = _gestiune.CautaDoarVictorii();
            AfiseazaRezultate(rezultate);
        }

        private void AfiseazaRezultate(InregistrareJoc[] rezultate)
        {
            if (rezultate.Length == 0)
            {
                Console.WriteLine("Nu s-au gasit inregistrari.");
                return;
            }

            Console.WriteLine($"Gasite {rezultate.Length} inregistrari:\n");
            for (int i = 0; i < rezultate.Length; i++)
                Console.WriteLine($"[{i + 1}] {rezultate[i]}");
        }
    }
}
