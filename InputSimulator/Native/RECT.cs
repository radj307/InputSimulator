using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputSimulator.Native
{
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("Left = {left}, Top = {top}, Right = {right}, Bottom = {bottom}")]
    public struct RECT
    {
        #region Constructor
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        public RECT() { }
        #endregion Constructor

        #region Fields
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
        #endregion Fields

        #region Properties
        /// <returns><see langword="true"/> when all fields are equal to 0; otherwise, <see langword="false"/>.</returns>
        public bool IsZeroed => left == 0 && top == 0 && right == 0 && bottom == 0;
        #endregion Properties

        #region Methods
        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int GetWidth() => right - left;
        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int GetHeight() => bottom - top;

        /// <summary>
        /// Converts this instance to a <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <returns>An equivalent <see cref="System.Drawing.Rectangle"/> structure.</returns>
        public System.Drawing.Rectangle ToRectangle() => System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
        /// <summary>
        /// Converts the specified <paramref name="rectangle"/> to a <see cref="RECT"/>.
        /// </summary>
        /// <param name="rectangle">A <see cref="System.Drawing.Rectangle"/> structure to convert.</param>
        /// <returns>The equivalent <see cref="RECT"/> structure.</returns>
        public static RECT FromRectangle(System.Drawing.Rectangle rectangle) => new(
            left: rectangle.Left,
            top: rectangle.Top,
            right: rectangle.Right,
            bottom: rectangle.Bottom);
        #endregion Methods

        #region Conversion Operators
        /// <summary>
        /// Converts the specified <paramref name="rect"/> to a <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">A <see cref="RECT"/> structure to convert.</param>
        public static implicit operator System.Drawing.Rectangle(RECT rect) => rect.ToRectangle();
        /// <inheritdoc cref="FromRectangle(System.Drawing.Rectangle)"/>
        public static implicit operator RECT(System.Drawing.Rectangle rectangle) => FromRectangle(rectangle);
        #endregion Conversion Operators
    }
}
