using Meniu.Modeluri;

namespace Meniu.Jocuri.Connect4
{
    public class JocConnect4 : BazaJoc
    {
        public override string Nume => "Connect 4";

        private TablaConnect4 _tabla;
        private JucatorConnect4 _jucator1;
        private JucatorConnect4 _jucator2;
        private JucatorConnect4 _jucatorCurent;

        public JocConnect4()
        {
            _jucator1 = new JucatorConnect4("Jucator 1", valoareJeton: 1, simbolJeton: 'R');
            _jucator2 = new JucatorConnect4("Jucator 2", valoareJeton: 2, simbolJeton: 'G');
        }

        public override void Incepe()
        {
            base.Incepe();
            _tabla = new TablaConnect4();
            _jucatorCurent = _jucator1;
        }

        public bool AruncaJeton(int coloana)
        {
            return false;
        }

        private void SchimbaTurul()
        {
            _jucatorCurent = (_jucatorCurent == _jucator1) ? _jucator2 : _jucator1;
        }

        public JucatorConnect4 ObtineJucatorCurent() => _jucatorCurent;

        public override void Randeaza()
        {
        }
    }
}
