using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    /// <summary>
    /// Used by SendInput to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks.
    /// </summary>
    /// <remarks>
    /// The full documentation is available on MSDN: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input"/>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{type}")]
    public struct INPUT
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="INPUT"/> instance with the specified <paramref name="mouseInput"/> data.
        /// </summary>
        /// <param name="mouseInput">A <see cref="MOUSEINPUT"/> instance.</param>
        public INPUT(MOUSEINPUT mouseInput)
        {
            type = Type.INPUT_MOUSE;
            data = new() { mi = mouseInput };
        }
        /// <summary>
        /// Creates a new <see cref="INPUT"/> instance with the specified <paramref name="keyboardInput"/> data.
        /// </summary>
        /// <param name="keyboardInput">A <see cref="KEYBDINPUT"/> instance.</param>
        public INPUT(KEYBDINPUT keyboardInput)
        {
            type = Type.INPUT_KEYBOARD;
            data = new() { ki = keyboardInput };
        }
        /// <summary>
        /// Creates a new <see cref="INPUT"/> instance with the specified <paramref name="hardwareInput"/> data.
        /// </summary>
        /// <param name="hardwareInput">A <see cref="HARDWAREINPUT"/> instance.</param>
        public INPUT(HARDWAREINPUT hardwareInput)
        {
            type = Type.INPUT_HARDWARE;
            data = new() { hi = hardwareInput };
        }
        #endregion Constructors

        #region Fields
        /// <summary>
        /// The type of the input event. This can be any <see cref="Type"/> value.
        /// </summary>
        /// <returns>The type value for this instance, which determines </returns>
        public Type type;
        /// <summary>
        /// Union containing the type-specific data for this input event.
        /// </summary>
        public DUMMYUNIONNAME data;
        #endregion Fields

        #region (Struct) DUMMYUNIONNAME
        [StructLayout(LayoutKind.Explicit)]
        public struct DUMMYUNIONNAME
        {
            /// <summary>
            /// Input event data for mouse events.
            /// </summary>
            /// <remarks>
            /// Do not access this field unless the type is <see cref="INPUT_MOUSE"/>!
            /// </remarks>
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            /// <summary>
            /// Input event data for keyboard events.
            /// </summary>
            /// <remarks>
            /// Do not access this field unless the type is <see cref="INPUT_KEYBOARD"/>!
            /// </remarks>
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            /// <summary>
            /// Input event data for hardware events.
            /// </summary>
            /// <remarks>
            /// Do not access this field unless the type is <see cref="INPUT_HARDWARE"/>!
            /// </remarks>
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        #endregion (Struct) DUMMYUNIONNAME

        #region (Enum) TYPE
        public enum Type : uint
        {
            /// <summary>
            /// Mouse event type. Use <see cref="mi"/>.
            /// </summary>
            INPUT_MOUSE = 0,
            /// <summary>
            /// Keyboard event type. Use <see cref="ki"/>.
            /// </summary>
            INPUT_KEYBOARD = 1,
            /// <summary>
            /// Hardware event type. Use <see cref="hi"/>.
            /// </summary>
            INPUT_HARDWARE = 2,
        }
        #endregion (Enum) TYPE
    }
}
