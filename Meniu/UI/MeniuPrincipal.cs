using LibrarieModele;
using Meniu.Jocuri.Sarpe;
using Meniu.Jocuri.XSiZero;
using Meniu.Jocuri.Connect4;
using System;

namespace Meniu.UI
{
    public class MeniuPrincipal
    {
        private string[] _optiuni = { "Snake", "X si 0", "Connect 4", "Inregistrari", "Setari" };
        private int _indexSelectat = 0;

        public void Afiseaza()
        {
            bool activ = true;
            while (activ)
            {
                Randeaza();
                GestioneazaInput(ref activ);
            }
        }

        private void Randeaza()
        {
            Console.Clear();
            Console.WriteLine("=== MENIU PRINCIPAL ===\n");
            for (int i = 0; i < _optiuni.Length; i++)
            {
                string prefix = (i == _indexSelectat) ? "> " : "  ";
                Console.WriteLine($"{prefix}{_optiuni[i]}");
            }
            Console.WriteLine("\nFoloseste sagetile si Enter pentru selectie. Esc pentru iesire.");
        }

        private void GestioneazaInput(ref bool activ)
        {
            ConsoleKey tasta = Console.ReadKey(true).Key;
            switch (tasta)
            {
                case ConsoleKey.UpArrow:
                    _indexSelectat = (_indexSelectat - 1 + _optiuni.Length) % _optiuni.Length;
                    break;
                case ConsoleKey.DownArrow:
                    _indexSelectat = (_indexSelectat + 1) % _optiuni.Length;
                    break;
                case ConsoleKey.Enter:
                    LanseazaSelectat();
                    break;
                case ConsoleKey.Escape:
                    activ = false;
                    break;
            }
        }

        private void LanseazaSelectat()
        {
            switch (_indexSelectat)
            {
                case 0:
                    new JocSarpe().Incepe();
                    break;
                case 1:
                    AlegereMod(new JocXSiZero(false), new JocXSiZero(true));
                    break;
                case 2:
                    AlegereMod(new JocConnect4(false), new JocConnect4(true));
                    break;
                case 3:
                    new MeniuInregistrari().Afiseaza();
                    break;
                case 4:
                    new MeniuSetari().Afiseaza();
                    break;
            }
        }

        private void AlegereMod(BazaJoc douaJucatoare, BazaJoc vsCalculator)
        {
            Console.Clear();
            Console.WriteLine("1. 2 Jucatori");
            Console.WriteLine("2. vs Calculator");
            Console.Write("\nOptiunea ta: ");
            string opt = Console.ReadLine();
            if (opt == "1") douaJucatoare.Incepe();
            else if (opt == "2") vsCalculator.Incepe();
        }
    }
}
