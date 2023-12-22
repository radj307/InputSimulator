using InputSimulator.Native;
using System;

namespace InputSimulator
{
    /// <summary>
    /// Provides <see langword="static"/> methods for simulating mouse input.
    /// </summary>
    public static class MouseInput
    {
        #region Constants
        /// <summary>
        /// The default scroll wheel delta value.
        /// </summary>
        public const int WHEEL_DELTA = 120;
        #endregion Constants

        #region Move
        /// <summary>
        /// Synthesizes the mouse moving by the specified offset, relative to its current position.
        /// </summary>
        /// <param name="xOffset">The distance (in pixels) to move on the horizontal axis. Negative is left, positive is right.</param>
        /// <param name="yOffset">The distance (in pixels) to move on the vertical axis. Negative is up, positive is down.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool Move(int xOffset, int yOffset)
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
        public static bool Move(System.Drawing.Point offset)
            => Move(offset.X, offset.Y);
        #endregion Move

        #region SetPosition
        /// <summary>
        /// Synthesizes the mouse moving to the specified position.
        /// </summary>
        /// <remarks>
        /// <b>Note:</b> The mouse cannot go off-screen to get to the target point; it will be stopped at the first obstruction.
        /// </remarks>
        /// <param name="xPosition">The horizontal position (in pixels) of the target point, specified in virtual screen (all monitors) coordinates. (0 is the left side of the primary monitor)</param>
        /// <param name="yPosition">The vertical position (in pixels) of the target point, specified in virtual screen (all monitors) coordinates. (0 is the top side of the primary monitor)</param>
        /// <param name="virtualScreenRect">The size of the virtual screen in pixels. The virtual screen is the entire desktop across all connected monitors.<br/>
        /// You can get this from <see cref="NativeMethods.GetVirtualScreenRect"/>. It is <b>highly recommended</b> to cache this value for functions that use it to avoid re-getting it every single time.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool SetPosition(int xPosition, int yPosition, RECT virtualScreenRect)
        {
            return NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
                dx = ScreenCoordinateHelper.ToAbsCoordinateX(xPosition, virtualScreenRect),
                dy = ScreenCoordinateHelper.ToAbsCoordinateY(yPosition, virtualScreenRect),
            }));
        }
        /// <inheritdoc cref="SetPosition(int, int, RECT)"/>
        public static bool SetPosition(int xPosition, int yPosition) => SetPosition(xPosition, yPosition, NativeMethods.GetVirtualScreenRect());

        // with POINT:

        /// <inheritdoc cref="SetPosition(int, int, RECT)"/>
        /// <param name="point">The target point (in pixels), specified in virtual screen (all monitors) coordinates. (0,0 is the top-right corner of the primary monitor)</param>
        public static bool SetPosition(POINT point, RECT virtualScreenRect) => SetPosition(point.x, point.y, virtualScreenRect);
        /// <inheritdoc cref="SetPosition(POINT, RECT)"/>
        public static bool SetPosition(POINT point) => SetPosition(point.x, point.y, NativeMethods.GetVirtualScreenRect());

        // with MONITORINFO:

        /// <summary>
        /// Synthesizes the mouse moving to the specified point on the specified <paramref name="monitor"/>.
        /// </summary>
        /// <remarks>
        /// <b>Note:</b> The mouse cannot go off-screen to get to the target point; it will be stopped at the first obstruction.
        /// </remarks>
        /// <param name="monitor">The <see cref="MONITORINFO"/> structure for the target monitor.</param>
        /// <param name="xPosition">The horizontal position (in pixels) of the target point, relative to the specified <paramref name="monitor"/> (0 is the left side of the <paramref name="monitor"/>).</param>
        /// <param name="yPosition">The vertical position (in pixels) of the target point, relative to the specified <paramref name="monitor"/> (0 is the top side of the <paramref name="monitor"/>).</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool SetPosition(MONITORINFO monitor, int xPosition, int yPosition, RECT virtualScreenRect)
        {
            return NativeMethods.SendInput(new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
                dx = ScreenCoordinateHelper.ToAbsCoordinateX(monitor.rcMonitor.left + xPosition, virtualScreenRect),
                dy = ScreenCoordinateHelper.ToAbsCoordinateY(monitor.rcMonitor.top + yPosition, virtualScreenRect),
            }));
        }
        /// <inheritdoc cref="SetPosition(MONITORINFO, int, int, RECT)"/>
        public static bool SetPosition(MONITORINFO monitor, int xPosition, int yPosition) => SetPosition(monitor, xPosition, yPosition, NativeMethods.GetVirtualScreenRect());

        // with MONITORINFO & POINT:

        /// <inheritdoc cref="SetPosition(MONITORINFO, int, int, RECT)"/>
        /// <param name="point">The target point (in pixels), relative to the specified <paramref name="monitor"/> (0,0 is the top-left corner).</param>
        public static bool SetPosition(MONITORINFO monitor, POINT point, RECT virtualScreenRect) => SetPosition(monitor, point.x, point.y, virtualScreenRect);
        /// <inheritdoc cref="SetPosition(MONITORINFO, POINT, RECT)"/>
        public static bool SetPosition(MONITORINFO monitor, POINT point) => SetPosition(monitor, point, NativeMethods.GetVirtualScreenRect());
        #endregion SetPosition

        #region SetPositionAbs
        /// <summary>
        /// Synthesizes the mouse moving to the specified point.
        /// </summary>
        /// <remarks>
        /// <b>Absolute coordinates</b> are always in the range 0-65535, where (0,0) is the top-left corner &amp; (65535,65535) is the bottom-right corner.
        /// <br/>
        /// To specify virtual-screen (pixel) coordinates, use the <b>MoveTo</b> methods instead of <b>MoveToAbs</b>.
        /// <br/><br/>
        /// <b>Note:</b> The mouse cannot go off-screen to get to the target point; it will be stopped at the first obstruction.
        /// </remarks>
        /// <param name="xAbsPosition">The horizontal position (in absolute coordinates) of the target point.</param>
        /// <param name="yAbsPosition">The vertical position (in absolute coordinates) of the target point.</param>
        /// <param name="virtualScreen">When <see langword="true"/>, coordinates are relative to the entire virtual screen (all monitors),<br/>
        /// When <see langword="false"/>, coordinates are relative to the primary display monitor.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool SetPositionAbs(ushort xAbsPosition, ushort yAbsPosition, bool virtualScreen = true)
        {
            var input = new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE,
                dx = xAbsPosition,
                dy = yAbsPosition,
            });

            if (virtualScreen)
                input.data.mi.dwFlags |= MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK;

            return NativeMethods.SendInput(input);
        }
        /// <inheritdoc cref="SetPositionAbs(ushort, ushort, bool)"/>
        /// <param name="absPoint">The target point (in absolute coordinates).</param>
        public static bool SetPositionAbs(POINT absPoint, bool virtualScreen = true)
        {
            // validate the specified point is a valid absolute coordinate
            if (absPoint.x < ushort.MinValue || absPoint.x > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(absPoint), absPoint, $"The x-axis value {absPoint.x} is out-of-range for absolute coordinates! Expected a value within {ushort.MinValue}-{ushort.MaxValue}. (Did you mean to use {nameof(SetPosition)}()?)");
            else if (absPoint.y < ushort.MinValue || absPoint.y > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(absPoint), absPoint, $"The y-axis value {absPoint.y} is out-of-range for absolute coordinates! Expected a value within {ushort.MinValue}-{ushort.MaxValue}. (Did you mean to use {nameof(SetPosition)}()?)");

            return SetPositionAbs(unchecked((ushort)absPoint.x), unchecked((ushort)absPoint.y), virtualScreen);
        }
        #endregion SetPositionAbs

        #region VerticalScroll
        /// <summary>
        /// Synthesizes turning the mouse wheel to scroll vertically.
        /// </summary>
        /// <param name="delta">The delta (amount of change) value to apply to the scroll wheel. Negative values scroll up, positive values scroll down.<br/>The default (absolute) value is <see cref="WHEEL_DELTA"/>, which can be inverted as needed.</param>
        /// <param name="count">The number of times to scroll.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool VerticalScroll(int delta, int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseVerticalScroll(delta: delta, count: count));
        /// <inheritdoc cref="VerticalScroll(int, int)"/>
        public static bool VerticalScroll(int delta) => VerticalScroll(delta: delta, count: 1);
        #endregion VerticalScroll

        #region VerticalScrollUp
        /// <summary>
        /// Synthesizes turning the mouse wheel to scroll up.
        /// </summary>
        /// <param name="delta">The delta (amount of change) value to apply to the scroll wheel. The default is <see cref="WHEEL_DELTA"/>.<br/><br/><b>Note:</b> This is a <see cref="uint"/> to prevent negative values from being entered. To scroll up or down depending on the sign of <paramref name="delta"/>, use the regular <b>VerticalScroll</b> methods instead.</param>
        /// <param name="count">The number of times to scroll.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool VerticalScrollUp(uint delta, int count) => VerticalScroll(delta: -(int)delta, count: count);
        /// <inheritdoc cref="VerticalScrollUp(uint, int)"/>
        public static bool VerticalScrollUp(int count) => VerticalScrollUp(delta: WHEEL_DELTA, count: count);
        /// <inheritdoc cref="VerticalScrollUp(uint, int)"/>
        public static bool VerticalScrollUp() => VerticalScrollUp(delta: WHEEL_DELTA, count: 1);
        #endregion VerticalScrollUp

        #region VerticalScrollDown
        /// <summary>
        /// Synthesizes turning the mouse wheel to scroll down.
        /// </summary>
        /// <param name="delta">The delta (amount of change) value to apply to the scroll wheel. The default is <see cref="WHEEL_DELTA"/>.<br/><br/><b>Note:</b> This is a <see cref="uint"/> to prevent negative values from being entered. To scroll up or down depending on the sign of <paramref name="delta"/>, use the regular <b>VerticalScroll</b> methods instead.</param>
        /// <param name="count">The number of times to scroll.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool VerticalScrollDown(uint delta, int count) => VerticalScroll(delta: (int)delta, count: count);
        /// <inheritdoc cref="VerticalScrollDown(uint, int)"/>
        public static bool VerticalScrollDown(int count) => VerticalScrollDown(delta: WHEEL_DELTA, count: count);
        /// <inheritdoc cref="VerticalScrollDown(uint, int)"/>
        public static bool VerticalScrollDown() => VerticalScrollDown(delta: WHEEL_DELTA, count: 1);
        #endregion VerticalScrollDown

        #region HorizontalScroll
        /// <summary>
        /// Synthesizes tilting the mouse wheel to scroll horizontally.
        /// </summary>
        /// <param name="horizontalDelta">The delta (amount of change) value to apply to the scroll wheel. Negative values scroll to the left, positive values scroll to the right.<br/>The default (absolute) value is <see cref="WHEEL_DELTA"/>, which can be inverted as needed.</param>
        /// <param name="count">The number of times to scroll.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool HorizontalScroll(int horizontalDelta, int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseHorizontalScroll(horizontalDelta: horizontalDelta, count: count));
        /// <inheritdoc cref="HorizontalScroll(int, int)"/>
        public static bool HorizontalScroll(int horizontalDelta) => HorizontalScroll(horizontalDelta: horizontalDelta, count: 1);
        #endregion HorizontalScroll

        #region HorizontalScrollRight
        /// <summary>
        /// Synthesizes tilting the mouse wheel to scroll to the right.
        /// </summary>
        /// <param name="horizontalDelta">The delta (amount of change) value to apply to the scroll wheel. The default is <see cref="WHEEL_DELTA"/>.<br/><br/><b>Note:</b> This is a <see cref="uint"/> to prevent negative values from being entered. To scroll left or right depending on the sign of <paramref name="horizontalDelta"/>, use the regular <b>HorizontalScroll</b> methods instead.</param>
        /// <param name="count">The number of times to scroll.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool HorizontalScrollRight(uint horizontalDelta, int count) => HorizontalScroll(horizontalDelta: (int)horizontalDelta, count: count);
        /// <inheritdoc cref="HorizontalScrollRight(uint, int)"/>
        public static bool HorizontalScrollRight(int count) => HorizontalScrollRight(horizontalDelta: WHEEL_DELTA, count: count);
        /// <inheritdoc cref="HorizontalScrollRight(uint, int)"/>
        public static bool HorizontalScrollRight() => HorizontalScrollRight(horizontalDelta: WHEEL_DELTA, count: 1);
        #endregion HorizontalScrollRight

        #region HorizontalScrollLeft
        /// <summary>
        /// Synthesizes tilting the mouse wheel to scroll to the left.
        /// </summary>
        /// <param name="horizontalDelta">The delta (amount of change) value to apply to the scroll wheel. The default is <see cref="WHEEL_DELTA"/>.<br/><br/><b>Note:</b> This is a <see cref="uint"/> to prevent negative values from being entered. To scroll left or right depending on the sign of <paramref name="horizontalDelta"/>, use the regular <b>HorizontalScroll</b> methods instead.</param>
        /// <param name="count">The number of times to scroll.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool HorizontalScrollLeft(uint horizontalDelta, int count) => HorizontalScroll(horizontalDelta: -(int)horizontalDelta, count: count);
        /// <inheritdoc cref="HorizontalScrollLeft(uint, int)"/>
        public static bool HorizontalScrollLeft(int count) => HorizontalScrollLeft(horizontalDelta: WHEEL_DELTA, count: count);
        /// <inheritdoc cref="HorizontalScrollLeft(uint, int)"/>
        public static bool HorizontalScrollLeft() => HorizontalScrollLeft(horizontalDelta: WHEEL_DELTA, count: 1);
        #endregion HorizontalScrollLeft

        #region Button...
        /// <summary>
        /// Synthesizes pressing (▼) the specified <paramref name="mouseButton"/>.
        /// </summary>
        /// <param name="mouseButton">The mouse button to press down.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool ButtonDown(EMouseButton mouseButton)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonDown(mouseButton));
        /// <summary>
        /// Synthesizes releasing (▲) the specified <paramref name="mouseButton"/>.
        /// </summary>
        /// <param name="mouseButton">The mouse button to release.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool ButtonUp(EMouseButton mouseButton)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(mouseButton));
        /// <summary>
        /// Synthesizes clicking (▼▲...) the specified <paramref name="mouseButton"/> the specified number of times.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool ButtonClick(EMouseButton mouseButton, int count)
            => count * 2 == NativeMethods.SendInputs(InputHelper.BuildMouseButtonClick(mouseButton, count));
        /// <summary>
        /// Synthesizes clicking (▼▲) the specified <paramref name="mouseButton"/>.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool ButtonClick(EMouseButton mouseButton) => ButtonClick(mouseButton, count: 1);
        /// <summary>
        /// Synthesizes double-clicking (▼▲▼▲) the specified <paramref name="mouseButton"/>.
        /// </summary>
        /// <param name="mouseButton">The mouse button to click.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool ButtonDoubleClick(EMouseButton mouseButton) => ButtonClick(mouseButton, count: 2);
        #endregion Button...

        #region ButtonClickAt
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the specified <paramref name="mouseButton"/>, then optionally moving back to the original position.
        /// </summary>
        /// <remarks>
        /// <b>Note:</b> The mouse cannot go off-screen to get to the target point; it will be stopped at the first obstruction.
        /// </remarks>
        /// <param name="mouseButton">The mouse button to click.</param>
        /// <param name="x">The target X-axis (horizontal) position, in pixels.</param>
        /// <param name="y">The target Y-axis (vertical) position, in pixels.</param>
        /// <param name="clickCount">The number of times to click the specified <paramref name="mouseButton"/>.</param>
        /// <param name="restorePosition">When <see langword="true"/>, moves the mouse back to the starting point after clicking.<br/>When <see langword="false"/>, the mouse is left at the target position.</param>
        /// <param name="virtualScreenRect"></param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="clickCount"/> was negative.</exception>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, bool restorePosition, RECT virtualScreenRect)
        {
            if (restorePosition)
            {
                var currentMousePos = NativeMethods.GetCursorPos();
                return NativeMethods.SendInput(InputHelper.BuildMouseButtonClickAt(mouseButton, x, y, clickCount, currentMousePos.x, currentMousePos.y, virtualScreenRect));
            }
            else return NativeMethods.SendInput(InputHelper.BuildMouseButtonClickAt(mouseButton, x, y, clickCount, virtualScreenRect));
        }
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the specified <paramref name="mouseButton"/>, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(mouseButton, x, y, clickCount: 1, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the specified <paramref name="mouseButton"/>, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, RECT virtualScreenRect)
            => ButtonClickAt(mouseButton, x, y, clickCount, restorePosition: true, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the specified <paramref name="mouseButton"/>, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(mouseButton, x, y, clickCount: 1, true, virtualScreenRect);

        // with dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the specified <paramref name="mouseButton"/>, then optionally moving back to the original position.
        /// </summary>
        /// <param name="dpi">The system DPI.</param>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, bool restorePosition, uint dpi)
            => ButtonClickAt(mouseButton, x, y, clickCount, restorePosition, NativeMethods.GetVirtualScreenRect(dpi));
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the specified <paramref name="mouseButton"/>, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, bool restorePosition, uint dpi)
            => ButtonClickAt(mouseButton, x, y, clickCount: 1, restorePosition, NativeMethods.GetVirtualScreenRect(dpi));
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the specified <paramref name="mouseButton"/>, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, uint dpi)
            => ButtonClickAt(mouseButton, x, y, clickCount, restorePosition: true, NativeMethods.GetVirtualScreenRect(dpi));
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the specified <paramref name="mouseButton"/>, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, uint dpi)
            => ButtonClickAt(mouseButton, x, y, clickCount: 1, restorePosition: true, NativeMethods.GetVirtualScreenRect(dpi));

        // without virtual screen rect or dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the specified <paramref name="mouseButton"/>, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, bool restorePosition)
            => ButtonClickAt(mouseButton, x, y, clickCount, restorePosition, NativeMethods.GetVirtualScreenRect());
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the specified <paramref name="mouseButton"/>, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, bool restorePosition)
            => ButtonClickAt(mouseButton, x, y, clickCount: 1, restorePosition, NativeMethods.GetVirtualScreenRect());
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the specified <paramref name="mouseButton"/>, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount)
            => ButtonClickAt(mouseButton, x, y, clickCount, restorePosition: true, NativeMethods.GetVirtualScreenRect());
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the specified <paramref name="mouseButton"/>, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool ButtonClickAt(EMouseButton mouseButton, int x, int y)
            => ButtonClickAt(mouseButton, x, y, clickCount: 1, NativeMethods.GetVirtualScreenRect());
        #endregion ButtonClickAt

        #region LeftButton...
        /// <summary>
        /// Synthesizes pressing (▼) the primary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool LeftButtonDown()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonDown(EMouseButton.LeftButton));
        /// <summary>
        /// Synthesizes releasing (▲) the primary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool LeftButtonUp()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.LeftButton));
        /// <summary>
        /// Synthesizes clicking (▼▲...) the primary mouse button the specified number of times.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool LeftButtonClick(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.LeftButton, count));
        /// <summary>
        /// Synthesizes clicking (▼▲) the primary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool LeftButtonClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.LeftButton));
        /// <summary>
        /// Synthesizes double-clicking (▼▲▼▲) the primary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool LeftButtonDoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.LeftButton, 2));
        #endregion LeftButton...

        #region LeftButtonClickAt
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the primary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool LeftButtonClickAt(int x, int y, int clickCount, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, clickCount, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the primary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, RECT)"/>
        public static bool LeftButtonClickAt(int x, int y, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the primary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, RECT)"/>
        public static bool LeftButtonClickAt(int x, int y, int clickCount, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, clickCount, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the primary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, RECT)"/>
        public static bool LeftButtonClickAt(int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, virtualScreenRect);

        // with dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the primary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool LeftButtonClickAt(int x, int y, int clickCount, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, clickCount, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the primary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, uint)"/>
        public static bool LeftButtonClickAt(int x, int y, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the primary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, uint)"/>
        public static bool LeftButtonClickAt(int x, int y, int clickCount, uint dpi)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, clickCount, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the primary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, uint)"/>
        public static bool LeftButtonClickAt(int x, int y, uint dpi)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, dpi);

        // without virtual screen rect or dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the primary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool)"/>
        public static bool LeftButtonClickAt(int x, int y, int clickCount, bool restorePosition)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, clickCount, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the primary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool)"/>
        public static bool LeftButtonClickAt(int x, int y, bool restorePosition)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the primary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int)"/>
        public static bool LeftButtonClickAt(int x, int y, int clickCount)
            => ButtonClickAt(EMouseButton.LeftButton, x, y, clickCount);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the primary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int)"/>
        public static bool LeftButtonClickAt(int x, int y)
            => ButtonClickAt(EMouseButton.LeftButton, x, y);
        #endregion LeftButtonClickAt

        #region RightButton...
        /// <summary>
        /// Synthesizes pressing (▼) the secondary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool RightButtonDown()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.RightButton));
        /// <summary>
        /// Synthesizes releasing (▲) the secondary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool RightButtonUp()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.RightButton));
        /// <summary>
        /// Synthesizes clicking (▼▲...) the secondary mouse button the specified number of times.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool RightButtonClick(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.RightButton, count));
        /// <summary>
        /// Synthesizes clicking (▼▲) the secondary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool RightButtonClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.RightButton, 1));
        /// <summary>
        /// Synthesizes double-clicking (▼▲▼▲) the secondary mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static void RightButtonDoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.RightButton, 2));
        #endregion RightButton...

        #region RightButtonClickAt
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the secondary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool RightButtonClickAt(int x, int y, int clickCount, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.RightButton, x, y, clickCount, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the secondary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, RECT)"/>
        public static bool RightButtonClickAt(int x, int y, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.RightButton, x, y, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the secondary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, RECT)"/>
        public static bool RightButtonClickAt(int x, int y, int clickCount, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.RightButton, x, y, clickCount, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the secondary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, RECT)"/>
        public static bool RightButtonClickAt(int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.RightButton, x, y, virtualScreenRect);

        // with dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the secondary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool RightButtonClickAt(int x, int y, int clickCount, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.RightButton, x, y, clickCount, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the secondary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, uint)"/>
        public static bool RightButtonClickAt(int x, int y, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.RightButton, x, y, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the secondary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, uint)"/>
        public static bool RightButtonClickAt(int x, int y, int clickCount, uint dpi)
            => ButtonClickAt(EMouseButton.RightButton, x, y, clickCount, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the secondary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, uint)"/>
        public static bool RightButtonClickAt(int x, int y, uint dpi)
            => ButtonClickAt(EMouseButton.RightButton, x, y, dpi);

        // without virtual screen rect or dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the secondary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool)"/>
        public static bool RightButtonClickAt(int x, int y, int clickCount, bool restorePosition)
            => ButtonClickAt(EMouseButton.RightButton, x, y, clickCount, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the secondary mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool)"/>
        public static bool RightButtonClickAt(int x, int y, bool restorePosition)
            => ButtonClickAt(EMouseButton.RightButton, x, y, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the secondary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int)"/>
        public static bool RightButtonClickAt(int x, int y, int clickCount)
            => ButtonClickAt(EMouseButton.RightButton, x, y, clickCount);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the secondary mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int)"/>
        public static bool RightButtonClickAt(int x, int y)
            => ButtonClickAt(EMouseButton.RightButton, x, y);
        #endregion RightButtonClickAt

        #region MiddleButton...
        /// <summary>
        /// Synthesizes pressing (▼) the middle mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MiddleButtonDown()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.MiddleButton));
        /// <summary>
        /// Synthesizes releasing (▲) the middle mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MiddleButtonUp()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.MiddleButton));
        /// <summary>
        /// Synthesizes clicking (▼▲...) the middle mouse button the specified number of times.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MiddleButtonClick(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.MiddleButton, count));
        /// <summary>
        /// Synthesizes clicking (▼▲) the middle mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool MiddleButtonClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.MiddleButton, 1));
        /// <summary>
        /// Synthesizes double-clicking (▼▲▼▲) the middle mouse button.
        /// </summary>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static void MiddleButtonDoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.MiddleButton, 2));
        #endregion MiddleButton...

        #region MiddleButtonClickAt
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the middle mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool MiddleButtonClickAt(int x, int y, int clickCount, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, clickCount, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the middle mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, RECT)"/>
        public static bool MiddleButtonClickAt(int x, int y, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the middle mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, RECT)"/>
        public static bool MiddleButtonClickAt(int x, int y, int clickCount, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, clickCount, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the middle mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, RECT)"/>
        public static bool MiddleButtonClickAt(int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, virtualScreenRect);

        // with dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the middle mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool MiddleButtonClickAt(int x, int y, int clickCount, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, clickCount, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the middle mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, uint)"/>
        public static bool MiddleButtonClickAt(int x, int y, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the middle mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, uint)"/>
        public static bool MiddleButtonClickAt(int x, int y, int clickCount, uint dpi)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, clickCount, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the middle mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, uint)"/>
        public static bool MiddleButtonClickAt(int x, int y, uint dpi)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, dpi);

        // without virtual screen rect or dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the middle mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool)"/>
        public static bool MiddleButtonClickAt(int x, int y, int clickCount, bool restorePosition)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, clickCount, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the middle mouse button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool)"/>
        public static bool MiddleButtonClickAt(int x, int y, bool restorePosition)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the middle mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int)"/>
        public static bool MiddleButtonClickAt(int x, int y, int clickCount)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y, clickCount);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the middle mouse button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int)"/>
        public static bool MiddleButtonClickAt(int x, int y)
            => ButtonClickAt(EMouseButton.MiddleButton, x, y);
        #endregion MiddleButtonClickAt

        #region XButton1...
        /// <summary>
        /// Synthesizes pressing (▼) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton1 is usually the "back" button, closer to the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton1Down()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.XButton1));
        /// <summary>
        /// Synthesizes releasing (▲) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton1 is usually the "back" button, closer to the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton1Up()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.XButton1));
        /// <summary>
        /// Synthesizes clicking (▼▲...) the first mouse extension button the specified number of times.
        /// </summary>
        /// <remarks>
        /// XButton1 is usually the "back" button, closer to the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton1Click(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.XButton1, count));
        /// <summary>
        /// Synthesizes clicking (▼▲) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton1 is usually the "back" button, closer to the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton1Click()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.XButton1, 1));
        /// <summary>
        /// Synthesizes double-clicking (▼▲▼▲) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton1 is usually the "back" button, closer to the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static void XButton1DoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.XButton1, 2));
        #endregion XButton1...

        #region XButton1ClickAt
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the first mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool XButton1ClickAt(int x, int y, int clickCount, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton1, x, y, clickCount, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the first mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, RECT)"/>
        public static bool XButton1ClickAt(int x, int y, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton1, x, y, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the first mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, RECT)"/>
        public static bool XButton1ClickAt(int x, int y, int clickCount, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton1, x, y, clickCount, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the first mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, RECT)"/>
        public static bool XButton1ClickAt(int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton1, x, y, virtualScreenRect);

        // with dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the first mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool XButton1ClickAt(int x, int y, int clickCount, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.XButton1, x, y, clickCount, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the first mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, uint)"/>
        public static bool XButton1ClickAt(int x, int y, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.XButton1, x, y, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the first mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, uint)"/>
        public static bool XButton1ClickAt(int x, int y, int clickCount, uint dpi)
            => ButtonClickAt(EMouseButton.XButton1, x, y, clickCount, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the first mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, uint)"/>
        public static bool XButton1ClickAt(int x, int y, uint dpi)
            => ButtonClickAt(EMouseButton.XButton1, x, y, dpi);

        // without virtual screen rect or dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the first mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool)"/>
        public static bool XButton1ClickAt(int x, int y, int clickCount, bool restorePosition)
            => ButtonClickAt(EMouseButton.XButton1, x, y, clickCount, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the first mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool)"/>
        public static bool XButton1ClickAt(int x, int y, bool restorePosition)
            => ButtonClickAt(EMouseButton.XButton1, x, y, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the first mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int)"/>
        public static bool XButton1ClickAt(int x, int y, int clickCount)
            => ButtonClickAt(EMouseButton.XButton1, x, y, clickCount);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the first mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int)"/>
        public static bool XButton1ClickAt(int x, int y)
            => ButtonClickAt(EMouseButton.XButton1, x, y);
        #endregion XButton1ClickAt

        #region XButton2...
        /// <summary>
        /// Synthesizes pressing (▼) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton2 is usually the "forward" button, further from the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton2Down()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.XButton2));
        /// <summary>
        /// Synthesizes releasing (▲) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton2 is usually the "forward" button, further from the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton2Up()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonUp(EMouseButton.XButton2));
        /// <summary>
        /// Synthesizes clicking (▼▲...) the first mouse extension button the specified number of times.
        /// </summary>
        /// <remarks>
        /// XButton2 is usually the "forward" button, further from the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton2Click(int count)
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.XButton2, count));
        /// <summary>
        /// Synthesizes clicking (▼▲) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton2 is usually the "forward" button, further from the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool XButton2Click()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.XButton2, 1));
        /// <summary>
        /// Synthesizes double-clicking (▼▲▼▲) the first mouse extension button.
        /// </summary>
        /// <remarks>
        /// XButton2 is usually the "forward" button, further from the thumb. Not all mice have XButtons.
        /// </remarks>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static void XButton2DoubleClick()
            => NativeMethods.SendInput(InputHelper.BuildMouseButtonClick(EMouseButton.XButton2, 2));
        #endregion XButton2...

        #region XButton2ClickAt
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the second mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, RECT)"/>
        public static bool XButton2ClickAt(int x, int y, int clickCount, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton2, x, y, clickCount, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the second mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, RECT)"/>
        public static bool XButton2ClickAt(int x, int y, bool restorePosition, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton2, x, y, restorePosition, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the second mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, RECT)"/>
        public static bool XButton2ClickAt(int x, int y, int clickCount, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton2, x, y, clickCount, virtualScreenRect);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the second mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, RECT)"/>
        public static bool XButton2ClickAt(int x, int y, RECT virtualScreenRect)
            => ButtonClickAt(EMouseButton.XButton2, x, y, virtualScreenRect);

        // with dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the second mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool, uint)"/>
        public static bool XButton2ClickAt(int x, int y, int clickCount, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.XButton2, x, y, clickCount, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the second mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool, uint)"/>
        public static bool XButton2ClickAt(int x, int y, bool restorePosition, uint dpi)
            => ButtonClickAt(EMouseButton.XButton2, x, y, restorePosition, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the second mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, uint)"/>
        public static bool XButton2ClickAt(int x, int y, int clickCount, uint dpi)
            => ButtonClickAt(EMouseButton.XButton2, x, y, clickCount, dpi);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the second mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, uint)"/>
        public static bool XButton2ClickAt(int x, int y, uint dpi)
            => ButtonClickAt(EMouseButton.XButton2, x, y, dpi);

        // without virtual screen rect or dpi:

        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the second mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int, bool)"/>
        public static bool XButton2ClickAt(int x, int y, int clickCount, bool restorePosition)
            => ButtonClickAt(EMouseButton.XButton2, x, y, clickCount, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the second mouse extension button, then optionally moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, bool)"/>
        public static bool XButton2ClickAt(int x, int y, bool restorePosition)
            => ButtonClickAt(EMouseButton.XButton2, x, y, restorePosition);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲...) the second mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int, int)"/>
        public static bool XButton2ClickAt(int x, int y, int clickCount)
            => ButtonClickAt(EMouseButton.XButton2, x, y, clickCount);
        /// <summary>
        /// Synthesizes moving the mouse to the specified position and clicking (▼▲) the second mouse extension button, then moving back to the original position.
        /// </summary>
        /// <inheritdoc cref="ButtonClickAt(EMouseButton, int, int)"/>
        public static bool XButton2ClickAt(int x, int y)
            => ButtonClickAt(EMouseButton.XButton2, x, y);
        #endregion XButton2ClickAt
    }
}
