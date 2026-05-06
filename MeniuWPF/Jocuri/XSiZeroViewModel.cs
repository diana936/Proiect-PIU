using System;
using LibrarieModele;
using NivelStocareDate;

namespace MeniuWPF.Jocuri
{
    public class XSiZeroViewModel
    {
        private int[,] _tabla = new int[3, 3];
        private bool _esteX = true;
        public bool VsCalculator { get; }
        public bool JocTerminat  { get; private set; }
        public string? Castigator { get; private set; }
        public int NrMutari { get; private set; }
        private Random _random = new Random();

        public XSiZeroViewModel(bool vsCalculator) { VsCalculator = vsCalculator; }

        public string StatusText
        {
            get
            {
                if (JocTerminat)
                    return Castigator != null ? $"{Castigator} a castigat!" : "REMIZA!";
                string j = _esteX ? "Jucator 1 (X)" : (VsCalculator ? "Calculator (O)" : "Jucator 2 (O)");
                return $"Turul: {j}";
            }
        }

        public string ObtineSimbol(int r, int c) =>
            _tabla[r, c] == 1 ? "X" : _tabla[r, c] == 2 ? "O" : "";

        public bool MutareJucator(int r, int c)
        {
            if (JocTerminat || _tabla[r, c] != 0) return false;
            _tabla[r, c] = _esteX ? 1 : 2;
            NrMutari++;
            VerificaStare();
            if (!JocTerminat) _esteX = !_esteX;
            return true;
        }

        public void MutareCalculator()
        {
            if (JocTerminat) return;
            int adancime = SetariJoc.Instanta.AdancimeAI();

            (int r, int c) mutare;
            if (adancime == 0)
            {

                mutare = GasesteRandom();
            }
            else if (adancime == 1)
            {

                mutare = GasesteOptima(2) ?? GasesteOptima(1) ?? GasesteRandom();
            }
            else if (adancime == 2)
            {

                mutare = GasesteOptima(2) ?? GasesteOptima(1) ?? GasesteFork() ?? GasesteRandom();
            }
            else
            {

                mutare = GasesteMinimaxMutare();
            }

            _tabla[mutare.r, mutare.c] = 2;
            NrMutari++;
            VerificaStare();
            if (!JocTerminat) _esteX = true;
        }

        private (int r, int c)? GasesteOptima(int val)
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (_tabla[r, c] == 0)
                    {
                        _tabla[r, c] = val;
                        bool wins = VerificaCastigator(val);
                        _tabla[r, c] = 0;
                        if (wins) return (r, c);
                    }
            return null;
        }

        private (int r, int c)? GasesteFork()
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (_tabla[r, c] == 0)
                    {
                        _tabla[r, c] = 2;
                        int caiCastig = 0;
                        for (int r2 = 0; r2 < 3; r2++)
                            for (int c2 = 0; c2 < 3; c2++)
                                if (_tabla[r2, c2] == 0)
                                {
                                    _tabla[r2, c2] = 2;
                                    if (VerificaCastigator(2)) caiCastig++;
                                    _tabla[r2, c2] = 0;
                                }
                        _tabla[r, c] = 0;
                        if (caiCastig >= 2) return (r, c);
                    }
            return null;
        }

        private (int r, int c) GasesteMinimaxMutare()
        {
            int bestScore = int.MinValue;
            (int r, int c) bestMove = (-1, -1);
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (_tabla[r, c] == 0)
                    {
                        _tabla[r, c] = 2;
                        int score = Minimax(false, 0);
                        _tabla[r, c] = 0;
                        if (score > bestScore) { bestScore = score; bestMove = (r, c); }
                    }
            return bestMove == (-1, -1) ? GasesteRandom() : bestMove;
        }

        private int Minimax(bool eMaximizator, int adancime)
        {
            if (VerificaCastigator(2)) return 10 - adancime;
            if (VerificaCastigator(1)) return adancime - 10;
            if (EsteComplet()) return 0;

            if (eMaximizator)
            {
                int best = int.MinValue;
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        if (_tabla[r, c] == 0)
                        {
                            _tabla[r, c] = 2;
                            best = Math.Max(best, Minimax(false, adancime + 1));
                            _tabla[r, c] = 0;
                        }
                return best;
            }
            else
            {
                int best = int.MaxValue;
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        if (_tabla[r, c] == 0)
                        {
                            _tabla[r, c] = 1;
                            best = Math.Min(best, Minimax(true, adancime + 1));
                            _tabla[r, c] = 0;
                        }
                return best;
            }
        }

        private (int r, int c) GasesteRandom()
        {
            int r, c;
            do { r = _random.Next(3); c = _random.Next(3); } while (_tabla[r, c] != 0);
            return (r, c);
        }

        private void VerificaStare()
        {
            int val = _esteX ? 1 : 2;
            if (VerificaCastigator(val))
            {
                JocTerminat = true;
                Castigator  = _esteX ? "Jucator 1 (X)" : (VsCalculator ? "Calculator" : "Jucator 2 (O)");
            }
            else if (EsteComplet())
            {
                JocTerminat = true;
                Castigator  = null;
            }
        }

        private bool VerificaCastigator(int v)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_tabla[i,0]==v && _tabla[i,1]==v && _tabla[i,2]==v) return true;
                if (_tabla[0,i]==v && _tabla[1,i]==v && _tabla[2,i]==v) return true;
            }
            if (_tabla[0,0]==v && _tabla[1,1]==v && _tabla[2,2]==v) return true;
            if (_tabla[0,2]==v && _tabla[1,1]==v && _tabla[2,0]==v) return true;
            return false;
        }

        private bool EsteComplet()
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (_tabla[r, c] == 0) return false;
            return true;
        }

        public void Restart()
        {
            _tabla      = new int[3, 3];
            _esteX      = true;
            JocTerminat = false;
            Castigator  = null;
            NrMutari    = 0;
        }
    }
}
