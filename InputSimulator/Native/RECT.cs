using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("Left = {left}, Top = {top}, Right = {right}, Bottom = {bottom}")]
    public struct RECT
    {
        public RECT(int leftPos, int topPos, int rightPos, int bottomPos)
        {
            left = leftPos;
            top = topPos;
            right = rightPos;
            bottom = bottomPos;
        }

        /// <summary>
        /// The X (horizontal) position of the left side of the rectangle.
        /// </summary>
        public int left;
        /// <summary>
        /// The Y (vertical) coordinate of the top side of the rectangle.
        /// </summary>
        public int top;
        /// <summary>
        /// The X (horizontal) position of the right side of the rectangle.
        /// </summary>
        public int right;
        /// <summary>
        /// The Y (vertical) coordinate of the bottom side of the rectangle.
        /// </summary>
        public int bottom;

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int GetWidth() => right - left;
        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int GetHeight() => bottom - top;
    }
}
