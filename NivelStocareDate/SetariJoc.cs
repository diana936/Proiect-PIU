using LibrarieModele;

namespace NivelStocareDate
{
    public class SetariJoc
    {

        private static SetariJoc? _instanta;
        public static SetariJoc Instanta => _instanta ??= new SetariJoc();

        public Rezolutie RezolutieCurenta       { get; set; }
        public TasteControale Controale         { get; private set; }
        public bool SunetActivat                { get; set; }
        public int VitezaJoc                    { get; set; }
        public NivelDificultate Dificultate     { get; set; }

        private SetariJoc()
        {
            RezolutieCurenta = Rezolutie.Medie;
            Controale        = new TasteControale();
            SunetActivat     = false;
            VitezaJoc        = 3;
            Dificultate      = NivelDificultate.Mediu;

            StocareSetari.Instanta.Incarca(this);
        }

        public void Salveaza()          => StocareSetari.Instanta.Salveaza(this);

        public void ResetareImplicite()
        {
            RezolutieCurenta = Rezolutie.Medie;
            Controale        = new TasteControale();
            SunetActivat     = false;
            VitezaJoc        = 3;
            Dificultate      = NivelDificultate.Mediu;
            Salveaza();
        }

        public int VitezaSnakeMs()
        {

            int baza = 420 - (VitezaJoc * 34);

            return Dificultate switch
            {
                NivelDificultate.Usor    => baza + 80,
                NivelDificultate.Dificil => baza - 40,
                NivelDificultate.Expert  => baza - 80,
                _                        => baza
            };
        }

        public int AdancimeAI()
        {
            return Dificultate switch
            {
                NivelDificultate.Usor    => 0,
                NivelDificultate.Mediu   => 1,
                NivelDificultate.Dificil => 2,
                NivelDificultate.Expert  => 3,
                _                        => 1
            };
        }
    }
}
