using System;
using NivelStocareDate;

namespace MeniuWPF.Jocuri
{
    public class Connect4ViewModel
    {
        private int[,] _tabla = new int[6, 7];
        private bool _esteJucator1 = true;
        public bool VsCalculator { get; }
        public bool JocTerminat  { get; private set; }
        public string? Castigator { get; private set; }
        public int NrMutari { get; private set; }
        private Random _random = new Random();

        public Connect4ViewModel(bool vsCalculator) { VsCalculator = vsCalculator; }

        public string StatusText
        {
            get
            {
                if (JocTerminat)
                    return Castigator != null ? $"{Castigator} a castigat!" : "REMIZA!";
                return _esteJucator1
                    ? "Turul: Jucator 1 [R]"
                    : $"Turul: {(VsCalculator ? "Calculator" : "Jucator 2")} [Y]";
            }
        }

        public int ObtineCelula(int r, int c) => _tabla[r, c];

        public bool MutareJucator(int coloana)
        {
            if (JocTerminat) return false;
            int rand = AruncaJeton(coloana, _esteJucator1 ? 1 : 2);
            if (rand < 0) return false;
            NrMutari++;
            VerificaStare();
            if (!JocTerminat) _esteJucator1 = !_esteJucator1;
            return true;
        }

        public void MutareCalculator()
        {
            if (JocTerminat) return;
            int adancime = SetariJoc.Instanta.AdancimeAI();
            int col;
            if (adancime == 0)
                col = GasesteRandom();
            else if (adancime == 1)
                col = GasesteOptima(2) ?? GasesteOptima(1) ?? GasesteRandom();
            else
                col = GasesteMinimaxCol(adancime * 2);

            AruncaJeton(col, 2);
            NrMutari++;
            VerificaStare();
            if (!JocTerminat) _esteJucator1 = true;
        }

        private int AruncaJeton(int col, int val)
        {
            for (int r = 5; r >= 0; r--)
                if (_tabla[r, col] == 0) { _tabla[r, col] = val; return r; }
            return -1;
        }

        private void UndoJeton(int col)
        {
            for (int r = 0; r < 6; r++)
                if (_tabla[r, col] != 0) { _tabla[r, col] = 0; return; }
        }

        private int? GasesteOptima(int val)
        {
            for (int c = 0; c < 7; c++)
            {
                int r = AruncaJeton(c, val);
                if (r < 0) continue;
                bool wins = VerificaCastigator(val);
                UndoJeton(c);
                if (wins) return c;
            }
            return null;
        }

        private int GasesteMinimaxCol(int maxPly)
        {
            int bestScore = int.MinValue, bestCol = GasesteRandom();
            for (int c = 0; c < 7; c++)
            {
                int r = AruncaJeton(c, 2);
                if (r < 0) continue;
                int score = Minimax(maxPly - 1, false, int.MinValue, int.MaxValue);
                UndoJeton(c);
                if (score > bestScore) { bestScore = score; bestCol = c; }
            }
            return bestCol;
        }

        private int Minimax(int depth, bool eMaximizator, int alpha, int beta)
        {
            if (VerificaCastigator(2)) return  1000 + depth;
            if (VerificaCastigator(1)) return -1000 - depth;
            if (depth == 0 || EsteTablaPlina()) return EvalueazaTabla();

            if (eMaximizator)
            {
                int best = int.MinValue;
                for (int c = 0; c < 7; c++)
                {
                    int r = AruncaJeton(c, 2);
                    if (r < 0) continue;
                    best = Math.Max(best, Minimax(depth - 1, false, alpha, beta));
                    UndoJeton(c);
                    alpha = Math.Max(alpha, best);
                    if (beta <= alpha) break;
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;
                for (int c = 0; c < 7; c++)
                {
                    int r = AruncaJeton(c, 1);
                    if (r < 0) continue;
                    best = Math.Min(best, Minimax(depth - 1, true, alpha, beta));
                    UndoJeton(c);
                    beta = Math.Min(beta, best);
                    if (beta <= alpha) break;
                }
                return best;
            }
        }

        private int EvalueazaTabla()
        {
            int scor = 0;
            int[] weights = { 0, 1, 2, 4, 2, 1, 0 };
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 7; c++)
                {
                    if (_tabla[r, c] == 2) scor += weights[c];
                    else if (_tabla[r, c] == 1) scor -= weights[c];
                }
            return scor;
        }

        private int GasesteRandom()
        {
            int c;
            do { c = _random.Next(7); } while (_tabla[0, c] != 0);
            return c;
        }

        private void VerificaStare()
        {
            int val = _esteJucator1 ? 1 : 2;
            if (VerificaCastigator(val))
            {
                JocTerminat = true;
                Castigator  = _esteJucator1 ? "Jucator 1" : (VsCalculator ? "Calculator" : "Jucator 2");
            }
            else if (EsteTablaPlina())
            {
                JocTerminat = true;
                Castigator  = null;
            }
        }

        private bool VerificaCastigator(int v)
        {
            for (int r = 0; r < 6; r++)
                for (int c = 0; c <= 3; c++)
                    if (_tabla[r,c]==v && _tabla[r,c+1]==v && _tabla[r,c+2]==v && _tabla[r,c+3]==v) return true;
            for (int r = 0; r <= 2; r++)
                for (int c = 0; c < 7; c++)
                    if (_tabla[r,c]==v && _tabla[r+1,c]==v && _tabla[r+2,c]==v && _tabla[r+3,c]==v) return true;
            for (int r = 0; r <= 2; r++)
                for (int c = 0; c <= 3; c++)
                    if (_tabla[r,c]==v && _tabla[r+1,c+1]==v && _tabla[r+2,c+2]==v && _tabla[r+3,c+3]==v) return true;
            for (int r = 3; r < 6; r++)
                for (int c = 0; c <= 3; c++)
                    if (_tabla[r,c]==v && _tabla[r-1,c+1]==v && _tabla[r-2,c+2]==v && _tabla[r-3,c+3]==v) return true;
            return false;
        }

        private bool EsteTablaPlina()
        {
            for (int c = 0; c < 7; c++) if (_tabla[0, c] == 0) return false;
            return true;
        }

        public void Restart()
        {
            _tabla        = new int[6, 7];
            _esteJucator1 = true;
            JocTerminat   = false;
            Castigator    = null;
            NrMutari      = 0;
        }
    }
}
