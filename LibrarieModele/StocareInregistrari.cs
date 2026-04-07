using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace LibrarieModele
{
    /// <summary>
    /// Handles text file persistence for InregistrareJoc (leaderboard records).
    /// File format per line:
    ///   NumeJucator|TipJoc|Scor|NrMutari|Status
    /// Example:
    ///   Ana|Snake|150|0|Pierdut
    /// </summary>
    public class StocareInregistrari
    {
        // ── Singleton ────────────────────────────────────────────────────────────
        private static StocareInregistrari _instanta;
        public static StocareInregistrari Instanta => _instanta ??= new StocareInregistrari();

        // ── File path ────────────────────────────────────────────────────────────
        private readonly string _caleFisier;

        // Separator used between fields inside one line
        private const char SEPARATOR = '|';

        private StocareInregistrari()
        {
            // Saves next to the executable; change path as needed
            _caleFisier = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "inregistrari.txt");
        }

        // ════════════════════════════════════════════════════════════════════════
        // SAVE – writes the entire list to the file (overwrite)
        // ════════════════════════════════════════════════════════════════════════
        public void Salveaza(List<InregistrareJoc> inregistrari)
        {
            using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
            foreach (InregistrareJoc inreg in inregistrari)
            {
                // Each record becomes one line with | as separator
                sw.WriteLine(
                    $"{inreg.NumeJucator}{SEPARATOR}" +
                    $"{inreg.TipulJocului}{SEPARATOR}" +
                    $"{inreg.Scor}{SEPARATOR}" +
                    $"{inreg.NrMutari}{SEPARATOR}" +
                    $"{inreg.Status}");
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // LOAD – reads all records from the file
        // ════════════════════════════════════════════════════════════════════════
        public List<InregistrareJoc> Incarca()
        {
            List<InregistrareJoc> rezultat = new List<InregistrareJoc>();

            // If the file doesn't exist yet, return an empty list gracefully
            if (!File.Exists(_caleFisier))
                return rezultat;

            foreach (string linie in File.ReadAllLines(_caleFisier))
            {
                // Skip blank lines
                if (string.IsNullOrWhiteSpace(linie)) continue;

                string[] parti = linie.Split(SEPARATOR);

                // A valid line must have exactly 5 fields
                if (parti.Length != 5) continue;

                try
                {
                    string numeJucator       = parti[0];
                    TipJoc tipJoc            = Enum.Parse<TipJoc>(parti[1]);
                    int scor                 = int.Parse(parti[2]);
                    int nrMutari             = int.Parse(parti[3]);
                    StatusJoc status         = Enum.Parse<StatusJoc>(parti[4]);

                    rezultat.Add(new InregistrareJoc(numeJucator, tipJoc, scor, nrMutari, status));
                }
                catch
                {
                    // Skip malformed lines without crashing
                    Console.WriteLine($"[StocareInregistrari] Linie invalida ignorata: {linie}");
                }
            }

            return rezultat;
        }

        // ════════════════════════════════════════════════════════════════════════
        // SEARCH – find all records whose player name contains the search term
        // (case-insensitive)
        // ════════════════════════════════════════════════════════════════════════
        public List<InregistrareJoc> CautaDupaNumeJucator(string termenCautare)
        {
            List<InregistrareJoc> toate    = Incarca();
            List<InregistrareJoc> rezultat = new List<InregistrareJoc>();

            foreach (InregistrareJoc inreg in toate)
            {
                if (inreg.NumeJucator.IndexOf(
                        termenCautare,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rezultat.Add(inreg);
                }
            }

            return rezultat;
        }

        // ════════════════════════════════════════════════════════════════════════
        // SEARCH – find all records for a specific game type
        // ════════════════════════════════════════════════════════════════════════
        public List<InregistrareJoc> CautaDupaTipJoc(TipJoc tipJoc)
        {
            List<InregistrareJoc> toate    = Incarca();
            List<InregistrareJoc> rezultat = new List<InregistrareJoc>();

            foreach (InregistrareJoc inreg in toate)
                if (inreg.TipulJocului == tipJoc)
                    rezultat.Add(inreg);

            return rezultat;
        }

        // ════════════════════════════════════════════════════════════════════════
        // MODIFY – update the score of the first record that matches
        //          numeJucator + tipJoc exactly
        // Returns true if a record was found and updated, false otherwise.
        // ════════════════════════════════════════════════════════════════════════
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
                    break; // modify only the first match
                }
            }

            if (gasit)
                Salveaza(toate); // persist the change back to disk

            return gasit;
        }

        // ════════════════════════════════════════════════════════════════════════
        // DELETE – remove all records that match numeJucator + tipJoc
        // ════════════════════════════════════════════════════════════════════════
        public bool Sterge(string numeJucator, TipJoc tipJoc)
        {
            List<InregistrareJoc> toate   = Incarca();
            int countInitial              = toate.Count;

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
