using System;

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

        public Connect4ViewModel(bool vsCalculator)
        {
            VsCalculator = vsCalculator;
        }

        public string StatusText
        {
            get
            {
                if (JocTerminat)
                    return Castigator != null ? $"{Castigator} a castigat!" : "Remiza!";
                return _esteJucator1
                    ? "Turul: Jucator 1 🔴"
                    : $"Turul: {(VsCalculator ? "Calculator" : "Jucator 2")} 🟡";
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
            int col = GasesteOptima(2) ?? GasesteOptima(1) ?? GasesteRandom();
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

        private int? GasesteOptima(int val)
        {
            for (int c = 0; c < 7; c++)
            {
                if (_tabla[0, c] != 0) continue;
                int r = AruncaJeton(c, val);
                bool wins = VerificaCastigator(val);
                _tabla[r, c] = 0;
                if (wins) return c;
            }
            return null;
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
            // Horizontal
            for (int r = 0; r < 6; r++)
                for (int c = 0; c <= 3; c++)
                    if (_tabla[r,c]==v && _tabla[r,c+1]==v && _tabla[r,c+2]==v && _tabla[r,c+3]==v) return true;
            // Vertical
            for (int r = 0; r <= 2; r++)
                for (int c = 0; c < 7; c++)
                    if (_tabla[r,c]==v && _tabla[r+1,c]==v && _tabla[r+2,c]==v && _tabla[r+3,c]==v) return true;
            // Diagonal \
            for (int r = 0; r <= 2; r++)
                for (int c = 0; c <= 3; c++)
                    if (_tabla[r,c]==v && _tabla[r+1,c+1]==v && _tabla[r+2,c+2]==v && _tabla[r+3,c+3]==v) return true;
            // Diagonal /
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
