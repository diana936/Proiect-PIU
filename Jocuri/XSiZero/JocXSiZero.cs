using Meniu.Modeluri;

namespace Meniu.Jocuri.XSiZero
{
    public class JocXSiZero : BazaJoc
    {
        public override string Nume => "X si 0";

        private TablaXSiZero _tabla;
        private JucatorXSiZero _jucatorX;
        private JucatorXSiZero _jucatorO;
        private JucatorXSiZero _jucatorCurent;

        public JocXSiZero()
        {
            _jucatorX = new JucatorXSiZero("Jucator 1", StareCelula.X);
            _jucatorO = new JucatorXSiZero("Jucator 2", StareCelula.O);
        }

        public override void Incepe()
        {
            base.Incepe();
            _tabla = new TablaXSiZero();
            _jucatorCurent = _jucatorX;
        }

        public bool EfectueazaMutare(Pozitie poz)
        {
            return false;
        }

        private void SchimbaTurul()
        {
            _jucatorCurent = (_jucatorCurent == _jucatorX) ? _jucatorO : _jucatorX;
        }

        public JucatorXSiZero ObtineJucatorCurent() => _jucatorCurent;

        public override void Randeaza()
        {
        }
    }
}
