using UWPSystemBackdropNetNative.Backdrop;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWPSystemBackdropNetNative
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
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
            Background = new MicaBrush(MicaKind.Base, currentInputActiveState);

            currentTheme = ElementTheme.Default;
            currentInputActiveState = false;
            (Window.Current.Content as FrameworkElement).RequestedTheme = currentTheme;

            SystemBackdropNameText.Text = "MicaBase";
            ThemeNameText.Text = currentTheme.ToString();
            InputActiveStateText.Text = currentInputActiveState.ToString();
        }

        private void SwitchSystemBackdropClick(object sender, RoutedEventArgs args)
        {
            if (SystemBackdropNameText.Text == "None")
            {
                Background = new MicaBrush(MicaKind.Base, currentInputActiveState);

                SystemBackdropNameText.Text = "MicaBase";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "MicaBase")
            {
                Background = new MicaBrush(MicaKind.BaseAlt, currentInputActiveState);

                SystemBackdropNameText.Text = "MicaAlt";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "MicaAlt")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Default, currentInputActiveState);

                SystemBackdropNameText.Text = "DesktopAcrylicDefault";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicDefault")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Base, currentInputActiveState);

                SystemBackdropNameText.Text = "DesktopAcrylicBase";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicBase")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Thin, currentInputActiveState);

                SystemBackdropNameText.Text = "DesktopAcrylicThin";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicThin")
            {
                Background = null;

                SystemBackdropNameText.Text = "None";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();

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
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;
                currentTheme = ElementTheme.Light;
                ThemeNameText.Text = ElementTheme.Light.ToString();
            }
            else if (currentTheme is ElementTheme.Light)
            {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;
                currentTheme = ElementTheme.Dark;
                ThemeNameText.Text = ElementTheme.Dark.ToString();
            }
            else
            {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Default;
                currentTheme = ElementTheme.Default;
                ThemeNameText.Text = ElementTheme.Default.ToString();
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

            if (SystemBackdropNameText.Text == "MicaBase")
            {
                Background = new MicaBrush(MicaKind.Base, currentInputActiveState);
            }
            else if (SystemBackdropNameText.Text == "MicaAlt")
            {
                Background = new MicaBrush(MicaKind.BaseAlt, currentInputActiveState);
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicDefault")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Default, currentInputActiveState);
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicBase")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Base, currentInputActiveState);
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicThin")
            {
                Background = new DesktopAcrylicBrush(DesktopAcrylicKind.Thin, currentInputActiveState);
            }
        }

        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            if (Background is not XamlCompositionBrushBase)
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
