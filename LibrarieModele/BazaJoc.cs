using System;
using LibrarieModele;

namespace LibrarieModele
{
    public abstract class BazaJoc : IJoc
    {
        public abstract string Nume { get; }
        public bool EsteActiv { get; protected set; }

        protected int Scor { get; set; }
        protected int ScorMaxim { get; set; }
        protected string NumeJucator { get; set; } = "Jucator";

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

        protected void SalveazaScor(TipJoc tipJoc, StatusJoc status, int nrMutari = 0)
        {
            Console.Write("\nIntrodu numele tau pentru leaderboard: ");
            string nume = Console.ReadLine() ?? "Anonim";
            if (string.IsNullOrWhiteSpace(nume)) nume = "Anonim";

            InregistrareJoc inreg = new InregistrareJoc(nume, tipJoc, Scor, nrMutari, status);
            GestiuneInregistrari.Instanta.Adauga(inreg);
        }
    }
}