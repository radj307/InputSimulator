using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    /// <summary>
    /// Contains information about a simulated mouse event.
    /// </summary>
    /// <remarks>
    /// The full documentation is available on MSDN: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput"/>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("({dx}, {dy}), {dwFlags}, Data: {mouseData}")]
    public struct MOUSEINPUT
    {
        #region Fields
        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member.
        /// Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.
        /// </summary>
        public int dx;
        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member.
        /// Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.
        /// </summary>
        public int dy;
        /// <summary>
        /// Contains event data, the meaning of which depends on the values of the other fields.
        /// </summary>
        /// <remarks>
        /// If dwFlags contains <see cref="Flags.MOUSEEVENTF_WHEEL"/>, then mouseData specifies the amount of wheel movement.
        /// A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
        /// One wheel click is defined as <see cref="WHEEL_DELTA"/>, which is 120.
        /// <br/><br/>
        /// <b>Windows Vista (and up)</b>: If dwFlags contains <see cref="Flags.MOUSEEVENTF_HWHEEL"/>, then dwData specifies the amount of wheel movement.
        /// A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.
        /// One wheel click is defined as <see cref="WHEEL_DELTA"/>, which is 120.
        /// <br/><br/>
        /// If dwFlags does not contain <see cref="Flags.MOUSEEVENTF_WHEEL"/>, <see cref="Flags.MOUSEEVENTF_XDOWN"/>, or <see cref="Flags.MOUSEEVENTF_XUP"/>, then mouseData should be zero.
        /// <br/><br/>
        /// If dwFlags contains <see cref="Flags.MOUSEEVENTF_XDOWN"/> or <see cref="Flags.MOUSEEVENTF_XUP"/>, then mouseData specifies which X buttons were pressed or released.
        /// This value may be any combination of the following flags:
        /// <list type="table">
        /// <item><term><see cref="XBUTTON1"/></term><description> Set if the first X button is pressed or released.</description></item>
        /// <item><term><see cref="XBUTTON2"/></term><description> Set if the second X button is pressed or released.</description></item>
        /// </list>
        /// </remarks>
        public int mouseData;
        /// <summary>
        /// Contains event flags for this instance.
        /// </summary>
        /// <remarks>
        /// A set of bit flags that specify various aspects of mouse motion and button clicks.
        /// The bits in this member can be any reasonable combination of the <see cref="Flags"/> <see langword="enum"/>.
        /// <br/><br/>
        /// The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.
        /// For example, if the left mouse button is pressed and held down, <see cref="Flags.MOUSEEVENTF_LEFTDOWN"/> is set when the left button is first pressed, but not for subsequent motions.
        /// Similarly <see cref="Flags.MOUSEEVENTF_LEFTUP"/> is set only when the button is first released.
        /// <br/><br/>
        /// You cannot specify both the <see cref="Flags.MOUSEEVENTF_WHEEL"/> flag and either <see cref="Flags.MOUSEEVENTF_XDOWN"/> or <see cref="Flags.MOUSEEVENTF_XUP"/> flags simultaneously in the dwFlags parameter, because they both require use of the mouseData field.
        /// </remarks>
        public Flags dwFlags;
        /// <summary>
        /// The time stamp for the event, in milliseconds.
        /// If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public uint time;
        /// <summary>
        /// An additional value associated with the mouse event.
        /// An application calls GetMessageExtraInfo to obtain this extra information.
        /// </summary>
        public IntPtr dwExtraInfo;
        #endregion Fields

        #region Constants
        public const int WHEEL_DELTA = 120;
        public const int XBUTTON1 = 0x0001;
        public const int XBUTTON2 = 0x0002;
        #endregion Constants

        #region (Enum) Flags
        [Flags]
        public enum Flags : uint
        {
            /// <summary>
            /// Movement occurred.
            /// </summary>
            MOUSEEVENTF_MOVE = 0x0001,
            /// <summary>
            /// The left button was pressed.
            /// </summary>
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            /// <summary>
            /// The left button was released.
            /// </summary>
            MOUSEEVENTF_LEFTUP = 0x0004,
            /// <summary>
            /// The right button was pressed.
            /// </summary>
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            /// <summary>
            /// The right button was released.
            /// </summary>
            MOUSEEVENTF_RIGHTUP = 0x0010,
            /// <summary>
            /// The middle button was pressed.
            /// </summary>
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            /// <summary>
            /// The middle button was released.
            /// </summary>
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            /// <summary>
            /// An X button was pressed.
            /// </summary>
            MOUSEEVENTF_XDOWN = 0x0080,
            /// <summary>
            /// An X button was released.
            /// </summary>
            MOUSEEVENTF_XUP = 0x0100,
            /// <summary>
            /// The wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData.
            /// </summary>
            MOUSEEVENTF_WHEEL = 0x0800,
            /// <summary>
            /// The wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in mouseData.
            /// </summary>
            /// <remarks>
            /// <b>Windows XP/2000</b>: This value is not supported.
            /// </remarks>
            MOUSEEVENTF_HWHEEL = 0x1000,
            /// <summary>
            /// The WM_MOUSEMOVE messages will not be coalesced. The default behavior is to coalesce WM_MOUSEMOVE messages.
            /// </summary>
            /// <remarks>
            /// <b>Windows XP/2000</b>: This value is not supported.
            /// </remarks>
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
            /// <summary>
            /// Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
            /// </summary>
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            /// <summary>
            /// The dx and dy members contain normalized absolute coordinates. If the flag is not set, dxand dy contain relative data (the change in position since the last reported position). This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system. For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000,
        }
        #endregion (Enum) Flags
    }
}
