using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibrarieModele;
using MeniuWPF.Jocuri;
using NivelStocareDate;

namespace MeniuWPF.Views
{
    public partial class XSiZeroPage : Page
    {
        private XSiZeroViewModel _vm;

        private static readonly SolidColorBrush BrushX     = new(Color.FromRgb(255, 50, 50));
        private static readonly SolidColorBrush BrushO     = new(Color.FromRgb(0, 255, 65));
        private static readonly SolidColorBrush BrushEmpty = new(Color.FromRgb(0, 180, 45));

        public XSiZeroPage(bool vsCalculator)
        {
            InitializeComponent();
            _vm = new XSiZeroViewModel(vsCalculator);
            var setari = SetariJoc.Instanta;
            TxtDif.Text = $"[ DIFICULTATE: {setari.Dificultate.ToString().ToUpper()} | AI DEPTH: {setari.AdancimeAI()} ]";
            ActualizeazaUI();
        }

        private async void Cell_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.JocTerminat || Overlay.Visibility == Visibility.Visible) return;
            Button btn = (Button)sender;
            string[] parts = btn.Tag.ToString()!.Split(',');
            int rand = int.Parse(parts[0]), col = int.Parse(parts[1]);
            if (!_vm.MutareJucator(rand, col)) return;
            SunetJoc.BipMutare();
            ActualizeazaUI();
            if (_vm.JocTerminat) { ArataMesajFinal(); return; }

            if (_vm.VsCalculator)
            {
                TxtStatus.Text = ">> CALCULATORUL GANDESTE... <<";
                int delay = SetariJoc.Instanta.AdancimeAI() switch { 0 => 200, 1 => 400, 2 => 600, _ => 900 };
                await Task.Delay(delay);
                _vm.MutareCalculator();
                SunetJoc.BipMutare();
                ActualizeazaUI();
                if (_vm.JocTerminat) ArataMesajFinal();
            }
        }

        private void ActualizeazaUI()
        {
            foreach (Button btn in GameGrid.Children.OfType<Button>())
            {
                string[] parts = btn.Tag.ToString()!.Split(',');
                string simbol = _vm.ObtineSimbol(int.Parse(parts[0]), int.Parse(parts[1]));
                btn.Content    = simbol;
                btn.Foreground = simbol == "X" ? BrushX : simbol == "O" ? BrushO : BrushEmpty;
            }
            TxtStatus.Text = _vm.StatusText.ToUpper();
        }

        private void ArataMesajFinal()
        {
            ActualizeazaUI();
            if (_vm.Castigator != null && _vm.Castigator != "Calculator")
            {
                SunetJoc.BipCastigat();
                Overlay.ShowPlayerPicker(
                    "VICTORIE!",
                    $"{_vm.StatusText.ToUpper()}\n\nSELECTEAZA SAU SCRIE NUMELE:",
                    nume =>
                    {
                        if (string.IsNullOrWhiteSpace(nume)) nume = "ANONIM";
                        GestiuneInregistrari.Instanta.Adauga(
                            new InregistrareJoc(nume, TipJoc.XSiZero, 0, _vm.NrMutari, StatusJoc.Castigat));
                    });
            }
            else if (_vm.Castigator == "Calculator")
            {
                SunetJoc.BipGameOver();
                Overlay.ShowOk("JOC TERMINAT", _vm.StatusText.ToUpper());
            }
            else
            {
                SunetJoc.BipMeniu();
                Overlay.ShowOk("JOC TERMINAT", "REMIZA!");
            }
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            SunetJoc.BipMeniu();
            _vm.Restart();
            ActualizeazaUI();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            SunetJoc.BipMeniu();
            MainWindow.Instance.NavigateTo(new MainMenuPage());
        }
    }
}
