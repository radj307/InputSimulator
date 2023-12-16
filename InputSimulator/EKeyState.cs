namespace InputSimulator
{
    /// <summary>
    /// Defines the possible states that a keyboard key can be set to.
    /// </summary>
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
