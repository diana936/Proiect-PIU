using System.Windows;
using System.Windows.Controls;
using LibrarieModele;
using NivelStocareDate;

namespace MeniuWPF.Views
{
    public partial class SetariPage : Page
    {
        private SetariJoc _setari = SetariJoc.Instanta;

        public SetariPage()
        {
            InitializeComponent();
            SliderViteza.ValueChanged += (s, e) =>
                TxtViteza.Text = ((int)SliderViteza.Value).ToString();

            SliderViteza.Value = _setari.VitezaJoc;
            ChkSunet.IsChecked = _setari.SunetActivat;
            RbMic.IsChecked    = _setari.RezolutieCurenta.Latime == 80;
            RbMediu.IsChecked  = _setari.RezolutieCurenta.Latime == 120;
            RbMare.IsChecked   = _setari.RezolutieCurenta.Latime == 160;
            DifUsor.IsChecked    = _setari.Dificultate == NivelDificultate.Usor;
            DifMediu.IsChecked   = _setari.Dificultate == NivelDificultate.Mediu;
            DifDificil.IsChecked = _setari.Dificultate == NivelDificultate.Dificil;
            DifExpert.IsChecked  = _setari.Dificultate == NivelDificultate.Expert;
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (RbMic.IsChecked == true)       _setari.RezolutieCurenta = Rezolutie.Mica;
            else if (RbMare.IsChecked == true)  _setari.RezolutieCurenta = Rezolutie.Mare;
            else                                _setari.RezolutieCurenta = Rezolutie.Medie;

            _setari.VitezaJoc    = (int)SliderViteza.Value;
            _setari.SunetActivat = ChkSunet.IsChecked == true;

            if      (DifUsor.IsChecked    == true) _setari.Dificultate = NivelDificultate.Usor;
            else if (DifDificil.IsChecked == true) _setari.Dificultate = NivelDificultate.Dificil;
            else if (DifExpert.IsChecked  == true) _setari.Dificultate = NivelDificultate.Expert;
            else                                   _setari.Dificultate = NivelDificultate.Mediu;

            _setari.Salveaza();
            MainWindow.Instance.ApplyResolutionFromSettings();
            SunetJoc.BipMeniu();

            var orig = TxtViteza.Text;
            TxtViteza.Text = "OK!";
            var t = new System.Windows.Threading.DispatcherTimer
                { Interval = System.TimeSpan.FromSeconds(1) };
            t.Tick += (s2, e2) => { TxtViteza.Text = orig; t.Stop(); };
            t.Start();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _setari.ResetareImplicite();
            SliderViteza.Value   = 3;
            ChkSunet.IsChecked   = false;
            RbMediu.IsChecked    = true;
            DifMediu.IsChecked   = true;
            MainWindow.Instance.ApplyResolutionFromSettings();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            SunetJoc.BipMeniu();
            MainWindow.Instance.NavigateTo(new MainMenuPage());
        }
    }
}
