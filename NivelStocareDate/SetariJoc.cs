namespace NivelStocareDate
{
    public class SetariJoc
    {
        private static SetariJoc _instanta;
        public static SetariJoc Instanta => _instanta ??= new SetariJoc();

        public Rezolutie RezolutieCurenta { get; set; }
        public TasteControale Controale { get; private set; }
        public bool SunetActivat { get; set; }
        public int VitezaJoc { get; set; }

        private SetariJoc()
        {
            RezolutieCurenta = Rezolutie.Medie;
            Controale = new TasteControale();
            SunetActivat = false;
            VitezaJoc = 3;
        }

        public void ResetareImplicite()
        {
            RezolutieCurenta = Rezolutie.Medie;
            Controale = new TasteControale();
            SunetActivat = false;
            VitezaJoc = 3;
        }
    }
}
