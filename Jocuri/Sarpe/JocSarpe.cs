using Meniu.Modeluri;

namespace Meniu.Jocuri.Sarpe
{
    public class JocSarpe : BazaJoc
    {
        public override string Nume => "Snake";

        private CorpSarpe _sarpe;
        private Mancare _mancare;
        private Directie _directieCurenta;
        private int _latimeGrila;
        private int _inaltimeGrila;

        public JocSarpe(int latimeGrila = 20, int inaltimeGrila = 20)
        {
            _latimeGrila = latimeGrila;
            _inaltimeGrila = inaltimeGrila;
        }

        public override void Incepe()
        {
            base.Incepe();
            _sarpe = new CorpSarpe(new Pozitie(_inaltimeGrila / 2, _latimeGrila / 2));
            _mancare = new Mancare(new Pozitie(3, 3));
            _directieCurenta = Directie.Dreapta;
        }

        public void Actualizeaza()
        {
        }

        public void SchimbaDirectie(Directie directieNoua)
        {
            _directieCurenta = directieNoua;
        }

        private bool EsteInAfara()
        {
            return false;
        }

        private void GenereazaMancare()
        {
        }

        public override void Randeaza()
        {
        }
    }
}
