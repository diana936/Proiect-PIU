using NivelStocareDate;
using System;

namespace Meniu.UI
{
    public class MeniuSetari
    {
        private SetariJoc _setari = SetariJoc.Instanta;

        public void Afiseaza()
        {
            Console.Clear();
            Console.WriteLine("=== SETARI ===\n");
            Console.WriteLine($"Rezolutie: {_setari.RezolutieCurenta}");
            Console.WriteLine($"Viteza joc: {_setari.VitezaJoc}");
            Console.WriteLine($"Sunet: {(_setari.SunetActivat ? "Pornit" : "Oprit")}");
            Console.WriteLine("\nApasa orice tasta pentru a te intoarce...");
            Console.ReadKey(true);
        }
    }
}
