namespace Meniu.Modeluri
{
    public class Pozitie
    {
        public int Rand { get; set; }
        public int Coloana { get; set; }

        public Pozitie(int rand, int coloana)
        {
            Rand = rand;
            Coloana = coloana;
        }

        public bool EsteEgalaCu(Pozitie alta)
        {
            return Rand == alta.Rand && Coloana == alta.Coloana;
        }

        public override string ToString()
        {
            return $"({Rand}, {Coloana})";
        }
    }
}
