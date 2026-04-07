namespace NivelStocareDate
{
    public class SetariJoc
    {
        // ── Singleton ────────────────────────────────────────────────────────────
        private static SetariJoc _instanta;
        public static SetariJoc Instanta => _instanta ??= new SetariJoc();

        // ── Properties ───────────────────────────────────────────────────────────
        public Rezolutie RezolutieCurenta { get; set; }
        public TasteControale Controale   { get; private set; }
        public bool SunetActivat          { get; set; }
        public int VitezaJoc              { get; set; }

        private SetariJoc()
        {
            // Apply built-in defaults first
            RezolutieCurenta = Rezolutie.Medie;
            Controale        = new TasteControale();
            SunetActivat     = false;
            VitezaJoc        = 3;

            // Then overwrite with any previously saved values
            StocareSetari.Instanta.Incarca(this);
        }

        // ── Save current settings to file ────────────────────────────────────────
        public void Salveaza()
        {
            StocareSetari.Instanta.Salveaza(this);
        }

        // ── Reset to defaults and save ───────────────────────────────────────────
        public void ResetareImplicite()
        {
            RezolutieCurenta = Rezolutie.Medie;
            Controale        = new TasteControale();
            SunetActivat     = false;
            VitezaJoc        = 3;
            Salveaza();
        }
    }
}
