using InputSimulator.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InputSimulator
{
    [Flags]
    public enum EModifierKeys : byte
    {
        None = 0x0,
        /// <summary>
        /// Alt modifier key.
        /// </summary>
        Alt = 0x1,
        /// <summary>
        /// Ctrl modifier key.
        /// </summary>
        Ctrl = 0x2,
        /// <summary>
        /// Shift modifier key.
        /// </summary>
        Shift = 0x4,
        /// <summary>
        /// Windows modifier key.
        /// </summary>
        Win = 0x8,

        /// <summary>
        /// Super modifier key.
        /// </summary>
        /// <remarks>Alternative name for <see cref="Win"/> used by Linux.</remarks>
        Super = Win,
        /// <summary>
        /// Command modifier key.
        /// </summary>
        /// <remarks>Alternative name for <see cref="Win"/> used by Apple.</remarks>
        Cmd = Win,
    }

    public static class ModifierKeysExtensions
    {
        private static EVirtualKeyCode ModifierKeyToVirtualKey(EModifierKeys singleModifierKeyValue, bool distinguishLeftAndRight, bool useLeftVariant)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (singleModifierKeyValue)
            {
            case EModifierKeys.None:
                return EVirtualKeyCode.None;
            case EModifierKeys.Alt:
                return distinguishLeftAndRight
                    ? useLeftVariant ? EVirtualKeyCode.VK_LMENU : EVirtualKeyCode.VK_RMENU
                    : EVirtualKeyCode.VK_MENU;
            case EModifierKeys.Ctrl:
                return distinguishLeftAndRight
                    ? useLeftVariant ? EVirtualKeyCode.VK_LCONTROL : EVirtualKeyCode.VK_RCONTROL
                    : EVirtualKeyCode.VK_CONTROL;
            case EModifierKeys.Shift:
                return distinguishLeftAndRight
                    ? useLeftVariant ? EVirtualKeyCode.VK_LSHIFT : EVirtualKeyCode.VK_RSHIFT
                    : EVirtualKeyCode.VK_SHIFT;
            case EModifierKeys.Win:
                return useLeftVariant ? EVirtualKeyCode.VK_LWIN : EVirtualKeyCode.VK_RWIN;
            default:
                throw new InvalidEnumArgumentException(nameof(singleModifierKeyValue), (int)singleModifierKeyValue, typeof(EModifierKeys));
            }
#pragma warning restore IDE0066 // Convert switch statement to expression
        }

        public static EVirtualKeyCode[] ToVirtualKeyCodes(this EModifierKeys modifierKeys, bool useLeftSideKeyCodes)
        {
            List<EVirtualKeyCode> l = new();
            foreach (var modifierKey in modifierKeys.GetSingleValues())
            {
                l.Add(ModifierKeyToVirtualKey(modifierKey, distinguishLeftAndRight: true, useLeftSideKeyCodes));
            }
            return l.ToArray();
        }
        public static EVirtualKeyCode[] ToVirtualKeyCodes(this EModifierKeys modifierKeys)
        {
            List<EVirtualKeyCode> l = new();
            foreach (var modifierKey in modifierKeys.GetSingleValues())
            {
                l.Add(ModifierKeyToVirtualKey(modifierKey, distinguishLeftAndRight: false, useLeftVariant: true));
            }
            return l.ToArray();
        }
    }
}
