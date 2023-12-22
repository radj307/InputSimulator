using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace InputSimulator.Native
{
    /// <summary>
    /// Provides an API for various native Win32 functions.
    /// </summary>
    public static class NativeMethods
    {
        #region FormatMessage (P/Invoke)
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int FormatMessage(FORMAT_MESSAGE dwFlags, IntPtr lpSource, int dwMessageId, uint dwLanguageId, out StringBuilder msgOut, int nSize, IntPtr Arguments);
        [Flags]
        private enum FORMAT_MESSAGE : uint
        {
            /// <summary/>
            ALLOCATE_BUFFER = 0x00000100,
            /// <summary/>
            IGNORE_INSERTS = 0x00000200,
            /// <summary/>
            FROM_SYSTEM = 0x00001000,
            /// <summary/>
            ARGUMENT_ARRAY = 0x00002000,
            /// <summary/>
            FROM_HMODULE = 0x00000800,
            /// <summary/>
            FROM_STRING = 0x00000400
        }
        #endregion FormatMessage (P/Invoke)

        #region GetLastError
        /// <summary>
        /// Gets the last error to have occurred in the Win32 API.
        /// </summary>
        /// <returns>A <see cref="Win32Exception"/> object representing the last error if there is one; otherwise, <see langword="null"/>.</returns>
        public static Win32Exception? GetLastWin32Error()
        {
            var hr = Marshal.GetLastWin32Error();

            if (hr == 0) return null;

            _ = FormatMessage(
                FORMAT_MESSAGE.ALLOCATE_BUFFER | FORMAT_MESSAGE.FROM_SYSTEM | FORMAT_MESSAGE.IGNORE_INSERTS,
                IntPtr.Zero,
                hr,
                0,
                out StringBuilder msg,
                256,
                IntPtr.Zero
            );

            return new(hr, msg.ToString());
        }
        #endregion GetLastError

        #region SendInput (P/Invoke)
        /// <remarks>
        /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        /// <summary>
        /// The size of an <see cref="INPUT"/> structure, in bytes.
        /// </summary>
        private static readonly int SizeOfINPUT = Marshal.SizeOf(typeof(INPUT));
        #endregion SendInput (P/Invoke)

        #region SendInputs
        /// <summary>
        /// Synthesizes any number of keyboard, mouse, and/or hardware input events defined by the specified <paramref name="inputs"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="GetLastWin32Error"/> to get extended error information.
        /// <br/>
        /// See the <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput">MSDN documentation</see> for more information.
        /// </remarks>
        /// <param name="inputs">Any number of <see cref="INPUT"/> structures to send as input, in the order that they will be sent.</param>
        /// <returns>The number of <paramref name="inputs"/> that were successfully sent.</returns>
        public static uint SendInputs(params INPUT[] inputs)
        {
            return SendInput((uint)inputs.Length, inputs, SizeOfINPUT);
        }
        /// <inheritdoc cref="SendInputs(INPUT[])"/>
        public static uint SendInputs(IEnumerable<INPUT> inputs) => SendInputs(inputs.ToArray());
        #endregion SendInputs

        #region SendInput
        /// <returns><see langword="true"/> when all of the <paramref name="inputs"/> were sent successfully; otherwise, <see langword="false"/>.</returns>
        /// <inheritdoc cref="SendInputs(INPUT[])"/>
        public static bool SendInput(params INPUT[] inputs)
        {
            return inputs.Length == SendInputs(inputs);
        }
        /// <inheritdoc cref="SendInput(INPUT[])"/>
        public static bool SendInput(IEnumerable<INPUT> inputs) => SendInput(inputs.ToArray());
        #endregion SendInput

        #region GetMonitorInfo (P/Invoke)
        [DllImport("user32.dll", CharSet = CharSet.Ansi)] //< use GetMonitorInfoA because the unicode variant doesn't always fill out the struct correctly on Win2K according to System.Windows.Forms.Screen source code
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
        #endregion GetMonitorInfo (P/Invoke)

        #region GetMonitorInfo
        /// <summary>
        /// Gets the <see cref="MONITORINFO"/> structure for the monitor with the specified <paramref name="hMonitor"/> handle.
        /// </summary>
        /// <param name="hMonitor">The handle of the monitor to get the information of.</param>
        /// <returns><see cref="MONITORINFO"/> structure that contains the monitor dimensions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hMonitor"/> was <see cref="IntPtr.Zero"/>.</exception>
        public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
        {
            if (hMonitor == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hMonitor));

            MONITORINFO monitorInfo = new();
            GetMonitorInfo(hMonitor, ref monitorInfo);
            return monitorInfo;
        }
        #endregion GetMonitorInfo

        #region MonitorFromPoint (P/Invoke)
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);
        private const uint MONITOR_DEFAULTTONULL = 0;
        private const uint MONITOR_DEFAULTTONEAREST = 2;
        #endregion MonitorFromPoint (P/Invoke)

        #region GetMonitorFromPoint
        /// <summary>
        /// Gets the handle of the monitor that contains the specified <paramref name="point"/>.
        /// </summary>
        /// <remarks>
        /// Do not attempt to dispose of the returned handle, or it will trigger a memory access violation!
        /// </remarks>
        /// <param name="point">A point, specified in virtual screen coordinates.</param>
        /// <returns>The monitor handle when successful; otherwise, <see cref="IntPtr.Zero"/>.</returns>
        public static IntPtr GetMonitorFromPoint(POINT point)
            => MonitorFromPoint(point, MONITOR_DEFAULTTONULL);
        /// <inheritdoc cref="GetMonitorFromPoint(POINT)"/>
        /// <param name="x">The horizontal coordinate of the point, specified in virtual screen coordinates.</param>
        /// <param name="y">The vertical coordinate of the point, specified in virtual screen coordinates.</param>
        public static IntPtr GetMonitorFromPoint(int x, int y)
            => GetMonitorFromPoint(new(x, y));
        #endregion GetMonitorFromPoint

        #region GetNearestMonitorFromPoint
        /// <summary>
        /// Gets the handle of the monitor that contains, or is closest to, the specified <paramref name="point"/>.
        /// </summary>
        /// <remarks>
        /// Do not attempt to dispose of the returned handle, or it will trigger a memory access violation!
        /// </remarks>
        /// <param name="point">A point, specified in virtual screen coordinates.</param>
        /// <returns>The monitor handle.</returns>
        public static IntPtr GetNearestMonitorFromPoint(POINT point)
            => MonitorFromPoint(point, MONITOR_DEFAULTTONEAREST);
        /// <inheritdoc cref="GetNearestMonitorFromPoint(POINT)"/>
        /// <param name="x">The horizontal coordinate of the point, specified in virtual screen coordinates.</param>
        /// <param name="y">The vertical coordinate of the point, specified in virtual screen coordinates.</param>
        public static IntPtr GetNearestMonitorFromPoint(int x, int y)
            => GetNearestMonitorFromPoint(new(x, y));
        #endregion GetNearestMonitorFromPoint

        #region EnumDisplayMonitors (P/Invoke)
        /// <remarks>
        /// MSDN Documentation: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaymonitors"/>
        /// </remarks>
        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MONITORENUMPROC lpfnEnum, IntPtr dwData);
        /// <remarks>
        /// MSDN Documentation: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-monitorenumproc"/>
        /// </remarks>
        [return: MarshalAs(UnmanagedType.Bool)]
        private delegate bool MONITORENUMPROC(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);
        #endregion EnumDisplayMonitors (P/Invoke)

        #region GetAllMonitors
        /// <summary>
        /// Gets the handles for all monitors.
        /// </summary>
        /// <remarks>
        /// Do not attempt to dispose of the returned handles, or a memory access violation will be thrown!
        /// </remarks>
        /// <returns>Monitor handles as an <see cref="IntPtr"/> array.</returns>
        public static IntPtr[] GetAllMonitorHandles()
        {
            List<IntPtr> handles = new();

            EnumDisplayMonitors(
                hdc: IntPtr.Zero,
                lprcClip: IntPtr.Zero,
                lpfnEnum: (handle, _, _, _) =>
                {
                    handles.Add(handle);
                    return true;
                },
                dwData: IntPtr.Zero);

            return handles.ToArray();
        }
        #endregion GetAllMonitors

        #region GetSystemMetricsForDpi (P/Invoke)
        enum SystemMetric : int
        {
            /// <summary>
            /// The number of display monitors on a desktop.
            /// </summary>
            /// <remarks>
            /// GetSystemMetrics(SM_CMONITORS) counts only visible display monitors.
            /// This is different from EnumDisplayMonitors, which enumerates both visible display monitors and invisible pseudo-monitors that are associated with mirroring drivers.
            /// An invisible pseudo-monitor is associated with a pseudo-device used to mirror application drawing for remoting or other purposes.
            /// </remarks>
            SM_CMONITORS = 80,
            /// <summary>
            ///  The coordinates for the left side of the virtual screen.
            ///  The virtual screen is the bounding rectangle of all display monitors.
            ///  The SM_CXVIRTUALSCREEN metric is the width of the virtual screen. 
            /// </summary>
            SM_XVIRTUALSCREEN = 76,
            /// <summary>
            ///  The coordinates for the top of the virtual screen.
            ///  The virtual screen is the bounding rectangle of all display monitors.
            ///  The SM_CYVIRTUALSCREEN metric is the height of the virtual screen. 
            /// </summary>
            SM_YVIRTUALSCREEN = 77,
            /// <summary>
            ///  The width of the virtual screen, in pixels.
            ///  The virtual screen is the bounding rectangle of all display monitors.
            ///  The SM_XVIRTUALSCREEN metric is the coordinates for the left side of the virtual screen. 
            /// </summary>
            SM_CXVIRTUALSCREEN = 78,
            /// <summary>
            ///  The height of the virtual screen, in pixels.
            ///  The virtual screen is the bounding rectangle of all display monitors.
            ///  The SM_YVIRTUALSCREEN metric is the coordinates for the top of the virtual screen. 
            /// </summary>
            SM_CYVIRTUALSCREEN = 79,
        }
        [DllImport("user32.dll")]
        static extern int GetSystemMetricsForDpi(SystemMetric nIndex, uint dpi);
        #endregion GetSystemMetricsForDpi (P/Invoke)

        #region GetDpiForSystem (P/Invoke)
        [DllImport("user32.dll")]
        static extern uint GetDpiForSystem();
        #endregion GetDpiForSystem (P/Invoke)

        #region GetSystemDpi
        /// <summary>
        /// Gets the current system DPI.
        /// </summary>
        /// <returns>The system DPI as a <see cref="uint"/>.</returns>
        public static uint GetSystemDpi() => GetDpiForSystem();
        #endregion GetSystemDpi

        #region GetVirtualScreenX
        /// <summary>
        /// Gets the horizontal coordinate of the virtual screen's origin point.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <returns>The origin point's X-axis value.</returns>
        public static int GetVirtualScreenX(uint dpi) => GetSystemMetricsForDpi(SystemMetric.SM_XVIRTUALSCREEN, dpi);
        /// <inheritdoc cref="GetVirtualScreenX(uint)"/>
        public static int GetVirtualScreenX() => GetSystemMetricsForDpi(SystemMetric.SM_XVIRTUALSCREEN, GetSystemDpi());
        #endregion GetVirtualScreenX

        #region GetVirtualScreenY
        /// <summary>
        /// Gets the vertical coordinate of the virtual screen's origin point.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <returns>The origin point's Y-axis value.</returns>
        public static int GetVirtualScreenY(uint dpi) => GetSystemMetricsForDpi(SystemMetric.SM_YVIRTUALSCREEN, dpi);
        /// <inheritdoc cref="GetVirtualScreenY(uint)"/>
        public static int GetVirtualScreenY() => GetSystemMetricsForDpi(SystemMetric.SM_YVIRTUALSCREEN, GetSystemDpi());
        #endregion GetVirtualScreenY

        #region GetVirtualScreenWidth
        /// <summary>
        /// Gets the width of the virtual screen.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <returns>The width of the virtual screen's bounding rectangle.</returns>
        public static int GetVirtualScreenWidth(uint dpi) => GetSystemMetricsForDpi(SystemMetric.SM_CXVIRTUALSCREEN, dpi);
        /// <inheritdoc cref="GetVirtualScreenWidth(uint)"/>
        public static int GetVirtualScreenWidth() => GetSystemMetricsForDpi(SystemMetric.SM_CXVIRTUALSCREEN, GetSystemDpi());
        #endregion GetVirtualScreenWidth

        #region GetVirtualScreenHeight
        /// <summary>
        /// Gets the height of the virtual screen.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <returns>The height of the virtual screen's bounding rectangle.</returns>
        public static int GetVirtualScreenHeight(uint dpi) => GetSystemMetricsForDpi(SystemMetric.SM_CYVIRTUALSCREEN, dpi);
        /// <inheritdoc cref="GetVirtualScreenHeight(uint)"/>
        public static int GetVirtualScreenHeight() => GetSystemMetricsForDpi(SystemMetric.SM_CYVIRTUALSCREEN, GetSystemDpi());
        #endregion GetVirtualScreenHeight

        #region GetVirtualScreenSize
        /// <summary>
        /// Gets the virtual screen's bounding rectangle as a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <returns>The origin point, width, and height of the virtual screen.</returns>
        public static System.Drawing.Rectangle GetVirtualScreenSize(uint dpi)
        {
            return new(
                x: GetVirtualScreenX(dpi),
                y: GetVirtualScreenY(dpi),
                width: GetVirtualScreenWidth(dpi),
                height: GetVirtualScreenHeight(dpi));
        }
        /// <inheritdoc cref="GetVirtualScreenSize(uint)"/>
        public static System.Drawing.Rectangle GetVirtualScreenSize() => GetVirtualScreenSize(GetSystemDpi());
        #endregion GetVirtualScreenSize

        #region GetVirtualScreenRect
        /// <summary>
        /// Gets the virtual screen's bounding rectangle as a <see cref="RECT"/>.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <returns>The left, top, right, and bottom boundaries of the virtual screen.</returns>
        public static RECT GetVirtualScreenRect(uint dpi)
        {
            var x = GetVirtualScreenX(dpi);
            var y = GetVirtualScreenY(dpi);
            return new RECT(
                left: x,
                top: y,
                right: x + GetVirtualScreenWidth(dpi),
                bottom: y + GetVirtualScreenHeight(dpi));
        }
        /// <inheritdoc cref="GetVirtualScreenRect(uint)"/>
        public static RECT GetVirtualScreenRect() => GetVirtualScreenRect(GetSystemDpi());
        #endregion GetVirtualScreenRect

        #region IsIconic (P/Invoke)
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);
        #endregion IsIconic (P/Invoke)

        #region IsWindowMinimized
        /// <summary>
        /// Checks if the window with the specified <paramref name="hWnd"/> is minimized or not.
        /// </summary>
        /// <param name="hWnd">The handle of the window to check.</param>
        /// <returns><see langword="true"/> when the window is minimized; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hWnd"/> was <see cref="IntPtr.Zero"/>.</exception>
        public static bool IsWindowMinimized(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hWnd));

            return IsIconic(hWnd);
        }
        #endregion IsWindowMinimized

        #region GetWindowRect (P/Invoke)
        /// <remarks>
        /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrect"/>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        #endregion GetWindowRect (P/Invoke)

        #region GetWindowRect
        /// <summary>
        /// Gets the dimensions of the window with the specified <paramref name="hWnd"/>.
        /// </summary>
        /// <remarks>
        /// Window dimensions cannot be retrieved when the window is minimized, and <b>will</b> contain garbage data instead of the expected values when <paramref name="checkIfWindowIsMinimized"/> is <see langword="false"/>.
        /// When <paramref name="checkIfWindowIsMinimized"/> is <see langword="true"/> and the window <i>is</i> minimized, a <see langword="default"/> (zeroed) <see cref="RECT"/> structure is returned instead of trying to retrieve garbage data.
        /// (This is unnecessary if the calling code has already ensured that the window isn't minimized.)
        /// </remarks>
        /// <param name="hWnd">The handle of the target window.</param>
        /// <param name="checkIfWindowIsMinimized">When <see langword="true"/>, checks if the window is minimized before attempting to get its dimensions to prevent returning garbage data.<br/>When <see langword="false"/>, this check is skipped. Use <see langword="false"/> when the calling code has already ensured that the window is not minimized.<br/><br/>To check if a window is minimized, use <see cref="IsWindowMinimized(IntPtr)"/>.</param>
        /// <returns>The window's dimensions as a <see cref="RECT"/> structure.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hWnd"/> was <see cref="IntPtr.Zero"/>.</exception>
        /// <exception cref="Win32Exception">The <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrect">GetWindowRect</see> function failed.</exception>
        public static RECT GetWindowRect(IntPtr hWnd, bool checkIfWindowIsMinimized = true)
        {
            if (hWnd == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hWnd));

            // check if the window is minimized
            if (checkIfWindowIsMinimized && IsWindowMinimized(hWnd))
                return default;

            // get the window rect
            if (!GetWindowRect(hWnd, out var rect) && GetLastWin32Error() is Win32Exception lastError)
                throw lastError;
            return rect;
        }
        #endregion GetWindowRect

        #region TryGetWindowRect
        /// <summary>
        /// Attempts to get the dimensions of the window with the specified <paramref name="hWnd"/>.
        /// </summary>
        /// <remarks>
        /// Window dimensions cannot be retrieved when the window is minimized, and <b>will</b> contain garbage data instead of the expected values when <paramref name="checkIfWindowIsMinimized"/> is <see langword="false"/>.
        /// When <paramref name="checkIfWindowIsMinimized"/> is <see langword="true"/> and the window <i>is</i> minimized, a <see langword="default"/> (zeroed) <see cref="RECT"/> structure is returned instead of trying to retrieve garbage data.
        /// (This is unnecessary if the calling code has already ensured that the window isn't minimized.)
        /// </remarks>
        /// <param name="hWnd">The handle of the target window.</param>
        /// <param name="checkIfWindowIsMinimized">When <see langword="true"/>, checks if the window is minimized before attempting to get its dimensions to prevent returning garbage data.<br/>When <see langword="false"/>, this check is skipped. Use <see langword="false"/> when the calling code has already ensured that the window is not minimized.<br/>To check if a window is minimized, use <see cref="IsWindowMinimized(IntPtr)"/>.</param>
        /// <param name="windowRect">The window's dimensions as a <see cref="RECT"/> structure when successful; otherwise, </param>
        /// <returns><see langword="true"/> when the <paramref name="windowRect"/> was retrieved successfully and is not zeroed; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetWindowRect(IntPtr hWnd, bool checkIfWindowIsMinimized, out RECT windowRect)
        {
            try
            {
                windowRect = GetWindowRect(hWnd, checkIfWindowIsMinimized);
                return !windowRect.IsZeroed;
            }
            catch
            {
                windowRect = default;
                return false;
            }
        }
        /// <returns><see langword="true"/> when the window is not minimized and the <paramref name="windowRect"/> was retrieved successfully; otherwise, <see langword="false"/>.</returns>
        /// <inheritdoc cref="TryGetWindowRect(IntPtr, bool, out RECT)"/>
        public static bool TryGetWindowRect(IntPtr hWnd, out RECT windowRect) => TryGetWindowRect(hWnd, checkIfWindowIsMinimized: true, out windowRect);
        #endregion TryGetWindowRect

        #region GetClientRect (P/Invoke)
        /// <remarks>
        /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclientrect"/>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        #endregion GetClientRect (P/Invoke)

        #region GetWindowClientRect
        /// <summary>
        /// Gets the client area of the window with the specified <paramref name="hWnd"/>.
        /// </summary>
        /// <remarks>
        /// The client area is relative to the top-left corner of the window, so <see cref="RECT.left"/> &amp; <see cref="RECT.top"/> are always zero.
        /// </remarks>
        /// <param name="hWnd">The handle of a window to get the client area of.</param>
        /// <returns>The client area of the specified window as a <see cref="RECT"/> structure.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hWnd"/> was <see cref="IntPtr.Zero"/>.</exception>
        /// <exception cref="Win32Exception">The <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getclientrect">GetClientRect</see> function failed.</exception>
        public static RECT GetWindowClientRect(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hWnd));

            if (!GetClientRect(hWnd, out var rect) && GetLastWin32Error() is Win32Exception lastError)
                throw lastError;
            return rect;
        }
        #endregion GetWindowClientRect

        #region GetCursorPos (P/Invoke)
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);
        #endregion GetCursorPos (P/Invoke)

        #region GetCursorPos
        public static POINT GetCursorPos()
        {
            if (!GetCursorPos(out POINT lpPoint) && GetLastWin32Error() is Win32Exception lastError)
                throw lastError;
            return lpPoint;
        }
        public static POINT GetCursorPosAbs()
            => ScreenCoordinateHelper.ToAbsCoordinates(GetCursorPos());
        #endregion GetCursorPos

        #region GetKeyState (P/Invoke)
        [DllImport("User32.dll", EntryPoint = "GetKeyState")]
        private static extern ushort GetKeyState_Native(ushort nVirtKey);
        #endregion GetKeyState (P/Invoke)

        #region GetKeyState
        /// <summary>
        /// Checks the state of the specified <paramref name="virtualKeyCode"/>.
        /// </summary>
        /// <param name="virtualKeyCode">The virtual key code of the key to get the state of.</param>
        /// <returns>The specified key's state as an <see cref="EKeyStates"/>.</returns>
        public static EKeyStates GetKeyState(EVirtualKeyCode virtualKeyCode)
        {
            var state = GetKeyState_Native((ushort)virtualKeyCode);

            bool isDown = (state & 0x8000) != 0; //< check the most significant bit
            bool isToggled = (state & 0x0001) != 0; //< check the least significant bit

            if (isDown && isToggled)
                return EKeyStates.Down | EKeyStates.Toggled;
            else if (isDown)
                return EKeyStates.Down;
            else if (isToggled)
                return EKeyStates.Toggled;
            else
                return EKeyStates.Up;
        }
        #endregion GetKeyState

        #region MapVirtualKey (P/Invoke)
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);
        private const uint MAPVK_VK_TO_VSC = 0;
        private const uint MAPVK_VSC_TO_VK = 1;
        #endregion MapVirtualKey (P/Invoke)

        #region ScanCodeFromVirtualKeyCode
        /// <summary>
        /// Converts the specified <paramref name="virtualKeyCode"/> to the equivalent scan code.
        /// </summary>
        /// <param name="virtualKeyCode">A virtual key code to convert.</param>
        /// <returns>The scan code of the key represented by the <paramref name="virtualKeyCode"/>.</returns>
        public static ushort ScanCodeFromVirtualKeyCode(EVirtualKeyCode virtualKeyCode)
            => unchecked((ushort)MapVirtualKey((ushort)virtualKeyCode, MAPVK_VK_TO_VSC));
        #endregion ScanCodeFromVirtualKeyCode

        #region VirtualKeyCodeFromScanCode
        /// <summary>
        /// Converts the specified <paramref name="scanCode"/> to the equivalent <see cref="EVirtualKeyCode"/>.
        /// </summary>
        /// <param name="scanCode">A scan code to convert.</param>
        /// <returns>The <see cref="EVirtualKeyCode"/> of the key with the specified <paramref name="scanCode"/>.</returns>
        public static EVirtualKeyCode VirtualKeyCodeFromScanCode(ushort scanCode)
            => unchecked((EVirtualKeyCode)MapVirtualKey(scanCode, MAPVK_VSC_TO_VK));
        #endregion VirtualKeyCodeFromScanCode
    }
    public static class ScreenCoordinateHelper
    {
        #region Normalize
        /// <summary>
        /// Normalizes the specified <paramref name="value"/> from the specified <paramref name="oldRange"/> to the specified <paramref name="newRange"/>.
        /// </summary>
        /// <param name="value">A value within the specified <paramref name="oldRange"/> to normalize to the <paramref name="newRange"/>.</param>
        /// <param name="oldRange">The old minimum and maximum range boundaries.</param>
        /// <param name="newRange">The new minimum and maximum range boundaries.</param>
        /// <returns><paramref name="value"/> as its equivalent in the specified <paramref name="newRange"/>.</returns>
        public static double Normalize(double value, (double Min, double Max) oldRange, (double Min, double Max) newRange)
            => newRange.Min + (value - oldRange.Min) * (newRange.Max - newRange.Min) / (oldRange.Max - oldRange.Min);
        /// <summary>
        /// Normalizes the specified <paramref name="value"/> from the specified <paramref name="oldRange"/> to the specified <paramref name="newRange"/>, and truncates the result to an <see cref="int"/>.
        /// </summary>
        /// <inheritdoc cref="Normalize(double, (double Min, double Max), (double Min, double Max))"/>
        public static int NormalizeInt(double value, (double Min, double Max) oldRange, (double Min, double Max) newRange)
            => (int)Math.Truncate(Normalize(value, oldRange, newRange));
        #endregion Normalize

        #region ToAbsCoordinateX
        /// <summary>
        /// Converts the specified <paramref name="xPixels"/> value to absolute coordinates.
        /// </summary>
        /// <param name="xPixels">A horizontal position (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns>The equivalent absolute horizontal coordinate.</returns>
        public static int ToAbsCoordinateX(int xPixels, RECT virtualScreenRect)
        {
            return NormalizeInt(xPixels, (virtualScreenRect.left, virtualScreenRect.right), (ushort.MinValue, ushort.MaxValue));
        }
        /// <inheritdoc cref="ToAbsCoordinateX(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int ToAbsCoordinateX(int xPixels) => ToAbsCoordinateX(xPixels, NativeMethods.GetVirtualScreenRect());
        #endregion ToAbsCoordinateX

        #region ToAbsCoordinateY
        /// <summary>
        /// Converts the specified <paramref name="yPixels"/> value to absolute coordinates.
        /// </summary>
        /// <param name="yPixels">A vertical position (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns>The equivalent absolute vertical coordinate.</returns>
        public static int ToAbsCoordinateY(int yPixels, RECT virtualScreenRect)
        {
            return NormalizeInt(yPixels, (virtualScreenRect.top, virtualScreenRect.bottom), (ushort.MinValue, ushort.MaxValue));
        }
        /// <inheritdoc cref="ToAbsCoordinateY(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int ToAbsCoordinateY(int yPixels) => ToAbsCoordinateY(yPixels, NativeMethods.GetVirtualScreenRect());
        #endregion ToAbsCoordinateY

        #region ToAbsCoordinates
        /// <summary>
        /// Converts the specified <paramref name="xPixels"/> &amp; <paramref name="yPixels"/> values to absolute coordinates.
        /// </summary>
        /// <param name="xPixels">A horizontal position (in pixels) to convert.</param>
        /// <param name="yPixels">A vertical position (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns><see cref="POINT"/> containing the equivalent absolute coordinates.</returns>
        public static POINT ToAbsCoordinates(int xPixels, int yPixels, RECT virtualScreenRect)
        {
            return new(
                xPos: ToAbsCoordinateX(xPixels, virtualScreenRect),
                yPos: ToAbsCoordinateY(yPixels, virtualScreenRect));
        }
        /// <inheritdoc cref="ToAbsCoordinates(ushort, ushort, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT ToAbsCoordinates(int xPixels, int yPixels) => ToAbsCoordinates(xPixels, yPixels, NativeMethods.GetVirtualScreenRect());

        // with POINT:

        /// <summary>
        /// Converts the specified <paramref name="pxPoint"/> to absolute coordinates.
        /// </summary>
        /// <param name="pxPoint">A point (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns><see cref="POINT"/> containing the equivalent absolute coordinates.</returns>
        public static POINT ToAbsCoordinates(POINT pxPoint, RECT virtualScreenRect)
        {
            return new(
                xPos: ToAbsCoordinateX(pxPoint.x, virtualScreenRect),
                yPos: ToAbsCoordinateY(pxPoint.y, virtualScreenRect));
        }
        /// <inheritdoc cref="ToAbsCoordinates(POINT, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT ToAbsCoordinates(POINT pxPoint) => ToAbsCoordinates(pxPoint, NativeMethods.GetVirtualScreenRect());
        #endregion ToAbsCoordinates

        #region FromAbsCoordinateX
        public static int FromAbsCoordinateX(int xAbs, RECT virtualScreenRect)
        {
            return NormalizeInt(xAbs, (ushort.MinValue, ushort.MaxValue), (virtualScreenRect.left, virtualScreenRect.right));
        }
        /// <inheritdoc cref="FromAbsCoordinateX(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int FromAbsCoordinateX(int xAbs) => FromAbsCoordinateX(xAbs, NativeMethods.GetVirtualScreenRect());
        #endregion FromAbsCoordinateX

        #region FromAbsCoordinateY
        public static int FromAbsCoordinateY(int yAbs, RECT virtualScreenRect)
        {
            return NormalizeInt(yAbs, (ushort.MinValue, ushort.MaxValue), (virtualScreenRect.top, virtualScreenRect.bottom));
        }
        /// <inheritdoc cref="FromAbsCoordinateY(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int FromAbsCoordinateY(int yAbs) => FromAbsCoordinateY(yAbs, NativeMethods.GetVirtualScreenRect());
        #endregion FromAbsCoordinateY

        #region FromAbsCoordinates
        public static POINT FromAbsCoordinates(ushort xAbs, ushort yAbs, RECT virtualScreenRect)
        {
            return new(
                xPos: FromAbsCoordinateX(xAbs, virtualScreenRect),
                yPos: FromAbsCoordinateY(yAbs, virtualScreenRect));
        }
        /// <inheritdoc cref="FromAbsCoordinates(ushort, ushort, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT FromAbsCoordinates(ushort xAbs, ushort yAbs) => FromAbsCoordinates(xAbs, yAbs, NativeMethods.GetVirtualScreenRect());

        // with POINT:

        public static POINT FromAbsCoordinates(POINT absPoint, RECT virtualScreenRect)
        {
            return new(
                xPos: FromAbsCoordinateX(absPoint.x, virtualScreenRect),
                yPos: FromAbsCoordinateY(absPoint.y, virtualScreenRect));
        }
        /// <inheritdoc cref="FromAbsCoordinates(POINT, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT FromAbsCoordinates(POINT absPoint) => FromAbsCoordinates(absPoint, NativeMethods.GetVirtualScreenRect());
        #endregion FromAbsCoordinates

    }
}
