using System.Collections.Generic;
using LibrarieModele;

namespace MeniuWPF
{
    public static class RegistruJucatori
    {
        public static List<Jucator> Jucatori { get; } = new();

        private static readonly List<string> _istoricNume = new();
        public static IReadOnlyList<string> IstoricNume => _istoricNume;

        public static void AdaugaIstoricNume(string nume)
        {
            if (string.IsNullOrWhiteSpace(nume)) return;
            nume = nume.Trim().ToUpper();
            _istoricNume.Remove(nume);
            _istoricNume.Insert(0, nume);
            if (_istoricNume.Count > 10)
                _istoricNume.RemoveAt(_istoricNume.Count - 1);
        }

        public static List<string> ToateNumele()
        {
            var result = new List<string>();
            foreach (var j in Jucatori)
                result.Add(j.Nume.ToUpper());
            foreach (var n in _istoricNume)
                if (!result.Contains(n))
                    result.Add(n);
            return result;
        }
    }
}
