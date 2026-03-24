using System;
namespace LibrarieModele
{
    [Flags]
    public enum OptiuniJucator
    {
        Nimic         = 0,
        SunetActivat  = 1,
        MuzicaActivata = 2,
        AnimatiiActive = 4,
        ModIntuneric  = 8
    }
}
