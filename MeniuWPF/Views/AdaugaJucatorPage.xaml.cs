using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibrarieModele;

namespace MeniuWPF.Views
{
    public partial class AdaugaJucatorPage : Page
    {
        private const int LUNGIME_MINIMA_NUME = 2;
        private const int LUNGIME_MAXIMA_NUME = 20;

        public event Action<Jucator>? OnSaved;
        public event Action? OnCancelled;

        private Jucator? _jucatorDeEditat;

        public AdaugaJucatorPage()
        {
            InitializeComponent();
        }

        public AdaugaJucatorPage(Jucator jucatorDeEditat)
        {
            InitializeComponent();
            _jucatorDeEditat = jucatorDeEditat;
            txtTitlu.Text = "Editeaza Jucator";
            IncarcaDateJucator();
        }

        private void IncarcaDateJucator()
        {
            if (_jucatorDeEditat == null) return;
            txtNume.Text = _jucatorDeEditat.Nume;
            switch (_jucatorDeEditat.Dificultate)
            {
                case NivelDificultate.Usor:    rbUsor.IsChecked    = true; break;
                case NivelDificultate.Mediu:   rbMediu.IsChecked   = true; break;
                case NivelDificultate.Dificil: rbDificil.IsChecked = true; break;
                case NivelDificultate.Expert:  rbExpert.IsChecked  = true; break;
            }
            chkSunet.IsChecked     = _jucatorDeEditat.Optiuni.HasFlag(OptiuniJucator.SunetActivat);
            chkMuzica.IsChecked    = _jucatorDeEditat.Optiuni.HasFlag(OptiuniJucator.MuzicaActivata);
            chkAnimatii.IsChecked  = _jucatorDeEditat.Optiuni.HasFlag(OptiuniJucator.AnimatiiActive);
            chkIntuneric.IsChecked = _jucatorDeEditat.Optiuni.HasFlag(OptiuniJucator.ModIntuneric);
        }

        private bool Valideaza()
        {
            bool valid = true;
            lblNume.Foreground        = Brushes.White;
            lblDificultate.Foreground = Brushes.White;
            errNume.Visibility        = Visibility.Collapsed;
            errScor.Visibility        = Visibility.Collapsed;

            string nume = txtNume.Text.Trim();
            if (string.IsNullOrWhiteSpace(nume) ||
                nume.Length < LUNGIME_MINIMA_NUME ||
                nume.Length > LUNGIME_MAXIMA_NUME)
            {
                lblNume.Foreground = Brushes.OrangeRed;
                errNume.Text = $"Numele trebuie sa aiba intre {LUNGIME_MINIMA_NUME} si {LUNGIME_MAXIMA_NUME} caractere.";
                errNume.Visibility = Visibility.Visible;
                valid = false;
            }
            if (lstScor.SelectedItem == null)
            {
                errScor.Text = "Selecteaza un scor initial din lista.";
                errScor.Visibility = Visibility.Visible;
                valid = false;
            }
            return valid;
        }

        private NivelDificultate ObtineDificultate()
        {
            if (rbMediu.IsChecked   == true) return NivelDificultate.Mediu;
            if (rbDificil.IsChecked == true) return NivelDificultate.Dificil;
            if (rbExpert.IsChecked  == true) return NivelDificultate.Expert;
            return NivelDificultate.Usor;
        }

        private OptiuniJucator ObtineOptiuni()
        {
            OptiuniJucator optiuni = OptiuniJucator.Nimic;
            if (chkSunet.IsChecked     == true) optiuni |= OptiuniJucator.SunetActivat;
            if (chkMuzica.IsChecked    == true) optiuni |= OptiuniJucator.MuzicaActivata;
            if (chkAnimatii.IsChecked  == true) optiuni |= OptiuniJucator.AnimatiiActive;
            if (chkIntuneric.IsChecked == true) optiuni |= OptiuniJucator.ModIntuneric;
            return optiuni;
        }

        private void btnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (!Valideaza()) return;
            string nume = txtNume.Text.Trim();
            int scor = int.Parse(((ListBoxItem)lstScor.SelectedItem).Content.ToString()!);
            Jucator rezultat;
            if (_jucatorDeEditat != null)
            {
                _jucatorDeEditat.Nume        = nume;
                _jucatorDeEditat.Dificultate = ObtineDificultate();
                _jucatorDeEditat.Optiuni     = ObtineOptiuni();
                _jucatorDeEditat.Scor        = scor;
                rezultat = _jucatorDeEditat;
            }
            else
            {
                rezultat = new Jucator(nume)
                {
                    Dificultate = ObtineDificultate(),
                    Optiuni     = ObtineOptiuni(),
                    Scor        = scor
                };
            }
            OnSaved?.Invoke(rezultat);
        }

        private void btnAnuleaza_Click(object sender, RoutedEventArgs e)
            => OnCancelled?.Invoke();
    }
}
