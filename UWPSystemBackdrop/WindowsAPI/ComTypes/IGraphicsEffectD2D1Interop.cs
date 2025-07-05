using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UWPSystemBackdrop.WindowsAPI.ComTypes;
using Windows.Graphics.Effects;
using WinRT;
using WinRT.Interop;

// 抑制 IDE0030 警告
#pragma warning disable IDE0030

namespace ABI.UWPSystemBackdrop.WindowsAPI.ComTypes
{
    public static class IGraphicsEffectD2D1InteropMethods
    {
        public static Guid IID { get; } = typeof(IGraphicsEffectD2D1Interop).GUID;
        public static nint AbiToProjectionVftablePtr { get; } = IGraphicsEffectD2D1Interop.Vftbl.InitVtbl();
    }
}

namespace UWPSystemBackdrop.WindowsAPI.ComTypes
{
    [WindowsRuntimeType, Guid("2FC57384-A068-44D7-A331-30982FCF7177")]
    public partial interface IGraphicsEffectD2D1Interop
    {
        int GetEffectId(out Guid id);

        int GetNamedPropertyMapping(nint name, out uint index, out GRAPHICS_EFFECT_PROPERTY_MAPPING mapping);

        int GetPropertyCount(out uint count);

        int GetProperty(uint index, out nint value);

        int GetSource(uint index, out IGraphicsEffectSource source);

        int GetSourceCount(out uint count);

        internal unsafe struct Vftbl
        {
            public static nint InitVtbl()
            {
                Vftbl* lpVtbl = (Vftbl*)ComWrappersSupport.AllocateVtableMemory(typeof(Vftbl), sizeof(Vftbl));

                lpVtbl->IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl;
                lpVtbl->GetEffectId = &GetEffectIdFromAbi;
                lpVtbl->GetNamedPropertyMapping = &GetNamedPropertyMappingFromAbi;
                lpVtbl->GetPropertyCount = &GetPropertyCountFromAbi;
                lpVtbl->GetProperty = &GetPropertyFromAbi;
                lpVtbl->GetSource = &GetSourceFromAbi;
                lpVtbl->GetSourceCount = &GetSourceCountFromAbi;
                return (nint)lpVtbl;
            }

            private IUnknownVftbl IUnknownVftbl;

            // interface delegates
            private delegate* unmanaged[MemberFunction]<nint, Guid*, int> GetEffectId;

            private delegate* unmanaged[MemberFunction]<nint, nint, uint*, GRAPHICS_EFFECT_PROPERTY_MAPPING*, int> GetNamedPropertyMapping;
            private delegate* unmanaged[MemberFunction]<nint, uint*, int> GetPropertyCount;
            private delegate* unmanaged[MemberFunction]<nint, uint, nint*, int> GetProperty;
            private delegate* unmanaged[MemberFunction]<nint, uint, nint*, int> GetSource;
            private delegate* unmanaged[MemberFunction]<nint, uint*, int> GetSourceCount;

            // interface implementation
            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvMemberFunction)])]
            private static int GetEffectIdFromAbi(nint thisPtr, Guid* value)
            {
                try
                {
                    if (value != null)
                    {
                        *value = Guid.Empty;
                    }

                    int hr = ComWrappersSupport.FindObject<IGraphicsEffectD2D1Interop>(thisPtr).GetEffectId(out Guid v);
                    if (hr >= 0)
                    {
                        if (value != null)
                        {
                            *value = v;
                        }
                    }
                    return hr;
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);
                    return Marshal.GetHRForException(e);
                }
            }

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvMemberFunction)])]
            private static int GetNamedPropertyMappingFromAbi(nint thisPtr, nint name, uint* index, GRAPHICS_EFFECT_PROPERTY_MAPPING* mapping)
            {
                try
                {
                    if (index != null)
                    {
                        *index = 0;
                    }

                    if (mapping != null)
                    {
                        *mapping = 0;
                    }

                    int hr = ComWrappersSupport.FindObject<IGraphicsEffectD2D1Interop>(thisPtr).GetNamedPropertyMapping(name, out uint i, out GRAPHICS_EFFECT_PROPERTY_MAPPING m);
                    if (hr >= 0)
                    {
                        if (index != null)
                        {
                            *index = i;
                        }

                        if (mapping != null)
                        {
                            *mapping = m;
                        }
                    }
                    return hr;
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);
                    return Marshal.GetHRForException(e);
                }
            }

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvMemberFunction)])]
            private static int GetPropertyCountFromAbi(nint thisPtr, uint* value)
            {
                try
                {
                    if (value != null)
                    {
                        *value = 0;
                    }

                    int hr = ComWrappersSupport.FindObject<IGraphicsEffectD2D1Interop>(thisPtr).GetPropertyCount(out uint v);
                    if (hr >= 0)
                    {
                        *value = v;
                    }
                    return hr;
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);
                    return Marshal.GetHRForException(e);
                }
            }

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvMemberFunction)])]
            private static int GetPropertyFromAbi(nint thisPtr, uint index, nint* value)
            {
                try
                {
                    if (value != null)
                    {
                        *value = 0;
                    }

                    int hr = ComWrappersSupport.FindObject<IGraphicsEffectD2D1Interop>(thisPtr).GetProperty(index, out nint v);
                    if (hr >= 0)
                    {
                        *value = v;
                    }
                    return hr;
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);
                    return Marshal.GetHRForException(e);
                }
            }

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvMemberFunction)])]
            private static int GetSourceFromAbi(nint thisPtr, uint index, nint* value)
            {
                try
                {
                    if (value != null)
                    {
                        *value = 0;
                    }

                    int hr = ComWrappersSupport.FindObject<IGraphicsEffectD2D1Interop>(thisPtr).GetSource(index, out IGraphicsEffectSource v);
                    if (hr >= 0)
                    {
                        nint unk = MarshalInspectable<IGraphicsEffectSource>.FromManaged(v!);
                        *value = unk;
                    }
                    return hr;
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);
                    return Marshal.GetHRForException(e);
                }
            }

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvMemberFunction)])]
            private static int GetSourceCountFromAbi(nint thisPtr, uint* value)
            {
                try
                {
                    if (value != null)
                    {
                        *value = 0;
                    }

                    int hr = ComWrappersSupport.FindObject<IGraphicsEffectD2D1Interop>(thisPtr).GetSourceCount(out uint v);
                    if (hr >= 0)
                    {
                        *value = v;
                    }
                    return hr;
                }
                catch (Exception e)
                {
                    ExceptionHelpers.SetErrorInfo(e);
                    return Marshal.GetHRForException(e);
                }
            }
        }
    }
}