using System.Windows;

namespace MeniuWPF.Views
{
    public partial class ModSelectWindow : Window
    {
        private readonly string _tipJoc;

        public ModSelectWindow(string tipJoc)
        {
            InitializeComponent();
            _tipJoc = tipJoc;
        }

        private void BtnDoi_Click(object sender, RoutedEventArgs e)
        {
            LanseazaJoc(vsCalculator: false);
        }

        private void BtnCalculator_Click(object sender, RoutedEventArgs e)
        {
            LanseazaJoc(vsCalculator: true);
        }

        private void LanseazaJoc(bool vsCalculator)
        {
            if (_tipJoc == "XSiZero")
                new XSiZeroWindow(vsCalculator).ShowDialog();
            else
                new Connect4Window(vsCalculator).ShowDialog();
            Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => Close();
    }
}
