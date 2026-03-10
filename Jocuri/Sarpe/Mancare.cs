using Meniu.Modeluri;

namespace Meniu.Jocuri.Sarpe
{
    public class Mancare
    {
        public Pozitie Pozitie { get; private set; }
        public int Puncte { get; private set; }

        public Mancare(Pozitie pozitie, int puncte = 10)
        {
            Pozitie = pozitie;
            Puncte = puncte;
        }

        public void Muta(Pozitie pozitieNoua)
        {
            Pozitie = pozitieNoua;
        }
    }
}
