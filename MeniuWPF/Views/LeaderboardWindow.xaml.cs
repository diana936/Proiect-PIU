using System.Collections.Generic;
using System.Windows;
using LibrarieModele;

namespace MeniuWPF.Views
{
    public partial class LeaderboardWindow : Window
    {
        private TipJoc? _filtruCurent = null;

        public LeaderboardWindow()
        {
            InitializeComponent();
            IncarcaToate();
        }

        private void IncarcaToate()
        {
            _filtruCurent = null;
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
                string loc = i == 0 ? "🥇" : i == 1 ? "🥈" : i == 2 ? "🥉" : $"#{i + 1}";
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
        {
            _filtruCurent = TipJoc.Snake;
            AfiseazaLista(GestiuneInregistrari.Instanta.ObtineLeaderboard(TipJoc.Snake));
        }

        private void BtnXSiZero_Click(object sender, RoutedEventArgs e)
        {
            _filtruCurent = TipJoc.XSiZero;
            AfiseazaLista(GestiuneInregistrari.Instanta.ObtineLeaderboard(TipJoc.XSiZero));
        }

        private void BtnConnect4_Click(object sender, RoutedEventArgs e)
        {
            _filtruCurent = TipJoc.Connect4;
            AfiseazaLista(GestiuneInregistrari.Instanta.ObtineLeaderboard(TipJoc.Connect4));
        }

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            string termen = TxtCautare.Text.Trim();
            if (string.IsNullOrEmpty(termen)) { IncarcaToate(); return; }
            AfiseazaLista(GestiuneInregistrari.Instanta.CautaDupaNumeJucator(termen));
        }

        private void BtnModifica_Click(object sender, RoutedEventArgs e)
        {
            string nume = Microsoft.VisualBasic.Interaction.InputBox(
                "Numele jucatorului de modificat:", "Modifica Scor");
            if (string.IsNullOrWhiteSpace(nume)) return;

            string tipStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Tipul jocului (Snake / XSiZero / Connect4):", "Modifica Scor");
            if (!System.Enum.TryParse<TipJoc>(tipStr, out TipJoc tip)) {
                MessageBox.Show("Tip joc invalid."); return; }

            string scorStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Scorul nou:", "Modifica Scor");
            if (!int.TryParse(scorStr, out int scor)) {
                MessageBox.Show("Scor invalid."); return; }

            bool ok = GestiuneInregistrari.Instanta.ModificaScor(nume, tip, scor);
            MessageBox.Show(ok ? "Scor actualizat!" : "Inregistrarea nu a fost gasita.");
            IncarcaToate();
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            string nume = Microsoft.VisualBasic.Interaction.InputBox(
                "Numele jucatorului de sters:", "Sterge Inregistrare");
            if (string.IsNullOrWhiteSpace(nume)) return;

            string tipStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Tipul jocului (Snake / XSiZero / Connect4):", "Sterge");
            if (!System.Enum.TryParse<TipJoc>(tipStr, out TipJoc tip)) {
                MessageBox.Show("Tip joc invalid."); return; }

            bool ok = GestiuneInregistrari.Instanta.Sterge(nume, tip);
            MessageBox.Show(ok ? "Inregistrare stearsa!" : "Nu a fost gasita.");
            IncarcaToate();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
