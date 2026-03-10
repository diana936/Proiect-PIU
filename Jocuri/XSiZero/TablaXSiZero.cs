using Meniu.Modeluri;

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
            return false;
        }

        public bool EsteCompleta()
        {
            return false;
        }
    }
}
