using System.Windows;
using System.Windows.Input;
using NivelStocareDate;

namespace MeniuWPF.Views
{
    public partial class MainWindow : Window
    {
        private static MainWindow? _instance;
        public static MainWindow Instance => _instance!;

        public MainWindow()
        {
            _instance = this;
            InitializeComponent();
            ApplyResolutionFromSettings();
            MainFrame.Navigate(new MainMenuPage());
        }

        public void NavigateTo(System.Windows.Controls.Page page)
        {
            MainFrame.Navigate(page);
        }

        public void ApplyResolutionFromSettings()
        {
            var rez = SetariJoc.Instanta.RezolutieCurenta;
            if (rez.Latime == 80)         { Width = 560;  Height = 560; }
            else if (rez.Latime == 160)   { Width = 900;  Height = 800; }
            else                          { Width = 700;  Height = 660; }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) ToggleMaximize();
            else DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
            => ToggleMaximize();

        private void BtnClose_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        private void ToggleMaximize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                BtnMaxRestore.Content = "□";
            }
            else
            {
                WindowState = WindowState.Maximized;
                BtnMaxRestore.Content = "❐";
            }
        }
    }
}
