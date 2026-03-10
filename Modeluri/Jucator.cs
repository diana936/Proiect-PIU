namespace Meniu.Modeluri
{
    public class Jucator
    {
        public string Nume { get; set; }
        public int Scor { get; set; }
        public int JocuriCastigate { get; set; }
        public int JocuriJucate { get; set; }

        public Jucator(string nume)
        {
            Nume = nume;
            Scor = 0;
            JocuriCastigate = 0;
            JocuriJucate = 0;
        }

        public void ResetareScor()
        {
            Scor = 0;
        }

        public override string ToString()
        {
            return $"{Nume} | Scor: {Scor} | Victorii: {JocuriCastigate}/{JocuriJucate}";
        }
    }
}
