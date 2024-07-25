# UWPSystemBackdrop

----------

### 一个不依赖 Win2D 的 UWP SystemBackdrop 纯 c# 版本实现

#### 按照 Microsoft 官方的说明，如果我们需要在 UWP 项目中使用 Mica 或 Desktop Acrylic 背景色，需要引入 Microsoft.UI.Xaml （WinUI 2）Nuget 包依赖，而且 WinUI 2 只有 Mica Base，没有 Mica Alt。

#### 然而当我们不想使用 WinUI 2 ，想构建自定义的背景色时，需要引入 Win2D 开源库来完成这一操作。所以在本项目中，我们提供了一种不依赖于 Win2D 的 Mica 和 Desktop Acrylic 背景色实现，使用的 Mica 或 Desktop Acrylic 的 API 规范，默认提供的色彩样式基本与 Windows App SDK 项目保持一致。

----------

#### A pure c# version of the UWP SystemBackdrop that does not rely on Win2D

#### According to the official instructions of Microsoft, if we need to use Mica or Desktop Acrylic background color in UWP project, we need to introduce Microsoft.Ui.XAML (WinUI 2) Nuget package dependence. And WinUI 2 only has Mica Base, no Mica Alt.

#### However, when we do not want to use WinUI 2 and want to build a custom systembackdrop, we need to introduce Win2D open source library to accomplish this operation. Therefore, in this project, we provide a Mica and Desktop Acrylic background color implementation independent of Win2D, using Mica or Desktop Acrylic API specification. The color styles provided by default are basically consistent with the Windows App SDK project.

----------

### API 说明

#### 公有 API：

##### SystemBackdrop：代表背景色的抽象类，不可实例化

##### LightTintOpacity:	浅色模式下获取或设置颜色色调的不透明度。

##### LightLuminosityOpacity：浅色模式下获取或设置颜色发光度的不透明度。

##### DarkTintOpacity：深色模式下获取或设置颜色色调的不透明度。

##### DarkLuminosityOpacity：深色模式下获取或设置颜色发光度的不透明度。

##### LightTintColor：浅色模式下获取或设置背景色材料的颜色色调。

##### LightFallbackColor：浅色模式下获取或设置系统条件阻止呈现云母材料时要使用的纯色。

##### DarkTintColor：深色模式下获取或设置背景色材料的颜色色调。

##### DarkFallbackColor：深色模式下获取或设置系统条件阻止呈现云母材料时要使用的纯色。

##### RequestedTheme：获取或设置背景色主题值

##### IsInputActive：获取或设置一个值，该值指示系统背景是否应将当前窗口视为具有输入焦点。

##### IsSupported：确定当前操作系统是否支持背景色材料。

##### ResetProperties：将所有自定义属性重置为其系统默认值，并还原为自动浅色/深色主题处理。

----------

### API description

#### Public API:

##### SystemBackdrop: provides an abstract class against the background and is not available for instantiation

##### LightTintOpacity: Gets or sets the opacity of a color tone in Light mode.

##### LightLuminosityOpacity: Capture or set the opacity of the color in the light color mode.

##### DarkTintOpacity: Gets or sets the opacity of the color hue in dark mode.

##### DarkLuminosityOpacity: Captures or sets the opacity of the color in dark mode.

##### LightTintColor: Gets or sets the color tone of the background material in light mode.

##### LightFallbackColor: In Light mode gets or sets system conditions that prevent rendering of the solid color to be used when mica material.

##### DarkTintColor: Gets or sets the color hue of the background material in the dark mode.

##### DarkFallbackColor: Gets or sets a system condition in dark mode to prevent the solid color to be used when rendering mica material.

##### RequestedTheme: Gets or sets the background color theme value

##### IsInputActive: Gets or sets a value indicating whether the system background should treat the current window as having input focus.

##### IsSupported: specifies whether the current operating system supports background colors.

##### ResetProperties: Resets all custom properties to their system defaults and reverts to automatic light/dark theme handling.

----------

#### MicaBackdrop：Mica 背景色

#### MicaKind：Mica 背景色材料类型，两种类型（MicaBase 和 MicaAlt）

----------

#### MicaBackdrop: Mica SystemBackdrop

#### MicaKind: Mica background color material type, two types (MicaBase and MicaAlt)

----------

#### DesktopAcrylicBackdrop：Desktop Acrylic 背景色

##### DesktopAcrylicKind：Desktop Acrylic 背景色材料类型，两种类型（DesktopAcrylicDefault、DesktopAcrylicBase 和 DesktopAcrylicThin）

##### UseHostBackdropBrush：确定画笔采样的内容是来源于应用内容还是窗口后的内容。True 为窗口后的内容，False 为应用内容。

##### BlurAmount：获取或设置效果的模糊量（必须为正值）

##### IsHostBackdropSupported：确定当前操作系统是否支持画笔采样的内容来源于窗口后的内容。

----------

#### DesktopAcrylicBackdrop: DesktopAcrylic SystemBackdrop

##### DesktopAcrylicKind: DesktopAcrylic background color material type, two types (DesktopAcrylicDefault, DesktopAcrylicBase, and DesktopAcrylicThin)

##### UseHostBackdropBrush: Determines whether the brush samples content from the application content or from behind the window. True is the content behind the window, False is the content of the application.

##### BlurAmount: The amount of blurring to get or set the effect (must be positive)

##### IsHostBackdropSupported: Determines whether the current operating system supports brush sample content from behind a window.

----------

#### 参考资料（Reference）

云母背景色（Mica Systembackdrop）：https://learn.microsoft.com/windows/apps/design/style/mica

亚克力背景色（Acrylic Systembackdrop）：https://learn.microsoft.com/windows/apps/design/style/acrylic

Win32 中使用背景色（Win32 use Systembackdrop）：https://learn.microsoft.com/windows/apps/desktop/modernize/apply-mica-win32

WinUI 3 中使用背景色（WinUI 3 use Systembackdrop）：https://learn.microsoft.com/windows/apps/windows-app-sdk/system-backdrop-controller

API 来源说明（API source description）：https://learn.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.composition.systembackdrops?view=windows-app-sdk-1.5

----------

#### 参考项目（Reference Projects）

> * [BlueFire.Toolkit.WinUI3](https://github.com/cnbluefire/BlueFire.Toolkit.WinUI3)&emsp;
> * [DirectN](https://github.com/smourier/DirectN)&emsp;
> * [Uno](https://github.com/unoplatform/uno)&emsp;
> * [Wice](https://github.com/aelyo-softworks/Wice)&emsp;

----------

#### 其他说明

##### 本项目提供的是基于 UWP 目标下的不依赖 Win2D 的 UWP SystemBackdrop 纯 c# 版本实现，如果您的项目使用的是 Xaml Islands 或 WinUI 3，请参考下面列出的仓库链接。

##### This project provides a pure c# version of the UWP SystemBackdrop against the UWP target against Win2D. If your project is using Xaml Islands or WinUI 3, please refer to the repository links listed below.

##### Xaml Islands：[Mile.Xaml.Samples](https://github.com/ProjectMile/Mile.Xaml.Samples)&emsp;

##### WinUI 3：[BlueFire.Toolkit.WinUI3](https://github.com/cnbluefire/BlueFire.Toolkit.WinUI3)&emsp;
----------

#### 示例图（Demo）

##### 亮色 Mica Base（Mica Base Light）

![image](https://github.com/user-attachments/assets/e742feac-8727-4ddd-8755-7ca84e2de0a4)

##### 深色 Mica Dark（Mica Base Dark）

![image](https://github.com/user-attachments/assets/e0bb5736-4856-4db9-b094-ea382158add4)

##### 亮色 Mica Alt（Mica Alt Light）

![image](https://github.com/user-attachments/assets/c788867a-27ab-4488-8d37-824591cfe122)

##### 深色 Mica Alt（Mica Alt Dark）

![image](https://github.com/user-attachments/assets/176b1435-d542-49e6-9102-9d13409cd739)

##### 亮色 Desktop Acrylic Default（Desktop Acrylic Default Light）

![image](https://github.com/user-attachments/assets/6039bccd-3877-401e-848e-1b1658633821)

##### 深色 Desktop Acrylic Default（Desktop Acrylic Default Dark）

![image](https://github.com/user-attachments/assets/9ecc25cc-94c9-4cfa-b899-e22d25aa498a)

##### 亮色 Desktop Acrylic Base（Desktop Acrylic Base Light）

![image](https://github.com/user-attachments/assets/a2102c99-a690-4921-8a4a-76a77eb1fdea)

##### 深色 Desktop Acrylic Base（Desktop Acrylic Base Dark）

![image](https://github.com/user-attachments/assets/0523601f-bf99-42a4-9cd2-30170e6c72da)

##### 亮色 Desktop Acrylic Thin（Desktop Acrylic Thin Light）

![image](https://github.com/user-attachments/assets/9415454d-5534-449e-8ff3-7e5ffc1f675b)

##### 深色 Desktop Acrylic Default（Desktop Acrylic Thin Dark）

![image](https://github.com/user-attachments/assets/508d705a-b668-458b-8323-cd923f0f8053)
