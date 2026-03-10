namespace Meniu.Modeluri
{
    public interface IJoc
    {
        string Nume { get; }
        bool EsteActiv { get; }

        void Incepe();
        void Opreste();
        void Reporneste();
        void Randeaza();
    }
}
