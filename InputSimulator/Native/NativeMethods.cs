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
        public static Win32Exception GetLastWin32Error()
        {
            var hr = Marshal.GetLastWin32Error();

            if (hr == 0) return null!;

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
        #endregion SendInput (P/Invoke)

        #region SendInput
        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <remarks>
        /// See the full documentation on MSDN: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/>.
        /// <br/><br/>
        /// <b>Warning:</b> This function fails when it is blocked by UIPI.
        /// Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.<br/>
        /// See here for more information on UIPI: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changewindowmessagefilter#remarks"/>.
        /// </remarks>
        /// <param name="inputs">Any number of <see cref="INPUT"/> structures representing individual events to be inserted into the keyboard or mouse input streams.</param>
        /// <returns>The number of successful events.</returns>
        public static uint SendInputs(params INPUT[] inputs)
            => SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        /// <inheritdoc cref="SendInputs(INPUT[])"/>
        public static uint SendInputs(IEnumerable<INPUT> inputs)
            => SendInputs(inputs.ToArray());
        /// <param name="input">An <see cref="INPUT"/> structure representing the event to be inserted into the keyboard or mouse input stream.</param>
        /// <inheritdoc cref="SendInputs(INPUT[])"/>
        public static bool SendInput(INPUT input)
            => 1u == SendInput(1u, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
        /// <remarks>
        /// The only difference between this method and <see cref="SendInputs(INPUT[])"/> is that this method returns a <see cref="bool"/>.
        /// <br/><br/>
        /// See the full documentation on MSDN: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/>.
        /// <br/><br/>
        /// <b>Warning:</b> This function fails when it is blocked by UIPI.
        /// Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.<br/>
        /// See here for more information on UIPI: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changewindowmessagefilter#remarks"/>.
        /// </remarks>
        /// <returns><see langword="true"/> when all of the specified <paramref name="inputs"/> were sent successfully; otherwise, <see langword="false"/> when at least one input failed.</returns>
        /// <inheritdoc cref="SendInputs(INPUT[])"/>
        public static bool SendInput(params INPUT[] inputs)
            => inputs.Length == SendInputs(inputs);
        /// <remarks>
        /// The only difference between this method and <see cref="SendInputs(IEnumerable{INPUT})"/> is that this method returns a <see cref="bool"/>.
        /// <br/><br/>
        /// See the full documentation on MSDN: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/>.
        /// <br/><br/>
        /// <b>Warning:</b> This function fails when it is blocked by UIPI.
        /// Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.<br/>
        /// See here for more information on UIPI: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changewindowmessagefilter#remarks"/>.
        /// </remarks>
        /// <returns><see langword="true"/> when all of the specified <paramref name="inputs"/> were sent successfully; otherwise, <see langword="false"/> when at least one input failed.</returns>
        /// <inheritdoc cref="SendInputs(INPUT[])"/>
        public static bool SendInput(IEnumerable<INPUT> inputs)
            => SendInput(inputs.ToArray());
        #endregion SendInput

        #region GetMonitorInfo (P/Invoke)
        [DllImport("user32.dll", CharSet = CharSet.Ansi)] //< use GetMonitorInfoA because the unicode variant doesn't always fill out the struct correctly on Win2K according to System.Windows.Forms.Screen source code
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
        #endregion GetMonitorInfo (P/Invoke)

        #region GetMonitorInfo
        public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
        {
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
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetSystemMetricsForDpi(SystemMetric nIndex, uint dpi);
        #endregion GetSystemMetricsForDpi (P/Invoke)

        #region GetDpiForSystem (P/Invoke)
        [DllImport("user32.dll")]
        static extern uint GetDpiForSystem();
        #endregion GetDpiForSystem (P/Invoke)

        #region GetSystemDpi
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
        public static bool IsWindowMinimized(IntPtr hWnd) => IsIconic(hWnd);
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
        /// This function fails when the window is minimized.
        /// </remarks>
        /// <param name="hWnd">The handle of the target window.</param>
        /// <returns>The window's dimensions as a <see cref="RECT"/> structure when successful; otherwise, a default (zeroed) <see cref="RECT"/> structure.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static RECT GetWindowRect(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hWnd));

            if (IsWindowMinimized(hWnd)) return default;

            GetWindowRect(hWnd, out var rect);
            return rect;
        }
        #endregion GetWindowRect

        #region GetClientRect (P/Invoke)
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        #endregion GetClientRect (P/Invoke)

        #region GetWindowClientRect
        public static RECT GetWindowClientRect(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hWnd));
            GetClientRect(hWnd, out var rect);
            return rect;
        }
        #endregion GetWindowClientRect

        #region GetCursorPos (P/Invoke)
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);
        #endregion GetCursorPos (P/Invoke)

        #region GetCursorPos
        public static POINT GetCursorPos()
        {
            GetCursorPos(out POINT lpPoint);
            return lpPoint;
        }
        public static POINT GetCursorPosAbs()
            => InputHelper.ToAbsCoordinates(GetCursorPos());
        #endregion GetCursorPos
    }
}
