using System;
using System.Collections.Generic;

namespace LibrarieModele
{
    public class GestiuneInregistrari
    {
        private static GestiuneInregistrari _instanta = null!;
        public static GestiuneInregistrari Instanta => _instanta ??= new GestiuneInregistrari();

        private List<InregistrareJoc> _inregistrari;

        public GestiuneInregistrari()
        {
            // Load existing records from file on startup
            _inregistrari = StocareInregistrari.Instanta.Incarca();
        }

        public void Adauga(InregistrareJoc inregistrare)
        {
            _inregistrari.Add(inregistrare);
            StocareInregistrari.Instanta.Salveaza(_inregistrari);
        }

        public List<InregistrareJoc> ObtineLeaderboard(TipJoc tipJoc)
        {
            List<InregistrareJoc> rezultate = new List<InregistrareJoc>();
            foreach (InregistrareJoc inreg in _inregistrari)
                if (inreg.TipulJocului == tipJoc)
                    rezultate.Add(inreg);
            rezultate.Sort((a, b) => b.Scor.CompareTo(a.Scor));
            return rezultate;
        }

        public List<InregistrareJoc> ObtineToate() => _inregistrari;

        public List<InregistrareJoc> CautaDupaNumeJucator(string termen)
            => StocareInregistrari.Instanta.CautaDupaNumeJucator(termen);

        public List<InregistrareJoc> CautaDupaTipJoc(TipJoc tipJoc)
            => StocareInregistrari.Instanta.CautaDupaTipJoc(tipJoc);

        public bool ModificaScor(string numeJucator, TipJoc tipJoc, int scorNou)
        {
            bool ok = StocareInregistrari.Instanta.ModificaScor(numeJucator, tipJoc, scorNou);
            if (ok) _inregistrari = StocareInregistrari.Instanta.Incarca();
            return ok;
        }

        public bool Sterge(string numeJucator, TipJoc tipJoc)
        {
            bool ok = StocareInregistrari.Instanta.Sterge(numeJucator, tipJoc);
            if (ok) _inregistrari = StocareInregistrari.Instanta.Incarca();
            return ok;
        }
    }
}
