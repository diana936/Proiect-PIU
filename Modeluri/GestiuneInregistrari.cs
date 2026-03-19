namespace Meniu.Modeluri
{
    public class GestiuneInregistrari
    {
        private InregistrareJoc[] _inregistrari;
        private int _numarCurent;

        public GestiuneInregistrari(int capacitateMaxima = 100)
        {
            _inregistrari = new InregistrareJoc[capacitateMaxima];
            _numarCurent = 0;
        }

        public bool Adauga(InregistrareJoc inregistrare)
        {
            if (_numarCurent >= _inregistrari.Length)
                return false;

            _inregistrari[_numarCurent] = inregistrare;
            _numarCurent++;
            return true;
        }

        public void AfiseazaToate()
        {
            if (_numarCurent == 0)
            {
                Console.WriteLine("Nu exista inregistrari.");
                return;
            }

            for (int i = 0; i < _numarCurent; i++)
                Console.WriteLine($"[{i + 1}] {_inregistrari[i]}");
        }

        public InregistrareJoc[] CautaDupaNumeJucator(string nume)
        {
            int gasit = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].NumeJucator.ToLower() == nume.ToLower())
                    gasit++;

            InregistrareJoc[] rezultate = new InregistrareJoc[gasit];
            int index = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].NumeJucator.ToLower() == nume.ToLower())
                {
                    rezultate[index] = _inregistrari[i];
                    index++;
                }

            return rezultate;
        }

        public InregistrareJoc[] CautaDupaTipJoc(string tipJoc)
        {
            int gasit = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].TipJoc.ToLower() == tipJoc.ToLower())
                    gasit++;

            InregistrareJoc[] rezultate = new InregistrareJoc[gasit];
            int index = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].TipJoc.ToLower() == tipJoc.ToLower())
                {
                    rezultate[index] = _inregistrari[i];
                    index++;
                }

            return rezultate;
        }

        public InregistrareJoc[] CautaDupaScorMinim(int scorMinim)
        {
            int gasit = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].Scor >= scorMinim)
                    gasit++;

            InregistrareJoc[] rezultate = new InregistrareJoc[gasit];
            int index = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].Scor >= scorMinim)
                {
                    rezultate[index] = _inregistrari[i];
                    index++;
                }

            return rezultate;
        }

        public InregistrareJoc[] CautaDoarVictorii()
        {
            int gasit = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].AcastigRunda)
                    gasit++;

            InregistrareJoc[] rezultate = new InregistrareJoc[gasit];
            int index = 0;
            for (int i = 0; i < _numarCurent; i++)
                if (_inregistrari[i].AcastigRunda)
                {
                    rezultate[index] = _inregistrari[i];
                    index++;
                }

            return rezultate;
        }

        public int NumarInregistrari() => _numarCurent;
    }
}
