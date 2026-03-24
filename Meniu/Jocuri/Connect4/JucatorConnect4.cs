using LibrarieModele;

namespace Meniu.Jocuri.Connect4
{
    public class JucatorConnect4 : Jucator
    {
        public int ValoareJeton { get; private set; }
        public char SimbolJeton { get; private set; }

        public JucatorConnect4(string nume, int valoareJeton, char simbolJeton) : base(nume)
        {
            ValoareJeton = valoareJeton;
            SimbolJeton = simbolJeton;
        }
    }
}
