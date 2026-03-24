using System;
using LibrarieModele;

namespace Meniu.Jocuri.XSiZero
{
    public class TablaXSiZero : Tabla
    {
        public TablaXSiZero() : base(3, 3) { }

        public StareCelula ObtineStareCelula(Pozitie poz)
        {
            return (StareCelula)CitesteCelula(poz);
        }

        public bool PlaseazaSimbol(Pozitie poz, StareCelula simbol)
        {
            if (ObtineStareCelula(poz) != StareCelula.Goala)
                return false;
            ScrieCelula(poz, (int)simbol);
            return true;
        }

        public bool VerificaCastigator(StareCelula simbol)
        {
            int s = (int)simbol;

            for (int i = 0; i < 3; i++)
            {
                if (Grila[i, 0] == s && Grila[i, 1] == s && Grila[i, 2] == s) return true;
                if (Grila[0, i] == s && Grila[1, i] == s && Grila[2, i] == s) return true;
            }

            if (Grila[0, 0] == s && Grila[1, 1] == s && Grila[2, 2] == s) return true;
            if (Grila[0, 2] == s && Grila[1, 1] == s && Grila[2, 0] == s) return true;

            return false;
        }

        public bool EsteCompleta()
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (Grila[r, c] == (int)StareCelula.Goala)
                        return false;
            return true;
        }
    }
}
