namespace InputSimulator
{
    /// <summary>
    /// Defines the possible states that a keyboard key can be set to.
    /// </summary>
    /// <remarks>
    /// This is used as an input parameter by methods in the <see cref="KeyboardInput"/> class.
    /// </remarks>
    public enum EKeyState : byte
    {
        /// <summary>
        /// The key was depressed (▲).
        /// </summary>
        Up = 0,
        /// <summary>
        /// The key was pressed (▼).
        /// </summary>
        Down = 1,
    }
}
