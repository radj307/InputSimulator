using System;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        #region Constructor
        /// <summary>
        /// Creates a blank <see cref="MONITORINFO"/> structure.
        /// </summary>
        [Obsolete($"You should use {nameof(NativeMethods)}.{nameof(NativeMethods.GetMonitorInfo)} to get a {nameof(MONITORINFO)} struct instead of calling the constructor.", error: true)]
        public MONITORINFO() { }
        #endregion Constructor

        #region Fields
        /// <summary>
        /// The size of the structure, in bytes.
        /// </summary>
        public readonly uint cbSize = unchecked((uint)Marshal.SizeOf(typeof(MONITORINFO)));
        /// <summary>
        /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates.
        /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>
        public RECT rcMonitor;
        /// <summary>
        /// A RECT structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates.
        /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>
        public RECT rcWork;
        /// <summary>
        /// A set of flags that represent attributes of the display monitor.
        /// </summary>
        public Flags dwFlags;
        #endregion Fields

        #region Properties
        /// <returns><see langword="true"/> when the monitor is the primary display monitor; otherwise, <see langword="false"/>.</returns>
        public bool IsPrimaryMonitor => dwFlags.HasFlag(Flags.MONITORINFOF_PRIMARY);
        #endregion Properties

        #region (Enum) Flags
        public enum Flags : uint
        {
            MONITORINFOF_PRIMARY = 1,
        }
        #endregion (Enum) Flags
    }
}
