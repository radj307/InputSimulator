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
    /// Helper methods for performing conversions and creating <see cref="INPUT"/> events.
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

        #region (Class) InstanceOrArray<T>
        /// <summary>
        /// Variant helper class for the <see cref="Expand{T}(InstanceOrArray{T}[])"/> method that accepts <typeparamref name="T"/> instances or arrays.
        /// </summary>
        /// <remarks>
        /// Implicitly convertible from the following types:
        /// <list type="bullet">
        /// <item><typeparamref name="T"/></item>
        /// <item><typeparamref name="T"/>[]</item>
        /// <item><see cref="System.Collections.ObjectModel.Collection{T}"/> (where T is <typeparamref name="T"/>)</item>
        /// <item><see cref="List{T}"/> (where T is <typeparamref name="T"/>)</item>
        /// <item><see cref="Queue{T}"/> (where T is <typeparamref name="T"/>)</item>
        /// <item><see cref="Stack{T}"/> (where T is <typeparamref name="T"/>)</item>
        /// </list>
        /// </remarks>
        /// <typeparam name="T">The type of item being processed.</typeparam>
        public class InstanceOrArray<T> : IEnumerable<T>
        {
            #region Constructor
            internal InstanceOrArray(params T[] items) => _items = items;
            #endregion Constructor

            #region Fields
            private readonly T[] _items;
            #endregion Fields

            #region Conversion Operators
            public static implicit operator InstanceOrArray<T>(T item) => new(item);
            public static implicit operator InstanceOrArray<T>(T[] items) => new(items);
            // various collection types since conversion operators can't use interfaces
            public static implicit operator InstanceOrArray<T>(System.Collections.ObjectModel.Collection<T> items) => new(items.ToArray());
            public static implicit operator InstanceOrArray<T>(List<T> items) => new(items.ToArray());
            public static implicit operator InstanceOrArray<T>(Queue<T> items) => new(items.ToArray());
            public static implicit operator InstanceOrArray<T>(Stack<T> items) => new(items.ToArray());
            public static implicit operator T[](InstanceOrArray<T> instance) => instance._items;
            #endregion Conversion Operators

            #region IEnumerable<T>
            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
            #endregion IEnumerable<T>
        }
        #endregion (Class) InstanceOrArray<T>

        #region MakeArray
        /// <summary>
        /// Expands any arrays in the specified <paramref name="source"/> items into a contiguous sequence without disrupting the order.
        /// </summary>
        /// <remarks>
        /// You can use this to combine the outputs of many <c>Build</c>... methods that return <see cref="INPUT"/> instances or arrays.<br/>
        /// It works in a similar manner to <c>SelectMany</c>, except that it also accepts individual items in addition to enumerables.
        /// </remarks>
        /// <param name="source">Any number of items of type <typeparamref name="T"/> and/or <see cref="IEnumerable{T}"/> sequences of type <typeparamref name="T"/>.</param>
        /// <returns>A contiguous sequence of <typeparamref name="T"/> items in the same order as the <paramref name="source"/>.</returns>
        public static IEnumerable<INPUT> MakeEnumerable(params InstanceOrArray<INPUT>[] source) => source.SelectMany(item => item);
        public static INPUT[] MakeArray(params InstanceOrArray<INPUT>[] source) => MakeEnumerable(source).ToArray();
        #endregion MakeArray

        #region Keyboard

        #region BuildKeyState
        /// <summary>
        /// Creates an <see cref="INPUT"/> structure that represents an injected key event.
        /// </summary>
        /// <param name="keyCode">The key code of the key to set the state of.</param>
        /// <param name="scanCode">The scan code associated with the specified <paramref name="keyCode"/>.</param>
        /// <param name="state">The state to set the key to in the injected input event.</param>
        /// <returns><see cref="INPUT"/> structure that sets the specified key to the specified state.</returns>
        /// <exception cref="InvalidEnumArgumentException">The specified <paramref name="state"/> isn't a valid <see cref="EKeyState"/> value.</exception>
        public static INPUT BuildKeyState(EVirtualKeyCode keyCode, ushort scanCode, EKeyState state)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (state)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
            case EKeyState.Down:
                return BuildKeyDown(keyCode, scanCode);
            case EKeyState.Up:
                return BuildKeyUp(keyCode, scanCode);
            default:
                throw new InvalidEnumArgumentException(nameof(state), (int)state, typeof(EKeyState));
            }
        }
        /// <inheritdoc cref="BuildKeyState(EVirtualKeyCode, ushort, EKeyState)"/>
        public static INPUT BuildKeyState(EVirtualKeyCode keyCode, EKeyState state)
            => BuildKeyState(keyCode, NativeMethods.VirtualKeyCodeToScanCode(keyCode), state);
        #endregion BuildKeyState

        #region BuildKeyDown
        public static INPUT BuildKeyDown(EVirtualKeyCode keyCode, ushort scanCode)
        {
            return new INPUT(KEYBDINPUT.GetKeyDown(keyCode, scanCode));
        }
        public static INPUT BuildKeyDown(EVirtualKeyCode keyCode)
        {
            return new INPUT(KEYBDINPUT.GetKeyDown(keyCode));
        }
        #endregion BuildKeyDown

        #region BuildKeysDown
        public static INPUT[] BuildKeysDown(EVirtualKeyCode[] keys)
        {
            var keysLength = keys.Length;
            var inputs = new INPUT[keysLength];

            for (int i = 0; i < keysLength; ++i)
            {
                inputs[i] = BuildKeyDown(keys[i]);
            }

            return inputs.ToArray();
        }
        #endregion BuildKeysDown

        #region BuildKeyUp
        public static INPUT BuildKeyUp(EVirtualKeyCode keyCode, ushort scanCode)
        {
            return new INPUT(KEYBDINPUT.GetKeyUp(keyCode, scanCode));
        }
        public static INPUT BuildKeyUp(EVirtualKeyCode keyCode)
        {
            return new INPUT(KEYBDINPUT.GetKeyUp(keyCode));
        }
        #endregion BuildKeyUp

        #region BuildKeysUp
        public static INPUT[] BuildKeysUp(EVirtualKeyCode[] keys)
        {
            var keysLength = keys.Length;
            var inputs = new INPUT[keysLength];

            for (int i = 0; i < keysLength; ++i)
            {
                inputs[i] = BuildKeyUp(keys[i]);
            }

            return inputs.ToArray();
        }
        #endregion BuildKeysUp

        #region BuildKeyPress
        public static INPUT[] BuildKeyPress(EVirtualKeyCode keyCode, ushort scanCode)
            => new[]
            {
                BuildKeyDown(keyCode, scanCode),
                BuildKeyUp(keyCode, scanCode)
            };
        public static INPUT[] BuildKeyPress(EVirtualKeyCode keyCode)
            => new[]
            {
                BuildKeyDown(keyCode),
                BuildKeyUp(keyCode)
            };
        #endregion BuildKeyPress

        #region BuildKeyStrokeDown
        public static INPUT[] BuildKeyStrokeDown(EVirtualKeyCode[] modifierKeys, EVirtualKeyCode[] keys)
        {
            var inputs = new INPUT[modifierKeys.Length + keys.Length];
            int i_inputs = 0;

            foreach (var modifierKey in modifierKeys)
            {
                inputs[i_inputs++] = BuildKeyDown(modifierKey);
            }
            foreach (var key in keys)
            {
                inputs[i_inputs++] = BuildKeyDown(key);
            }

            return inputs;
        }
        #endregion BuildKeyStrokeDown

        #region BuildKeyStrokeUp
        public static INPUT[] BuildKeyStrokeUp(EVirtualKeyCode[] modifierKeys, EVirtualKeyCode[] keys)
        {
            var inputs = new INPUT[modifierKeys.Length + keys.Length];
            int i_inputs = 0;

            foreach (var key in keys)
            {
                inputs[i_inputs++] = BuildKeyUp(key);
            }
            foreach (var modifierKey in modifierKeys)
            {
                inputs[i_inputs++] = BuildKeyUp(modifierKey);
            }

            return inputs;
        }
        #endregion BuildKeyStrokeUp

        #region BuildKeyStroke
        /// <summary>
        /// Creates <see cref="INPUT"/> structures for the specified key stroke.
        /// </summary>
        /// <param name="modifierKeys">The <see cref="EVirtualKeyCode"/> values of the modifier keys in the key stroke.</param>
        /// <param name="keys">The <see cref="EVirtualKeyCode"/> values of the keys in the key stroke.</param>
        /// <returns>
        /// An <see cref="INPUT"/> array that contains structures to perform the following steps:<br/>
        /// 1. All of the <paramref name="modifierKeys"/> are pressed down (▼...).<br/>
        /// 2. Each of the <paramref name="keys"/> is pressed in sequence. (▼▲...)<br/>
        /// 2. All of the <paramref name="modifierKeys"/> are all released (▲...).<br/>
        /// The length of the returned array is equal to 2 times the combined number of <paramref name="modifierKeys"/> &amp; <paramref name="keys"/>.
        /// </returns>
        public static INPUT[] BuildKeyStroke(EVirtualKeyCode[] modifierKeys, EVirtualKeyCode[] keys)
        {
            var inputs = new INPUT[modifierKeys.Length * 2 + keys.Length * 2];
            int i_inputs = 0;

            int i_modKeyUpInputs = modifierKeys.Length + keys.Length * 2;
            foreach (var modifier in modifierKeys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyDown(modifier));
                inputs[i_modKeyUpInputs++] = BuildKeyUp(modifier);
            }

            int i_keyUpInputs = i_inputs + keys.Length;
            foreach (var key in keys)
            {
                inputs[i_inputs++] = new(KEYBDINPUT.GetKeyDown(key));
                inputs[i_keyUpInputs++] = BuildKeyUp(key);
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
            case EMouseButton.LeftButton:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_LEFTDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_LEFTUP;
            case EMouseButton.RightButton:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_RIGHTDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_RIGHTUP;
            case EMouseButton.MiddleButton:
                return buttonDown
                    ? MOUSEINPUT.Flags.MOUSEEVENTF_MIDDLEDOWN
                    : MOUSEINPUT.Flags.MOUSEEVENTF_MIDDLEUP;
            case EMouseButton.XButton1:
            case EMouseButton.XButton2:
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
            case EMouseButton.XButton1:
                buttonInput.mouseData = MOUSEINPUT.XBUTTON1;
                break;
            case EMouseButton.XButton2:
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
        public static INPUT[] BuildMouseButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, int restoreToX, int restoreToY, RECT virtualScreenRect)
        {
            if (clickCount < 0)
                throw new ArgumentOutOfRangeException(nameof(clickCount), clickCount, $"{nameof(clickCount)} cannot be negative.");
            else if (clickCount == 0)
                return Array.Empty<INPUT>();

            INPUT[] inputSequence = new INPUT[2 + (2 * clickCount)];
            int i = 0;

            inputSequence[i++] = BuildMouseMoveTo(x, y, virtualScreenRect);
            foreach (var input in BuildMouseButtonClick(mouseButton, clickCount))
            {
                inputSequence[i++] = input;
            }
            inputSequence[i++] = BuildMouseMoveTo(restoreToX, restoreToY, virtualScreenRect);

            return inputSequence;
        }
        public static INPUT[] BuildMouseButtonClickAt(EMouseButton mouseButton, int x, int y, int clickCount, RECT virtualScreenRect)
        {
            if (clickCount < 0)
                throw new ArgumentOutOfRangeException(nameof(clickCount), clickCount, $"{nameof(clickCount)} cannot be negative.");
            else if (clickCount == 0)
                return Array.Empty<INPUT>();

            INPUT[] inputSequence = new INPUT[1 + (2 * clickCount)];
            int i = 0;

            inputSequence[i++] = BuildMouseMoveTo(x, y, virtualScreenRect);
            foreach (var input in BuildMouseButtonClick(mouseButton, clickCount))
            {
                inputSequence[i++] = input;
            }

            return inputSequence;
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

        #region BuildMouseVerticalScroll
        public static INPUT BuildMouseVerticalScroll(int delta)
        {
            return new(new MOUSEINPUT()
            {
                dwFlags = MOUSEINPUT.Flags.MOUSEEVENTF_WHEEL,
                mouseData = delta
            });
        }
        public static INPUT[] BuildMouseVerticalScroll(int delta, int count)
        {
            return new INPUT[count].SetValuesTo(BuildMouseVerticalScroll(delta));
        }
        #endregion BuildMouseVerticalScroll

        #region BuildMouseHorizontalScroll
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
        #endregion BuildMouseHorizontalScroll

        #endregion Mouse
    }
}
