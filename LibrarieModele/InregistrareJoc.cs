using System;

namespace LibrarieModele
{
    public class InregistrareJoc
    {
        public string NumeJucator { get; set; }
        public TipJoc TipulJocului { get; set; }
        public int Scor { get; set; }
        public int NrMutari { get; set; }
        public StatusJoc Status { get; set; }

        public InregistrareJoc(string numeJucator, TipJoc tipJoc, int scor, int nrMutari, StatusJoc status)
        {
            NumeJucator = numeJucator;
            TipulJocului = tipJoc;
            Scor = scor;
            NrMutari = nrMutari;
            Status = status;
        }

        public override string ToString()
        {
            return $"{NumeJucator,-15} | Scor: {Scor,5} | Mutari: {NrMutari,4} | Status: {Status}";
        }
    }
}
