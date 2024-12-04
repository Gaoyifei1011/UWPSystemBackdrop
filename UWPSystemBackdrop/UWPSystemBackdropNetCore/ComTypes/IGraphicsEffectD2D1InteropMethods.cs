using System;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using UWPSystemBackdropNetCore.ComTypes;

// 抑制 IDE0130 警告
#pragma warning disable IDE0130

namespace ABI.UWPSystemBackdropNetCore.ComTypes
{
    public static unsafe class IGraphicsEffectD2D1InteropMethods
    {
        public static Guid IID { get; } = typeof(IGraphicsEffectD2D1Interop).GUID;

        public static nint AbiToProjectionVftablePtr { get; } = (nint)((IIUnknownDerivedDetails)typeof(IGraphicsEffectD2D1Interop).GetCustomAttribute(typeof(IUnknownDerivedAttribute<,>))).ManagedVirtualMethodTable;
    }
}
