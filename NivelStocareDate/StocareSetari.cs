using System;
using System.Collections.Generic;
using System.IO;

namespace NivelStocareDate
{
    /// <summary>
    /// Handles text file persistence for SetariJoc (game settings).
    /// File format – one setting per line as key=value pairs:
    ///   Rezolutie=120x30
    ///   VitezaJoc=3
    ///   SunetActivat=False
    ///   Tasta_sarpe_sus=UpArrow
    ///   Tasta_sarpe_jos=DownArrow
    ///   ... (one line per key binding)
    /// </summary>
    public class StocareSetari
    {
        // ── Singleton ────────────────────────────────────────────────────────────
        private static StocareSetari _instanta;
        public static StocareSetari Instanta => _instanta ??= new StocareSetari();

        // ── File path ────────────────────────────────────────────────────────────
        private readonly string _caleFisier;

        private const char SEPARATOR = '=';

        private StocareSetari()
        {
            _caleFisier = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "setari.txt");
        }

        // ════════════════════════════════════════════════════════════════════════
        // SAVE – serialize the SetariJoc singleton to the file
        // ════════════════════════════════════════════════════════════════════════
        public void Salveaza(SetariJoc setari)
        {
            using StreamWriter sw = new StreamWriter(_caleFisier, append: false);

            // Write resolution as WidthxHeight
            sw.WriteLine($"Rezolutie{SEPARATOR}{setari.RezolutieCurenta.Latime}x{setari.RezolutieCurenta.Inaltime}");

            // Write game speed
            sw.WriteLine($"VitezaJoc{SEPARATOR}{setari.VitezaJoc}");

            // Write sound toggle
            sw.WriteLine($"SunetActivat{SEPARATOR}{setari.SunetActivat}");

            // Write every key binding stored in TasteControale
            foreach (string actiune in setari.Controale.ObtineToateActiunile())
            {
                ConsoleKey tasta = setari.Controale.ObtineTasta(actiune);
                sw.WriteLine($"Tasta_{actiune}{SEPARATOR}{tasta}");
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // LOAD – read file and apply values to the SetariJoc singleton
        // ════════════════════════════════════════════════════════════════════════
        public void Incarca(SetariJoc setari)
        {
            if (!File.Exists(_caleFisier))
                return; // no saved settings yet — keep defaults

            foreach (string linie in File.ReadAllLines(_caleFisier))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;

                // Split only on the first '=' so values containing '=' are safe
                int idx = linie.IndexOf(SEPARATOR);
                if (idx < 0) continue;

                string cheie  = linie.Substring(0, idx).Trim();
                string valoare = linie.Substring(idx + 1).Trim();

                try
                {
                    AplicaSetare(setari, cheie, valoare);
                }
                catch
                {
                    Console.WriteLine($"[StocareSetari] Setare invalida ignorata: {linie}");
                }
            }
        }

        // Applies a single key=value pair to the settings object
        private void AplicaSetare(SetariJoc setari, string cheie, string valoare)
        {
            if (cheie == "Rezolutie")
            {
                // Format: "120x30"
                string[] parts = valoare.Split('x');
                if (parts.Length == 2
                    && int.TryParse(parts[0], out int latime)
                    && int.TryParse(parts[1], out int inaltime))
                {
                    setari.RezolutieCurenta = new Rezolutie(latime, inaltime);
                }
            }
            else if (cheie == "VitezaJoc")
            {
                if (int.TryParse(valoare, out int viteza))
                    setari.VitezaJoc = viteza;
            }
            else if (cheie == "SunetActivat")
            {
                if (bool.TryParse(valoare, out bool sunet))
                    setari.SunetActivat = sunet;
            }
            else if (cheie.StartsWith("Tasta_"))
            {
                // Format: "Tasta_sarpe_sus" → action = "sarpe_sus"
                string actiune = cheie.Substring("Tasta_".Length);
                if (Enum.TryParse<ConsoleKey>(valoare, out ConsoleKey tasta))
                    setari.Controale.SeteazaTasta(actiune, tasta);
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        // SEARCH – read the raw key=value map from the file
        //          and return the value for a specific key, or null if not found
        // ════════════════════════════════════════════════════════════════════════
        public string CautaDupaCheie(string cheie)
        {
            Dictionary<string, string> toate = CitesteToate();
            return toate.TryGetValue(cheie, out string val) ? val : null;
        }

        // Returns all key=value pairs from the file as a dictionary
        public Dictionary<string, string> CitesteToate()
        {
            Dictionary<string, string> rezultat = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(_caleFisier))
                return rezultat;

            foreach (string linie in File.ReadAllLines(_caleFisier))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                int idx = linie.IndexOf(SEPARATOR);
                if (idx < 0) continue;
                string cheie   = linie.Substring(0, idx).Trim();
                string valoare = linie.Substring(idx + 1).Trim();
                rezultat[cheie] = valoare;
            }

            return rezultat;
        }

        // ════════════════════════════════════════════════════════════════════════
        // MODIFY – change a single setting by key, leaving all others intact
        // Returns true if the key existed and was updated, false if it was added
        // as a new entry (upsert behaviour).
        // ════════════════════════════════════════════════════════════════════════
        public bool ModificaSetare(string cheie, string valoareNoua)
        {
            Dictionary<string, string> toate = CitesteToate();
            bool exista = toate.ContainsKey(cheie);
            toate[cheie] = valoareNoua; // insert or update

            // Write the updated map back to disk
            using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
            foreach (KeyValuePair<string, string> pereche in toate)
                sw.WriteLine($"{pereche.Key}{SEPARATOR}{pereche.Value}");

            return exista;
        }

        // ════════════════════════════════════════════════════════════════════════
        // DELETE – remove a setting entry by key
        // ════════════════════════════════════════════════════════════════════════
        public bool StergeSetare(string cheie)
        {
            Dictionary<string, string> toate = CitesteToate();
            bool gasit = toate.Remove(cheie);

            if (gasit)
            {
                using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
                foreach (KeyValuePair<string, string> pereche in toate)
                    sw.WriteLine($"{pereche.Key}{SEPARATOR}{pereche.Value}");
            }

            return gasit;
        }
    }
}
