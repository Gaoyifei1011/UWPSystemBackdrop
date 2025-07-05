using System;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using UWPSystemBackdrop.WindowsAPI.ComTypes;

// 抑制 IDE0130 警告
#pragma warning disable IDE0130

namespace ABI.UWPSystemBackdrop.WindowsAPI.ComTypes
{
    public static class IGraphicsEffectD2D1InteropMethods
    {
        public static Guid IID { get; } = typeof(IGraphicsEffectD2D1Interop).GUID;

        public static IntPtr AbiToProjectionVftablePtr { get; }

        static IGraphicsEffectD2D1InteropMethods()
        {
            unsafe
            {
                AbiToProjectionVftablePtr = (IntPtr)((IIUnknownDerivedDetails)typeof(IGraphicsEffectD2D1Interop).GetCustomAttribute(typeof(IUnknownDerivedAttribute<,>))).ManagedVirtualMethodTable;
            }
        }
    }
}
