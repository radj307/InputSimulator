using InputSimulator;
using System.Windows.Input;

namespace InputSimulator.Interop
{
    public static class VirtualKeyExtensions
    {
        #region ToVirtualKeyCode
        /// <summary>
        /// Convert to Key to a <see cref="EVirtualKeyCode"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A <see cref="EVirtualKeyCode"/> value representing the specified <see cref="Key"/> value.</returns>
        public static EVirtualKeyCode ToVirtualKeyCode(this Key key)
            => (EVirtualKeyCode)KeyInterop.VirtualKeyFromKey(key);
        /// <summary>
        /// Convert to Key to a <see cref="EVirtualKeyCode"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A <see cref="EVirtualKeyCode"/> value representing the specified <see cref="System.Windows.Forms.Keys"/> value.</returns>
        public static EVirtualKeyCode ToVirtualKeyCode(this System.Windows.Forms.Keys key)
            => unchecked((EVirtualKeyCode)key); //< 4 bytes => 2 bytes
        #endregion ToVirtualKeyCode

        /// <summary>
        /// Convert the VirtualKey to a <see cref="Key"/> value.
        /// </summary>
        /// <param name="virtualKey"></param>
        /// <returns>A <see cref="Key"/> value representing the specified <see cref="EVirtualKeyCode"/> value.</returns>
        public static Key ToKey(this EVirtualKeyCode virtualKey)
            => KeyInterop.KeyFromVirtualKey((int)virtualKey);
        /// <summary>
        /// Convert the VirtualKey to a <see cref="Key"/> value.
        /// </summary>
        /// <param name="virtualKey"></param>
        /// <returns>A <see cref="Key"/> value representing the specified <see cref="EVirtualKeyCode"/> value.</returns>
        public static System.Windows.Forms.Keys ToFormsKey(this EVirtualKeyCode virtualKey)
            => (System.Windows.Forms.Keys)virtualKey; //< 2 bytes => 4 bytes
    }
}