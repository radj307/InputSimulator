using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{x}, {y}")]
    public struct POINT
    {
        public POINT() { }
        public POINT(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }

        /// <summary>
        /// The horizontal position.
        /// </summary>
        public int x;
        /// <summary>
        /// The vertical position.
        /// </summary>
        public int y;
    }
}
