using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;

namespace MeniuWPF.Views
{
    public partial class JucatoriPage : Page
    {

        private List<Jucator> _jucatori => RegistruJucatori.Jucatori;

        public JucatoriPage()
        {
            InitializeComponent();
            SincronizeazaDinLeaderboard();
            RefreshLista();
        }

        private void SincronizeazaDinLeaderboard()
        {
            var existente = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var j in _jucatori) existente.Add(j.Nume);

            foreach (var inreg in GestiuneInregistrari.Instanta.ObtineToate())
            {
                string nume = inreg.NumeJucator?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(nume)) continue;
                if (nume.Equals("ANONIM", StringComparison.OrdinalIgnoreCase)) continue;
                if (nume.Equals("REMIZA", StringComparison.OrdinalIgnoreCase)) continue;
                if (existente.Contains(nume)) continue;

                var j = new Jucator(nume)
                {
                    Scor        = inreg.Scor,
                    Dificultate = NivelDificultate.Mediu
                };
                _jucatori.Add(j);
                existente.Add(nume);
            }
        }

        private void RefreshLista()
        {
            lstJucatori.Items.Clear();
            foreach (Jucator j in _jucatori)
                lstJucatori.Items.Add(j.ToString());
        }

        private void FlashStatus(string msg)
        {
            TxtStatus.Text = msg;
            var t = new System.Windows.Threading.DispatcherTimer
                { Interval = TimeSpan.FromSeconds(2) };
            t.Tick += (s, e) => { TxtStatus.Text = ""; t.Stop(); };
            t.Start();
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            var page = new AdaugaJucatorPage();
            page.OnSaved += (jucator) =>
            {
                _jucatori.Add(jucator);
                RefreshLista();
                MainWindow.Instance.NavigateTo(this);
                FlashStatus($">> {jucator.Nume} ADAUGAT! <<");
            };
            page.OnCancelled += () => MainWindow.Instance.NavigateTo(this);
            MainWindow.Instance.NavigateTo(page);
        }

        private void btnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            int index = lstJucatori.SelectedIndex;
            if (index < 0)
            {
                Overlay.ShowOk("ATENTIE", "SELECTEAZA UN JUCATOR DIN LISTA PENTRU A-L EDITA.");
                return;
            }
            var page = new AdaugaJucatorPage(_jucatori[index]);
            page.OnSaved += (jucator) =>
            {
                _jucatori[index] = jucator;
                RefreshLista();
                MainWindow.Instance.NavigateTo(this);
                FlashStatus($">> {jucator.Nume} ACTUALIZAT! <<");
            };
            page.OnCancelled += () => MainWindow.Instance.NavigateTo(this);
            MainWindow.Instance.NavigateTo(page);
        }

        private void btnInapoi_Click(object sender, RoutedEventArgs e)
        {
            SunetJoc.BipMeniu();
            MainWindow.Instance.NavigateTo(new MainMenuPage());
        }
    }
}
