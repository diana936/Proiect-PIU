using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibrarieModele;
using MeniuWPF.Jocuri;

namespace MeniuWPF.Views
{
    public partial class XSiZeroWindow : Window
    {
        private XSiZeroViewModel _vm;

        public XSiZeroWindow(bool vsCalculator)
        {
            InitializeComponent();
            _vm = new XSiZeroViewModel(vsCalculator);
            ActualizeazaUI();
        }

        private async void Cell_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.JocTerminat) return;

            Button btn = (Button)sender;
            string[] parts = btn.Tag.ToString().Split(',');
            int rand = int.Parse(parts[0]);
            int col  = int.Parse(parts[1]);

            bool mutat = _vm.MutareJucator(rand, col);
            if (!mutat) return;

            ActualizeazaUI();
            if (_vm.JocTerminat) { ArataMesajFinal(); return; }

            // Computer turn
            if (_vm.VsCalculator && !_vm.JocTerminat)
            {
                TxtStatus.Text = "Calculatorul se gandeste...";
                await Task.Delay(600);
                _vm.MutareCalculator();
                ActualizeazaUI();
                if (_vm.JocTerminat) ArataMesajFinal();
            }
        }

        private void ActualizeazaUI()
        {
            // Update each cell button
            foreach (Button btn in GameGrid.Children.OfType<Button>())
            {
                string[] parts = btn.Tag.ToString().Split(',');
                int rand = int.Parse(parts[0]);
                int col  = int.Parse(parts[1]);

                string simbol = _vm.ObtineSimbol(rand, col);
                btn.Content    = simbol;
                btn.Foreground = simbol == "X"
                    ? new SolidColorBrush(Color.FromRgb(233, 69, 96))   // red for X
                    : simbol == "O"
                        ? new SolidColorBrush(Color.FromRgb(78, 204, 163)) // green for O
                        : new SolidColorBrush(Color.FromRgb(234, 234, 234));
            }

            TxtStatus.Text = _vm.StatusText;
        }

        private void ArataMesajFinal()
        {
            ActualizeazaUI();
            if (_vm.Castigator != null)
                SalveazaScor(_vm.Castigator, StatusJoc.Castigat);
            else
                SalveazaScor("Remiza", StatusJoc.Remiza);

            MessageBox.Show(_vm.StatusText, "Joc terminat",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SalveazaScor(string numeJucator, StatusJoc status)
        {
            if (numeJucator == "Remiza" || numeJucator == "Calculator") return;
            string nume = Microsoft.VisualBasic.Interaction.InputBox(
                "Introdu numele tau pentru leaderboard:", "Salveaza scor", numeJucator);
            if (string.IsNullOrWhiteSpace(nume)) nume = "Anonim";
            var inreg = new InregistrareJoc(nume, TipJoc.XSiZero, 0, _vm.NrMutari, status);
            GestiuneInregistrari.Instanta.Adauga(inreg);
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            _vm.Restart();
            ActualizeazaUI();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => Close();
    }
}
