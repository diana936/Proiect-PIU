using System.Windows;

namespace MeniuWPF.Views
{
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void BtnSnake_Click(object sender, RoutedEventArgs e)
        {
            new SnakeWindow().ShowDialog();
        }

        private void BtnXSiZero_Click(object sender, RoutedEventArgs e)
        {
            new ModSelectWindow("XSiZero").ShowDialog();
        }

        private void BtnConnect4_Click(object sender, RoutedEventArgs e)
        {
            new ModSelectWindow("Connect4").ShowDialog();
        }

        private void BtnLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            new LeaderboardWindow().ShowDialog();
        }

        private void BtnSetari_Click(object sender, RoutedEventArgs e)
        {
            new SetariWindow().ShowDialog();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
