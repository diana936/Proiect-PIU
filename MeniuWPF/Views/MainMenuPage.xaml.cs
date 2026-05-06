using System.Windows;
using System.Windows.Controls;

namespace MeniuWPF.Views
{
    public partial class MainMenuPage : Page
    {
        public MainMenuPage() { InitializeComponent(); }

        private void BtnSnake_Click(object sender, RoutedEventArgs e)
        { SunetJoc.BipMeniu(); MainWindow.Instance.NavigateTo(new SnakePage()); }

        private void BtnXSiZero_Click(object sender, RoutedEventArgs e)
        { SunetJoc.BipMeniu(); MainWindow.Instance.NavigateTo(new ModSelectPage("XSiZero")); }

        private void BtnConnect4_Click(object sender, RoutedEventArgs e)
        { SunetJoc.BipMeniu(); MainWindow.Instance.NavigateTo(new ModSelectPage("Connect4")); }

        private void BtnLeaderboard_Click(object sender, RoutedEventArgs e)
        { SunetJoc.BipMeniu(); MainWindow.Instance.NavigateTo(new LeaderboardPage()); }

        private void BtnJucatori_Click(object sender, RoutedEventArgs e)
        { SunetJoc.BipMeniu(); MainWindow.Instance.NavigateTo(new JucatoriPage()); }

        private void BtnSetari_Click(object sender, RoutedEventArgs e)
        { SunetJoc.BipMeniu(); MainWindow.Instance.NavigateTo(new SetariPage()); }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();
    }
}
