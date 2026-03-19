namespace Meniu.Modeluri
{
    public class InregistrareJoc
    {
        public string NumeJucator { get; set; }
        public string TipJoc { get; set; }
        public int Scor { get; set; }
        public int NrMutari { get; set; }
        public bool AcastigRunda { get; set; }

        public InregistrareJoc(string numeJucator, string tipJoc, int scor, int nrMutari, bool aCastigRunda)
        {
            NumeJucator = numeJucator;
            TipJoc = tipJoc;
            Scor = scor;
            NrMutari = nrMutari;
            AcastigRunda = aCastigRunda;
        }

        public override string ToString()
        {
            string rezultat = AcastigRunda ? "Victorie" : "Infrangere";
            return $"Jucator: {NumeJucator} | Joc: {TipJoc} | Scor: {Scor} | Mutari: {NrMutari} | Rezultat: {rezultat}";
        }
    }
}
