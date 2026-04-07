using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LibrarieModele;
using MeniuWPF.Jocuri;

namespace MeniuWPF.Views
{
    public partial class Connect4Window : Window
    {
        private Connect4ViewModel _vm;
        private const int CELL = 64;
        private Ellipse[,] _cells = new Ellipse[6, 7];

        public Connect4Window(bool vsCalculator)
        {
            InitializeComponent();
            _vm = new Connect4ViewModel(vsCalculator);
            BuildBoard();
            ActualizeazaUI();
        }

        private void BuildBoard()
        {
            // Column drop buttons
            for (int c = 0; c < 7; c++)
            {
                int col = c;
                Button btn = new Button
                {
                    Content = "▼",
                    Width   = CELL,
                    Height  = 32,
                    Margin  = new Thickness(2),
                    Tag     = col,
                    Style   = (Style)FindResource("SmallButton")
                };
                btn.Click += ColBtn_Click;
                ColButtons.Children.Add(btn);
            }

            // Board grid
            for (int r = 0; r < 6; r++)
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CELL) });
            for (int c = 0; c < 7; c++)
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CELL) });

            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 7; c++)
                {
                    Ellipse el = new Ellipse
                    {
                        Width  = CELL - 10,
                        Height = CELL - 10,
                        Fill   = new SolidColorBrush(Color.FromRgb(30, 30, 60)),
                        Margin = new Thickness(5)
                    };
                    Grid.SetRow(el, r);
                    Grid.SetColumn(el, c);
                    BoardGrid.Children.Add(el);
                    _cells[r, c] = el;
                }
        }

        private async void ColBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.JocTerminat) return;
            int col = (int)((Button)sender).Tag;

            bool ok = _vm.MutareJucator(col);
            if (!ok) return;

            ActualizeazaUI();
            if (_vm.JocTerminat) { ArataMesajFinal(); return; }

            if (_vm.VsCalculator)
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
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 7; c++)
                {
                    int val = _vm.ObtineCelula(r, c);
                    _cells[r, c].Fill = val == 1
                        ? new SolidColorBrush(Color.FromRgb(233, 69, 96))   // red player 1
                        : val == 2
                            ? new SolidColorBrush(Color.FromRgb(245, 166, 35)) // yellow player 2
                            : new SolidColorBrush(Color.FromRgb(30, 30, 60));  // empty
                }
            TxtStatus.Text = _vm.StatusText;
        }

        private void ArataMesajFinal()
        {
            ActualizeazaUI();
            if (_vm.Castigator != null && _vm.Castigator != "Calculator")
            {
                string nume = Microsoft.VisualBasic.Interaction.InputBox(
                    "Introdu numele tau:", "Salveaza scor", _vm.Castigator);
                if (string.IsNullOrWhiteSpace(nume)) nume = "Anonim";
                GestiuneInregistrari.Instanta.Adauga(
                    new InregistrareJoc(nume, TipJoc.Connect4, 0, _vm.NrMutari, StatusJoc.Castigat));
            }
            else if (_vm.Castigator == null)
            {
                GestiuneInregistrari.Instanta.Adauga(
                    new InregistrareJoc("Remiza", TipJoc.Connect4, 0, _vm.NrMutari, StatusJoc.Remiza));
            }
            MessageBox.Show(_vm.StatusText, "Joc terminat", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            _vm.Restart();
            ActualizeazaUI();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => Close();
    }
}
