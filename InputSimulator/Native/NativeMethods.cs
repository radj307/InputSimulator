using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    /// <summary>
    /// Provides an API for various native Win32 functions.
    /// </summary>
    public static class NativeMethods
    {
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
        public static uint SendInput(params INPUT[] inputs)
            => SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        /// <inheritdoc cref="SendInput(INPUT[])"/>
        public static uint SendInput(IEnumerable<INPUT> inputs)
            => SendInput(inputs.ToArray());
        /// <param name="input">An <see cref="INPUT"/> structure representing the event to be inserted into the keyboard or mouse input stream.</param>
        /// <inheritdoc cref="SendInput(INPUT[])"/>
        public static bool SendInput(INPUT input)
            => 1u == SendInput(1u, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
        #endregion SendInput

        #region GetMonitorInfo (P/Invoke)
        [DllImport("user32.dll")]
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

        #region GetSystemMetricsForDpi (P/Invoke)
        enum SystemMetric : int
        {
            /// <summary>
            ///  The width of the screen of the primary display monitor, in pixels.
            ///  This is the same value obtained by calling GetDeviceCaps as follows: GetDeviceCaps(hdcPrimaryMonitor, HORZRES). 
            /// </summary>
            SM_CXSCREEN = 0,
            /// <summary>
            ///  The height of the screen of the primary display monitor, in pixels.
            ///  This is the same value obtained by calling GetDeviceCaps as follows: GetDeviceCaps(hdcPrimaryMonitor, VERTRES). 
            /// </summary>
            SM_CYSCREEN = 1,
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
        public static Rectangle GetVirtualScreenSize(uint dpi)
        {
            return new(
                x: GetVirtualScreenX(dpi),
                y: GetVirtualScreenY(dpi),
                width: GetVirtualScreenWidth(dpi),
                height: GetVirtualScreenHeight(dpi));
        }
        /// <inheritdoc cref="GetVirtualScreenSize(uint)"/>
        public static Rectangle GetVirtualScreenSize() => GetVirtualScreenSize(GetSystemDpi());
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
                leftPos: x,
                topPos: y,
                rightPos: x + GetVirtualScreenWidth(dpi),
                bottomPos: y + GetVirtualScreenHeight(dpi));
        }
        /// <inheritdoc cref="GetVirtualScreenRect(uint)"/>
        public static RECT GetVirtualScreenRect() => GetVirtualScreenRect(GetSystemDpi());
        #endregion GetVirtualScreenRect
    }
}
