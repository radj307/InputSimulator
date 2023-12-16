namespace InputSimulator
{
    /// <summary>
    /// Defines supported mouse buttons.
    /// </summary>
    public enum EMouseButton : byte
    {
        /// <summary>
        /// The left mouse button.
        /// </summary>
        LeftButton,
        /// <summary>
        /// The right mouse button.
        /// </summary>
        RightButton,
        /// <summary>
        /// The middle mouse button.
        /// </summary>
        MiddleButton,
        /// <summary>
        /// The first extension button.
        /// </summary>
        /// <remarks>
        /// XButton1 is usually the "back" button, closer to the thumb. Not all mice have XButtons.
        /// </remarks>
        XButton1,
        /// <summary>
        /// The second extension button.
        /// </summary>
        /// <remarks>
        /// XButton2 is usually the "forward" button, further from the thumb. Not all mice have XButtons.
        /// </remarks>
        XButton2,
    }
}
