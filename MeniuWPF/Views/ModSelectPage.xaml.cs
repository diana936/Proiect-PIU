using System.Windows;
using System.Windows.Controls;

namespace MeniuWPF.Views
{
    public partial class ModSelectPage : Page
    {
        private readonly string _tipJoc;

        public ModSelectPage(string tipJoc)
        {
            InitializeComponent();
            _tipJoc = tipJoc;
            TxtJoc.Text = $"[ {tipJoc.ToUpper()} ]";
        }

        private void BtnDoi_Click(object sender, RoutedEventArgs e) { SunetJoc.BipMeniu(); LanseazaJoc(false); }
        private void BtnCalculator_Click(object sender, RoutedEventArgs e) { SunetJoc.BipMeniu(); LanseazaJoc(true); }

        private void LanseazaJoc(bool vsCalculator)
        {
            if (_tipJoc == "XSiZero")
                MainWindow.Instance.NavigateTo(new XSiZeroPage(vsCalculator));
            else
                MainWindow.Instance.NavigateTo(new Connect4Page(vsCalculator));
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            SunetJoc.BipMeniu();
            MainWindow.Instance.NavigateTo(new MainMenuPage());
        }
    }
}
