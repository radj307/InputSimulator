using InputSimulator;
using System;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput"/>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        #region Fields
        /// <summary>
        /// A virtual-key code.
        /// The code must be a value in the range 1 to 254. If the dwFlags member specifies <see cref="Flags.KEYEVENTF_UNICODE"/>, wVk must be 0.
        /// </summary>
        public EVirtualKeyCode wVk;
        /// <summary>
        /// A hardware scan code for the key.
        /// If dwFlags specifies <see cref="Flags.KEYEVENTF_UNICODE"/>, wScan specifies a Unicode character which is to be sent to the foreground application.
        /// </summary>
        public ushort wScan;
        /// <summary>
        /// Specifies various aspects of a keystroke.
        /// </summary>
        public Flags dwFlags;
        /// <summary>
        /// The time stamp for the event, in milliseconds.
        /// If this parameter is zero, the system will provide its own time stamp.
        /// </summary>
        public uint time;
        /// <summary>
        /// An additional value associated with the keystroke.
        /// Use the GetMessageExtraInfo function to obtain this information.
        /// </summary>
        public IntPtr dwExtraInfo;
        #endregion Fields

        #region (Enum) Flags
        [Flags]
        public enum Flags : uint
        {
            /// <summary>
            /// If specified, the key is being pressed.
            /// </summary>
            KEYEVENTF_KEYDOWN = 0x0000,
            /// <summary>
            /// If specified, the wScan scan code consists of a sequence of two bytes, where the first byte has a value of 0xE0. See Extended-Key Flag for more info.
            /// </summary>
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            /// <summary>
            /// If specified, the key is being released. If not specified, the key is being pressed.
            /// </summary>
            KEYEVENTF_KEYUP = 0x0002,
            /// <summary>
            /// If specified, wScan identifies the key and wVk is ignored.
            /// </summary>
            KEYEVENTF_SCANCODE = 0x0008,
            /// <summary>
            /// If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section. 
            /// </summary>
            KEYEVENTF_UNICODE = 0x0004,
        }
        #endregion (Enum) Flags

        #region Methods
        public static KEYBDINPUT GetKeyDown(EVirtualKeyCode keyCode)
            => new() { wVk = keyCode };
        public static KEYBDINPUT GetKeyUp(EVirtualKeyCode keyCode)
            => new() { wVk = keyCode, dwFlags = Flags.KEYEVENTF_KEYUP };
        public static KEYBDINPUT GetKeyDown(char @char, bool isExtendedKey)
        {
            var keyDownInput = new KEYBDINPUT() { wScan = @char, dwFlags = Flags.KEYEVENTF_UNICODE };

            if (isExtendedKey)
                keyDownInput.dwFlags |= Flags.KEYEVENTF_EXTENDEDKEY;

            return keyDownInput;
        }
        public static KEYBDINPUT GetKeyDown(char @char) => GetKeyDown(@char, InputHelper.IsExtendedKeyChar(@char));
        public static KEYBDINPUT GetKeyUp(char @char, bool isExtendedKey)
        {
            var keyUpInput = new KEYBDINPUT() { wScan = @char, dwFlags = Flags.KEYEVENTF_UNICODE | Flags.KEYEVENTF_KEYUP };

            if (isExtendedKey)
                keyUpInput.dwFlags |= Flags.KEYEVENTF_EXTENDEDKEY;

            return keyUpInput;
        }
        public static KEYBDINPUT GetKeyUp(char @char) => GetKeyUp(@char, InputHelper.IsExtendedKeyChar(@char));
        #endregion Methods
    }
}
