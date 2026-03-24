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
            _inregistrari = new List<InregistrareJoc>();
        }

        public void Adauga(InregistrareJoc inregistrare)
        {
            _inregistrari.Add(inregistrare);
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

        public List<InregistrareJoc> ObtineToate()
        {
            return _inregistrari;
        }
    }
}
