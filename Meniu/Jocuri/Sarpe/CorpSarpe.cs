using System;
using System.Collections.Generic;
using LibrarieModele;

namespace Meniu.Jocuri.Sarpe
{
    public class CorpSarpe
    {
        private List<SegmentSarpe> _segmente;

        public SegmentSarpe Cap => _segmente[0];
        public int Lungime => _segmente.Count;

        public CorpSarpe(Pozitie pozitieInitiala)
        {
            _segmente = new List<SegmentSarpe>
            {
                new SegmentSarpe(pozitieInitiala, esteCap: true)
            };
        }

        public void Muta(Directie directie, bool creste = false)
        {
            Pozitie pozitieVeche = Cap.Pozitie;
            Pozitie pozitieNoua;

            switch (directie)
            {
                case Directie.Sus:
                    pozitieNoua = new Pozitie(pozitieVeche.Rand - 1, pozitieVeche.Coloana);
                    break;
                case Directie.Jos:
                    pozitieNoua = new Pozitie(pozitieVeche.Rand + 1, pozitieVeche.Coloana);
                    break;
                case Directie.Stanga:
                    pozitieNoua = new Pozitie(pozitieVeche.Rand, pozitieVeche.Coloana - 1);
                    break;
                default:
                    pozitieNoua = new Pozitie(pozitieVeche.Rand, pozitieVeche.Coloana + 1);
                    break;
            }

            _segmente[0].EsteCap = false;
            _segmente.Insert(0, new SegmentSarpe(pozitieNoua, esteCap: true));

            if (!creste)
                _segmente.RemoveAt(_segmente.Count - 1);
        }

        public bool SeIntersecteazaCuSine()
        {
            for (int i = 1; i < _segmente.Count; i++)
                if (_segmente[i].Pozitie.EsteEgalaCu(_segmente[0].Pozitie))
                    return true;
            return false;
        }

        public List<SegmentSarpe> ObtineSegmente()
        {
            return _segmente;
        }
    }
}