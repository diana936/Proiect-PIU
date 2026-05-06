using System;
using System.Threading.Tasks;
using NivelStocareDate;

namespace MeniuWPF
{
    public static class SunetJoc
    {
        private static bool Activ => SetariJoc.Instanta.SunetActivat;

        public static void BipMeniu()
        {
            if (!Activ) return;
            Task.Run(() => { try { Console.Beep(880, 60); } catch { } });
        }

        public static void BipMancare()
        {
            if (!Activ) return;
            Task.Run(() => {
                try {
                    Console.Beep(523, 60);
                    Console.Beep(659, 80);
                } catch { }
            });
        }

        public static void BipGameOver()
        {
            if (!Activ) return;
            Task.Run(() => {
                try {
                    Console.Beep(400, 120);
                    Console.Beep(300, 120);
                    Console.Beep(200, 200);
                } catch { }
            });
        }

        public static void BipCastigat()
        {
            if (!Activ) return;
            Task.Run(() => {
                try {
                    Console.Beep(523, 80);
                    Console.Beep(659, 80);
                    Console.Beep(784, 80);
                    Console.Beep(1047, 180);
                } catch { }
            });
        }

        public static void BipMutare()
        {
            if (!Activ) return;
            Task.Run(() => { try { Console.Beep(440, 40); } catch { } });
        }
    }
}
