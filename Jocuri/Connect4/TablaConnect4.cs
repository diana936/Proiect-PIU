using Meniu.Modeluri;

namespace Meniu.Jocuri.Connect4
{
    public class TablaConnect4 : Tabla
    {
        public const int RanduriStandard = 6;
        public const int ColoaneStandard = 7;

        public TablaConnect4() : base(RanduriStandard, ColoaneStandard) { }

        public int AruncaJeton(int coloana, int valoareJucator)
        {
            return -1;
        }

        public bool EsteColoanaPlina(int coloana)
        {
            return false;
        }

        public bool VerificaCastigator(int valoareJucator)
        {
            return false;
        }

        public bool EsteTablaPlina()
        {
            return false;
        }
    }
}
