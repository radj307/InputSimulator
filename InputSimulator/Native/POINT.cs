using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{x}, {y}")]
    public struct POINT
    {
        #region Constructors
        public POINT() { }
        public POINT(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }
        public POINT((int X, int Y) pos)
        {
            x = pos.X;
            y = pos.Y;
        }
        #endregion Constructors

        #region Fields
        /// <summary>
        /// The horizontal position.
        /// </summary>
        public int x;
        /// <summary>
        /// The vertical position.
        /// </summary>
        public int y;
        #endregion Fields

        #region Methods
        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }
        #endregion Methods

        #region Conversion Operators
        public static implicit operator System.Drawing.Point(POINT point) => new(point.x, point.y);
        public static implicit operator POINT(System.Drawing.Point point) => new(point.X, point.Y);

        public static implicit operator System.Drawing.PointF(POINT point) => new(point.x, point.y);
        public static implicit operator POINT(System.Drawing.PointF point) => new((int)point.X, (int)point.Y);
        #endregion Conversion Operators
    }
}
