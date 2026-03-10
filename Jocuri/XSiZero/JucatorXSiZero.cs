using Meniu.Modeluri;

namespace Meniu.Jocuri.XSiZero
{
    public class JucatorXSiZero : Jucator
    {
        public StareCelula Simbol { get; private set; }

        public JucatorXSiZero(string nume, StareCelula simbol) : base(nume)
        {
            Simbol = simbol;
        }
    }
}
