using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using LibrarieModele;

namespace MeniuWPF.Views
{
    public partial class SnakeWindow : Window
    {
        // Grid settings
        private const int CELL   = 20;
        private const int COLS   = 25;
        private const int ROWS   = 25;

        // Game state
        private LinkedList<(int r, int c)> _sarpe = new();
        private (int r, int c) _mancare;
        private (int dr, int dc) _directie   = (0, 0);
        private (int dr, int dc) _urmatoarea = (0, 1); // initial direction: right
        private int  _scor    = 0;
        private bool _activ   = false;
        private bool _pauza   = false;

        private DispatcherTimer _timer;
        private Random _random = new Random();

        // Visuals
        private Rectangle[,] _celule = new Rectangle[ROWS, COLS];

        public SnakeWindow()
        {
            InitializeComponent();
            BuildGrid();
            InitGame();
        }

        private void BuildGrid()
        {
            for (int r = 0; r < ROWS; r++)
                for (int c = 0; c < COLS; c++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width  = CELL - 1,
                        Height = CELL - 1,
                        Fill   = Brushes.Transparent
                    };
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
            _directie   = (0, 0);
            _urmatoarea = (0, 1);
            _scor       = 0;
            _activ      = false;
            _pauza      = false;
            GenereazaMancare();
            Randeaza();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(150)
            };
            _timer.Tick += Timer_Tick;
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
            int nr = cap.r + _directie.dr;
            int nc = cap.c + _directie.dc;

            // Wall collision
            if (nr < 0 || nr >= ROWS || nc < 0 || nc >= COLS)
            { GameOver(); return; }

            // Self collision
            foreach (var seg in _sarpe)
                if (seg.r == nr && seg.c == nc)
                { GameOver(); return; }

            bool mancat = (nr == _mancare.r && nc == _mancare.c);
            _sarpe.AddFirst((nr, nc));
            if (mancat) { _scor += 10; GenereazaMancare(); }
            else        _sarpe.RemoveLast();
        }

        private void Randeaza()
        {
            // Clear all
            for (int r = 0; r < ROWS; r++)
                for (int c = 0; c < COLS; c++)
                    _celule[r, c].Fill = Brushes.Transparent;

            // Draw food
            _celule[_mancare.r, _mancare.c].Fill =
                new SolidColorBrush(Color.FromRgb(233, 69, 96));

            // Draw snake
            bool esteCap = true;
            foreach (var seg in _sarpe)
            {
                _celule[seg.r, seg.c].Fill = esteCap
                    ? new SolidColorBrush(Color.FromRgb(255, 255, 255))
                    : new SolidColorBrush(Color.FromRgb(78, 204, 163));
                esteCap = false;
            }

            TxtScor.Text    = $"Scor: {_scor}";
            TxtLungime.Text = $"Lungime: {_sarpe.Count}";
        }

        private void GenereazaMancare()
        {
            int r, c;
            do
            {
                r = _random.Next(1, ROWS - 1);
                c = _random.Next(1, COLS - 1);
            } while (SarpeContine(r, c));
            _mancare = (r, c);
        }

        private bool SarpeContine(int r, int c)
        {
            foreach (var seg in _sarpe)
                if (seg.r == r && seg.c == c) return true;
            return false;
        }

        private void GameOver()
        {
            _activ = false;
            _timer.Stop();
            TxtStatus.Text = $"Game Over! Scor final: {_scor}";

            // Save score
            string nume = Microsoft.VisualBasic.Interaction.InputBox(
                $"Game Over! Scorul tau: {_scor}\nIntrodu numele pentru leaderboard:",
                "Salveaza scor", "Jucator");
            if (string.IsNullOrWhiteSpace(nume)) nume = "Anonim";
            GestiuneInregistrari.Instanta.Adauga(
                new InregistrareJoc(nume, TipJoc.Snake, _scor, _sarpe.Count, StatusJoc.Pierdut));

            if (MessageBox.Show("Vrei sa joci din nou?", "Game Over",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                InitGame();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:    if (_directie != (1, 0))  _urmatoarea = (-1, 0); break;
                case Key.Down:  if (_directie != (-1, 0)) _urmatoarea = (1, 0);  break;
                case Key.Left:  if (_directie != (0, 1))  _urmatoarea = (0, -1); break;
                case Key.Right: if (_directie != (0, -1)) _urmatoarea = (0, 1);  break;
                case Key.Escape:
                    _pauza = !_pauza;
                    TxtStatus.Text = _pauza ? "PAUZA - apasa ESC pentru a continua" : "";
                    return;
            }

            // Start game on first arrow key press
            if (!_activ && _directie == (0, 0))
            {
                _activ = true;
                _timer.Start();
                TxtStatus.Text = "";
            }
        }
    }
}
