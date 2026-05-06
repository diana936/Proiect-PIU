using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using LibrarieModele;
using NivelStocareDate;

namespace MeniuWPF.Views
{
    public partial class SnakePage : Page
    {
        private const int CELL = 20, COLS = 25, ROWS = 25;
        private LinkedList<(int r, int c)> _sarpe = new();
        private (int r, int c) _mancare;
        private (int dr, int dc) _directie   = (0, 0);
        private (int dr, int dc) _urmatoarea = (0, 1);
        private int  _scor  = 0;
        private bool _activ = false;
        private bool _pauza = false;
        private DispatcherTimer _timer = new();
        private Random _random = new Random();
        private Rectangle[,] _celule = new Rectangle[ROWS, COLS];

        private static readonly SolidColorBrush BrushCap  = new(Color.FromRgb(0, 255, 65));
        private static readonly SolidColorBrush BrushBody = new(Color.FromRgb(0, 160, 40));
        private static readonly SolidColorBrush BrushFood = new(Color.FromRgb(255, 50,  50));
        private static readonly SolidColorBrush BrushBg   = new(Color.FromRgb(1,   8,   1));

        public SnakePage()
        {
            InitializeComponent();
            Loaded += (s, e) => { BuildGrid(); InitGame(); Focus(); };
        }

        private void BuildGrid()
        {
            for (int r = 0; r < ROWS; r++)
                for (int c = 0; c < COLS; c++)
                {
                    var rect = new Rectangle { Width = CELL - 1, Height = CELL - 1, Fill = BrushBg };
                    Canvas.SetLeft(rect, c * CELL);
                    Canvas.SetTop(rect,  r * CELL);
                    GameCanvas.Children.Add(rect);
                    _celule[r, c] = rect;
                }
        }

        private void InitGame()
        {
            _sarpe.Clear();
            _sarpe.AddFirst((ROWS / 2, COLS / 2));
            _directie = (0, 0); _urmatoarea = (0, 1);
            _scor = 0; _activ = false; _pauza = false;
            GenereazaMancare();
            Randeaza();

            var setari = SetariJoc.Instanta;
            int ms = Math.Max(60, setari.VitezaSnakeMs());
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(ms) };
            _timer.Tick += Timer_Tick;

            TxtDif.Text    = $"[{setari.Dificultate.ToString().ToUpper()}]";
            TxtStatus.Text = "[ APASA O SAGEATA PENTRU START | ESC = PAUZA ]";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_pauza || !_activ) return;
            _directie = _urmatoarea;
            Actualizeaza();
            Randeaza();
        }

        private void Actualizeaza()
        {
            var cap = _sarpe.First!.Value;
            int nr = cap.r + _directie.dr, nc = cap.c + _directie.dc;
            if (nr < 0 || nr >= ROWS || nc < 0 || nc >= COLS) { GameOver(); return; }
            foreach (var seg in _sarpe)
                if (seg.r == nr && seg.c == nc) { GameOver(); return; }
            bool mancat = (nr == _mancare.r && nc == _mancare.c);
            _sarpe.AddFirst((nr, nc));
            if (mancat) { _scor += 10; SunetJoc.BipMancare(); GenereazaMancare(); }
            else _sarpe.RemoveLast();
        }

        private void Randeaza()
        {
            for (int r = 0; r < ROWS; r++)
                for (int c = 0; c < COLS; c++)
                    _celule[r, c].Fill = BrushBg;
            _celule[_mancare.r, _mancare.c].Fill = BrushFood;
            bool esteCap = true;
            foreach (var seg in _sarpe)
            {
                _celule[seg.r, seg.c].Fill = esteCap ? BrushCap : BrushBody;
                esteCap = false;
            }
            TxtScor.Text    = $"SCORE: {_scor}";
            TxtLungime.Text = $"LEN: {_sarpe.Count}";
        }

        private void GenereazaMancare()
        {
            int r, c;
            do { r = _random.Next(1, ROWS - 1); c = _random.Next(1, COLS - 1); }
            while (SarpeContine(r, c));
            _mancare = (r, c);
        }

        private bool SarpeContine(int r, int c)
        {
            foreach (var seg in _sarpe) if (seg.r == r && seg.c == c) return true;
            return false;
        }

        private void GameOver()
        {
            _activ = false;
            _timer.Stop();
            SunetJoc.BipGameOver();
            TxtStatus.Text = $">> GAME OVER! SCOR: {_scor} <<";

            Overlay.ShowPlayerPicker(
                "GAME OVER",
                $"SCORUL TAU: {_scor}\n\nSELECTEAZA SAU SCRIE NUMELE:",
                nume =>
                {
                    if (string.IsNullOrWhiteSpace(nume)) nume = "ANONIM";
                    GestiuneInregistrari.Instanta.Adauga(
                        new InregistrareJoc(nume, TipJoc.Snake, _scor, _sarpe.Count, StatusJoc.Pierdut));

                    Overlay.ShowYesNo("PLAY AGAIN?", "VREI SA JOCI DIN NOU?", playAgain =>
                    {
                        if (playAgain) InitGame();
                    });
                });
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible) return;
            switch (e.Key)
            {
                case Key.Up:    if (_directie != ( 1, 0)) _urmatoarea = (-1, 0); break;
                case Key.Down:  if (_directie != (-1, 0)) _urmatoarea = ( 1, 0); break;
                case Key.Left:  if (_directie != ( 0, 1)) _urmatoarea = ( 0,-1); break;
                case Key.Right: if (_directie != ( 0,-1)) _urmatoarea = ( 0, 1); break;
                case Key.Escape:
                    _pauza = !_pauza;
                    TxtStatus.Text = _pauza ? ">> PAUZA — ESC PENTRU CONTINUA <<" : "";
                    return;
            }
            if (!_activ && _directie == (0, 0))
            {
                _activ = true;
                _timer.Start();
                TxtStatus.Text = "";
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            SunetJoc.BipMeniu();
            MainWindow.Instance.NavigateTo(new MainMenuPage());
        }
    }
}
