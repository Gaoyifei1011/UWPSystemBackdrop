using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using UWPSystemBackdrop.Backdrop;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWPSystemBackdrop
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
            Background = new MicaBackdrop()
            {
                Kind = MicaKind.Base,
                RequestedTheme = currentTheme,
                IsInputActive = currentInputActiveState
            };

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
                Background = new MicaBackdrop()
                {
                    Kind = MicaKind.Base,
                    RequestedTheme = currentTheme,
                    IsInputActive = currentInputActiveState
                };

                SystemBackdropNameText.Text = "MicaBase";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "MicaBase")
            {
                Background = new MicaBackdrop()
                {
                    Kind = MicaKind.BaseAlt,
                    RequestedTheme = currentTheme,
                    IsInputActive = currentInputActiveState
                };

                SystemBackdropNameText.Text = "MicaAlt";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "MicaAlt")
            {
                Background = new DesktopAcrylicBackdrop()
                {
                    Kind = DesktopAcrylicKind.Default,
                    RequestedTheme = currentTheme,
                    IsInputActive = currentInputActiveState,
                    UseHostBackdropBrush = true
                };

                SystemBackdropNameText.Text = "DesktopAcrylicDefault";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicDefault")
            {
                Background = new DesktopAcrylicBackdrop()
                {
                    Kind = DesktopAcrylicKind.Base,
                    RequestedTheme = currentTheme,
                    IsInputActive = currentInputActiveState,
                    UseHostBackdropBrush = true
                };

                SystemBackdropNameText.Text = "DesktopAcrylicBase";
                ThemeNameText.Text = currentTheme.ToString();
                InputActiveStateText.Text = currentInputActiveState.ToString();
            }
            else if (SystemBackdropNameText.Text == "DesktopAcrylicBase")
            {
                Background = new DesktopAcrylicBackdrop()
                {
                    Kind = DesktopAcrylicKind.Thin,
                    RequestedTheme = currentTheme,
                    IsInputActive = currentInputActiveState,
                    UseHostBackdropBrush = true
                };

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
                RequestedTheme = ElementTheme.Light;
                currentTheme = ElementTheme.Light;
                ThemeNameText.Text = ElementTheme.Light.ToString();
                if (Background as SystemBackdrop is SystemBackdrop systemBackdrop)
                {
                    systemBackdrop.RequestedTheme = ElementTheme.Light;
                }
                else
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 243, 243, 243));
                }
            }
            else if (currentTheme is ElementTheme.Light)
            {
                RequestedTheme = ElementTheme.Dark;
                currentTheme = ElementTheme.Dark;
                ThemeNameText.Text = ElementTheme.Dark.ToString();
                if (Background as SystemBackdrop is SystemBackdrop systemBackdrop)
                {
                    systemBackdrop.RequestedTheme = ElementTheme.Dark;
                }
                else
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 32, 32, 32));
                }
            }
            else
            {
                RequestedTheme = ElementTheme.Default;
                currentTheme = ElementTheme.Default;
                ThemeNameText.Text = ElementTheme.Default.ToString();
                if (Background as SystemBackdrop is SystemBackdrop systemBackdrop)
                {
                    systemBackdrop.RequestedTheme = ElementTheme.Default;
                }
                else
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

        private void SwitchInputActiveStateClick(object sender, RoutedEventArgs args)
        {
            if (currentInputActiveState)
            {
                currentInputActiveState = false;
                InputActiveStateText.Text = currentInputActiveState.ToString();
                if (Background as SystemBackdrop is SystemBackdrop systemBackdrop)
                {
                    systemBackdrop.IsInputActive = false;
                }
            }
            else
            {
                currentInputActiveState = true;
                InputActiveStateText.Text = currentInputActiveState.ToString();
                if (Background as SystemBackdrop is SystemBackdrop systemBackdrop)
                {
                    systemBackdrop.IsInputActive = true;
                }
            }
        }

        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            if (Background as SystemBackdrop is not SystemBackdrop systemBackdrop)
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