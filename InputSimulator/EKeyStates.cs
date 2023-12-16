using System;

namespace InputSimulator
{
    /// <summary>
    /// Defines the possible states of a physical key on the keyboard.
    /// </summary>
    [Flags]
    public enum EKeyStates : byte
    {
        /// <summary>
        /// The key is not currently being pressed down.<br/>
        /// For toggleable keys like CapsLock, NumLock, &amp; ScrollLock, this also means it is toggled off (disabled).
        /// </summary>
        Up = 0,
        /// <summary>
        /// The key is currently pressed down.
        /// </summary>
        Down = 1,
        /// <summary>
        /// The key is toggled on.
        /// </summary>
        /// <remarks>
        /// This state is only used by toggleable keys like CapsLock, NumLock, &amp; ScrollLock.
        /// </remarks>
        Toggled = 2,
    }
}
