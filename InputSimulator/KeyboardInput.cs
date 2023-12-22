using InputSimulator.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InputSimulator
{
    /// <summary>
    /// Provides <see langword="static"/> methods for simulating keyboard input.
    /// </summary>
    public static class KeyboardInput
    {
        #region KeyState
        /// <summary>
        /// Synthesizes (de)pressing (▼|▲) the specified <paramref name="key"/>, depending on the specified <paramref name="state"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <param name="state">Whether to inject a KeyDown or KeyUp event.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyState(EVirtualKeyCode key, ushort scanCode, EKeyState state)
        {
            return NativeMethods.SendInput(InputHelper.BuildKeyState(key, scanCode, state));
        }
        /// <inheritdoc cref="KeyState(EVirtualKeyCode, ushort, EKeyState)"/>
        /// <param name="keyDown"><see langword="true"/> is <see cref="EKeyState.Down"/>; <see langword="false"/> is <see cref="EKeyState.Up"/>.</param>
        public static bool KeyState(EVirtualKeyCode key, ushort scanCode, bool keyDown) => KeyState(key, scanCode, keyDown ? EKeyState.Down : EKeyState.Up);
        /// <summary>
        /// Synthesizes (de)pressing (▼|▲) the specified <paramref name="key"/>, depending on the specified <paramref name="state"/>.
        /// The associated scan code will be determined automatically.
        /// </summary>
        /// <inheritdoc cref="KeyState(EVirtualKeyCode, ushort, EKeyState)"/>
        public static bool KeyState(EVirtualKeyCode key, EKeyState state)
        {
            return NativeMethods.SendInput(InputHelper.BuildKeyState(key, state));
        }
        /// <inheritdoc cref="KeyState(EVirtualKeyCode, EKeyState)"/>
        /// <param name="keyDown"><see langword="true"/> is <see cref="EKeyState.Down"/>; <see langword="false"/> is <see cref="EKeyState.Up"/>.</param>
        public static bool KeyState(EVirtualKeyCode key, bool keyDown) => KeyState(key, keyDown ? EKeyState.Down : EKeyState.Up);
        /// <summary>
        /// Synthesizes (de)pressing (▼|▲) the specified <paramref name="char"/>, depending on the specified <paramref name="state"/>.
        /// </summary>
        /// <param name="char">A <see cref="char"/> to inject a key event for.</param>
        /// <param name="state">Whether to inject a key down or key up event.</param>
        /// <inheritdoc cref="KeyState(EVirtualKeyCode, ushort, EKeyState)"/>
        public static bool KeyState(char @char, EKeyState state)
        {
            return NativeMethods.SendInput(InputHelper.BuildCharState(@char, state));
        }
        /// <inheritdoc cref="KeyState(char, EKeyState)"/>
        /// <param name="keyDown"><see langword="true"/> is <see cref="EKeyState.Down"/>; <see langword="false"/> is <see cref="EKeyState.Up"/>.</param>
        public static bool KeyState(char @char, bool keyDown) => KeyState(@char, keyDown);
        #endregion KeyState

        #region KeyDown
        /// <summary>
        /// Synthesizes pressing (▼) the key with the specified <paramref name="scanCode"/>.
        /// The associated virtual key code will be determined automatically.
        /// </summary>
        /// <param name="scanCode">The scan code for the key to inject a key down event for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyDown(ushort scanCode) => KeyState(NativeMethods.VirtualKeyCodeFromScanCode(scanCode), scanCode, EKeyState.Down);
        /// <summary>
        /// Synthesizes pressing (▼) the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key down event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyDown(EVirtualKeyCode key, ushort scanCode) => KeyState(key, scanCode, EKeyState.Down);
        /// <summary>
        /// Synthesizes pressing (▼) the specified <paramref name="key"/>.
        /// The associated scan code will be determined automatically.
        /// </summary>
        /// <inheritdoc cref="KeyDown(EVirtualKeyCode, ushort)"/>
        public static bool KeyDown(EVirtualKeyCode key) => KeyState(key, EKeyState.Down);

        // with multiple keys:

        /// <summary>
        /// Synthesizes pressing (▼) the specified <paramref name="keys"/>.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keys">An enumerable list of key codes to inject key down events for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyDown(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(InputHelper.BuildKeyDown(key));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <param name="keys">An array of key codes to inject key down events for.</param>
        /// <inheritdoc cref="KeyDown(IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyDown(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                inputs[i] = InputHelper.BuildKeyDown(keys[i]);
            }
            return NativeMethods.SendInput(inputs);
        }

        // with multiple key-scan code pairs:

        /// <summary>
        /// Synthesizes pressing (▼) the specified keys.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key down events for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyDown(IEnumerable<KeyValuePair<EVirtualKeyCode, ushort>> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyDown(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static bool KeyDown(IEnumerable<(EVirtualKeyCode Key, ushort ScanCode)> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <param name="keys">Array of key-scan code pairs to inject key down events for.</param>
        /// <inheritdoc cref="KeyDown(IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyDown(params KeyValuePair<EVirtualKeyCode, ushort>[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyDown(key, scanCode);
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyDown(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static bool KeyDown(params (EVirtualKeyCode Key, ushort ScanCode)[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyDown(key, scanCode);
            }
            return NativeMethods.SendInput(inputs);
        }

        // with char:

        /// <summary>
        /// Synthesizes pressing (▼) the key for the specified <paramref name="char"/>.
        /// The associated virtual key and scan code will be determined automatically by the OS.
        /// </summary>
        /// <remarks>
        /// Note that a return value of <see langword="true"/> does not necessarily mean the character was actually typed; only that the OS successfully received the request to type it.
        /// The character must actually be typable in order for this to work.
        /// </remarks>
        /// <param name="char">A <see cref="char"/> to simulate pressing the key for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyDown(char @char)
        {
            return NativeMethods.SendInput(InputHelper.BuildCharDown(@char));
        }
        /// <summary>
        /// Synthesizes pressing (▼) the keys for the specified <paramref name="chars"/>.
        /// The associated virtual keys and scan codes will be determined automatically by the OS.
        /// </summary>
        /// <param name="chars">Any number of <see cref="char"/>s to simulate pressing the keys for.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        /// <inheritdoc cref="KeyDown(char)"/>
        public static bool KeyDown(IEnumerable<char> chars)
        {
            List<INPUT> inputs = new();
            foreach (var @char in chars)
            {
                inputs.Add(InputHelper.BuildCharDown(@char));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyDown(IEnumerable{char})"/>
        public static bool KeyDown(params char[] chars)
        {
            var count = chars.Length;
            var inputs = new INPUT[count];
            for (int i = 0; i < count; ++i)
            {
                inputs[i] = InputHelper.BuildCharDown(chars[i]);
            }
            return NativeMethods.SendInput(inputs);
        }
        #endregion KeyDown

        #region KeyUp
        /// <summary>
        /// Synthesizes depressing (▲) the key with the specified <paramref name="scanCode"/>.
        /// The associated virtual key code will be determined automatically.
        /// </summary>
        /// <param name="scanCode">The scan code for the key to inject a key up event for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyUp(ushort scanCode) => KeyState(NativeMethods.VirtualKeyCodeFromScanCode(scanCode), scanCode, EKeyState.Up);
        /// <summary>
        /// Synthesizes depressing (▲) the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key up event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyUp(EVirtualKeyCode key, ushort scanCode) => KeyState(key, scanCode, EKeyState.Up);
        /// <summary>
        /// Synthesizes depressing (▲) the specified <paramref name="key"/>.
        /// The associated scan code will be determined automatically.
        /// </summary>
        /// <inheritdoc cref="KeyUp(EVirtualKeyCode, ushort)"/>
        public static bool KeyUp(EVirtualKeyCode key) => KeyState(key, EKeyState.Up);

        // with multiple keys:

        /// <summary>
        /// Synthesizes depressing (▲) the specified <paramref name="keys"/>.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keys">An enumerable list of key codes to inject key up events for.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyUp(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(InputHelper.BuildKeyUp(key));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <param name="keys">An array of key codes to inject key up events for.</param>
        /// <inheritdoc cref="KeyUp(IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyUp(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                inputs[i] = InputHelper.BuildKeyUp(keys[i]);
            }
            return NativeMethods.SendInput(inputs);
        }

        // with multiple key-scan code pairs:

        /// <summary>
        /// Synthesizes depressing (▲) the specified keys.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key up events for.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyUp(IEnumerable<KeyValuePair<EVirtualKeyCode, ushort>> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyUp(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static bool KeyUp(IEnumerable<(EVirtualKeyCode Key, ushort ScanCode)> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <param name="keys">Array of key-scan code pairs to inject key up events for.</param>
        /// <inheritdoc cref="KeyUp(IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyUp(params KeyValuePair<EVirtualKeyCode, ushort>[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyUp(key, scanCode);
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyUp(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static bool KeyUp(params (EVirtualKeyCode Key, ushort ScanCode)[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyUp(key, scanCode);
            }
            return NativeMethods.SendInput(inputs);
        }

        // with char:

        /// <summary>
        /// Synthesizes depressing (▲) the key for the specified <paramref name="char"/>.
        /// The associated virtual key and scan code will be determined automatically by the OS.
        /// </summary>
        /// <remarks>
        /// Note that a return value of <see langword="true"/> does not necessarily mean the character was actually typed; only that the OS successfully received the request to type it.
        /// The character must actually be typable in order for this to work.
        /// </remarks>
        /// <param name="char">A <see cref="char"/> to simulate depressing the key for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyUp(char @char)
        {
            return NativeMethods.SendInput(InputHelper.BuildCharUp(@char));
        }
        /// <summary>
        /// Synthesizes depressing (▲) the keys for the specified <paramref name="chars"/>.
        /// The associated virtual keys and scan codes will be determined automatically by the OS.
        /// </summary>
        /// <param name="chars">Any number of <see cref="char"/>s to simulate depressing the keys for.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        /// <inheritdoc cref="KeyUp(char)"/>
        public static bool KeyUp(IEnumerable<char> chars)
        {
            List<INPUT> inputs = new();
            foreach (var @char in chars)
            {
                inputs.Add(InputHelper.BuildCharUp(@char));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyUp(IEnumerable{char})"/>
        public static bool KeyUp(params char[] chars)
        {
            var count = chars.Length;
            var inputs = new INPUT[count];
            for (int i = 0; i < count; ++i)
            {
                inputs[i] = InputHelper.BuildCharUp(chars[i]);
            }
            return NativeMethods.SendInput(inputs);
        }
        #endregion KeyUp

        #region KeyPress
        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the key with the specified <paramref name="scanCode"/>.
        /// The associated virtual key code will be determined automatically.
        /// </summary>
        /// <param name="scanCode">The scan code for the key to inject a key down &amp; key up event for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyPress(ushort scanCode) => NativeMethods.SendInput(InputHelper.BuildKeyPress(NativeMethods.VirtualKeyCodeFromScanCode(scanCode), scanCode));
        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key down &amp; key up event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyPress(EVirtualKeyCode key, ushort scanCode) => NativeMethods.SendInput(InputHelper.BuildKeyPress(key, scanCode));
        /// <inheritdoc cref="KeyPress(EVirtualKeyCode, ushort)"/>
        public static bool KeyPress(EVirtualKeyCode key) => NativeMethods.SendInput(InputHelper.BuildKeyPress(key));

        // with multiple keys

        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the specified <paramref name="keys"/>.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keys">An enumerable list of key codes to inject key down &amp; key up events for.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyPress(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.AddRange(InputHelper.BuildKeyPress(key));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <param name="keys">An array of key codes to inject key key down &amp; key up events for.</param>
        /// <inheritdoc cref="KeyPress(IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyPress(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount * 2];
            int i_inputs = 0;
            for (int i = 0; i < keysCount; ++i)
            {
                var key = keys[i];
                inputs[i_inputs++] = InputHelper.BuildKeyDown(key);
                inputs[i_inputs++] = InputHelper.BuildKeyUp(key);
            }
            return NativeMethods.SendInput(inputs);
        }

        // with multiple key-scan code pairs

        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the specified keys.
        /// </summary>
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key up events for.</param>
        /// <inheritdoc cref="KeyPress(IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyPress(IEnumerable<KeyValuePair<EVirtualKeyCode, ushort>> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyPress(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static bool KeyPress(IEnumerable<(EVirtualKeyCode Key, ushort ScanCode)> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key up events for.</param>
        /// <inheritdoc cref="KeyPress(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static bool KeyPress(params KeyValuePair<EVirtualKeyCode, ushort>[] keyScanCodePairs)
        {
            var keysCount = keyScanCodePairs.Length;
            var inputs = new INPUT[keysCount * 2];
            int i_inputs = 0;
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keyScanCodePairs[i];
                inputs[i_inputs++] = InputHelper.BuildKeyDown(key, scanCode);
                inputs[i_inputs++] = InputHelper.BuildKeyUp(key, scanCode);
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyPress(KeyValuePair{EVirtualKeyCode, ushort}[])"/>
        public static bool KeyPress(params (EVirtualKeyCode Key, ushort ScanCode)[] keyScanCodePairs)
        {
            var keysCount = keyScanCodePairs.Length;
            var inputs = new INPUT[keysCount * 2];
            int i_inputs = 0;
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keyScanCodePairs[i];
                inputs[i_inputs++] = InputHelper.BuildKeyDown(key, scanCode);
                inputs[i_inputs++] = InputHelper.BuildKeyUp(key, scanCode);
            }
            return NativeMethods.SendInput(inputs);
        }

        // with char:

        /// <summary>
        /// Synthesizes depressing (▲) the key for the specified <paramref name="char"/>.
        /// The associated virtual key and scan code will be determined automatically by the OS.
        /// </summary>
        /// <remarks>
        /// Note that a return value of <see langword="true"/> does not necessarily mean the character was actually typed; only that the OS successfully received the request to type it.
        /// The character must actually be typable in order for this to work.
        /// </remarks>
        /// <param name="char">A <see cref="char"/> to simulate depressing the key for.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyPress(char @char)
        {
            return NativeMethods.SendInput(InputHelper.BuildCharPress(@char));
        }
        /// <summary>
        /// Synthesizes depressing (▲) the keys for the specified <paramref name="chars"/>.
        /// The associated virtual keys and scan codes will be determined automatically by the OS.
        /// </summary>
        /// <param name="chars">Any number of <see cref="char"/>s to simulate depressing the keys for.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        /// <inheritdoc cref="KeyPress(char)"/>
        public static bool KeyPress(IEnumerable<char> chars)
        {
            List<INPUT> inputs = new();
            foreach (var @char in chars)
            {
                inputs.AddRange(InputHelper.BuildCharPress(@char));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyPress(IEnumerable{char})"/>
        public static bool KeyPress(params char[] chars)
        {
            var count = chars.Length;
            var inputs = new INPUT[count * 2];
            int i_inputs = 0;
            for (int i = 0; i < count; ++i)
            {
                var charDownUpInputs = InputHelper.BuildCharPress(chars[i]);
                inputs[i_inputs++] = charDownUpInputs[0];
                inputs[i_inputs++] = charDownUpInputs[1];
            }
            return NativeMethods.SendInput(inputs);
        }
        #endregion KeyPress

        #region KeyStroke
        /// <summary>
        /// Synthesizes pressing (▼...,▲...) all of the specified <paramref name="keys"/>, then releasing all of them.
        /// </summary>
        /// <param name="keys">The keys in the key stroke.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyStroke(params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(Array.Empty<EVirtualKeyCode>(), keys));
        /// <inheritdoc cref="KeyStroke(EVirtualKeyCode[])"/>
        public static bool KeyStroke(IEnumerable<EVirtualKeyCode> keys)
            => KeyStroke(keys.ToArray());

        // with enumerable modifier keys:

        /// <summary>
        /// Synthesizes pressing (▼...,▲...) the specified key stroke.
        /// </summary>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        /// <param name="keys">The keys in the key stroke.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyStroke(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(modifierKeys.ToArray(), keys.ToArray()));
        /// <summary>
        /// Synthesizes pressing (▼...,▲...) the specified key stroke.
        /// </summary>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        /// <param name="keys">The keys in the key stroke.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyStroke(IEnumerable<EVirtualKeyCode> modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(modifierKeys.ToArray(), keys));
        /// <summary>
        /// Synthesizes pressing (▼...,▲...) the specified key stroke.
        /// </summary>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        /// <param name="chars">The <see cref="char"/>s in the key stroke.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool KeyStroke(IEnumerable<EVirtualKeyCode> modifierKeys, params char[] chars)
            => NativeMethods.SendInput(InputHelper.BuildCharStroke(modifierKeys.ToArray(), chars));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyStroke(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(modifierKeys.ToVirtualKeyCodes(), keys.ToArray()));
        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static bool KeyStroke(EModifierKeys modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => KeyStroke(modifierKeys, keys.ToArray());
        #endregion KeyStroke

        #region FromText
        /// <summary>
        /// Synthesizes the keystroke(s) to type the specified <paramref name="char"/>.
        /// </summary>
        /// <param name="char">A <see cref="char"/> to synthesize as keyboard input.</param>
        /// <returns><see langword="true"/> when the input was successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool FromText(char @char)
            => NativeMethods.SendInput(InputHelper.CharToKeyINPUT(@char));
        /// <summary>
        /// Synthesizes the keystroke(s) to type the specified <paramref name="string"/>.
        /// </summary>
        /// <param name="string">A <see cref="string"/> to synthesize as keyboard input.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool FromText(string @string)
            => NativeMethods.SendInput(InputHelper.StringToINPUT(@string));
        /// <summary>
        /// Synthesizes the keystroke(s) to type the specified <paramref name="strings"/>.
        /// </summary>
        /// <param name="strings">An enumerable list of strings to concatenate together and synthesize as keyboard input.</param>
        /// <returns><see langword="true"/> when the inputs were successfully injected; otherwise, <see langword="false"/>.</returns>
        public static bool FromText(IEnumerable<string> strings) => FromText(string.Join(string.Empty, strings));
        /// <param name="strings"><see cref="string"/> array to concatenate together and synthesize as keyboard input.</param>
        /// <inheritdoc cref="FromText(IEnumerable{string})"/>
        public static bool FromText(params string[] strings) => FromText(string.Join(string.Empty, strings));
        /// <inheritdoc cref="FromText(IEnumerable{string})"/>
        public static bool FromText(params InputHelper.InstanceOrArray<string>[] strings) => FromText(strings.SelectMany(s => s));
        #endregion FromText
    }
}
