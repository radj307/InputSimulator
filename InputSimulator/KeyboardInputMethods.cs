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
        #region KeyDown
        /// <summary>
        /// Synthesizes the specified <paramref name="key"/> being pressed.
        /// </summary>
        /// <param name="key">The <see cref="EVirtualKeyCode"/> value associated with the key to simulate pressing.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool KeyDown(EVirtualKeyCode key)
            => NativeMethods.SendInput(new INPUT(new KEYBDINPUT()
            {
                wVk = key
            }));
        /// <summary>
        /// Synthesizes pressing the specified <paramref name="keys"/> in sequence.
        /// </summary>
        /// <remarks>
        /// Differs from <see cref="KeyStrokeDown(IEnumerable{EVirtualKeyCode})"/> in that each key is pressed one after the other, rather than all at once.
        /// </remarks>
        /// <param name="keys">Any number of <see cref="EVirtualKeyCode"/> values associated with the keys to press.</param>
        /// <returns>The number of key inputs that were successfully sent.</returns>
        public static uint KeyDown(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(new(new KEYBDINPUT()
                {
                    wVk = key
                }));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyDown(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyDown(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                inputs[i] = new(new KEYBDINPUT()
                {
                    wVk = keys[i]
                });
            }
            return NativeMethods.SendInput(inputs);
        }
        #endregion KeyDown

        #region KeyUp
        /// <summary>
        /// Synthesizes the specified <paramref name="key"/> being released.
        /// </summary>
        /// <param name="key">The <see cref="EVirtualKeyCode"/> value associated with the key to simulate pressing.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/>.</returns>
        public static bool KeyUp(EVirtualKeyCode key)
            => NativeMethods.SendInput(new INPUT(new KEYBDINPUT()
            {
                wVk = key,
                dwFlags = KEYBDINPUT.Flags.KEYEVENTF_KEYUP
            }));
        /// <summary>
        /// Synthesizes releasing the specified <paramref name="keys"/> in sequence.
        /// </summary>
        /// <remarks>
        /// Differs from <see cref="KeyStrokeUp(IEnumerable{EVirtualKeyCode})"/> in that each key is released one after the other, rather than all at once.
        /// </remarks>
        /// <param name="keys">Any number of <see cref="EVirtualKeyCode"/> values associated with the keys to release.</param>
        /// <returns>The number of key inputs that were successfully sent.</returns>
        public static uint KeyUp(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(new(new KEYBDINPUT()
                {
                    wVk = key,
                    dwFlags = KEYBDINPUT.Flags.KEYEVENTF_KEYUP
                }));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyUp(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyUp(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount];
            for (int i = 0; i < keysCount; ++i)
            {
                inputs[i] = new(new KEYBDINPUT()
                {
                    wVk = keys[i]
                });
            }
            return NativeMethods.SendInput(inputs);
        }
        #endregion KeyUp

        #region KeyPress
        /// <summary>
        /// Synthesizes a key press of the specified <paramref name="key"/>, including a KeyDown &amp; KeyUp event.
        /// </summary>
        /// <param name="key">The <see cref="EVirtualKeyCode"/> value associated with the key to simulate pressing.</param>
        /// <returns><see langword="true"/> when successful; otherwise, <see langword="false"/> when either the key down or key up event failed.</returns>
        public static bool KeyPress(EVirtualKeyCode key)
            => 2u == NativeMethods.SendInput(
                new(new KEYBDINPUT()
                { // key down
                    wVk = key
                }),
                new(new KEYBDINPUT()
                { // key up
                    wVk = key,
                    dwFlags = KEYBDINPUT.Flags.KEYEVENTF_KEYUP
                }));
        /// <summary>
        /// Synthesizes pressing &amp; releasing the specified <paramref name="keys"/> in sequence.
        /// </summary>
        /// <remarks>
        /// Differs from <see cref="KeyStroke(IEnumerable{EVirtualKeyCode})"/> in that each key is pressed &amp; released one after the other, rather than all at once.
        /// </remarks>
        /// <param name="keys">Any number of <see cref="EVirtualKeyCode"/> values associated with the keys to press &amp; release.</param>
        /// <returns>The number of key inputs that were successfully sent.</returns>
        public static uint KeyPress(IEnumerable<EVirtualKeyCode> keys)
        {
            List<INPUT> inputs = new();
            foreach (var key in keys)
            {
                inputs.Add(new(new KEYBDINPUT()
                { // key down
                    wVk = key
                }));
                inputs.Add(new(new KEYBDINPUT()
                { // key up
                    wVk = key,
                    dwFlags = KEYBDINPUT.Flags.KEYEVENTF_KEYUP
                }));
            }
            return NativeMethods.SendInput(inputs);
        }
        /// <inheritdoc cref="KeyPress(IEnumerable{EVirtualKeyCode})"/>
        public static uint KeyPress(params EVirtualKeyCode[] keys)
        {
            var keysCount = keys.Length;
            var inputs = new INPUT[keysCount * 2];
            int i_inputs = 0;
            for (int i = 0; i < keysCount; ++i)
            {
                var key = keys[i];
                inputs[i_inputs++] = new(new KEYBDINPUT()
                {
                    wVk = key
                });
                inputs[i_inputs++] = new(new KEYBDINPUT()
                {
                    wVk = key,
                    dwFlags = KEYBDINPUT.Flags.KEYEVENTF_KEYUP
                });
            }
            return NativeMethods.SendInput(inputs);
        }
        #endregion KeyPress

        #region KeyStrokeDown
        /// <summary>
        /// Synthesizes pressing the specified <paramref name="keys"/> at the same time.
        /// </summary>
        /// <param name="keys">The keys in the key stroke.</param>
        public static void KeyStrokeDown(params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStrokeDown(Array.Empty<EVirtualKeyCode>(), keys));
        /// <inheritdoc cref="KeyStrokeDown(EVirtualKeyCode[])"/>
        public static void KeyStrokeDown(IEnumerable<EVirtualKeyCode> keys)
            => KeyStrokeDown(keys.ToArray());

        // with enumerable modifier keys:

        /// <inheritdoc cref="KeyStrokeDown(IEnumerable{EVirtualKeyCode})"/>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        public static void KeyStrokeDown(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStrokeDown(modifierKeys.ToArray(), keys.ToArray()));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStrokeDown(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeDown(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStrokeDown(modifierKeys.ToVirtualKeyCodes(), keys.ToArray()));
        /// <inheritdoc cref="KeyStrokeDown(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeDown(EModifierKeys modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => KeyStrokeDown(modifierKeys, keys.ToArray());
        #endregion KeyStrokeDown

        #region KeyStrokeUp
        /// <summary>
        /// Synthesizes releasing the specified <paramref name="keys"/> at the same time.
        /// </summary>
        /// <param name="keys">The keys in the key stroke.</param>
        public static void KeyStrokeUp(params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStrokeUp(Array.Empty<EVirtualKeyCode>(), keys));
        /// <inheritdoc cref="KeyStrokeUp(EVirtualKeyCode[])"/>
        public static void KeyStrokeUp(IEnumerable<EVirtualKeyCode> keys)
            => KeyStrokeUp(keys.ToArray());

        // with enumerable modifier keys:

        /// <inheritdoc cref="KeyStrokeUp(IEnumerable{EVirtualKeyCode})"/>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        public static void KeyStrokeUp(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStrokeUp(modifierKeys.ToArray(), keys.ToArray()));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStrokeUp(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeUp(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStrokeUp(modifierKeys.ToVirtualKeyCodes(), keys));
        /// <inheritdoc cref="KeyStrokeUp(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStrokeUp(EModifierKeys modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => KeyStrokeUp(modifierKeys, keys.ToArray());
        #endregion KeyStrokeUp

        #region KeyStroke
        /// <summary>
        /// Synthesizes pressing and then releasing the specified <paramref name="keys"/> at the same time.
        /// </summary>
        /// <param name="keys">The keys in the key stroke.</param>
        public static void KeyStroke(params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(Array.Empty<EVirtualKeyCode>(), keys));
        /// <inheritdoc cref="KeyStroke(EVirtualKeyCode[])"/>
        public static void KeyStroke(IEnumerable<EVirtualKeyCode> keys)
            => KeyStroke(keys.ToArray());

        // with enumerable modifier keys:

        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode})"/>
        /// <param name="modifierKeys">The modifier keys in the key stroke.</param>
        public static void KeyStroke(IEnumerable<EVirtualKeyCode> modifierKeys, IEnumerable<EVirtualKeyCode> keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(modifierKeys.ToArray(), keys.ToArray()));

        // with modifier keys enum:

        /// <inheritdoc cref="KeyStroke(IEnumerable{EVirtualKeyCode}, IEnumerable{EVirtualKeyCode})"/>
        public static void KeyStroke(EModifierKeys modifierKeys, params EVirtualKeyCode[] keys)
            => NativeMethods.SendInput(InputHelper.BuildKeyStroke(modifierKeys.ToVirtualKeyCodes(), keys.ToArray()));
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
            => NativeMethods.SendInput(InputHelper.CharToKeyINPUT(character));
        /// <summary>
        /// Synthesizes the keystroke(s) to type the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="character">A <see cref="string"/> to simulate typing.</param>
        public static void WriteText(string text)
            => NativeMethods.SendInput(InputHelper.StringToINPUT(text));
        #endregion WriteText
    }
}
