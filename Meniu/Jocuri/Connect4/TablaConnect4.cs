using System;
using LibrarieModele;

namespace Meniu.Jocuri.Connect4
{
    public class TablaConnect4 : Tabla
    {
        public const int RanduriStandard = 6;
        public const int ColoaneStandard = 7;

        public TablaConnect4() : base(RanduriStandard, ColoaneStandard) { }

        public int AruncaJeton(int coloana, int valoareJucator)
        {
            for (int r = RanduriStandard - 1; r >= 0; r--)
            {
                Pozitie poz = new Pozitie(r, coloana);
                if (CitesteCelula(poz) == 0)
                {
                    ScrieCelula(poz, valoareJucator);
                    return r;
                }
            }
            return -1;
        }

        public bool EsteColoanaPlina(int coloana)
        {
            return CitesteCelula(new Pozitie(0, coloana)) != 0;
        }

        public bool VerificaCastigator(int valoareJucator)
        {
            // orizontal
            for (int r = 0; r < RanduriStandard; r++)
                for (int c = 0; c <= ColoaneStandard - 4; c++)
                    if (Grila[r, c] == valoareJucator &&
                        Grila[r, c + 1] == valoareJucator &&
                        Grila[r, c + 2] == valoareJucator &&
                        Grila[r, c + 3] == valoareJucator)
                        return true;

            // vertical
            for (int r = 0; r <= RanduriStandard - 4; r++)
                for (int c = 0; c < ColoaneStandard; c++)
                    if (Grila[r, c] == valoareJucator &&
                        Grila[r + 1, c] == valoareJucator &&
                        Grila[r + 2, c] == valoareJucator &&
                        Grila[r + 3, c] == valoareJucator)
                        return true;

            // diagonala \
            for (int r = 0; r <= RanduriStandard - 4; r++)
                for (int c = 0; c <= ColoaneStandard - 4; c++)
                    if (Grila[r, c] == valoareJucator &&
                        Grila[r + 1, c + 1] == valoareJucator &&
                        Grila[r + 2, c + 2] == valoareJucator &&
                        Grila[r + 3, c + 3] == valoareJucator)
                        return true;

            // diagonala /
            for (int r = 3; r < RanduriStandard; r++)
                for (int c = 0; c <= ColoaneStandard - 4; c++)
                    if (Grila[r, c] == valoareJucator &&
                        Grila[r - 1, c + 1] == valoareJucator &&
                        Grila[r - 2, c + 2] == valoareJucator &&
                        Grila[r - 3, c + 3] == valoareJucator)
                        return true;

            return false;
        }

        public bool EsteTablaPlina()
        {
            for (int c = 0; c < ColoaneStandard; c++)
                if (!EsteColoanaPlina(c))
                    return false;
            return true;
        }
    }
}
