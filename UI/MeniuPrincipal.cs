using Meniu.Jocuri.Sarpe;
using Meniu.Jocuri.XSiZero;
using Meniu.Jocuri.Connect4;
using System;

namespace Meniu.UI
{
    public class MeniuPrincipal
    {
        private string[] _optiuni = { "Snake", "X si 0", "Connect 4", "Setari" };
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
                    new JocXSiZero().Incepe();
                    break;
                case 2:
                    new JocConnect4().Incepe();
                    break;
                case 3:
                    new MeniuSetari().Afiseaza();
                    break;
            }
        }
    }
}
