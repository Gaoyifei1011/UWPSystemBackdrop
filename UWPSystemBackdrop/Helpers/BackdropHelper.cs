using UWPSystemBackdrop.WindowsAPI.ComTypes;
using Windows.Foundation;

namespace UWPSystemBackdrop.Helpers
{
    public static class BackdropHelper
    {
        public static IPropertyValueStatics PropertyValueStatics { get; } = PropertyValue.As<IPropertyValueStatics>();
    }
}