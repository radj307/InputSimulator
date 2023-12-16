﻿using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    /// <summary>
    /// Defines a simulated hardware input event generated by an input device other than a keyboard or mouse.
    /// </summary>
    /// <remarks>
    /// MSDN Documentation: <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-hardwareinput"/>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        #region Constructor
        /// <summary>
        /// Creates a new <see cref="HARDWAREINPUT"/> instance with the specified <paramref name="uMsg"/>.
        /// </summary>
        /// <param name="uMsg">The hardware input message code.</param>
        public HARDWAREINPUT(uint uMsg)
        {
            this.uMsg = uMsg;
            (wParamH, wParamL) = GetHighAndLowOrderBytes(uMsg);
        }
        #endregion Constructor

        #region Fields
        /// <summary>
        /// The message generated by the input hardware.
        /// </summary>
        public uint uMsg;
        /// <summary>
        /// The low-order word of the lParam parameter for uMsg.
        /// </summary>
        public ushort wParamL;
        /// <summary>
        /// The high-order word of the lParam parameter for uMsg.
        /// </summary>
        public ushort wParamH;
        #endregion Fields

        #region GetHighAndLowOrderBytes
        /// <summary>
        /// Splits the specified 4-byte <see cref="uint"/> <paramref name="value"/> to retrieve the high and low order bytes.
        /// </summary>
        /// <param name="value">A 4-byte <see cref="uint"/> value to split.</param>
        /// <returns>A pair of 2-byte <see cref="ushort"/> values that contain the high and low order bytes from the specified <paramref name="value"/>.</returns>
        private static (ushort HighOrder, ushort LowOrder) GetHighAndLowOrderBytes(uint value)
            => (HighOrder: (ushort)((ushort)(value & 0xFFFF0000) >> (Marshal.SizeOf<ushort>() * 8)), LowOrder: (ushort)(value & 0x0000FFFF));
        #endregion GetHighAndLowOrderBytes
    }
}
