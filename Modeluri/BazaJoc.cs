namespace Meniu.Modeluri
{
    public abstract class BazaJoc : IJoc
    {
        public abstract string Nume { get; }
        public bool EsteActiv { get; protected set; }

        protected int Scor { get; set; }
        protected int ScorMaxim { get; set; }

        public virtual void Incepe()
        {
            EsteActiv = true;
            Scor = 0;
        }

        public virtual void Opreste()
        {
            EsteActiv = false;
            if (Scor > ScorMaxim)
                ScorMaxim = Scor;
        }

        public virtual void Reporneste()
        {
            Opreste();
            Incepe();
        }

        public abstract void Randeaza();

        protected void ActualizeazaScor(int puncte)
        {
            Scor += puncte;
        }
    }
}
