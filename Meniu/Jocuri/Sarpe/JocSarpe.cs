using System;
using System.Collections.Generic;
using System.Threading;
using LibrarieModele;

namespace Meniu.Jocuri.Sarpe
{
    public class JocSarpe : BazaJoc
    {
        public override string Nume => "Snake";

        private CorpSarpe _sarpe;
        private Mancare _mancare;
        private Directie _directieCurenta;
        private int _latimeGrila;
        private int _inaltimeGrila;
        private Random _random;

        public JocSarpe(int latimeGrila = 20, int inaltimeGrila = 20)
        {
            _latimeGrila = latimeGrila;
            _inaltimeGrila = inaltimeGrila;
            _random = new Random();
        }

        public override void Incepe()
        {
            base.Incepe();
            Console.Clear();
            Console.CursorVisible = false;
            _sarpe = new CorpSarpe(new Pozitie(_inaltimeGrila / 2, _latimeGrila / 2));
            _directieCurenta = Directie.Dreapta;
            GenereazaMancare();

            while (EsteActiv)
            {
                Randeaza();
                GestioneazaInput();
                Actualizeaza();
                Thread.Sleep(150);
            }

            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine($"Game Over! Scor final: {Scor}");
            SalveazaScor(TipJoc.Snake, StatusJoc.Pierdut);
            Console.WriteLine("Apasa orice tasta pentru a continua...");
            Console.ReadKey(true);
        }

        private void GestioneazaInput()
        {
            if (!Console.KeyAvailable) return;
            ConsoleKey tasta = Console.ReadKey(true).Key;
            switch (tasta)
            {
                case ConsoleKey.UpArrow:
                    if (_directieCurenta != Directie.Jos)
                        _directieCurenta = Directie.Sus;
                    break;
                case ConsoleKey.DownArrow:
                    if (_directieCurenta != Directie.Sus)
                        _directieCurenta = Directie.Jos;
                    break;
                case ConsoleKey.LeftArrow:
                    if (_directieCurenta != Directie.Dreapta)
                        _directieCurenta = Directie.Stanga;
                    break;
                case ConsoleKey.RightArrow:
                    if (_directieCurenta != Directie.Stanga)
                        _directieCurenta = Directie.Dreapta;
                    break;
                case ConsoleKey.Escape:
                    Opreste();
                    break;
            }
        }

        public void Actualizeaza()
        {
            bool mancat = _sarpe.Cap.Pozitie.EsteEgalaCu(_mancare.Pozitie);

            _sarpe.Muta(_directieCurenta, mancat);

            if (mancat)
            {
                ActualizeazaScor(_mancare.Puncte);
                GenereazaMancare();
            }

            if (EsteInAfara() || _sarpe.SeIntersecteazaCuSine())
                Opreste();
        }

        private bool EsteInAfara()
        {
            Pozitie cap = _sarpe.Cap.Pozitie;
            return cap.Rand < 0 || cap.Rand >= _inaltimeGrila ||
                   cap.Coloana < 0 || cap.Coloana >= _latimeGrila;
        }

        private void GenereazaMancare()
        {
            Pozitie pozitieNoua;
            List<SegmentSarpe> segmente = _sarpe.ObtineSegmente();
            do
            {
                pozitieNoua = new Pozitie(
                    _random.Next(1, _inaltimeGrila - 1),
                    _random.Next(1, _latimeGrila - 1)
                );
            } while (segmente.Exists(s => s.Pozitie.EsteEgalaCu(pozitieNoua)));

            _mancare = new Mancare(pozitieNoua, 10);
        }

        public override void Randeaza()
        {
            Console.SetCursorPosition(0, 0);

            for (int r = 0; r < _inaltimeGrila; r++)
            {
                for (int c = 0; c < _latimeGrila; c++)
                {
                    Pozitie poz = new Pozitie(r, c);

                    if (r == 0 || r == _inaltimeGrila - 1 || c == 0 || c == _latimeGrila - 1)
                    {
                        Console.Write('#');
                    }
                    else if (_mancare.Pozitie.EsteEgalaCu(poz))
                    {
                        Console.Write('*');
                    }
                    else
                    {
                        SegmentSarpe segment = _sarpe.ObtineSegmente()
                            .Find(s => s.Pozitie.EsteEgalaCu(poz));
                        if (segment != null)
                            Console.Write(segment.EsteCap ? 'O' : 'o');
                        else
                            Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Scor: {Scor}  |  Lungime: {_sarpe.Lungime}  |  ESC = iesire   ");
        }
    }
}