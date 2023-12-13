﻿using InputSimulator.Native;
using System;

namespace InputSimulator
{
    /// <summary>
    /// Provides <see langword="static"/> methods for simulating mouse input.
    /// </summary>
    public static class MouseInputMethods
    {
        #region Constants
        /// <summary>
        /// The default scroll wheel delta value.
        /// </summary>
        public const int WHEEL_DELTA = 120;
        #endregion Constants

        #region MoveBy
        /// <summary>
        /// Synthesizes the mouse moving by the specified offset, relative to its current position.
        /// </summary>
        /// <param name="xOffset">The distance (in pixels) to move on the horizontal axis. Negative is left, positive is right.</param>
        /// <param name="yOffset">The distance (in pixels) to move on the vertical axis. Negative is up, positive is down.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MoveBy(int xOffset, int yOffset)
        {
            return NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE,
                dx = xOffset,
                dy = yOffset
            }));
        }
        /// <summary>
        /// Synthesizes the mouse moving by the specified <paramref name="offset"/>, relative to its current position.
        /// </summary>
        /// <param name="offset">The distance (in pixels) to move on the horizontal (X) &amp; vertical (Y) axes. Negative is left/up, positive is right/down.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MoveBy(System.Drawing.Point offset)
            => MoveBy(offset.X, offset.Y);
        #endregion MoveBy

        #region MoveTo
        /// <summary>
        /// Synthesizes the mouse moving to the specified position.
        /// </summary>
        /// <param name="xPosition">The horizontal position (in pixels) of the target point, specified in virtual screen (all monitors) coordinates. (0 is the left side of the primary monitor)</param>
        /// <param name="yPosition">The vertical position (in pixels) of the target point, specified in virtual screen (all monitors) coordinates. (0 is the top side of the primary monitor)</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MoveTo(int xPosition, int yPosition, RECT virtualScreenRect)
        {
            return NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
                dx = InputHelper.ToAbsCoordinateX(xPosition, virtualScreenRect),
                dy = InputHelper.ToAbsCoordinateY(yPosition, virtualScreenRect),
            }));
        }
        /// <inheritdoc cref="MoveTo(int, int, RECT)"/>
        public static bool MoveTo(int xPosition, int yPosition) => MoveTo(xPosition, yPosition, NativeMethods.GetVirtualScreenRect());

        // with POINT:

        /// <inheritdoc cref="MoveTo(int, int, RECT)"/>
        /// <param name="point">The target point (in pixels), specified in virtual screen (all monitors) coordinates. (0,0 is the top-right corner of the primary monitor)</param>
        public static bool MoveTo(POINT point, RECT virtualScreenRect) => MoveTo(point.x, point.y, virtualScreenRect);
        /// <inheritdoc cref="MoveTo(POINT, RECT)"/>
        public static bool MoveTo(POINT point) => MoveTo(point.x, point.y, NativeMethods.GetVirtualScreenRect());

        // with MONITORINFO:

        /// <summary>
        /// Synthesizes the mouse moving to the specified point on the specified <paramref name="monitor"/>.
        /// </summary>
        /// <param name="monitor">The <see cref="MONITORINFO"/> structure for the target monitor.</param>
        /// <param name="xPosition">The horizontal position (in pixels) of the target point, relative to the specified <paramref name="monitor"/> (0 is the left side of the <paramref name="monitor"/>).</param>
        /// <param name="yPosition">The vertical position (in pixels) of the target point, relative to the specified <paramref name="monitor"/> (0 is the top side of the <paramref name="monitor"/>).</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MoveTo(MONITORINFO monitor, int xPosition, int yPosition, RECT virtualScreenRect)
        {
            return NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
                dx = InputHelper.ToAbsCoordinateX(monitor.rcMonitor.left + xPosition, virtualScreenRect),
                dy = InputHelper.ToAbsCoordinateY(monitor.rcMonitor.top + yPosition, virtualScreenRect),
            }));
        }
        /// <inheritdoc cref="MoveTo(MONITORINFO, int, int, RECT)"/>
        public static bool MoveTo(MONITORINFO monitor, int xPosition, int yPosition) => MoveTo(monitor, xPosition, yPosition, NativeMethods.GetVirtualScreenRect());

        // with MONITORINFO & POINT:

        /// <inheritdoc cref="MoveTo(MONITORINFO, int, int, RECT)"/>
        /// <param name="point">The target point (in pixels), relative to the specified <paramref name="monitor"/> (0,0 is the top-left corner).</param>
        public static bool MoveTo(MONITORINFO monitor, POINT point, RECT virtualScreenRect) => MoveTo(monitor, point.x, point.y, virtualScreenRect);
        /// <inheritdoc cref="MoveTo(MONITORINFO, POINT, RECT)"/>
        public static bool MoveTo(MONITORINFO monitor, POINT point) => MoveTo(monitor, point, NativeMethods.GetVirtualScreenRect());
        #endregion MoveTo

        #region MoveToAbs
        /// <summary>
        /// Synthesizes the mouse moving to the specified point.
        /// </summary>
        /// <remarks>
        /// <b>Absolute coordinates</b> are always in the range 0-65535, where (0,0) is the top-left corner &amp; (65535,65535) is the bottom-right corner.
        /// <br/>
        /// To specify virtual-screen (pixel) coordinates, use the <b>MoveTo</b> methods instead of <b>MoveToAbs</b>.
        /// </remarks>
        /// <param name="xAbsPosition">The horizontal position (in absolute coordinates) of the target point.</param>
        /// <param name="yAbsPosition">The vertical position (in absolute coordinates) of the target point.</param>
        /// <param name="primaryMonitorOnly">When <see langword="true"/>, coordinates are relative to the primary display monitor.<br/>
        /// When <see langword="false"/>, coordinates are relative to the entire virtual screen (all monitors).</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MoveToAbs(ushort xAbsPosition, ushort yAbsPosition, bool primaryMonitorOnly = false)
        {
            var input = new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE,
                dx = xAbsPosition,
                dy = yAbsPosition,
            });

            if (!primaryMonitorOnly)
                input.data.mi.dwFlags |= MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK;

            return NativeMethods.SendInput(input);
        }
        /// <inheritdoc cref="MoveToAbs(ushort, ushort, bool)"/>
        /// <param name="absPoint">The target point (in absolute coordinates).</param>
        public static bool MoveToAbs(POINT absPoint, bool primaryMonitorOnly = false)
        {
            // validate the specified point is a valid absolute coordinate
            if (absPoint.x < ushort.MinValue || absPoint.x > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(absPoint), absPoint, $"The x-axis value {absPoint.x} is out-of-range for absolute coordinates! Expected a value within {ushort.MinValue}-{ushort.MaxValue}. (Did you mean to use {nameof(MoveTo)}()?)");
            else if (absPoint.y < ushort.MinValue || absPoint.y > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(absPoint), absPoint, $"The y-axis value {absPoint.y} is out-of-range for absolute coordinates! Expected a value within {ushort.MinValue}-{ushort.MaxValue}. (Did you mean to use {nameof(MoveTo)}()?)");

            return MoveToAbs(unchecked((ushort)absPoint.x), unchecked((ushort)absPoint.y), primaryMonitorOnly);
        }
        #endregion MoveToAbs

        #region VerticalScrollBy
        /// <summary>
        /// Synthesizes scrolling the mouse wheel by the specified <paramref name="delta"/>.
        /// </summary>
        /// <param name="delta">The delta (change amount) to apply to the mouse wheel. Negative values scroll up, positive values scroll down.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool VerticalScrollBy(int delta)
        {
            return NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_WHEEL,
                mouseData = delta
            }));
        }
        #endregion VerticalScrollBy

        #region VerticalScroll
        /// <summary>
        /// Synthesizes scrolling vertically by the specified amount.
        /// </summary>
        /// <param name="count">The number of times to scroll. Negative values scroll up, positive values scroll down.</param>
        /// <param name="delta">The delta (change amount) to apply to the mouse wheel each time.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool VerticalScroll(int count, uint delta)
            => VerticalScrollBy(unchecked((int)delta) * count);
        /// <inheritdoc cref="VerticalScroll(int, uint)"/>
        public static bool VerticalScroll(int count)
            => VerticalScroll(count, WHEEL_DELTA);
        #endregion VerticalScroll

        #region HorizontalScrollBy
        /// <summary>
        /// Synthesizes tilting the mouse wheel by the specified <paramref name="horizontalDelta"/>.
        /// </summary>
        /// <param name="delta">The delta (change amount) to apply to the mouse wheel. Negative values scroll left, positive values scroll right.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool HorizontalScrollBy(int horizontalDelta)
            => NativeMethods.SendInput(InputHelper.BuildMouseHorizontalScroll(horizontalDelta));
        #endregion HorizontalScrollBy

        #region HorizontalScroll
        public static bool HorizontalScroll(int count, int horizontalDelta)
            => NativeMethods.SendInput(InputHelper.BuildMouseHorizontalScroll(horizontalDelta, count));
        public static bool ScrollRight(int count) => HorizontalScroll(count, WHEEL_DELTA);
        public static bool ScrollRight() => ScrollRight(1);
        public static bool ScrollLeft(int count) => HorizontalScroll(count, -WHEEL_DELTA);
        public static bool ScrollLeft() => ScrollLeft(1);
        #endregion HorizontalScroll

        #region Button...
        public static bool ButtonDown(EMouseButton mouseButton)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonDown(mouseButton));
        public static bool ButtonUp(EMouseButton mouseButton)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(mouseButton));
        public static bool ButtonClick(EMouseButton mouseButton, int count)
            => count * 2 == NativeMethods.SendInputs(InputHelper.BuildMouseButtonClick(mouseButton, count));
        public static bool ButtonClick(EMouseButton mouseButton) => ButtonClick(mouseButton, count: 1);
        public static bool ButtonDoubleClick(EMouseButton mouseButton) => ButtonClick(mouseButton, count: 2);
        #endregion Button...

        #region ButtonClickAt
        public static bool ButtonClickAt(EMouseButton mouseButton, int clickCount, int x, int y, RECT virtualScreenRect)
        {
            var currentMousePos = NativeMethods.GetCursorPos();
            return NativeMethods.SendInput(InputHelper.ExpandInputs(
                InputHelper.BuildMouseMoveTo(x, y, virtualScreenRect),
                InputHelper.BuildMouseButtonClick(mouseButton, clickCount),
                InputHelper.BuildMouseMoveTo(currentMousePos.x, currentMousePos.y, virtualScreenRect)));
        }
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(mouseButton, 1, x, y, virtualScreenRect);
        public static bool ButtonClickAt(EMouseButton mouseButton, int clickCount, int x, int y, uint dpi)
            => ButtonClickAt(mouseButton, clickCount, x, y, NativeMethods.GetVirtualScreenRect(dpi));
        public static bool ButtonClickAt(EMouseButton mouseButton, int clickCount, int x, int y)
            => ButtonClickAt(mouseButton, clickCount, x, y, NativeMethods.GetVirtualScreenRect());
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, uint dpi)
            => ButtonClickAt(mouseButton, 1, x, y, NativeMethods.GetVirtualScreenRect(dpi));
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y)
            => ButtonClickAt(mouseButton, 1, x, y, NativeMethods.GetVirtualScreenRect());
        #endregion ButtonClickAt

        #region LeftButton
        public static bool LeftButtonDown()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonDown(EMouseButton.Left));
        public static bool LeftButtonUp()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.Left));
        public static bool LeftButtonClick(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Left, count));
        public static bool LeftButtonClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Left));
        public static bool LeftButtonDoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Left, 2));
        #endregion LeftButton

        #region RightButton
        public static bool RightButtonDown()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.Right));
        public static bool RightButtonUp()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.Right));
        public static bool RightButtonClick(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Right, count));
        public static bool RightButtonClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Right, 1));
        public static void RightButtonDoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Right, 2));
        #endregion RightButton

        #region MiddleButton
        public static bool MiddleButtonDown()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.Middle));
        public static bool MiddleButtonUp()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.Middle));
        public static bool MiddleButtonClick(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Middle, count));
        public static bool MiddleButtonClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Middle, 1));
        public static void MiddleButtonDoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.Middle, 2));
        #endregion MiddleButton

        #region XButton1
        public static bool XButton1Down()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.X1));
        public static bool XButton1Up()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.X1));
        public static bool XButton1Click(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.X1, count));
        public static bool XButton1Click()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.X1, 1));
        public static void XButton1DoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.X1, 2));
        #endregion XButton1

        #region XButton2
        public static bool XButton2Down()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.X2));
        public static bool XButton2Up()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.X2));
        public static bool XButton2Click(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.X2, count));
        public static bool XButton2Click()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.X2, 1));
        public static void XButton2DoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.X2, 2));
        #endregion XButton2
    }
}
