using UWPSystemBackdrop.UI.Backdrop;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWPSystemBackdrop
{
    public sealed partial class MainPage : Page
    {
        private ElementTheme currentTheme = ElementTheme.Default;
        private bool currentInputActiveState = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Background = new MicaBrush(MicaKind.Base, this, currentInputActiveState);

            currentTheme = ElementTheme.Default;
            currentInputActiveState = false;
            RequestedTheme = currentTheme;

            SystemBackdropNameText.Text = "MicaBase";
            ThemeNameText.Text = currentTheme.ToString();
            InputActiveStateText.Text = currentInputActiveState.ToString();
        }

        private void SwitchSystemBackdropClick(object sender, RoutedEventArgs args)
        {
            if (SystemBackdropNameText.Text == "None")
            {
                Background = new SolidColorBrush(Colors.Transparent);
                Background = new MicaBrush(MicaKind.Base, this, currentInputActiveState);

                SystemBackdropNameText.Text = "MicaBase";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "MicaBase")
            {
                Background = new MicaBrush(MicaKind.BaseAlt, this, currentInputActiveState);

                SystemBackdropNameText.Text = "MicaAlt";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "MicaAlt")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Default, this, currentInputActiveState, true);

                SystemBackdropNameText.Text = "DesktopAcrylicDefault";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicDefault")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Base, this, currentInputActiveState, true);

                SystemBackdropNameText.Text = "DesktopAcrylicBase";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicBase")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Thin, this, currentInputActiveState, true);

                SystemBackdropNameText.Text = "DesktopAcrylicThin";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicThin")
            {
                SystemBackdropNameText.Text = "None";
                ThemeNameText.Text = currentTheme.ToString();

                if (ActualTheme is ElementTheme.Light)
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 243, 243, 243));
                }
                else
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 32, 32, 32));
                }
            }
        }

        private void SwitchThemeClick(object sender, RoutedEventArgs args)
        {
            if (currentTheme is ElementTheme.Default)
            {
                RequestedTheme = ElementTheme.Light;
                currentTheme = ElementTheme.Light;
                ThemeNameText.Text = ElementTheme.Light.ToString();
                if (SystemBackdropNameText.Text == "None")
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 243, 243, 243));
                }
            }
            else if (currentTheme is ElementTheme.Light)
            {
                RequestedTheme = ElementTheme.Dark;
                currentTheme = ElementTheme.Dark;
                ThemeNameText.Text = ElementTheme.Dark.ToString();
                if (SystemBackdropNameText.Text == "None")
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 32, 32, 32));
                }
            }
            else
            {
                RequestedTheme = ElementTheme.Default;
                currentTheme = ElementTheme.Default;
                ThemeNameText.Text = ElementTheme.Default.ToString();
                if (ActualTheme is ElementTheme.Light)
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 243, 243, 243));
                }
                else
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 32, 32, 32));
                }
            }
        }

        private void SwitchInputActiveStateClick(object sender, RoutedEventArgs args)
        {
            if (currentInputActiveState)
            {
                currentInputActiveState = false;
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else
            {
                currentInputActiveState = true;
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }

            if (SystemBackdropNameText.Text == "Base")
            {
                Background = new MicaBrush(MicaKind.Base, this, currentInputActiveState);
            }
            else if (SystemBackdropNameText.Text == "MicaAlt")
            {
                Background = new MicaBrush(MicaKind.BaseAlt, this, currentInputActiveState);
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicDefault")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Default, this, currentInputActiveState, true);
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicBase")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Base, this, currentInputActiveState, true);
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicThin")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Thin, this, currentInputActiveState, true);
            }
        }

        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            if (SystemBackdropNameText.Text == "None")
            {
                if (ActualTheme is ElementTheme.Light)
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 243, 243, 243));
                }
                else
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 32, 32, 32));
                }
            }
        }
    }
}
