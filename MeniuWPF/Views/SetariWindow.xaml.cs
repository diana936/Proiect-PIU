using System.Windows;
using NivelStocareDate;

namespace MeniuWPF.Views
{
    public partial class SetariWindow : Window
    {
        private SetariJoc _setari = SetariJoc.Instanta;

        public SetariWindow()
        {
            InitializeComponent();
            SliderViteza.ValueChanged += (s, e) =>
                TxtViteza.Text = ((int)SliderViteza.Value).ToString();

            // Load current settings into controls
            SliderViteza.Value = _setari.VitezaJoc;
            ChkSunet.IsChecked = _setari.SunetActivat;
            CmbRezolutie.SelectedIndex =
                _setari.RezolutieCurenta.Latime == 80  ? 0 :
                _setari.RezolutieCurenta.Latime == 160 ? 2 : 1;
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            _setari.RezolutieCurenta = CmbRezolutie.SelectedIndex == 0 ? Rezolutie.Mica
                                     : CmbRezolutie.SelectedIndex == 2 ? Rezolutie.Mare
                                     : Rezolutie.Medie;
            _setari.VitezaJoc    = (int)SliderViteza.Value;
            _setari.SunetActivat = ChkSunet.IsChecked == true;
            _setari.Salveaza();
            MessageBox.Show("Setari salvate!", "Succes",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _setari.ResetareImplicite();
            SliderViteza.Value         = 3;
            ChkSunet.IsChecked         = false;
            CmbRezolutie.SelectedIndex = 1;
            MessageBox.Show("Setari resetate la valorile implicite!", "Reset",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
