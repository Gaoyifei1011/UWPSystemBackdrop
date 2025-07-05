using Microsoft.Win32;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.System.Power;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

// 抑制 CA1822，IDE0028，IDE0060 警告
#pragma warning disable CA1822,IDE0028,IDE0060

namespace UWPSystemBackdrop.UI.Backdrop
{
    /// <summary>
    /// Desktop Acrylic 背景色
    /// </summary>
    public sealed partial class DesktopAcrylicBrush : XamlCompositionBrushBase
    {
        private bool isConnected;
        private bool isActivated = true;
        private bool useDesktopAcrylicBrush;

        private readonly UISettings uiSettings = new();
        private readonly AccessibilitySettings accessibilitySettings = new();
        private readonly CompositionCapabilities compositionCapabilities = CompositionCapabilities.GetForCurrentView();
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private readonly float desktopAcrylicDefaultLightTintOpacity = 0;
        private readonly float desktopAcrylicDefaultLightLuminosityOpacity = 0.85f;
        private readonly float desktopAcrylicDefaultDarkTintOpacity = 0.15f;
        private readonly float desktopAcrylicDefaultDarkLuminosityOpacity = 96;
        private readonly Color desktopAcrylicDefaultLightTintColor = Color.FromArgb(255, 252, 252, 252);
        private readonly Color desktopAcrylicDefaultLightFallbackColor = Color.FromArgb(255, 249, 249, 249);
        private readonly Color desktopAcrylicDefaultDarkTintColor = Color.FromArgb(255, 44, 44, 44);
        private readonly Color desktopAcrylicDefaultDarkFallbackColor = Color.FromArgb(255, 44, 44, 44);

        private readonly float desktopAcrylicBaseLightTintOpacity = 0;
        private readonly float desktopAcrylicBaseLightLuminosityOpacity = 0.9f;
        private readonly float desktopAcrylicBaseDarkTintOpacity = 0.5f;
        private readonly float desktopAcrylicBaseDarkLuminosityOpacity = 0.96f;
        private readonly Color desktopAcrylicBaseLightTintColor = Color.FromArgb(255, 243, 243, 243);
        private readonly Color desktopAcrylicBaseLightFallbackColor = Color.FromArgb(255, 238, 238, 238);
        private readonly Color desktopAcrylicBaseDarkTintColor = Color.FromArgb(255, 32, 32, 32);
        private readonly Color desktopAcrylicBaseDarkFallbackColor = Color.FromArgb(255, 28, 28, 28);

        private readonly float desktopAcrylicThinLightTintOpacity = 0;
        private readonly float desktopAcrylicThinLightLuminosityOpacity = 0.44f;
        private readonly float desktopAcrylicThinDarkTintOpacity = 0.15f;
        private readonly float desktopAcrylicThinDarkLuminosityOpacity = 0.64f;
        private readonly Color desktopAcrylicThinLightTintColor = Color.FromArgb(255, 211, 211, 211);
        private readonly Color desktopAcrylicThinLightFallbackColor = Color.FromArgb(255, 211, 211, 211);
        private readonly Color desktopAcrylicThinDarkTintColor = Color.FromArgb(255, 84, 84, 84);
        private readonly Color desktopAcrylicThinDarkFallbackColor = Color.FromArgb(255, 84, 84, 84);

        private readonly bool isInputActive;
        private readonly bool useHostBackdropBrush;
        private readonly float blurAmount = 30f;
        private readonly float lightTintOpacity;
        private readonly float lightLuminosityOpacity;
        private readonly float darkTintOpacity;
        private readonly float darkLuminosityOpacity;
        private readonly FrameworkElement backgroundElement;

        private Color lightTintColor = Color.FromArgb(0, 0, 0, 0);
        private Color lightFallbackColor = Color.FromArgb(0, 0, 0, 0);
        private Color darkTintColor = Color.FromArgb(0, 0, 0, 0);
        private Color darkFallbackColor = Color.FromArgb(0, 0, 0, 0);

        /// <summary>
        /// 检查是否支持亚克力背景色
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Wallpaper");
                if (key is not null)
                {
                    object value = key.GetValue("WallpaperSurfaceProvidedToDwm");
                    if (value is not null && Convert.ToInt32(value) is 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public DesktopAcrylicBrush(DesktopAcrylicKind desktopAcrylicKind, FrameworkElement frameworkElement, bool isinputActive, bool usehostBackdropBrush)
        {
            if (frameworkElement is null)
            {
                throw new ArgumentNullException(string.Format("参数 {0} 不可以为空值", nameof(frameworkElement)));
            }

            isInputActive = isinputActive;
            useHostBackdropBrush = usehostBackdropBrush;
            backgroundElement = frameworkElement;

            if (desktopAcrylicKind is DesktopAcrylicKind.Default)
            {
                lightTintOpacity = desktopAcrylicDefaultLightTintOpacity;
                lightLuminosityOpacity = desktopAcrylicDefaultLightLuminosityOpacity;
                darkTintOpacity = desktopAcrylicDefaultDarkTintOpacity;
                darkLuminosityOpacity = desktopAcrylicDefaultDarkLuminosityOpacity;
                lightTintColor = desktopAcrylicDefaultLightTintColor;
                lightFallbackColor = desktopAcrylicDefaultLightFallbackColor;
                darkTintColor = desktopAcrylicDefaultDarkTintColor;
                darkFallbackColor = desktopAcrylicDefaultDarkFallbackColor;
            }
            else if (desktopAcrylicKind is DesktopAcrylicKind.Base)
            {
                lightTintOpacity = desktopAcrylicBaseLightTintOpacity;
                lightLuminosityOpacity = desktopAcrylicBaseLightLuminosityOpacity;
                darkTintOpacity = desktopAcrylicBaseDarkTintOpacity;
                darkLuminosityOpacity = desktopAcrylicBaseDarkLuminosityOpacity;
                lightTintColor = desktopAcrylicBaseLightTintColor;
                lightFallbackColor = desktopAcrylicBaseLightFallbackColor;
                darkTintColor = desktopAcrylicBaseDarkTintColor;
                darkFallbackColor = desktopAcrylicBaseDarkFallbackColor;
            }
            else
            {
                lightTintOpacity = desktopAcrylicThinLightTintOpacity;
                lightLuminosityOpacity = desktopAcrylicThinLightLuminosityOpacity;
                darkTintOpacity = desktopAcrylicThinDarkTintOpacity;
                darkLuminosityOpacity = desktopAcrylicThinDarkLuminosityOpacity;
                lightTintColor = desktopAcrylicThinLightTintColor;
                lightFallbackColor = desktopAcrylicThinLightFallbackColor;
                darkTintColor = desktopAcrylicThinDarkTintColor;
                darkFallbackColor = desktopAcrylicThinDarkFallbackColor;
            }
        }

        /// <summary>
        /// 在屏幕上首次使用画笔绘制元素时调用。
        /// </summary>
        protected override void OnConnected()
        {
            base.OnConnected();

            if (!isConnected)
            {
                isConnected = true;
                uiSettings.ColorValuesChanged += OnColorValuesChanged;
                Window.Current.CoreWindow.Activated += OnActivated;
                accessibilitySettings.HighContrastChanged += OnHighContrastChanged;
                compositionCapabilities.Changed += OnCompositionCapabilitiesChanged;
                PowerManager.EnergySaverStatusChanged += OnEnergySaverStatusChanged;

                if (backgroundElement is not null)
                {
                    backgroundElement.ActualThemeChanged += OnActualThemeChanged;
                }

                UpdateBrush();
            }
        }

        /// <summary>
        /// 不再使用画笔绘制任何元素时调用。
        /// </summary>
        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            lock (this)
            {
                if (isConnected)
                {
                    isConnected = false;
                    uiSettings.ColorValuesChanged -= OnColorValuesChanged;
                    Window.Current.CoreWindow.Activated -= OnActivated;
                    accessibilitySettings.HighContrastChanged -= OnHighContrastChanged;
                    compositionCapabilities.Changed -= OnCompositionCapabilitiesChanged;
                    PowerManager.EnergySaverStatusChanged -= OnEnergySaverStatusChanged;

                    if (backgroundElement is not null)
                    {
                        backgroundElement.ActualThemeChanged -= OnActualThemeChanged;
                    }

                    if (CompositionBrush is not null)
                    {
                        CompositionBrush.Dispose();
                        CompositionBrush = null;
                    }
                }
            }
        }

        /// <summary>
        /// 颜色值更改时发生的事件
        /// </summary>
        private void OnColorValuesChanged(UISettings sender, object args)
        {
            dispatcherQueue.TryEnqueue(UpdateBrush);
        }

        /// <summary>
        /// 在窗口完成激活或停用时触发的事件
        /// </summary>
        private void OnActivated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            isActivated = Window.Current.CoreWindow.ActivationMode is not CoreWindowActivationMode.Deactivated;
            UpdateBrush();
        }

        /// <summary>
        /// 当系统高对比度功能打开或关闭时发生的事件
        /// </summary>
        private void OnHighContrastChanged(AccessibilitySettings sender, object args)
        {
            dispatcherQueue.TryEnqueue(UpdateBrush);
        }

        /// <summary>
        /// 当支持的合成功能发生更改时触发的事件
        /// </summary>
        private void OnCompositionCapabilitiesChanged(CompositionCapabilities sender, object args)
        {
            dispatcherQueue.TryEnqueue(UpdateBrush);
        }

        /// <summary>
        /// 在设备的节电模式状态更改时触发的事件
        /// </summary>
        private void OnEnergySaverStatusChanged(object sender, object args)
        {
            dispatcherQueue.TryEnqueue(UpdateBrush);
        }

        /// <summary>
        /// 在 ActualTheme 属性值更改时触发的事件
        /// </summary>
        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            UpdateBrush();
        }

        /// <summary>
        /// 更新应用的背景色
        /// </summary>
        private void UpdateBrush()
        {
            if (isConnected)
            {
                float tintOpacity;
                float luminosityOpacity;
                Color tintColor;
                Color fallbackColor;

                if (backgroundElement.ActualTheme is ElementTheme.Light)
                {
                    tintOpacity = lightTintOpacity;
                    luminosityOpacity = lightLuminosityOpacity;
                    tintColor = lightTintColor;
                    fallbackColor = lightFallbackColor;
                }
                else
                {
                    tintOpacity = darkTintOpacity;
                    luminosityOpacity = darkLuminosityOpacity;
                    tintColor = darkTintColor;
                    fallbackColor = darkFallbackColor;
                }

                useDesktopAcrylicBrush = IsSupported && uiSettings.AdvancedEffectsEnabled && PowerManager.EnergySaverStatus is not EnergySaverStatus.On && compositionCapabilities.AreEffectsSupported() && (isInputActive || isActivated);

                if (accessibilitySettings.HighContrast)
                {
                    tintColor = new UISettings().GetColorValue(UIColorType.Background);
                    useDesktopAcrylicBrush = false;
                }

                Compositor compositor = Window.Current.Compositor;
                CompositionBrush newBrush = useDesktopAcrylicBrush ? BuildDesktopAcrylicEffectBrush(compositor, tintColor, tintOpacity, luminosityOpacity) : compositor.CreateColorBrush(fallbackColor);
                CompositionBrush oldBrush = CompositionBrush;

                if (oldBrush is null || oldBrush.Comment is "Crossfade")
                {
                    // 直接设置新笔刷
                    oldBrush?.Dispose();
                    CompositionBrush = newBrush;
                }
                else
                {
                    // 回退色切换时的动画颜色
                    CompositionBrush crossFadeBrush = CreateCrossFadeEffectBrush(compositor, oldBrush, newBrush);
                    ScalarKeyFrameAnimation animation = CreateCrossFadeAnimation(compositor);
                    CompositionBrush = crossFadeBrush;

                    CompositionScopedBatch crossFadeAnimationBatch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                    crossFadeBrush.StartAnimation("CrossFade.CrossFade", animation);
                    crossFadeAnimationBatch.End();

                    crossFadeAnimationBatch.Completed += (o, a) =>
                    {
                        crossFadeBrush.Dispose();
                        oldBrush.Dispose();
                        CompositionBrush = newBrush;
                    };
                }
            }
        }

        /// <summary>
        /// 创建 DesktopAcrylic 背景色
        /// </summary>
        private CompositionEffectBrush BuildDesktopAcrylicEffectBrush(Compositor compositor, Color tintColor, float tintOpacity, float luminosityOpacity)
        {
            Color convertedLuminosityColor = ColorConversion.GetEffectiveLuminosityColor(tintColor, tintOpacity, luminosityOpacity);
            Color convertedTintColor = ColorConversion.GetEffectiveTintColor(tintColor, tintOpacity, luminosityOpacity);

            // Source 1 : Host backdrop layer effect
            ColorSourceEffect hostBackdropEffect = new()
            {
                Color = Color.FromArgb(255, 0, 0, 0)
            };

            OpacityEffect hostBackdropLayerEffect = new()
            {
                Name = "FixHostBackdropLayer",
                Opacity = useHostBackdropBrush ? 1 : 0,
                Source = hostBackdropEffect,
            };

            // Source 2 : Tint color effect
            GaussianBlurEffect gaussianBlurEffect = new()
            {
                Name = "GaussianBlurEffect",
                BlurAmount = useHostBackdropBrush ? Math.Max(blurAmount - 30, 0) : blurAmount,
                Source = new CompositionEffectSourceParameter("source"),
                BorderMode = EffectBorderMode.Hard
            };

            BlendEffect luminosityColorEffect = new()
            {
                Mode = BlendEffectMode.Color,
                Foreground = new ColorSourceEffect
                {
                    Name = "LuminosityColorEffect",
                    Color = convertedLuminosityColor,
                },
                Background = gaussianBlurEffect
            };

            ColorSourceEffect tintColorEffect = new()
            {
                Name = "TintColorEffect",
                Color = convertedTintColor,
            };

            BlendEffect tintAndLuminosityColorEffect = new()
            {
                Mode = BlendEffectMode.Luminosity,
                Foreground = tintColorEffect,
                Background = luminosityColorEffect
            };

            OpacityEffect tintColorOpacityEffect = new()
            {
                Name = "TintColorOpacityEffect",
                Opacity = convertedTintColor.A is 255 ? 0f : 1f,
                Source = tintAndLuminosityColorEffect
            };

            // Source 3: Tint color effect without alpha
            ColorSourceEffect tintColorEffectWithoutAlphaEffect = new()
            {
                Name = "TintColorEffectWithoutAlpha",
                Color = convertedTintColor
            };

            OpacityEffect tintColorWithoutAlphaOpacityEffect = new()
            {
                Name = "TintColorWithoutAlphaOpacityEffect",
                Opacity = convertedTintColor.A is 255 ? 1f : 0f,
                Source = tintColorEffectWithoutAlphaEffect,
            };

            // Source 4 : Noise border effect
            BorderEffect noiseBorderEffect = new()
            {
                Source = new CompositionEffectSourceParameter("noise"),
                ExtendX = CanvasEdgeBehavior.Wrap,
                ExtendY = CanvasEdgeBehavior.Wrap,
            };

            OpacityEffect noiseEffect = new()
            {
                Opacity = 0.02f,
                Source = noiseBorderEffect,
            };

            CompositeEffect compositeEffect = new()
            {
                Mode = CanvasComposite.SourceOver,
                Sources =
                {
                    hostBackdropLayerEffect,
                    tintColorOpacityEffect,
                    tintColorWithoutAlphaOpacityEffect,
                    noiseEffect
                }
            };

            CompositionSurfaceBrush noiseBrush = compositor.CreateSurfaceBrush();
            noiseBrush.Stretch = CompositionStretch.None;
            noiseBrush.Surface = LoadedImageSurface.StartLoadFromUri(new Uri("ms-appx:///NoiseAsset_256x256_PNG.png"));

            CompositionEffectBrush desktopAcrylicBrush = compositor.CreateEffectFactory(compositeEffect).CreateBrush();
            CompositionBrush backdropBrush = useHostBackdropBrush ? compositor.CreateHostBackdropBrush() : compositor.CreateBackdropBrush();

            desktopAcrylicBrush.SetSourceParameter("source", backdropBrush);
            desktopAcrylicBrush.SetSourceParameter("noise", noiseBrush);

            return desktopAcrylicBrush;
        }

        /// <summary>
        /// 创建回退色切换时的动画颜色
        /// </summary>
        private CompositionEffectBrush CreateCrossFadeEffectBrush(Compositor compositor, CompositionBrush from, CompositionBrush to)
        {
            CrossFadeEffect crossFadeEffect = new()
            {
                Name = "Crossfade", // Name to reference when starting the animation.
                Source1 = new CompositionEffectSourceParameter("source1"),
                Source2 = new CompositionEffectSourceParameter("source2"),
                CrossFade = 0
            };

            CompositionEffectBrush crossFadeEffectBrush = compositor.CreateEffectFactory(crossFadeEffect, new List<string> { "Crossfade.CrossFade" }).CreateBrush();
            crossFadeEffectBrush.Comment = "Crossfade";

            crossFadeEffectBrush.SetSourceParameter("source1", from);
            crossFadeEffectBrush.SetSourceParameter("source2", to);
            return crossFadeEffectBrush;
        }

        /// <summary>
        /// 为回退色创建动画效果
        /// </summary>
        private ScalarKeyFrameAnimation CreateCrossFadeAnimation(Compositor compositor)
        {
            ScalarKeyFrameAnimation animation = compositor.CreateScalarKeyFrameAnimation();
            LinearEasingFunction linearEasing = compositor.CreateLinearEasingFunction();
            animation.InsertKeyFrame(0.0f, 0.0f, linearEasing);
            animation.InsertKeyFrame(1.0f, 1.0f, linearEasing);
            animation.Duration = TimeSpan.FromMilliseconds(250);
            return animation;
        }
    }
}
