using System;
using LibrarieModele;

namespace Meniu.Jocuri.Connect4
{
    public class JocConnect4 : BazaJoc
    {
        public override string Nume => "Connect 4";

        private TablaConnect4 _tabla = null!;
        private JucatorConnect4 _jucator1 = null!;
        private JucatorConnect4 _jucator2 = null!;
        private JucatorConnect4 _jucatorCurent = null!;
        private bool _vsCalculator;
        private Random _random;

        public JocConnect4(bool vsCalculator = false)
        {
            _vsCalculator = vsCalculator;
            _random = new Random();
            _jucator1 = new JucatorConnect4("Jucator 1", valoareJeton: 1, simbolJeton: 'R');
            _jucator2 = vsCalculator
                ? new JucatorConnect4("Calculator", valoareJeton: 2, simbolJeton: 'G')
                : new JucatorConnect4("Jucator 2", valoareJeton: 2, simbolJeton: 'G');
        }

        public override void Incepe()
        {
            base.Incepe();
            _tabla = new TablaConnect4();
            _jucatorCurent = _jucator1;

            while (EsteActiv)
            {
                Randeaza();

                if (_vsCalculator && _jucatorCurent == _jucator2)
                    MutareCalculator();
                else
                    MutareJucator();

                if (_tabla.VerificaCastigator(_jucatorCurent.ValoareJeton))
                {
                    Randeaza();
                    Console.WriteLine($"\n{_jucatorCurent.Nume} a castigat!");
                    Opreste();
                }
                else if (_tabla.EsteTablaPlina())
                {
                    Randeaza();
                    Console.WriteLine("\nRemiza!");
                    Opreste();
                }
                else
                {
                    SchimbaTurul();
                }
            }

            if (_tabla.VerificaCastigator(_jucatorCurent.ValoareJeton))
            {
                Randeaza();
                Console.WriteLine($"\n{_jucatorCurent.Nume} a castigat!");
                if (!_vsCalculator || _jucatorCurent == _jucator1)
                    SalveazaScor(TipJoc.Connect4, StatusJoc.Castigat);
                Opreste();
            }
            else if (_tabla.EsteTablaPlina())
            {
                Randeaza();
                Console.WriteLine("\nRemiza!");
                SalveazaScor(TipJoc.Connect4, StatusJoc.Remiza);
                Opreste();
            }
        }

        private void MutareJucator()
        {
            bool mutareValida = false;
            while (!mutareValida)
            {
                Console.Write($"\n{_jucatorCurent.Nume} ({_jucatorCurent.SimbolJeton}) - alege coloana (0-6): ");
                bool valid = int.TryParse(Console.ReadLine(), out int coloana);

                if (!valid || coloana < 0 || coloana > 6)
                {
                    Console.WriteLine("Coloana invalida, incearca din nou.");
                    continue;
                }

                if (_tabla.EsteColoanaPlina(coloana))
                {
                    Console.WriteLine("Coloana plina, incearca din nou.");
                    continue;
                }

                _tabla.AruncaJeton(coloana, _jucatorCurent.ValoareJeton);
                mutareValida = true;
            }
        }

        private void MutareCalculator()
        {
            Console.WriteLine("\nCalculatorul se gandeste...");
            System.Threading.Thread.Sleep(600);

            int coloana = GasesteColoanaOptima(2)
                       ?? GasesteColoanaOptima(1)
                       ?? GasesteColoanaRandom();

            _tabla.AruncaJeton(coloana, _jucator2.ValoareJeton);
        }

        private int? GasesteColoanaOptima(int valoare)
        {
            for (int c = 0; c < TablaConnect4.ColoaneStandard; c++)
            {
                if (_tabla.EsteColoanaPlina(c)) continue;

                _tabla.AruncaJeton(c, valoare);
                bool castiga = _tabla.VerificaCastigator(valoare);

                // anuleaza mutarea
                for (int r = 0; r < TablaConnect4.RanduriStandard; r++)
                {
                    Pozitie poz = new Pozitie(r, c);
                    if (_tabla.CitesteCelula(poz) == valoare)
                    {
                        _tabla.ScrieCelula(poz, 0);
                        break;
                    }
                }

                if (castiga) return c;
            }
            return null;
        }

        private int GasesteColoanaRandom()
        {
            int coloana;
            do
            {
                coloana = _random.Next(0, TablaConnect4.ColoaneStandard);
            } while (_tabla.EsteColoanaPlina(coloana));
            return coloana;
        }

        private void SchimbaTurul()
        {
            _jucatorCurent = (_jucatorCurent == _jucator1) ? _jucator2 : _jucator1;
        }

        public JucatorConnect4 ObtineJucatorCurent() => _jucatorCurent;

        public override void Randeaza()
        {
            Console.Clear();
            Console.WriteLine("=== CONNECT 4 ===\n");
            Console.WriteLine(" 0 1 2 3 4 5 6");

            for (int r = 0; r < TablaConnect4.RanduriStandard; r++)
            {
                Console.Write("|");
                for (int c = 0; c < TablaConnect4.ColoaneStandard; c++)
                {
                    int val = _tabla.CitesteCelula(new Pozitie(r, c));
                    char simbol = val == 1 ? 'R' : val == 2 ? 'G' : '.';
                    Console.Write($"{simbol}|");
                }
                Console.WriteLine();
            }

            Console.WriteLine("---------------");
            Console.WriteLine($"\nTurul: {_jucatorCurent.Nume} ({_jucatorCurent.SimbolJeton})");
        }
    }
}