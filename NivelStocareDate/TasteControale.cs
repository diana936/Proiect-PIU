using System;
using System.Collections.Generic;

namespace NivelStocareDate
{
    public class TasteControale
    {
        private Dictionary<string, ConsoleKey> _asocieri;

        public TasteControale()
        {
            _asocieri = new Dictionary<string, ConsoleKey>();
            IncarcaImplicite();
        }

        private void IncarcaImplicite()
        {
            _asocieri["sarpe_sus"]      = ConsoleKey.UpArrow;
            _asocieri["sarpe_jos"]      = ConsoleKey.DownArrow;
            _asocieri["sarpe_stanga"]   = ConsoleKey.LeftArrow;
            _asocieri["sarpe_dreapta"]  = ConsoleKey.RightArrow;
            _asocieri["meniu_confirma"] = ConsoleKey.Enter;
            _asocieri["meniu_inapoi"]   = ConsoleKey.Escape;
        }

        public ConsoleKey ObtineTasta(string actiune)
        {
            return _asocieri.ContainsKey(actiune) ? _asocieri[actiune] : ConsoleKey.NoName;
        }

        public void SeteazaTasta(string actiune, ConsoleKey tasta)
        {
            _asocieri[actiune] = tasta;
        }

        // Returns all registered action names (needed for file serialization)
        public IEnumerable<string> ObtineToateActiunile()
        {
            return _asocieri.Keys;
        }
    }
}
