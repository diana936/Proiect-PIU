using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class StocareSetari
    {
        private static StocareSetari? _instanta;
        public static StocareSetari Instanta => _instanta ??= new StocareSetari();

        private readonly string _caleFisier;
        private const char SEPARATOR = '=';

        private StocareSetari()
        {
            _caleFisier = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setari.txt");
        }

        public void Salveaza(SetariJoc setari)
        {
            using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
            sw.WriteLine($"Rezolutie{SEPARATOR}{setari.RezolutieCurenta.Latime}x{setari.RezolutieCurenta.Inaltime}");
            sw.WriteLine($"VitezaJoc{SEPARATOR}{setari.VitezaJoc}");
            sw.WriteLine($"SunetActivat{SEPARATOR}{setari.SunetActivat}");
            sw.WriteLine($"Dificultate{SEPARATOR}{setari.Dificultate}");
            foreach (string actiune in setari.Controale.ObtineToateActiunile())
            {
                ConsoleKey tasta = setari.Controale.ObtineTasta(actiune);
                sw.WriteLine($"Tasta_{actiune}{SEPARATOR}{tasta}");
            }
        }

        public void Incarca(SetariJoc setari)
        {
            if (!File.Exists(_caleFisier)) return;
            foreach (string linie in File.ReadAllLines(_caleFisier))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                int idx = linie.IndexOf(SEPARATOR);
                if (idx < 0) continue;
                string cheie   = linie.Substring(0, idx).Trim();
                string valoare = linie.Substring(idx + 1).Trim();
                try { AplicaSetare(setari, cheie, valoare); }
                catch { }
            }
        }

        private void AplicaSetare(SetariJoc setari, string cheie, string valoare)
        {
            if (cheie == "Rezolutie")
            {
                string[] parts = valoare.Split('x');
                if (parts.Length == 2 && int.TryParse(parts[0], out int l) && int.TryParse(parts[1], out int h))
                    setari.RezolutieCurenta = new Rezolutie(l, h);
            }
            else if (cheie == "VitezaJoc")
            {
                if (int.TryParse(valoare, out int v)) setari.VitezaJoc = v;
            }
            else if (cheie == "SunetActivat")
            {
                if (bool.TryParse(valoare, out bool s)) setari.SunetActivat = s;
            }
            else if (cheie == "Dificultate")
            {
                if (Enum.TryParse<NivelDificultate>(valoare, out NivelDificultate d)) setari.Dificultate = d;
            }
            else if (cheie.StartsWith("Tasta_"))
            {
                string actiune = cheie.Substring("Tasta_".Length);
                if (Enum.TryParse<ConsoleKey>(valoare, out ConsoleKey t)) setari.Controale.SeteazaTasta(actiune, t);
            }
        }

        public string? CautaDupaCheie(string cheie)
        {
            var toate = CitesteToate();
            return toate.TryGetValue(cheie, out string? val) ? val : null;
        }

        public Dictionary<string, string> CitesteToate()
        {
            var rezultat = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(_caleFisier)) return rezultat;
            foreach (string linie in File.ReadAllLines(_caleFisier))
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                int idx = linie.IndexOf(SEPARATOR);
                if (idx < 0) continue;
                rezultat[linie.Substring(0, idx).Trim()] = linie.Substring(idx + 1).Trim();
            }
            return rezultat;
        }

        public bool ModificaSetare(string cheie, string valoareNoua)
        {
            var toate = CitesteToate();
            bool exista = toate.ContainsKey(cheie);
            toate[cheie] = valoareNoua;
            using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
            foreach (var p in toate) sw.WriteLine($"{p.Key}{SEPARATOR}{p.Value}");
            return exista;
        }

        public bool StergeSetare(string cheie)
        {
            var toate = CitesteToate();
            bool gasit = toate.Remove(cheie);
            if (gasit)
            {
                using StreamWriter sw = new StreamWriter(_caleFisier, append: false);
                foreach (var p in toate) sw.WriteLine($"{p.Key}{SEPARATOR}{p.Value}");
            }
            return gasit;
        }
    }
}
