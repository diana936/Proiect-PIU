using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;

namespace MeniuWPF.Views
{
    public partial class LeaderboardPage : Page
    {
        public LeaderboardPage()
        {
            InitializeComponent();
            IncarcaToate();
        }

        private void FlashStatus(string msg)
        {
            TxtStatus.Text = msg;
            var t = new System.Windows.Threading.DispatcherTimer
                { Interval = TimeSpan.FromSeconds(2) };
            t.Tick += (s, e) => { TxtStatus.Text = ""; t.Stop(); };
            t.Start();
        }

        private void IncarcaToate()
        {
            var toate = GestiuneInregistrari.Instanta.ObtineToate();
            toate.Sort((a, b) => b.Scor.CompareTo(a.Scor));
            AfiseazaLista(toate);
        }

        private void AfiseazaLista(List<InregistrareJoc> lista)
        {
            LstRecords.Items.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                var inreg = lista[i];
                string loc = i == 0 ? "#1" : i == 1 ? "#2" : i == 2 ? "#3" : $"#{i + 1}";
                LstRecords.Items.Add(new
                {
                    Loc    = loc,
                    Nume   = inreg.NumeJucator,
                    Joc    = inreg.TipulJocului.ToString(),
                    Scor   = inreg.Scor,
                    Mutari = inreg.NrMutari,
                    Status = inreg.Status.ToString()
                });
            }
        }

        private void BtnToate_Click(object sender, RoutedEventArgs e) => IncarcaToate();
        private void BtnSnake_Click(object sender, RoutedEventArgs e)
            => AfiseazaLista(GestiuneInregistrari.Instanta.ObtineLeaderboard(TipJoc.Snake));
        private void BtnXSiZero_Click(object sender, RoutedEventArgs e)
            => AfiseazaLista(GestiuneInregistrari.Instanta.ObtineLeaderboard(TipJoc.XSiZero));
        private void BtnConnect4_Click(object sender, RoutedEventArgs e)
            => AfiseazaLista(GestiuneInregistrari.Instanta.ObtineLeaderboard(TipJoc.Connect4));

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            string termen = TxtCautare.Text.Trim();
            if (string.IsNullOrEmpty(termen)) { IncarcaToate(); return; }
            AfiseazaLista(GestiuneInregistrari.Instanta.CautaDupaNumeJucator(termen));
        }

        private void BtnModifica_Click(object sender, RoutedEventArgs e)
        {
            Overlay.ShowInput("MODIFICA SCOR", "NUMELE JUCATORULUI:", "", nume =>
            {
                if (string.IsNullOrWhiteSpace(nume)) return;
                Overlay.ShowInput("MODIFICA SCOR", "TIPUL JOCULUI:\n(SNAKE / XSIZERO / CONNECT4)", "", tipStr =>
                {
                    if (!Enum.TryParse<TipJoc>(tipStr, true, out TipJoc tip))
                    { FlashStatus(">> TIP JOC INVALID <<"); return; }
                    Overlay.ShowInput("MODIFICA SCOR", "SCORUL NOU:", "0", scorStr =>
                    {
                        if (!int.TryParse(scorStr, out int scor))
                        { FlashStatus(">> SCOR INVALID <<"); return; }
                        bool ok = GestiuneInregistrari.Instanta.ModificaScor(nume, tip, scor);
                        FlashStatus(ok ? ">> SCOR ACTUALIZAT! <<" : ">> INREGISTRAREA NU A FOST GASITA <<");
                        IncarcaToate();
                    });
                });
            });
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            Overlay.ShowInput("STERGE", "NUMELE JUCATORULUI:", "", nume =>
            {
                if (string.IsNullOrWhiteSpace(nume)) return;
                Overlay.ShowInput("STERGE", "TIPUL JOCULUI:\n(SNAKE / XSIZERO / CONNECT4)", "", tipStr =>
                {
                    if (!Enum.TryParse<TipJoc>(tipStr, true, out TipJoc tip))
                    { FlashStatus(">> TIP JOC INVALID <<"); return; }
                    Overlay.ShowYesNo("CONFIRMARE", $"STERGI {nume.ToUpper()} DIN {tip}?", confirmed =>
                    {
                        if (!confirmed) return;
                        bool ok = GestiuneInregistrari.Instanta.Sterge(nume, tip);
                        FlashStatus(ok ? ">> STERS! <<" : ">> NU A FOST GASIT <<");
                        IncarcaToate();
                    });
                });
            });
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            SunetJoc.BipMeniu();
            MainWindow.Instance.NavigateTo(new MainMenuPage());
        }
    }
}
