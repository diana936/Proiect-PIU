using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LibrarieModele;
using MeniuWPF.Jocuri;
using NivelStocareDate;

namespace MeniuWPF.Views
{
    public partial class Connect4Page : Page
    {
        private Connect4ViewModel _vm;
        private const int CELL = 60;
        private Ellipse[,] _cells = new Ellipse[6, 7];

        private static readonly SolidColorBrush BrushJ1   = new(Color.FromRgb(255, 50,  50));
        private static readonly SolidColorBrush BrushJ2   = new(Color.FromRgb(255, 230,  0));
        private static readonly SolidColorBrush BrushBg   = new(Color.FromRgb(1,   8,   1));
        private static readonly SolidColorBrush BrushDrop = new(Color.FromRgb(0, 100, 25));

        public Connect4Page(bool vsCalculator)
        {
            InitializeComponent();
            _vm = new Connect4ViewModel(vsCalculator);
            var setari = SetariJoc.Instanta;
            TxtDif.Text = $"[ DIFICULTATE: {setari.Dificultate.ToString().ToUpper()} | AI DEPTH: {setari.AdancimeAI()} ]";
            Loaded += (s, e) => { BuildBoard(); ActualizeazaUI(); };
        }

        private void BuildBoard()
        {
            ColButtons.Children.Clear();
            BoardGrid.Children.Clear();
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();

            for (int c = 0; c < 7; c++)
            {
                int col = c;
                var btn = new Button
                {
                    Content         = "▼",
                    Width           = CELL,
                    Height          = 28,
                    Margin          = new Thickness(2, 0, 2, 0),
                    Background      = BrushDrop,
                    Foreground      = new SolidColorBrush(Color.FromRgb(0, 255, 65)),
                    FontFamily      = new FontFamily("Lucida Console, Courier New"),
                    FontSize        = 14,
                    FontWeight      = FontWeights.Bold,
                    BorderThickness = new Thickness(1),
                    BorderBrush     = new SolidColorBrush(Color.FromRgb(0, 255, 65)),
                    Cursor          = System.Windows.Input.Cursors.Hand,
                    Tag             = col
                };
                btn.Click += ColBtn_Click;
                ColButtons.Children.Add(btn);
            }

            for (int r = 0; r < 6; r++)
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CELL) });
            for (int c = 0; c < 7; c++)
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CELL) });

            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 7; c++)
                {
                    var el = new Ellipse
                    {
                        Width           = CELL - 10,
                        Height          = CELL - 10,
                        Fill            = BrushBg,
                        Stroke          = new SolidColorBrush(Color.FromRgb(0, 60, 15)),
                        StrokeThickness = 2,
                        Margin          = new Thickness(5)
                    };
                    Grid.SetRow(el, r);
                    Grid.SetColumn(el, c);
                    BoardGrid.Children.Add(el);
                    _cells[r, c] = el;
                }
        }

        private async void ColBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.JocTerminat || Overlay.Visibility == Visibility.Visible) return;
            int col = (int)((Button)sender).Tag;
            if (!_vm.MutareJucator(col)) return;
            SunetJoc.BipMutare();
            ActualizeazaUI();
            if (_vm.JocTerminat) { ArataMesajFinal(); return; }

            if (_vm.VsCalculator)
            {
                TxtStatus.Text = ">> CALCULATORUL GANDESTE... <<";
                int delay = SetariJoc.Instanta.AdancimeAI() switch { 0 => 200, 1 => 400, 2 => 700, _ => 1000 };
                await Task.Delay(delay);
                _vm.MutareCalculator();
                SunetJoc.BipMutare();
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
                    _cells[r, c].Fill = val == 1 ? BrushJ1 : val == 2 ? BrushJ2 : BrushBg;
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
                            new InregistrareJoc(nume, TipJoc.Connect4, 0, _vm.NrMutari, StatusJoc.Castigat));
                    });
            }
            else if (_vm.Castigator == "Calculator")
            {
                SunetJoc.BipGameOver();
                Overlay.ShowOk("JOC TERMINAT", "CALCULATORUL A CASTIGAT!");
            }
            else
            {
                SunetJoc.BipMeniu();
                GestiuneInregistrari.Instanta.Adauga(
                    new InregistrareJoc("REMIZA", TipJoc.Connect4, 0, _vm.NrMutari, StatusJoc.Remiza));
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
