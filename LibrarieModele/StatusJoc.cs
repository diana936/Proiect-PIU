using System;
namespace LibrarieModele
{
    [Flags]
    public enum StatusJoc
    {
        Nimic      = 0,
        Castigat   = 1,
        Pierdut    = 2,
        Remiza     = 4,
        Abandonat  = 8
    }
}
