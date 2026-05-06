using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace LibrarieModele
{
    public class StocareInregistrari
    {
        private static StocareInregistrari _instanta;
        public static StocareInregistrari Instanta => _instanta ??= new StocareInregistrari();

        private readonly string _caleFisier;
        private const char SEPARATOR = '|';

        private StocareInregistrari()
        {
            _caleFisier = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inregistrari.txt");
        }

        public void Salveaza(List<InregistrareJoc> inregistrari)
        {
            using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
            foreach (InregistrareJoc inreg in inregistrari)
            {
                sw.WriteLine(
                    $"{inreg.NumeJucator}{SEPARATOR}" +
                    $"{inreg.TipulJocului}{SEPARATOR}" +
                    $"{inreg.Scor}{SEPARATOR}" +
                    $"{inreg.NrMutari}{SEPARATOR}" +
                    $"{inreg.Status}");
            }
        }

        public List<InregistrareJoc> Incarca()
        {
            List<InregistrareJoc> rezultat = new List<InregistrareJoc>();

            if (!File.Exists(_caleFisier))
                return rezultat;

            foreach (string linie in File.ReadAllLines(_caleFisier))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;

                string[] parti = linie.Split(SEPARATOR);

                if (parti.Length != 5) continue;

                try
                {
                    string numeJucator = parti[0];
                    TipJoc tipJoc      = Enum.Parse<TipJoc>(parti[1]);
                    int scor           = int.Parse(parti[2]);
                    int nrMutari       = int.Parse(parti[3]);
                    StatusJoc status   = Enum.Parse<StatusJoc>(parti[4]);

                    rezultat.Add(new InregistrareJoc(numeJucator, tipJoc, scor, nrMutari, status));
                }
                catch { }
            }

            return rezultat;
        }

        public List<InregistrareJoc> CautaDupaNumeJucator(string termenCautare)
        {
            List<InregistrareJoc> toate    = Incarca();
            List<InregistrareJoc> rezultat = new List<InregistrareJoc>();

            foreach (InregistrareJoc inreg in toate)
                if (inreg.NumeJucator.IndexOf(termenCautare, StringComparison.OrdinalIgnoreCase) >= 0)
                    rezultat.Add(inreg);

            return rezultat;
        }

        public List<InregistrareJoc> CautaDupaTipJoc(TipJoc tipJoc)
        {
            List<InregistrareJoc> toate    = Incarca();
            List<InregistrareJoc> rezultat = new List<InregistrareJoc>();

            foreach (InregistrareJoc inreg in toate)
                if (inreg.TipulJocului == tipJoc)
                    rezultat.Add(inreg);

            return rezultat;
        }

        public bool ModificaScor(string numeJucator, TipJoc tipJoc, int scorNou)
        {
            List<InregistrareJoc> toate = Incarca();
            bool gasit = false;

            foreach (InregistrareJoc inreg in toate)
            {
                if (inreg.NumeJucator.Equals(numeJucator, StringComparison.OrdinalIgnoreCase)
                    && inreg.TipulJocului == tipJoc)
                {
                    inreg.Scor = scorNou;
                    gasit = true;
                    break;
                }
            }

            if (gasit)
                Salveaza(toate);

            return gasit;
        }

        public bool Sterge(string numeJucator, TipJoc tipJoc)
        {
            List<InregistrareJoc> toate = Incarca();
            int countInitial            = toate.Count;

            toate.RemoveAll(inreg =>
                inreg.NumeJucator.Equals(numeJucator, StringComparison.OrdinalIgnoreCase)
                && inreg.TipulJocului == tipJoc);

            bool sterse = toate.Count < countInitial;
            if (sterse)
                Salveaza(toate);

            return sterse;
        }
    }
}
