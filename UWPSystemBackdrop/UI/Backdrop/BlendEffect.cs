﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using UWPSystemBackdrop.Helpers;
using UWPSystemBackdrop.WindowsAPI.ComTypes;
using Windows.Graphics.Effects;
using WinRT;

namespace UWPSystemBackdrop.UI.Backdrop
{
    [GeneratedComClass, Guid("81C5B77B-13F8-4CDD-AD20-C890547AC65D")]
    public sealed partial class BlendEffect : IGraphicsEffect, IGraphicsEffectSource, IGraphicsEffectD2D1Interop
    {
        public BlendEffectMode Mode { get; set; } = BlendEffectMode.Multiply;

        public string Name { get; set; } = string.Empty;

        public IGraphicsEffectSource Background { get; set; }

        public IGraphicsEffectSource Foreground { get; set; }

        public int GetEffectId(out Guid id)
        {
            id = typeof(BlendEffect).GUID;
            return 0;
        }

        public int GetNamedPropertyMapping(string name, out uint index, out GRAPHICS_EFFECT_PROPERTY_MAPPING mapping)
        {
            switch (name)
            {
                case nameof(Mode):
                    {
                        index = 0;
                        mapping = GRAPHICS_EFFECT_PROPERTY_MAPPING.GRAPHICS_EFFECT_PROPERTY_MAPPING_DIRECT;
                        break;
                    }
                default:
                    {
                        index = 0xFF;
                        mapping = (GRAPHICS_EFFECT_PROPERTY_MAPPING)0xFF;
                        break;
                    }
            }

            return 0;
        }

        public int GetProperty(uint index, out IntPtr source)
        {
            if (index is 0)
            {
                BackdropHelper.PropertyValueStatics.CreateUInt32((uint)Mode, out IntPtr ptr);
                if (ptr != IntPtr.Zero)
                {
                    source = ptr;
                    return 0;
                }
            }

            source = IntPtr.Zero;
            return -2147483637;
        }

        public int GetPropertyCount(out uint count)
        {
            count = 1;
            return 0;
        }

        public int GetSource(uint index, out IntPtr source)
        {
            if (index is 0)
            {
                source = MarshalInterface<IGraphicsEffectSource>.FromManaged(Background);
                return 0;
            }
            else if (index is 1)
            {
                source = MarshalInterface<IGraphicsEffectSource>.FromManaged(Foreground);
                return 0;
            }
            else
            {
                source = IntPtr.Zero;
                return 2147483637;
            }
        }

        public int GetSourceCount(out uint count)
        {
            count = 2;
            return 0;
        }
    }
}
