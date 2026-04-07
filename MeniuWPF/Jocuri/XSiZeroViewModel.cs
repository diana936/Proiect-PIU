using System;
using LibrarieModele;

namespace MeniuWPF.Jocuri
{
    public class XSiZeroViewModel
    {
        // 0=empty, 1=X, 2=O
        private int[,] _tabla = new int[3, 3];
        private bool _esteX = true;  // X always goes first
        public bool VsCalculator { get; }
        public bool JocTerminat  { get; private set; }
        public string? Castigator { get; private set; }
        public int NrMutari { get; private set; }

        private Random _random = new Random();

        public XSiZeroViewModel(bool vsCalculator)
        {
            VsCalculator = vsCalculator;
        }

        public string StatusText
        {
            get
            {
                if (JocTerminat)
                    return Castigator != null ? $"{Castigator} a castigat!" : "Remiza!";
                string jucator = _esteX ? "Jucator 1 (X)" : (VsCalculator ? "Calculator (O)" : "Jucator 2 (O)");
                return $"Turul: {jucator}";
            }
        }

        public string ObtineSimbol(int rand, int col)
        {
            return _tabla[rand, col] == 1 ? "X" : _tabla[rand, col] == 2 ? "O" : "";
        }

        public bool MutareJucator(int rand, int col)
        {
            if (JocTerminat || _tabla[rand, col] != 0) return false;
            _tabla[rand, col] = _esteX ? 1 : 2;
            NrMutari++;
            VerificaStare();
            if (!JocTerminat) _esteX = !_esteX;
            return true;
        }

        public void MutareCalculator()
        {
            if (JocTerminat) return;

            // Try to win, then block, then random
            (int r, int c) = GasesteOptima(2) ?? GasesteOptima(1) ?? GasesteRandom();
            _tabla[r, c] = 2;
            NrMutari++;
            VerificaStare();
            if (!JocTerminat) _esteX = true;
        }

        private (int, int)? GasesteOptima(int val)
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

        private (int, int) GasesteRandom()
        {
            int r, c;
            do { r = _random.Next(3); c = _random.Next(3); }
            while (_tabla[r, c] != 0);
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
