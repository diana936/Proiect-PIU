namespace Meniu.Modeluri
{
    public class Tabla
    {
        public int Randuri { get; private set; }
        public int Coloane { get; private set; }
        protected int[,] Grila { get; private set; }

        public Tabla(int randuri, int coloane)
        {
            Randuri = randuri;
            Coloane = coloane;
            Grila = new int[randuri, coloane];
        }

        public virtual void Goleste()
        {
            Grila = new int[Randuri, Coloane];
        }

        public bool PozitieValida(Pozitie poz)
        {
            return poz.Rand >= 0 && poz.Rand < Randuri &&
                   poz.Coloana >= 0 && poz.Coloana < Coloane;
        }

        public int CitesteCelula(Pozitie poz)
        {
            return Grila[poz.Rand, poz.Coloana];
        }

        public void ScrieCelula(Pozitie poz, int valoare)
        {
            if (PozitieValida(poz))
                Grila[poz.Rand, poz.Coloana] = valoare;
        }
    }
}
