using InputSimulator.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InputSimulator
{
    /// <summary>
    /// Provides <see langword="static"/> methods for simulating keyboard input.
    /// </summary>
    public static class KeyboardInputMethods
    {
        #region KeyState
        /// <summary>
        /// Synthesizes (de)pressing (▼|▲) the specified <paramref name="key"/>, depending on the specified <paramref name="state"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <param name="state">Whether to inject a KeyDown or KeyUp event.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool KeyState(EVirtualKeyCode key, ushort scanCode, EKeyState state)
        {
            return NativeMethods.SendInput(InputHelper.BuildKeyState(key, scanCode, state));
        }
        /// <inheritdoc cref="KeyState(EVirtualKeyCode, ushort, EKeyState)"/>
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
        #endregion KeyState

        #region KeyDown
        /// <summary>
        /// Synthesizes pressing (▼) the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key down event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
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
        /// <returns>The number of inputs that were successfully injected.</returns>
        public static uint KeyDown(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(InputHelper.BuildKeyDown(key));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <param name="keys">An array of key codes to inject key down events for.</param>
        /// <inheritdoc cref="KeyDown(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyDown(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                inputs[i] = InputHelper.BuildKeyDown(keys[i]);
            }
            return NativeMethods.SendInputs(inputs);
        }

        // with multiple key-scan code pairs:

        /// <summary>
        /// Synthesizes pressing (▼) the specified keys.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key down events for.</param>
        /// <returns>The number of inputs that were successfully injected.</returns>
        public static uint KeyDown(IEnumerable<KeyValuePair<EVirtualKeyCode, ushort>> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <inheritdoc cref="KeyDown(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static uint KeyDown(IEnumerable<(EVirtualKeyCode Key, ushort ScanCode)> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <param name="keys">Array of key-scan code pairs to inject key down events for.</param>
        /// <inheritdoc cref="KeyDown(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyDown(params KeyValuePair<EVirtualKeyCode, ushort>[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyDown(key, scanCode);
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <inheritdoc cref="KeyDown(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static uint KeyDown(params (EVirtualKeyCode Key, ushort ScanCode)[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyDown(key, scanCode);
            }
            return NativeMethods.SendInputs(inputs);
        }
        #endregion KeyDown

        #region KeyUp
        /// <summary>
        /// Synthesizes depressing (▲) the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key up event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
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
        /// <returns>The number of inputs that were successfully injected.</returns>
        public static uint KeyUp(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(InputHelper.BuildKeyUp(key));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <param name="keys">An array of key codes to inject key up events for.</param>
        /// <inheritdoc cref="KeyUp(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyUp(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                inputs[i] = InputHelper.BuildKeyUp(keys[i]);
            }
            return NativeMethods.SendInputs(inputs);
        }

        // with multiple key-scan code pairs:

        /// <summary>
        /// Synthesizes depressing (▲) the specified keys.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key up events for.</param>
        /// <returns>The number of inputs that were successfully injected.</returns>
        public static uint KeyUp(IEnumerable<KeyValuePair<EVirtualKeyCode, ushort>> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <inheritdoc cref="KeyUp(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static uint KeyUp(IEnumerable<(EVirtualKeyCode Key, ushort ScanCode)> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <param name="keys">Array of key-scan code pairs to inject key up events for.</param>
        /// <inheritdoc cref="KeyUp(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyUp(params KeyValuePair<EVirtualKeyCode, ushort>[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyUp(key, scanCode);
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <inheritdoc cref="KeyUp(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static uint KeyUp(params (EVirtualKeyCode Key, ushort ScanCode)[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                var (key, scanCode) = keys[i];
                inputs[i] = InputHelper.BuildKeyUp(key, scanCode);
            }
            return NativeMethods.SendInputs(inputs);
        }
        #endregion KeyUp

        #region KeyPress
        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key code to inject a key down &amp; key up event for.</param>
        /// <param name="scanCode">The scan code for the specified <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool KeyPress(EVirtualKeyCode key, ushort scanCode)
            => NativeMethods.SendInput(InputHelper.BuildKeyPress(key, scanCode));
        /// <inheritdoc cref="KeyPress(EVirtualKeyCode, ushort)"/>
        public static bool KeyPress(EVirtualKeyCode key)
            => NativeMethods.SendInput(InputHelper.BuildKeyPress(key));

        // with multiple keys

        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the specified <paramref name="keys"/>.
        /// The associated scan codes will be determined automatically.
        /// </summary>
        /// <param name="keys">An enumerable list of key codes to inject key down &amp; key up events for.</param>
        /// <returns>The number of inputs that were successfully injected.</returns>
        public static uint KeyPress(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(InputHelper.BuildKeyDown(key));
                inputs.Add(InputHelper.BuildKeyUp(key));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <param name="keys">An array of key codes to inject key key down &amp; key up events for.</param>
        /// <inheritdoc cref="KeyPress(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyPress(params EVirtualKeyCode[] keys)
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
            return NativeMethods.SendInputs(inputs);
        }

        // with multiple key-scan code pairs

        /// <summary>
        /// Synthesizes pressing &amp; depressing (▼▲) the specified keys.
        /// </summary>
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key up events for.</param>
        /// <inheritdoc cref="KeyPress(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyPress(IEnumerable<KeyValuePair<EVirtualKeyCode, ushort>> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <inheritdoc cref="KeyPress(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static uint KeyPress(IEnumerable<(EVirtualKeyCode Key, ushort ScanCode)> keyScanCodePairs)
        {
            List<INPUT> inputs = new();
            foreach (var (key, scanCode) in keyScanCodePairs)
            {
                inputs.Add(InputHelper.BuildKeyDown(key, scanCode));
                inputs.Add(InputHelper.BuildKeyUp(key, scanCode));
            }
            return NativeMethods.SendInputs(inputs);
        }
        /// <param name="keyScanCodePairs">An enumerable list of key-scan code pairs to inject key up events for.</param>
        /// <inheritdoc cref="KeyPress(IEnumerable{KeyValuePair{EVirtualKeyCode, ushort}})"/>
        public static uint KeyPress(params KeyValuePair<EVirtualKeyCode, ushort>[] keyScanCodePairs)
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
            return NativeMethods.SendInputs(inputs);
        }
        /// <inheritdoc cref="KeyPress(KeyValuePair{EVirtualKeyCode, ushort}[])"/>
        public static uint KeyPress(params (EVirtualKeyCode Key, ushort ScanCode)[] keyScanCodePairs)
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
            return NativeMethods.SendInputs(inputs);
        }
        #endregion KeyPress

        #region KeyStrokeDown
        /// <summary>
        /// Synthesizes pressing (▼...,▼...) the specified <paramref name="modifierKeys"/>, then the specified <paramref name="keys"/>.
        /// </summary>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        /// <param name="keys">The keys in the key stroke.</param>
        public static void KeyStrokeDown(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStrokeDown(modifierKeys.ToArray(), keys.ToArray()));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStrokeDown(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeDown(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStrokeDown(modifierKeys.ToVirtualKeyCodes(), keys.ToArray()));
        /// <inheritdoc cref="KeyStrokeDown(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeDown(EModifierKeys modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => KeyStrokeDown(modifierKeys, keys.ToArray());
        #endregion KeyStrokeDown

        #region KeyStrokeUp
        /// <summary>
        /// Synthesizes releasing (▲...,▲...) the specified <paramref name="keys"/>, then the specified <paramref name="modifierKeys"/>.
        /// </summary>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        /// <param name="keys">The keys in the key stroke.</param>
        public static void KeyStrokeUp(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStrokeUp(modifierKeys.ToArray(), keys.ToArray()));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStrokeUp(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeUp(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStrokeUp(modifierKeys.ToVirtualKeyCodes(), keys));
        /// <inheritdoc cref="KeyStrokeUp(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeUp(EModifierKeys modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => KeyStrokeUp(modifierKeys, keys.ToArray());
        #endregion KeyStrokeUp

        #region KeyStroke
        /// <summary>
        /// Synthesizes pressing and then releasing (▼...,▲...) the specified <paramref name="keys"/> at the same time.
        /// </summary>
        /// <param name="keys">The keys in the key stroke.</param>
        public static void KeyStroke(params EVirtualKeyCode[] keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStroke(Array.Empty<EVirtualKeyCode>(), keys));
        /// <inheritdoc cref="KeyStroke(EVirtualKeyCode[])"/>
        public static void KeyStroke(IEnumerable<EVirtualKeyCode> keys)
            => KeyStroke(keys.ToArray());

        // with enumerable modifier keys:

        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode})"/>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        public static void KeyStroke(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStroke(modifierKeys.ToArray(), keys.ToArray()));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStroke(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInputs(InputHelper.BuildKeyStroke(modifierKeys.ToVirtualKeyCodes(), keys.ToArray()));
        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStroke(EModifierKeys modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => KeyStroke(modifierKeys, keys.ToArray());
        #endregion SendKeyStroke

        #region WriteText
        /// <summary>
        /// Synthesizes the keystroke(s) to type the specified <paramref name="character"/>.
        /// </summary>
        /// <param name="character">A <see cref="char"/> to simulate typing.</param>
        public static void WriteText(char character)
            => NativeMethods.SendInputs(InputHelper.CharToKeyINPUT(character));
        /// <summary>
        /// Synthesizes the keystroke(s) to type the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="character">A <see cref="string"/> to simulate typing.</param>
        public static void WriteText(string text)
            => NativeMethods.SendInputs(InputHelper.StringToINPUT(text));
        #endregion WriteText
    }
}
