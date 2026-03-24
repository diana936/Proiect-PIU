using LibrarieModele;

namespace Meniu.Jocuri.Sarpe
{
    public class SegmentSarpe
    {
        public Pozitie Pozitie { get; set; }
        public bool EsteCap { get; set; }

        public SegmentSarpe(Pozitie pozitie, bool esteCap = false)
        {
            Pozitie = pozitie;
            EsteCap = esteCap;
        }
    }
}
