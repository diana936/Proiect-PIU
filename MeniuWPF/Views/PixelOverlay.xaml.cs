using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MeniuWPF.Views
{
    public partial class PixelOverlay : UserControl
    {
        private Action<bool>?   _yesNoCallback;
        private Action<string>? _inputCallback;
        private Action?         _okCallback;

        public PixelOverlay() { InitializeComponent(); }

        public void ShowOk(string title, string message, Action? onOk = null)
        {
            Setup(title, message, showPicker: false, showInput: false);
            _okCallback = onOk;
            BuildButtons("ok");
            Visibility = Visibility.Visible;
        }

        public void ShowYesNo(string title, string message, Action<bool> callback)
        {
            Setup(title, message, showPicker: false, showInput: false);
            _yesNoCallback = callback;
            BuildButtons("yesno");
            Visibility = Visibility.Visible;
        }

        public void ShowInput(string title, string message, string defaultValue, Action<string> callback)
        {
            Setup(title, message, showPicker: false, showInput: true);
            TxtInput.Text  = defaultValue;
            _inputCallback = callback;
            BuildButtons("input");
            Visibility = Visibility.Visible;
            TxtInput.Focus();
            TxtInput.SelectAll();
        }

        public void ShowPlayerPicker(string title, string message, Action<string> callback)
        {
            Setup(title, message, showPicker: true, showInput: true);
            TxtInput.Text  = "";
            _inputCallback = callback;

            ProfileList.Children.Clear();
            var todasNume = RegistruJucatori.ToateNumele();

            if (todasNume.Count == 0)
            {
                var empty = new TextBlock
                {
                    Text       = "  (niciun profil sau istoric)",
                    Foreground = new SolidColorBrush(Color.FromRgb(0, 80, 20)),
                    FontFamily = new FontFamily("Lucida Console, Courier New"),
                    FontSize   = 10,
                    Padding    = new Thickness(8, 4, 8, 4)
                };
                ProfileList.Children.Add(empty);
            }
            else
            {
                bool firstProfile = true;

                var jucatori = RegistruJucatori.Jucatori;
                if (jucatori.Count > 0)
                {
                    ProfileList.Children.Add(MakeSectionLabel("-- PROFILURI --"));
                    foreach (var j in jucatori)
                        ProfileList.Children.Add(MakePickBtn(j.Nume.ToUpper(), "#00FF41", "#001800", callback));
                }

                var recent = new List<string>(RegistruJucatori.IstoricNume);

                var profileNames = new HashSet<string>();
                foreach (var j in jucatori) profileNames.Add(j.Nume.ToUpper());
                recent.RemoveAll(n => profileNames.Contains(n));
                if (recent.Count > 0)
                {
                    ProfileList.Children.Add(MakeSectionLabel("-- RECENT --"));
                    foreach (var n in recent)
                        ProfileList.Children.Add(MakePickBtn(n, "#00CCCC", "#001818", callback));
                }
            }

            BuildButtons("input");
            Visibility = Visibility.Visible;
            TxtInput.Focus();
        }

        private void Setup(string title, string message, bool showPicker, bool showInput)
        {
            TxtTitle.Text   = $">> {title.ToUpper()} <<";
            TxtMessage.Text = message;
            PickerArea.Visibility = showPicker ? Visibility.Visible : Visibility.Collapsed;
            InputArea.Visibility  = showInput  ? Visibility.Visible : Visibility.Collapsed;
            _yesNoCallback = null;
            _inputCallback = null;
            _okCallback    = null;
        }

        private TextBlock MakeSectionLabel(string text) => new TextBlock
        {
            Text       = text,
            Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 40)),
            FontFamily = new FontFamily("Lucida Console, Courier New"),
            FontSize   = 9,
            Padding    = new Thickness(8, 4, 8, 2)
        };

        private Button MakePickBtn(string name, string fg, string bg, Action<string> callback)
        {
            var btn = new Button
            {
                Content         = $"  {name}",
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Foreground      = new SolidColorBrush((Color)ColorConverter.ConvertFromString(fg)),
                Background      = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bg)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                BorderBrush     = new SolidColorBrush(Color.FromRgb(0, 50, 20)),
                FontFamily      = new FontFamily("Lucida Console, Courier New"),
                FontSize        = 12,
                FontWeight      = FontWeights.Bold,
                Padding         = new Thickness(8, 5, 8, 5),
                Cursor          = System.Windows.Input.Cursors.Hand
            };
            btn.Click += (s, e) =>
            {
                RegistruJucatori.AdaugaIstoricNume(name);
                Visibility = Visibility.Collapsed;
                callback(name);
            };
            return btn;
        }

        private void BuildButtons(string mode)
        {
            ButtonPanel.Children.Clear();
            if (mode == "yesno")
            {
                ButtonPanel.Children.Add(MakeBtn("[DA]",     "#00FF41", "#0D0208", () => Respond(true)));
                ButtonPanel.Children.Add(MakeBtn("[NU]",     "#FF2222", "#0D0208", () => Respond(false)));
            }
            else if (mode == "input")
            {
                ButtonPanel.Children.Add(MakeBtn("[OK]",     "#00FF41", "#0D0208", () => RespondInput()));
                ButtonPanel.Children.Add(MakeBtn("[CANCEL]", "#FF2222", "#0D0208", () => Cancel()));
            }
            else
            {
                ButtonPanel.Children.Add(MakeBtn("[OK]", "#00FF41", "#0D0208", () => RespondOk()));
            }
        }

        private Button MakeBtn(string label, string fg, string bg, Action onClick)
        {
            var btn = new Button
            {
                Content         = label,
                Foreground      = new SolidColorBrush((Color)ColorConverter.ConvertFromString(fg)),
                Background      = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bg)),
                BorderBrush     = new SolidColorBrush((Color)ColorConverter.ConvertFromString(fg)),
                BorderThickness = new Thickness(2),
                FontFamily      = new FontFamily("Lucida Console, Courier New"),
                FontSize        = 12,
                FontWeight      = FontWeights.Bold,
                Padding         = new Thickness(16, 8, 16, 8),
                Margin          = new Thickness(6, 0, 6, 0),
                Cursor          = System.Windows.Input.Cursors.Hand
            };
            btn.Click += (s, e) => onClick();
            return btn;
        }

        private void Respond(bool yes)
        {
            Visibility = Visibility.Collapsed;
            _yesNoCallback?.Invoke(yes);
        }

        private void RespondInput()
        {
            string val = TxtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(val)) val = "ANONIM";
            else val = val.ToUpper();
            RegistruJucatori.AdaugaIstoricNume(val);
            Visibility = Visibility.Collapsed;
            _inputCallback?.Invoke(val);
        }

        private void RespondOk()
        {
            Visibility = Visibility.Collapsed;
            _okCallback?.Invoke();
        }

        private void Cancel()
        {
            Visibility = Visibility.Collapsed;
            _inputCallback?.Invoke("");
        }
    }
}
