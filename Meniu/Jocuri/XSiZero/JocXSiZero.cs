using System;
using LibrarieModele;

namespace Meniu.Jocuri.XSiZero
{
    public class JocXSiZero : BazaJoc
    {
        public override string Nume => "X si 0";

        private TablaXSiZero _tabla;
        private JucatorXSiZero _jucatorX;
        private JucatorXSiZero _jucatorO;
        private JucatorXSiZero _jucatorCurent;
        private bool _vsCalculator;
        private Random _random;

        public JocXSiZero(bool vsCalculator = false)
        {
            _vsCalculator = vsCalculator;
            _random = new Random();
            _jucatorX = new JucatorXSiZero("Jucator 1", StareCelula.X);
            _jucatorO = vsCalculator
                ? new JucatorXSiZero("Calculator", StareCelula.O)
                : new JucatorXSiZero("Jucator 2", StareCelula.O);
        }

        public override void Incepe()
        {
            base.Incepe();
            _tabla = new TablaXSiZero();
            _jucatorCurent = _jucatorX;

            while (EsteActiv)
            {
                Randeaza();

                if (_vsCalculator && _jucatorCurent == _jucatorO)
                    MutareCalculator();
                else
                    MutareJucator();

                if (_tabla.VerificaCastigator(_jucatorCurent.Simbol))
                {
                    Randeaza();
                    Console.WriteLine($"\n{_jucatorCurent.Nume} a castigat!");
                    Opreste();
                }
                else if (_tabla.EsteCompleta())
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

            if (_tabla.VerificaCastigator(_jucatorCurent.Simbol))
            {
                Randeaza();
                Console.WriteLine($"\n{_jucatorCurent.Nume} a castigat!");
                if (!_vsCalculator || _jucatorCurent == _jucatorX)
                    SalveazaScor(TipJoc.XSiZero, StatusJoc.Castigat);
                Opreste();
            }
            else if (_tabla.EsteCompleta())
            {
                Randeaza();
                Console.WriteLine("\nRemiza!");
                SalveazaScor(TipJoc.XSiZero, StatusJoc.Remiza);
                Opreste();
            }
        }

        private void MutareJucator()
        {
            bool mutareValida = false;
            while (!mutareValida)
            {
                Console.Write($"\n{_jucatorCurent.Nume} ({_jucatorCurent.Simbol}) - rand (0-2): ");
                bool randValid = int.TryParse(Console.ReadLine(), out int rand);
                Console.Write($"{_jucatorCurent.Nume} ({_jucatorCurent.Simbol}) - coloana (0-2): ");
                bool colValid = int.TryParse(Console.ReadLine(), out int col);

                if (!randValid || !colValid || rand < 0 || rand > 2 || col < 0 || col > 2)
                {
                    Console.WriteLine("Pozitie invalida, incearca din nou.");
                    continue;
                }

                Pozitie poz = new Pozitie(rand, col);
                mutareValida = _tabla.PlaseazaSimbol(poz, _jucatorCurent.Simbol);
                if (!mutareValida)
                    Console.WriteLine("Celula ocupata, incearca din nou.");
            }
        }

        private void MutareCalculator()
        {
            Console.WriteLine("\nCalculatorul se gandeste...");
            System.Threading.Thread.Sleep(600);

            Pozitie mutare = GasesteMutareCastigatoare(StareCelula.O)
                          ?? GasesteMutareCastigatoare(StareCelula.X)
                          ?? GasesteMutareRandom();

            _tabla.PlaseazaSimbol(mutare, StareCelula.O);
        }

        private Pozitie GasesteMutareCastigatoare(StareCelula simbol)
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Pozitie poz = new Pozitie(r, c);
                    if (_tabla.ObtineStareCelula(poz) == StareCelula.Goala)
                    {
                        _tabla.PlaseazaSimbol(poz, simbol);
                        bool castiga = _tabla.VerificaCastigator(simbol);
                        _tabla.ScrieCelula(poz, (int)StareCelula.Goala);
                        if (castiga) return poz;
                    }
                }
            }
            return null;
        }

        private Pozitie GasesteMutareRandom()
        {
            Pozitie poz;
            do
            {
                poz = new Pozitie(_random.Next(0, 3), _random.Next(0, 3));
            } while (_tabla.ObtineStareCelula(poz) != StareCelula.Goala);
            return poz;
        }

        private void SchimbaTurul()
        {
            _jucatorCurent = (_jucatorCurent == _jucatorX) ? _jucatorO : _jucatorX;
        }

        public JucatorXSiZero ObtineJucatorCurent() => _jucatorCurent;

        public override void Randeaza()
        {
            Console.Clear();
            Console.WriteLine("=== X SI 0 ===\n");
            Console.WriteLine("  0 1 2");
            for (int r = 0; r < 3; r++)
            {
                Console.Write($"{r} ");
                for (int c = 0; c < 3; c++)
                {
                    StareCelula stare = _tabla.ObtineStareCelula(new Pozitie(r, c));
                    char simbol = stare == StareCelula.X ? 'X' :
                                  stare == StareCelula.O ? 'O' : '.';
                    Console.Write(simbol);
                    if (c < 2) Console.Write("|");
                }
                Console.WriteLine();
                if (r < 2) Console.WriteLine("  -----");
            }
            Console.WriteLine($"\nTurul: {_jucatorCurent.Nume} ({_jucatorCurent.Simbol})");
        }
    }
}
