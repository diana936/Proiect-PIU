namespace NivelStocareDate
{
    public class Rezolutie
    {
        public int Latime { get; private set; }
        public int Inaltime { get; private set; }

        public Rezolutie(int latime, int inaltime)
        {
            Latime = latime;
            Inaltime = inaltime;
        }

        public static Rezolutie Mica  => new Rezolutie(80, 24);
        public static Rezolutie Medie => new Rezolutie(120, 30);
        public static Rezolutie Mare  => new Rezolutie(160, 40);

        public override string ToString() => $"{Latime} x {Inaltime}";
    }
}
