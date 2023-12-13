using InputSimulator.Internal;
using InputSimulator.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace InputSimulator
{
    /// <summary>
    /// Helper methods for performing conversions and creating sequences of <see cref="INPUT"/> events.
    /// </summary>
    public static class InputHelper
    {
        #region Normalize
        /// <summary>
        /// Normalizes the specified <paramref name="value"/> from the specified <paramref name="oldRange"/> to the specified <paramref name="newRange"/>.
        /// </summary>
        /// <param name="value">A value within the specified <paramref name="oldRange"/> to normalize to the <paramref name="newRange"/>.</param>
        /// <param name="oldRange">The old minimum and maximum range boundaries.</param>
        /// <param name="newRange">The new minimum and maximum range boundaries.</param>
        /// <returns><paramref name="value"/> as its equivalent in the specified <paramref name="newRange"/>.</returns>
        public static double Normalize(double value, (double Min, double Max) oldRange, (double Min, double Max) newRange)
            => newRange.Min + (value - oldRange.Min) * (newRange.Max - newRange.Min) / (oldRange.Max - oldRange.Min);
        /// <summary>
        /// Normalizes the specified <paramref name="value"/> from the specified <paramref name="oldRange"/> to the specified <paramref name="newRange"/>, and truncates the result to an <see cref="int"/>.
        /// </summary>
        /// <inheritdoc cref="Normalize(double, (double Min, double Max), (double Min, double Max))"/>
        public static int NormalizeInt(double value, (double Min, double Max) oldRange, (double Min, double Max) newRange)
            => (int)Math.Truncate(Normalize(value, oldRange, newRange));
        #endregion Normalize

        #region ExpandInputs
        public static INPUT[] ExpandInputs(params SingleOrEnumerable<INPUT>[] inputs)
            => inputs.SelectMany(item => item).ToArray();

        #region (Class) SingleOrEnumerable<T>
        public class SingleOrEnumerable<T> : IEnumerable<T>
        {
            public SingleOrEnumerable(params T[] items)
            {
                _items = items;
            }

            private readonly T[] _items;

            public int Length => _items.Length;

            public static implicit operator SingleOrEnumerable<T>(T item) => new(item);
            public static implicit operator SingleOrEnumerable<T>(T[] items) => new(items);
            public static implicit operator T[](SingleOrEnumerable<T> instance) => instance._items;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
        }
        #endregion (Class) SingleOrEnumerable<T>

        #endregion ExpandInputs

        #region Keyboard

        #region BuildKeyStrokeDown
        public static INPUT[] BuildKeyStrokeDown(EVirtualKeyCode[] modifierKeys, EVirtualKeyCode[] keys)
        {
            var inputs = new INPUT[modifierKeys.Length + keys.Length];
            int i_inputs = 0;

            foreach (var modifier in modifierKeys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyDown(modifier));
            }

            foreach (var key in keys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyDown(key));
            }

            return inputs.ToArray();
        }
        #endregion BuildKeyStrokeDown

        #region BuildKeyStrokeUp
        public static INPUT[] BuildKeyStrokeUp(EVirtualKeyCode[] modifierKeys, EVirtualKeyCode[] keys)
        {
            var inputs = new INPUT[modifierKeys.Length + keys.Length];
            int i_inputs = 0;

            foreach (var modifier in modifierKeys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyUp(modifier));
            }

            foreach (var key in keys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyUp(key));
            }

            return inputs.ToArray();
        }
        #endregion BuildKeyStrokeUp

        #region BuildKeyStroke
        public static INPUT[] BuildKeyStroke(EVirtualKeyCode[] modifierKeys, EVirtualKeyCode[] keys)
        {
            var inputs = new INPUT[modifierKeys.Length * 2 + keys.Length * 2];
            int i_inputs = 0;

            int i_modKeyUpInputs = modifierKeys.Length + keys.Length * 2;
            foreach (var modifier in modifierKeys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyDown(modifier));
                inputs[i_modKeyUpInputs++] = new(KEYBDINPUT.GetKeyUp(modifier));
            }

            int i_keyUpInputs = i_inputs + keys.Length;
            foreach (var key in keys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyDown(key));
                inputs[i_keyUpInputs++] = new(KEYBDINPUT.GetKeyUp(key));
            }

            return inputs.ToArray();
        }
        #endregion BuildKeyStroke

        #region IsExtendedKeyChar
        /// <summary>
        /// Checks if the specified <paramref name="char"/> is an extended key or not.
        /// </summary>
        /// <param name="char">A <see cref="char"/> value.</param>
        /// <returns><see langword="true"/> when the specified <paramref name="char"/> is an extended key; otherwise, <see langword="false"/>.</returns>
        public static bool IsExtendedKeyChar(char @char) => (@char & 0xFF00) == 0xE000;
        #endregion IsExtendedKeyChar

        #region CharToKeyINPUT
        /// <summary>
        /// Gets the sequence of <see cref="INPUT"/> events required to type the specified <paramref name="char"/>.
        /// </summary>
        /// <param name="char">A <see cref="char"/> value.</param>
        /// <param name="keyDown">When <see langword="true"/>, the resulting enumeration includes a key down <see cref="INPUT"/> event; otherwise, it doesn't.</param>
        /// <param name="keyUp">When <see langword="true"/>, the resulting enumeration includes a key up <see cref="INPUT"/> event; otherwise, it doesn't.</param>
        /// <returns>The sequence of <see cref="INPUT"/> events required to type the specified <paramref name="char"/>.</returns>
        public static IEnumerable<INPUT> CharToKeyINPUT(char @char, bool keyDown = true, bool keyUp = true)
        {
            if (keyDown || keyUp)
            {
                switch (@char)
                {
                case '\n': // newline
                    if (keyDown)
                        yield return new(KEYBDINPUT.GetKeyDown(EVirtualKeyCode.VK_RETURN));
                    if (keyUp)
                        yield return new(KEYBDINPUT.GetKeyUp(EVirtualKeyCode.VK_RETURN));
                    break;
                case '\t': // tab
                    if (keyDown)
                        yield return new(KEYBDINPUT.GetKeyDown(EVirtualKeyCode.VK_TAB));
                    if (keyUp)
                        yield return new(KEYBDINPUT.GetKeyUp(EVirtualKeyCode.VK_TAB));
                    break;
                case '\b': // backspace
                    if (keyDown)
                        yield return new(KEYBDINPUT.GetKeyDown(EVirtualKeyCode.VK_BACK));
                    if (keyUp)
                        yield return new(KEYBDINPUT.GetKeyUp(EVirtualKeyCode.VK_BACK));
                    break;
                default:
                    bool isExtendedKey = IsExtendedKeyChar(@char);
                    if (keyDown)
                        yield return new(KEYBDINPUT.GetKeyDown(@char, isExtendedKey));
                    if (keyUp)
                        yield return new(KEYBDINPUT.GetKeyUp(@char, isExtendedKey));
                    break;
                }
            }
        }
        #endregion CharToKeyINPUT

        #region StringToINPUT
        /// <summary>
        /// Gets the sequence of <see cref="INPUT"/> events required to type the specified <paramref name="string"/>.
        /// </summary>
        /// <param name="char">A <see cref="char"/> value.</param>
        /// <param name="keyDown">When <see langword="true"/>, the resulting enumeration includes a key down <see cref="INPUT"/> event; otherwise, it doesn't.</param>
        /// <param name="keyUp">When <see langword="true"/>, the resulting enumeration includes a key up <see cref="INPUT"/> event; otherwise, it doesn't.</param>
        /// <returns>The sequence of <see cref="INPUT"/> events required to type the specified <paramref name="string"/>.</returns>
        public static IEnumerable<INPUT> StringToINPUT(string @string, bool keyDown = true, bool keyUp = true)
            => @string.SelectMany(ch => CharToKeyINPUT(ch, keyDown, keyUp));
        #endregion StringToINPUT

        #endregion Keyboard

        #region Mouse

        #region Constants
        private const ushort ABSOLUTE_COORDINATE_MAX = ushort.MaxValue - 1;
        #endregion Constants

        #region ToAbsCoordinateX
        /// <summary>
        /// Converts the specified <paramref name="xPixels"/> value to absolute coordinates.
        /// </summary>
        /// <param name="xPixels">A horizontal position (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns>The equivalent absolute horizontal coordinate.</returns>
        public static int ToAbsCoordinateX(int xPixels, RECT virtualScreenRect)
        {
            return NormalizeInt(xPixels, (virtualScreenRect.left, virtualScreenRect.right), (ushort.MinValue, ushort.MaxValue));
        }
        /// <inheritdoc cref="ToAbsCoordinateX(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int ToAbsCoordinateX(int xPixels) => ToAbsCoordinateX(xPixels, NativeMethods.GetVirtualScreenRect());
        #endregion ToAbsCoordinateX

        #region ToAbsCoordinateY
        /// <summary>
        /// Converts the specified <paramref name="yPixels"/> value to absolute coordinates.
        /// </summary>
        /// <param name="yPixels">A vertical position (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns>The equivalent absolute vertical coordinate.</returns>
        public static int ToAbsCoordinateY(int yPixels, RECT virtualScreenRect)
        {
            return NormalizeInt(yPixels, (virtualScreenRect.top, virtualScreenRect.bottom), (ushort.MinValue, ushort.MaxValue));
        }
        /// <inheritdoc cref="ToAbsCoordinateY(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int ToAbsCoordinateY(int yPixels) => ToAbsCoordinateY(yPixels, NativeMethods.GetVirtualScreenRect());
        #endregion ToAbsCoordinateY

        #region ToAbsCoordinates
        /// <summary>
        /// Converts the specified <paramref name="xPixels"/> &amp; <paramref name="yPixels"/> values to absolute coordinates.
        /// </summary>
        /// <param name="xPixels">A horizontal position (in pixels) to convert.</param>
        /// <param name="yPixels">A vertical position (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns><see cref="POINT"/> containing the equivalent absolute coordinates.</returns>
        public static POINT ToAbsCoordinates(int xPixels, int yPixels, RECT virtualScreenRect)
        {
            return new(
                xPos: ToAbsCoordinateX(xPixels, virtualScreenRect),
                yPos: ToAbsCoordinateY(yPixels, virtualScreenRect));
        }
        /// <inheritdoc cref="ToAbsCoordinates(ushort, ushort, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT ToAbsCoordinates(int xPixels, int yPixels) => ToAbsCoordinates(xPixels, yPixels, NativeMethods.GetVirtualScreenRect());

        // with POINT:

        /// <summary>
        /// Converts the specified <paramref name="pxPoint"/> to absolute coordinates.
        /// </summary>
        /// <param name="pxPoint">A point (in pixels) to convert.</param>
        /// <param name="virtualScreenRect">The virtual screen rect, from <see cref="NativeMethods.GetVirtualScreenRect"/></param>
        /// <returns><see cref="POINT"/> containing the equivalent absolute coordinates.</returns>
        public static POINT ToAbsCoordinates(POINT pxPoint, RECT virtualScreenRect)
        {
            return new(
                xPos: ToAbsCoordinateX(pxPoint.x, virtualScreenRect),
                yPos: ToAbsCoordinateY(pxPoint.y, virtualScreenRect));
        }
        /// <inheritdoc cref="ToAbsCoordinates(POINT, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT ToAbsCoordinates(POINT pxPoint) => ToAbsCoordinates(pxPoint, NativeMethods.GetVirtualScreenRect());
        #endregion ToAbsCoordinates

        #region FromAbsCoordinateX
        public static int FromAbsCoordinateX(int xAbs, RECT virtualScreenRect)
        {
            return NormalizeInt(xAbs, (ushort.MinValue, ushort.MaxValue), (virtualScreenRect.left, virtualScreenRect.right));
        }
        /// <inheritdoc cref="FromAbsCoordinateX(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int FromAbsCoordinateX(int xAbs) => FromAbsCoordinateX(xAbs, NativeMethods.GetVirtualScreenRect());
        #endregion FromAbsCoordinateX

        #region FromAbsCoordinateY
        public static int FromAbsCoordinateY(int yAbs, RECT virtualScreenRect)
        {
            return NormalizeInt(yAbs, (ushort.MinValue, ushort.MaxValue), (virtualScreenRect.top, virtualScreenRect.bottom));
        }
        /// <inheritdoc cref="FromAbsCoordinateY(int, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static int FromAbsCoordinateY(int yAbs) => FromAbsCoordinateY(yAbs, NativeMethods.GetVirtualScreenRect());
        #endregion FromAbsCoordinateY

        #region FromAbsCoordinates
        public static POINT FromAbsCoordinates(ushort xAbs, ushort yAbs, RECT virtualScreenRect)
        {
            return new(
                xPos: FromAbsCoordinateX(xAbs, virtualScreenRect),
                yPos: FromAbsCoordinateY(yAbs, virtualScreenRect));
        }
        /// <inheritdoc cref="FromAbsCoordinates(ushort, ushort, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT FromAbsCoordinates(ushort xAbs, ushort yAbs) => FromAbsCoordinates(xAbs, yAbs, NativeMethods.GetVirtualScreenRect());

        // with POINT:

        public static POINT FromAbsCoordinates(POINT absPoint, RECT virtualScreenRect)
        {
            return new(
                xPos: FromAbsCoordinateX(absPoint.x, virtualScreenRect),
                yPos: FromAbsCoordinateY(absPoint.y, virtualScreenRect));
        }
        /// <inheritdoc cref="FromAbsCoordinates(POINT, RECT)"/>
        /// <remarks>
        /// When performing multiple conversions, use the overload that accepts the virtual screen <see cref="RECT"/> for better performance.<br/>
        /// The virtual screen rect can be acquired with <see cref="NativeMethods.GetVirtualScreenRect()"/>.
        /// </remarks>
        public static POINT FromAbsCoordinates(POINT absPoint) => FromAbsCoordinates(absPoint, NativeMethods.GetVirtualScreenRect());
        #endregion FromAbsCoordinates

        #region MouseButtonToFlags
        /// <summary>
        /// Converts the specified <paramref name="mouseButton"/> value to the associated <see cref="MOUSEINPUT.Flags"/> value.
        /// </summary>
        /// <param name="mouseButton">The mouse button to get the flag of.</param>
        /// <param name="buttonDown">When <see langword="true"/>, returns the button down flag.<br/>When <see langword="false"/>, returns the button up flag.</param>
        /// <returns><see cref="MOUSEINPUT.Flags"/> value for the specified <paramref name="mouseButton"/>. Whether it is the button down/up flag depends on <paramref name="buttonDown"/>.</returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static MOUSEINPUT.Flags MouseButtonToFlags(EMouseButton mouseButton, bool buttonDown)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (mouseButton)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
            case EMouseButton.Left:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_LEFTDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_LEFTUP;
            case EMouseButton.Right:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_RIGHTDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_RIGHTUP;
            case EMouseButton.Middle:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_MIDDLEDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_MIDDLEUP;
            case EMouseButton.X1:
            case EMouseButton.X2:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_XDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_XUP;
            default:
                throw new InvalidEnumArgumentException(nameof(EMouseButton), (int)mouseButton, typeof(EMouseButton));
            }
        }
        #endregion MouseButtonToFlags

        #region BuildMouseButton...
        public static INPUT BuildMouseButton(EMouseButton mouseButton, bool buttonDown)
        {
            var buttonInput = new MOUSEINPUT()
            {
                dwFlags = MouseButtonToFlags(mouseButton, buttonDown)
            };
            switch (mouseButton)
            { // special handling for XButtons
            case EMouseButton.X1:
                buttonInput.mouseData = MOUSEINPUT.XBUTTON1;
                break;
            case EMouseButton.X2:
                buttonInput.mouseData = MOUSEINPUT.XBUTTON2;
                break;
            }
            return new(buttonInput);
        }
        public static INPUT BuildMouseButtonDown(EMouseButton mouseButton) => BuildMouseButton(mouseButton, buttonDown: true);
        public static INPUT BuildMouseButtonUp(EMouseButton mouseButton) => BuildMouseButton(mouseButton, buttonDown: false);
        public static INPUT[] BuildMouseButtonClick(EMouseButton mouseButton, int clickCount = 1)
        {
            if (clickCount <= 0) throw new ArgumentOutOfRangeException(nameof(clickCount), clickCount, $"Expected a value greater than 0 for {nameof(clickCount)}; actual value was {clickCount}.");

            return new INPUT[clickCount * 2].SetValuesTo(new INPUT[]
            {
                BuildMouseButtonDown(mouseButton),
                BuildMouseButtonUp(mouseButton)
            });
        }
        #endregion BuildMouseButton...

        #region BuildMouseMoveTo
        public static INPUT BuildMouseMoveTo(int xPosition, int yPosition, RECT virtualScreenRect, bool primaryMonitorOnly = false)
        {
            return new INPUT(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_ABSOLUTE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
                dx = ToAbsCoordinateX(xPosition, virtualScreenRect),
                dy = ToAbsCoordinateY(yPosition, virtualScreenRect),
            });
        }
        public static INPUT BuildMouseMoveTo(int x, int y, uint dpi, bool primaryMonitorOnly = false)
            => BuildMouseMoveTo(x, y, NativeMethods.GetVirtualScreenRect(dpi), primaryMonitorOnly);
        public static INPUT BuildMouseMoveTo(int x, int y, bool primaryMonitorOnly = false)
            => BuildMouseMoveTo(x, y, NativeMethods.GetVirtualScreenRect(), primaryMonitorOnly);
        #endregion BuildMouseMoveTo

        #region BuildMouseMoveToAbs
        public static INPUT BuildMouseMoveToAbs(int absX, int absY, bool primaryMonitorOnly = false)
        {
            var mouseInput = new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_MOVE | MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK,
                dx = absX,
                dy = absY,
            };

            if (!primaryMonitorOnly)
                mouseInput.dwFlags |= MOUSEINPUT.Flags.MOUSEEVENTF_VIRTUALDESK;

            return new(mouseInput);
        }
        #endregion BuildMouseMoveToAbs

        public static INPUT BuildMouseHorizontalScroll(int horizontalDelta)
        {
            return new(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_HWHEEL,
                mouseData = horizontalDelta
            });
        }
        public static INPUT[] BuildMouseHorizontalScroll(int horizontalDelta, int count)
        {
            return new INPUT[count].SetValuesTo(BuildMouseHorizontalScroll(horizontalDelta));
        }

        #endregion Mouse
    }
}
