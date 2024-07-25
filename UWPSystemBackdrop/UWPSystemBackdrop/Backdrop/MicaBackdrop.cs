using System;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.System.Power;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace UWPSystemBackdrop.Backdrop
{
    /// <summary>
    /// Mica 背景色
    /// </summary>
    public class MicaBackdrop : SystemBackdrop
    {
        private bool isInitialized;
        private bool isActivated = Window.Current.CoreWindow.ActivationMode is not CoreWindowActivationMode.Deactivated;
        private bool useMicaBrush;
        private readonly FrameworkElement rootElement = Window.Current.Content as FrameworkElement;
        private readonly UISettings uiSettings = new();
        private readonly AccessibilitySettings accessibilitySettings = new();
        private readonly CompositionCapabilities compositionCapabilities = CompositionCapabilities.GetForCurrentView();
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private readonly float defaultMicaBaseLightTintOpacity = 0.5f;
        private readonly float defaultMicaBaseLightLuminosityOpacity = 1;
        private readonly float defaultMicaBaseDarkTintOpacity = 0.8f;
        private readonly float defaultMicaBaseDarkLuminosityOpacity = 1;
        private readonly Color defaultMicaBaseLightTintColor = Color.FromArgb(255, 243, 243, 243);
        private readonly Color defaultMicaBaseLightFallbackColor = Color.FromArgb(255, 243, 243, 243);
        private readonly Color defaultMicaBaseDarkTintColor = Color.FromArgb(255, 32, 32, 32);
        private readonly Color defaultMicaBaseDarkFallbackColor = Color.FromArgb(255, 32, 32, 32);

        private readonly float defaultMicaAltLightTintOpacity = 0.5f;
        private readonly float defaultMicaAltLightLuminosityOpacity = 1;
        private readonly float defaultMicaAltDarkTintOpacity = 0;
        private readonly float defaultMicaAltDarkLuminosityOpacity = 1;
        private readonly Color defaultMicaAltLightTintColor = Color.FromArgb(255, 218, 218, 218);
        private readonly Color defaultMicaAltLightFallbackColor = Color.FromArgb(255, 218, 218, 218);
        private readonly Color defaultMicaAltDarkTintColor = Color.FromArgb(255, 10, 10, 10);
        private readonly Color defaultMicaAltDarkFallbackColor = Color.FromArgb(255, 10, 10, 10);

        public MicaKind Kind { get; set; } = MicaKind.Base;

        private float _lightTintOpacity = 0;

        public override float LightTintOpacity
        {
            get { return _lightTintOpacity; }

            set
            {
                if (!Equals(_lightTintOpacity, value))
                {
                    _lightTintOpacity = value;
                    if (value < 0 || value > 1)
                    {
                        throw new ArgumentException("值必须在 0 到 1 之间");
                    }

                    UpdateBrush();
                }
            }
        }

        private float _lightLuminosityOpacity = 0;

        public override float LightLuminosityOpacity
        {
            get { return _lightLuminosityOpacity; }

            set
            {
                if (!Equals(_lightLuminosityOpacity, value))
                {
                    _lightLuminosityOpacity = value;
                    if (value < 0 || value > 1)
                    {
                        throw new ArgumentException("值必须在 0 到 1 之间");
                    }

                    UpdateBrush();
                }
            }
        }

        private float _darkTintOpacity = 0;

        public override float DarkTintOpacity
        {
            get { return _darkTintOpacity; }

            set
            {
                if (!Equals(_darkTintOpacity, value))
                {
                    _darkTintOpacity = value;
                    if (value < 0 || value > 1)
                    {
                        throw new ArgumentException("值必须在 0 到 1 之间");
                    }

                    UpdateBrush();
                }
            }
        }

        private float _darkLuminosityOpacity = 0;

        public override float DarkLuminosityOpacity
        {
            get { return _darkLuminosityOpacity; }

            set
            {
                if (!Equals(_darkLuminosityOpacity, value))
                {
                    _darkLuminosityOpacity = value;
                    if (value < 0 || value > 1)
                    {
                        throw new ArgumentException("值必须在 0 到 1 之间");
                    }

                    UpdateBrush();
                }
            }
        }

        private Color _lightTintColor = Color.FromArgb(0, 0, 0, 0);

        public override Color LightTintColor
        {
            get { return _lightTintColor; }

            set
            {
                if (!Equals(_lightTintColor, value))
                {
                    _lightTintColor = value;
                    UpdateBrush();
                }
            }
        }

        private Color _lightFallbackColor = Color.FromArgb(0, 0, 0, 0);

        public override Color LightFallbackColor
        {
            get { return _lightFallbackColor; }

            set
            {
                if (!Equals(_lightFallbackColor, value))
                {
                    _lightFallbackColor = value;
                    UpdateBrush();
                }
            }
        }

        private Color _darkTintColor = Color.FromArgb(0, 0, 0, 0);

        public override Color DarkTintColor
        {
            get { return _darkTintColor; }

            set
            {
                if (!Equals(_darkTintColor, value))
                {
                    _darkTintColor = value;
                    UpdateBrush();
                }
            }
        }

        private Color _darkFallbackColor = Color.FromArgb(0, 0, 0, 0);

        public override Color DarkFallbackColor
        {
            get { return _darkFallbackColor; }

            set
            {
                if (!Equals(_darkFallbackColor, value))
                {
                    _darkFallbackColor = value;
                    UpdateBrush();
                }
            }
        }

        private ElementTheme _requestedTheme = ElementTheme.Default;

        public override ElementTheme RequestedTheme
        {
            get { return _requestedTheme; }

            set
            {
                if (!Equals(_requestedTheme, value))
                {
                    _requestedTheme = value;
                    UpdateBrush();
                }
            }
        }

        private bool _isInputActive = false;

        public override bool IsInputActive
        {
            get { return _isInputActive; }

            set
            {
                if (!Equals(_isInputActive, value))
                {
                    _isInputActive = value;
                }
            }
        }

        public override bool IsSupported
        {
            get { return ApiInformation.IsMethodPresent(typeof(Compositor).FullName, nameof(Compositor.TryCreateBlurredWallpaperBackdropBrush)); }
        }

        /// <summary>
        /// 在屏幕上首次使用画笔绘制元素时调用。
        /// </summary>
        protected override void OnConnected()
        {
            base.OnConnected();

            if (!isInitialized)
            {
                float defaultOpacityValue = 0;
                Color defaultColorValue = Color.FromArgb(0, 0, 0, 0);

                if (Kind is MicaKind.Base)
                {
                    _lightTintOpacity = _lightTintOpacity.Equals(defaultOpacityValue) ? defaultMicaBaseLightTintOpacity : _lightTintOpacity;
                    _lightLuminosityOpacity = _lightLuminosityOpacity.Equals(defaultOpacityValue) ? defaultMicaBaseLightLuminosityOpacity : _lightLuminosityOpacity;
                    _darkTintOpacity = _darkTintOpacity.Equals(defaultOpacityValue) ? defaultMicaBaseDarkTintOpacity : _darkTintOpacity;
                    _darkLuminosityOpacity = _darkLuminosityOpacity.Equals(defaultOpacityValue) ? defaultMicaBaseDarkLuminosityOpacity : _darkLuminosityOpacity;
                    _lightTintColor = _lightTintColor.Equals(defaultColorValue) ? defaultMicaBaseLightTintColor : _lightTintColor;
                    _lightFallbackColor = _lightFallbackColor.Equals(defaultColorValue) ? defaultMicaBaseLightFallbackColor : _lightFallbackColor;
                    _darkTintColor = _darkTintColor.Equals(defaultColorValue) ? defaultMicaBaseDarkTintColor : _darkTintColor;
                    _darkFallbackColor = _darkFallbackColor.Equals(defaultColorValue) ? defaultMicaBaseDarkFallbackColor : _darkFallbackColor;
                }
                else
                {
                    _lightTintOpacity = _lightTintOpacity.Equals(defaultOpacityValue) ? defaultMicaAltLightTintOpacity : _lightTintOpacity;
                    _lightLuminosityOpacity = _lightLuminosityOpacity.Equals(defaultOpacityValue) ? defaultMicaAltLightLuminosityOpacity : _lightLuminosityOpacity;
                    _darkTintOpacity = _darkTintOpacity.Equals(defaultOpacityValue) ? defaultMicaAltDarkTintOpacity : _darkTintOpacity;
                    _darkLuminosityOpacity = _darkLuminosityOpacity.Equals(defaultOpacityValue) ? defaultMicaAltDarkLuminosityOpacity : _darkLuminosityOpacity;
                    _lightTintColor = _lightTintColor.Equals(defaultColorValue) ? defaultMicaAltLightTintColor : _lightTintColor;
                    _lightFallbackColor = _lightFallbackColor.Equals(defaultColorValue) ? defaultMicaAltLightFallbackColor : _lightFallbackColor;
                    _darkTintColor = _darkTintColor.Equals(defaultColorValue) ? defaultMicaAltDarkTintColor : _darkTintColor;
                    _darkFallbackColor = _darkFallbackColor.Equals(defaultColorValue) ? defaultMicaAltDarkFallbackColor : _darkFallbackColor;
                }

                uiSettings.ColorValuesChanged += OnColorValuesChanged;
                Window.Current.CoreWindow.Activated += OnActivated;
                accessibilitySettings.HighContrastChanged += OnHighContrastChanged;
                compositionCapabilities.Changed += OnCompositionCapabilitiesChanged;
                PowerManager.EnergySaverStatusChanged += OnEnergySaverStatusChanged;

                if (rootElement is not null)
                {
                    rootElement.ActualThemeChanged += OnActualThemeChanged;
                }

                isInitialized = true;

                UpdateBrush();
            }
        }

        /// <summary>
        /// 在画笔不再用于绘制任何元素时调用。
        /// </summary>
        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            if (isInitialized)
            {
                isInitialized = false;

                uiSettings.ColorValuesChanged -= OnColorValuesChanged;
                Window.Current.CoreWindow.Activated -= OnActivated;
                accessibilitySettings.HighContrastChanged -= OnHighContrastChanged;
                compositionCapabilities.Changed -= OnCompositionCapabilitiesChanged;
                PowerManager.EnergySaverStatusChanged -= OnEnergySaverStatusChanged;

                if (rootElement is not null)
                {
                    rootElement.ActualThemeChanged -= OnActualThemeChanged;
                }

                if (CompositionBrush is not null)
                {
                    CompositionBrush.Dispose();
                    CompositionBrush = null;
                }
            }
        }

        /// <summary>
        /// 恢复默认值
        /// </summary>
        public override void ResetProperties()
        {
            if (Kind is MicaKind.Base)
            {
                _lightTintOpacity = defaultMicaBaseLightTintOpacity;
                _lightLuminosityOpacity = defaultMicaBaseLightLuminosityOpacity;
                _darkTintOpacity = defaultMicaBaseDarkTintOpacity;
                _darkLuminosityOpacity = defaultMicaBaseDarkLuminosityOpacity;
                _lightTintColor = defaultMicaBaseLightTintColor;
                _lightFallbackColor = defaultMicaBaseLightFallbackColor;
                _darkTintColor = defaultMicaBaseDarkTintColor;
                _darkFallbackColor = defaultMicaBaseDarkFallbackColor;
            }
            else
            {
                _lightTintOpacity = defaultMicaAltLightTintOpacity;
                _lightLuminosityOpacity = defaultMicaAltLightLuminosityOpacity;
                _darkTintOpacity = defaultMicaAltDarkTintOpacity;
                _darkLuminosityOpacity = defaultMicaAltDarkLuminosityOpacity;
                _lightTintColor = defaultMicaAltLightTintColor;
                _lightFallbackColor = defaultMicaAltLightFallbackColor;
                _darkTintColor = defaultMicaAltDarkTintColor;
                _darkFallbackColor = defaultMicaAltDarkFallbackColor;
            }

            _requestedTheme = ElementTheme.Default;
            _isInputActive = false;
            if (isInitialized)
            {
                UpdateBrush();
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
            if (isInitialized)
            {
                ElementTheme actualTheme = ElementTheme.Default;

                // 如果传入的 FrameworkElement 为空值，则由系统默认主题色值决定窗口的背景色
                if (rootElement is not null)
                {
                    // 主题值为默认时，窗口背景色主题值则由 FrameworkElement 决定
                    actualTheme = RequestedTheme is ElementTheme.Default ? rootElement.ActualTheme : RequestedTheme;
                }
                else
                {
                    actualTheme = Application.Current.RequestedTheme is ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
                }

                float tintOpacity;
                float luminosityOpacity;
                Color tintColor;
                Color fallbackColor;

                if (actualTheme is ElementTheme.Light)
                {
                    tintOpacity = LightTintOpacity;
                    luminosityOpacity = LightLuminosityOpacity;
                    tintColor = LightTintColor;
                    fallbackColor = LightFallbackColor;
                }
                else
                {
                    tintOpacity = DarkTintOpacity;
                    luminosityOpacity = DarkLuminosityOpacity;
                    tintColor = DarkTintColor;
                    fallbackColor = DarkFallbackColor;
                }

                useMicaBrush = IsSupported && uiSettings.AdvancedEffectsEnabled && PowerManager.EnergySaverStatus is not EnergySaverStatus.On && compositionCapabilities.AreEffectsSupported() && (IsInputActive || isActivated);

                if (accessibilitySettings.HighContrast)
                {
                    tintColor = new UISettings().GetColorValue(UIColorType.Background);
                    useMicaBrush = false;
                }

                Compositor compositor = Window.Current.Compositor;

                CompositionBrush newBrush = useMicaBrush ? BuildMicaEffectBrush(compositor, tintColor, tintOpacity, luminosityOpacity) : compositor.CreateColorBrush(fallbackColor);

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
        /// 创建 Mica 背景色
        /// </summary>
        private CompositionBrush BuildMicaEffectBrush(Compositor compositor, Color tintColor, float tintOpacity, float luminosityOpacity)
        {
            // Tint Color.
            ColorSourceEffect tintColorEffect = new()
            {
                Name = "TintColor",
                Color = tintColor
            };

            // OpacityEffect applied to Tint.
            OpacityEffect tintOpacityEffect = new()
            {
                Name = "TintOpacity",
                Opacity = tintOpacity,
                Source = tintColorEffect
            };

            // Apply Luminosity:

            // Luminosity Color.
            ColorSourceEffect luminosityColorEffect = new()
            {
                Color = tintColor
            };

            // OpacityEffect applied to Luminosity.
            OpacityEffect luminosityOpacityEffect = new()
            {
                Name = "LuminosityOpacity",
                Opacity = luminosityOpacity,
                Source = luminosityColorEffect
            };

            // Luminosity Blend.
            // NOTE: There is currently a bug where the names of BlendEffectMode::Luminosity and BlendEffectMode::Color are flipped.
            BlendEffect luminosityBlendEffect = new()
            {
                Mode = BlendEffectMode.Color,
                Background = new CompositionEffectSourceParameter("BlurredWallpaperBackdrop"),
                Foreground = luminosityOpacityEffect
            };

            // Apply Tint:

            // Color Blend.
            // NOTE: There is currently a bug where the names of BlendEffectMode::Luminosity and BlendEffectMode::Color are flipped.
            BlendEffect colorBlendEffect = new()
            {
                Mode = BlendEffectMode.Luminosity,
                Background = luminosityBlendEffect,
                Foreground = tintOpacityEffect
            };

            CompositionEffectBrush micaEffectBrush = compositor.CreateEffectFactory(colorBlendEffect).CreateBrush();
            micaEffectBrush.SetSourceParameter("BlurredWallpaperBackdrop", compositor.TryCreateBlurredWallpaperBackdropBrush());

            return micaEffectBrush;
        }

        /// <summary>
        /// 创建回退色切换时的动画颜色
        /// </summary>
        private CompositionBrush CreateCrossFadeEffectBrush(Compositor compositor, CompositionBrush from, CompositionBrush to)
        {
            CrossFadeEffect crossFadeEffect = new()
            {
                Name = "Crossfade", // Name to reference when starting the animation.
                Source1 = new CompositionEffectSourceParameter("source1"),
                Source2 = new CompositionEffectSourceParameter("source2"),
                CrossFade = 0,
            };

            CompositionEffectBrush crossFadeEffectBrush = compositor.CreateEffectFactory(crossFadeEffect, ["Crossfade.CrossFade"]).CreateBrush();
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