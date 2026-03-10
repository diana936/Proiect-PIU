using Meniu.Modeluri;
using System.Collections.Generic;

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
        }

        public bool SeIntersecteazaCuSine()
        {
            return false;
        }

        public List<SegmentSarpe> ObtineSegmente()
        {
            return _segmente;
        }
    }
}
