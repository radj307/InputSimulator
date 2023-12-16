namespace InputSimulator
{
    /// <summary>
    /// Windows Virtual Key Codes.<br/>
    /// MSDN Documentation: <see href="https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes"/>
    /// </summary>
    /// <remarks>
    /// This includes every labelled virtual key used by windows, as well as alternative common names for most keys.
    /// <br/><br/>
    /// Names that start with <b><c>VK_</c></b> are the default virtual key names, as well as the letter keys (<b><c>A-Z</c></b>) and digits (<b><c>D0-D9</c></b>).<br/>
    /// All other names are alternatives added for easier usage.
    /// </remarks>
    public enum EVirtualKeyCode : ushort
    {
        /// <summary>Null</summary>
        None = 0x00,

        /// <summary>Left mouse button</summary>
        VK_LBUTTON = 0x01,
        /// <inheritdoc cref="VK_LBUTTON" />
        MouseLeftButton = VK_LBUTTON,

        /// <summary>Right mouse button</summary>
        VK_RBUTTON = 0x02,
        /// <inheritdoc cref="VK_RBUTTON" />
        MouseRightButton = VK_RBUTTON,

        /// <summary>Control-break processing</summary>
        VK_CANCEL = 0x03,
        /// <inheritdoc cref="VK_CANCEL" />
        Cancel = VK_CANCEL,

        /// <summary>Middle mouse button (three-button mouse)</summary>
        VK_MBUTTON = 0x04,
        /// <inheritdoc cref="VK_MBUTTON" />
        MouseMiddleButton = VK_MBUTTON,

        /// <summary>X1 mouse button</summary>
        VK_XBUTTON1 = 0x05,
        /// <inheritdoc cref="VK_XBUTTON1" />
        MouseXButton1 = VK_XBUTTON1,

        /// <summary>X2 mouse button</summary>
        VK_XBUTTON2 = 0x06,
        /// <inheritdoc cref="VK_XBUTTON2" />
        MouseXButton2 = VK_XBUTTON2,

        /// <summary>BACKSPACE key</summary>
        VK_BACK = 0x08,
        /// <inheritdoc cref="VK_BACK" />
        Backspace = VK_BACK,

        /// <summary>TAB key</summary>
        VK_TAB = 0x09,
        /// <inheritdoc cref="VK_TAB" />
        Tab = VK_TAB,

        /// <summary>CLEAR key</summary>
        VK_CLEAR = 0x0C,
        /// <inheritdoc cref="VK_CLEAR" />
        Clear = VK_CLEAR,

        /// <summary>ENTER key</summary>
        /// <remarks>This is used for both the primary and numpad Enter keys.</remarks>
        VK_RETURN = 0x0D,
        /// <inheritdoc cref="VK_RETURN" />
        Enter = VK_RETURN,
        /// <inheritdoc cref="VK_RETURN" />
        Return = VK_RETURN,

        /// <summary>SHIFT key</summary>
        VK_SHIFT = 0x10,
        /// <inheritdoc cref="VK_SHIFT" />
        Shift = VK_SHIFT,
       
        /// <summary>CTRL key</summary>
        VK_CONTROL = 0x11,
        /// <inheritdoc cref="VK_CONTROL" />
        Control = VK_CONTROL,
      
        /// <summary>ALT key</summary>
        VK_MENU = 0x12,
        /// <inheritdoc cref="VK_MENU" />
        Alt = VK_MENU,
      
        /// <summary>PAUSE key</summary>
        VK_PAUSE = 0x13,
        /// <inheritdoc cref="VK_PAUSE" />
        Pause = VK_PAUSE,
     
        /// <summary>CAPS LOCK key</summary>
        VK_CAPITAL = 0x14,
        /// <inheritdoc cref="VK_CAPITAL" />
        CapsLock = VK_CAPITAL,
       
        /// <summary>IME Kana mode</summary>
        VK_KANA = 0x15,
        /// <inheritdoc cref="VK_KANA" />
        ImeKana = VK_KANA,
       
        /// <summary>IME Hangul mode</summary>
        VK_HANGUL = 0x15,
        /// <inheritdoc cref="VK_HANGUL" />
        ImeHangul = VK_HANGUL,
      
        /// <summary>IME On</summary>
        VK_IME_ON = 0x16,
        /// <inheritdoc cref="VK_IME_ON" />
        ImeOn = VK_IME_ON,
      
        /// <summary>IME Junja mode</summary>
        VK_JUNJA = 0x17,
        /// <inheritdoc cref="VK_JUNJA" />
        ImeJunja = VK_JUNJA,
      
        /// <summary>IME final mode</summary>
        VK_FINAL = 0x18,
        /// <inheritdoc cref="VK_FINAL" />
        ImeFinal = VK_FINAL,
      
        /// <summary>IME Hanja mode</summary>
        VK_HANJA = 0x19,
        /// <inheritdoc cref="VK_HANJA" />
        ImeHanja = VK_HANJA,
       
        /// <summary>IME Kanji mode</summary>
        VK_KANJI = 0x19,
        /// <inheritdoc cref="VK_KANJI" />
        ImeKanji = VK_KANJI,
       
        /// <summary>IME Off</summary>
        VK_IME_OFF = 0x1A,
        /// <inheritdoc cref="VK_IME_OFF" />
        ImeOff = VK_IME_OFF,
       
        /// <summary>ESC key</summary>
        VK_ESCAPE = 0x1B,
        /// <inheritdoc cref="VK_ESCAPE" />
        Escape = VK_ESCAPE,
       
        /// <summary>IME convert</summary>
        VK_CONVERT = 0x1C,
        /// <inheritdoc cref="VK_CONVERT" />
        ImeConvert = VK_CONVERT,
       
        /// <summary>IME nonconvert</summary>
        VK_NONCONVERT = 0x1D,
        /// <inheritdoc cref="VK_NONCONVERT" />
        ImeNonConvert = VK_NONCONVERT,
       
        /// <summary>IME accept</summary>
        VK_ACCEPT = 0x1E,
        /// <inheritdoc cref="VK_ACCEPT" />
        ImeAccept = VK_ACCEPT,
        
        /// <summary>IME mode change request</summary>
        VK_MODECHANGE = 0x1F,
        /// <inheritdoc cref="VK_MODECHANGE" />
        ImeModeChange = VK_MODECHANGE,
        
        /// <summary>SPACEBAR</summary>
        VK_SPACE = 0x20,
        /// <inheritdoc cref="VK_SPACE" />
        Space = VK_SPACE,
        
        /// <summary>PAGE UP key</summary>
        VK_PRIOR = 0x21,
        /// <inheritdoc cref="VK_PRIOR" />
        PageUp = VK_PRIOR,
        
        /// <summary>PAGE DOWN key</summary>
        VK_NEXT = 0x22,
        /// <inheritdoc cref="VK_NEXT" />
        PageDown = VK_NEXT,
       
        /// <summary>END key</summary>
        VK_END = 0x23,
        /// <inheritdoc cref="VK_END" />
        End = VK_END,
        
        /// <summary>HOME key</summary>
        VK_HOME = 0x24,
        /// <inheritdoc cref="VK_HOME" />
        Home = VK_HOME,
       
        /// <summary>LEFT ARROW key</summary>
        VK_LEFT = 0x25,
        /// <inheritdoc cref="VK_LEFT" />
        LeftArrow = VK_LEFT,
       
        /// <summary>UP ARROW key</summary>
        VK_UP = 0x26,
        /// <inheritdoc cref="VK_UP" />
        UpArrow = VK_UP,
        
        /// <summary>RIGHT ARROW key</summary>
        VK_RIGHT = 0x27,
        /// <inheritdoc cref="VK_RIGHT" />
        RightArrow = VK_RIGHT,
       
        /// <summary>DOWN ARROW key</summary>
        VK_DOWN = 0x28,
        /// <inheritdoc cref="VK_DOWN" />
        DownArrow = VK_DOWN,
       
        /// <summary>SELECT key</summary>
        VK_SELECT = 0x29,
        /// <inheritdoc cref="VK_SELECT" />
        Select = VK_SELECT,
        
        /// <summary>PRINT key</summary>
        /// <remarks><b>Note:</b> This not NOT the printscreen key! (See <see cref="VK_SNAPSHOT"/>)</remarks>
        VK_PRINT = 0x2A,
        /// <inheritdoc cref="VK_PRINT" />
        Print = VK_PRINT,
        
        /// <summary>EXECUTE key</summary>
        VK_EXECUTE = 0x2B,
        /// <inheritdoc cref="VK_EXECUTE" />
        Execute = VK_EXECUTE,
       
        /// <summary>PRINT SCREEN key</summary>
        VK_SNAPSHOT = 0x2C,
        /// <inheritdoc cref="VK_SNAPSHOT" />
        PrintScreen = VK_SNAPSHOT,
       
        /// <summary>INS key</summary>
        VK_INSERT = 0x2D,
        /// <inheritdoc cref="VK_INSERT" />
        Insert = VK_INSERT,
       
        /// <summary>DEL key</summary>
        VK_DELETE = 0x2E,
        /// <inheritdoc cref="VK_DELETE" />
        Delete = VK_DELETE,
        
        /// <summary>HELP key</summary>
        VK_HELP = 0x2F,
        /// <inheritdoc cref="VK_HELP" />
        Help = VK_HELP,
        
        /// <summary>0 Key</summary>
        D0 = 0x30,
        /// <inheritdoc cref="D0" />
        Zero = D0,
       
        /// <summary>1 Key</summary>
        D1 = 0x31,
        /// <inheritdoc cref="D1" />
        One = D1,
      
        /// <summary>2 Key</summary>
        D2 = 0x32,
        /// <inheritdoc cref="D2" />
        Two = D2,
       
        /// <summary>3 Key</summary>
        D3 = 0x33,
        /// <inheritdoc cref="D3" />
        Three = D3,
       
        /// <summary>4 Key</summary>
        D4 = 0x34,
        /// <inheritdoc cref="D4" />
        Four = D4,
      
        /// <summary>5 Key</summary>
        D5 = 0x35,
        /// <inheritdoc cref="D5" />
        Five = D5,
      
        /// <summary>6 Key</summary>
        D6 = 0x36,
        /// <inheritdoc cref="D6" />
        Six = D6,
      
        /// <summary>7 Key</summary>
        D7 = 0x37,
        /// <inheritdoc cref="D7" />
        Seven = D7,
      
        /// <summary>8 Key</summary>
        D8 = 0x38,
        /// <inheritdoc cref="D8" />
        Eight = D8,
      
        /// <summary>9 Key</summary>
        D9 = 0x39,
        /// <inheritdoc cref="D9" />
        Nine = D9,
      
        /// <summary>A Key</summary>
        A = 0x41,
        /// <summary>B Key</summary>
        B = 0x42,
        /// <summary>C Key</summary>
        C = 0x43,
        /// <summary>D Key</summary>
        D = 0x44,
        /// <summary>E Key</summary>
        E = 0x45,
        /// <summary>F Key</summary>
        F = 0x46,
        /// <summary>G Key</summary>
        G = 0x47,
        /// <summary>H Key</summary>
        H = 0x48,
        /// <summary>I Key</summary>
        I = 0x49,
        /// <summary>J Key</summary>
        J = 0x4A,
        /// <summary>K Key</summary>
        K = 0x4B,
        /// <summary>L Key</summary>
        L = 0x4C,
        /// <summary>M Key</summary>
        M = 0x4D,
        /// <summary>N Key</summary>
        N = 0x4E,
        /// <summary>O Key</summary>
        O = 0x4F,
        /// <summary>P Key</summary>
        P = 0x50,
        /// <summary>Q Key</summary>
        Q = 0x51,
        /// <summary>R Key</summary>
        R = 0x52,
        /// <summary>S Key</summary>
        S = 0x53,
        /// <summary>T Key</summary>
        T = 0x54,
        /// <summary>U Key</summary>
        U = 0x55,
        /// <summary>V Key</summary>
        V = 0x56,
        /// <summary>W Key</summary>
        W = 0x57,
        /// <summary>X Key</summary>
        X = 0x58,
        /// <summary>Y Key</summary>
        Y = 0x59,
        /// <summary>Z Key</summary>
        Z = 0x5A,
       
        /// <summary>Left Windows key (Natural keyboard)</summary>
        VK_LWIN = 0x5B,
        /// <inheritdoc cref="VK_LWIN" />
        LeftWindows = VK_LWIN,
      
        /// <summary>Right Windows key (Natural keyboard)</summary>
        VK_RWIN = 0x5C,
        /// <inheritdoc cref="VK_RWIN" />
        RightWindows = VK_RWIN,
       
        /// <summary>Applications key (Natural keyboard)</summary>
        VK_APPS = 0x5D,
        /// <inheritdoc cref="VK_APPS" />
        Apps = VK_APPS,
       
        /// <summary>Computer Sleep key</summary>
        VK_SLEEP = 0x5F,
        /// <inheritdoc cref="VK_SLEEP" />
        Sleep = VK_SLEEP,
        
        /// <summary>Numeric keypad 0 key</summary>
        VK_NUMPAD0 = 0x60,
        /// <inheritdoc cref="VK_NUMPAD0" />
        Numpad0 = VK_NUMPAD0,
       
        /// <summary>Numeric keypad 1 key</summary>
        VK_NUMPAD1 = 0x61,
        /// <inheritdoc cref="VK_NUMPAD1" />
        Numpad1 = VK_NUMPAD1,
       
        /// <summary>Numeric keypad 2 key</summary>
        VK_NUMPAD2 = 0x62,
        /// <inheritdoc cref="VK_NUMPAD2" />
        Numpad2 = VK_NUMPAD2,
       
        /// <summary>Numeric keypad 3 key</summary>
        VK_NUMPAD3 = 0x63,
        /// <inheritdoc cref="VK_NUMPAD3" />
        Numpad3 = VK_NUMPAD3,
       
        /// <summary>Numeric keypad 4 key</summary>
        VK_NUMPAD4 = 0x64,
        /// <inheritdoc cref="VK_NUMPAD4" />
        Numpad4 = VK_NUMPAD4,
       
        /// <summary>Numeric keypad 5 key</summary>
        VK_NUMPAD5 = 0x65,
        /// <inheritdoc cref="VK_NUMPAD5" />
        Numpad5 = VK_NUMPAD5,
      
        /// <summary>Numeric keypad 6 key</summary>
        VK_NUMPAD6 = 0x66,
        /// <inheritdoc cref="VK_NUMPAD6" />
        Numpad6 = VK_NUMPAD6,
      
        /// <summary>Numeric keypad 7 key</summary>
        VK_NUMPAD7 = 0x67,
        /// <inheritdoc cref="VK_NUMPAD7" />
        Numpad7 = VK_NUMPAD7,
      
        /// <summary>Numeric keypad 8 key</summary>
        VK_NUMPAD8 = 0x68,
        /// <inheritdoc cref="VK_NUMPAD8" />
        Numpad8 = VK_NUMPAD8,
      
        /// <summary>Numeric keypad 9 key</summary>
        VK_NUMPAD9 = 0x69,
        /// <inheritdoc cref="VK_NUMPAD9" />
        Numpad9 = VK_NUMPAD9,
       
        /// <summary>Multiply key</summary>
        VK_MULTIPLY = 0x6A,
        /// <inheritdoc cref="VK_MULTIPLY" />
        NumpadMultiply = VK_MULTIPLY,
        
        /// <summary>Add key</summary>
        VK_ADD = 0x6B,
        /// <inheritdoc cref="VK_ADD" />
        NumpadAdd = VK_ADD,
        
        /// <summary>Separator key. (Not generally found on keyboards.)</summary>
        VK_SEPARATOR = 0x6C,
        /// <inheritdoc cref="VK_SEPARATOR" />
        Separator = VK_SEPARATOR,
        
        /// <summary>Subtract key</summary>
        VK_SUBTRACT = 0x6D,
        /// <inheritdoc cref="VK_SUBTRACT" />
        NumpadSubtract = VK_SUBTRACT,
        
        /// <summary>Decimal key</summary>
        VK_DECIMAL = 0x6E,
        /// <inheritdoc cref="VK_DECIMAL" />
        NumpadDecimal = VK_DECIMAL,
        
        /// <summary>Divide key</summary>
        VK_DIVIDE = 0x6F,
        /// <inheritdoc cref="VK_DIVIDE" />
        NumpadDivide = VK_DIVIDE,
        
        /// <summary>F1 key</summary>
        VK_F1 = 0x70,
        /// <inheritdoc cref="VK_F1" />
        F1 = VK_F1,
        
        /// <summary>F2 key</summary>
        VK_F2 = 0x71,
        /// <inheritdoc cref="VK_F2" />
        F2 = VK_F2,
        
        /// <summary>F3 key</summary>
        VK_F3 = 0x72,
        /// <inheritdoc cref="VK_F3" />
        F3 = VK_F3,
        
        /// <summary>F4 key</summary>
        VK_F4 = 0x73,
        /// <inheritdoc cref="VK_F4" />
        F4 = VK_F4,
        
        /// <summary>F5 key</summary>
        VK_F5 = 0x74,
        /// <inheritdoc cref="VK_F5" />
        F5 = VK_F5,
        
        /// <summary>F6 key</summary>
        VK_F6 = 0x75,
        /// <inheritdoc cref="VK_F6" />
        F6 = VK_F6,
        
        /// <summary>F7 key</summary>
        VK_F7 = 0x76,
        /// <inheritdoc cref="VK_F7" />
        F7 = VK_F7,
        
        /// <summary>F8 key</summary>
        VK_F8 = 0x77,
        /// <inheritdoc cref="VK_F8" />
        F8 = VK_F8,
        
        /// <summary>F9 key</summary>
        VK_F9 = 0x78,
        /// <inheritdoc cref="VK_F9" />
        F9 = VK_F9,
        
        /// <summary>F10 key</summary>
        VK_F10 = 0x79,
        /// <inheritdoc cref="VK_F10" />
        F10 = VK_F10,
        
        /// <summary>F11 key</summary>
        VK_F11 = 0x7A,
        /// <inheritdoc cref="VK_F11" />
        F11 = VK_F11,
        
        /// <summary>F12 key</summary>
        VK_F12 = 0x7B,
        /// <inheritdoc cref="VK_F12" />
        F12 = VK_F12,
        
        /// <summary>F13 key</summary>
        VK_F13 = 0x7C,
        /// <inheritdoc cref="VK_F13" />
        F13 = VK_F13,
        
        /// <summary>F14 key</summary>
        VK_F14 = 0x7D,
        /// <inheritdoc cref="VK_F14" />
        F14 = VK_F14,
        
        /// <summary>F15 key</summary>
        VK_F15 = 0x7E,
        /// <inheritdoc cref="VK_F15" />
        F15 = VK_F15,
        
        /// <summary>F16 key</summary>
        VK_F16 = 0x7F,
        /// <inheritdoc cref="VK_F16" />
        F16 = VK_F16,
        
        /// <summary>F17 key</summary>
        VK_F17 = 0x80,
        /// <inheritdoc cref="VK_F17" />
        F17 = VK_F17,
        
        /// <summary>F18 key</summary>
        VK_F18 = 0x81,
        /// <inheritdoc cref="VK_F18" />
        F18 = VK_F18,
        
        /// <summary>F19 key</summary>
        VK_F19 = 0x82,
        /// <inheritdoc cref="VK_F19" />
        F19 = VK_F19,
        
        /// <summary>F20 key</summary>
        VK_F20 = 0x83,
        /// <inheritdoc cref="VK_F20" />
        F20 = VK_F20,
        
        /// <summary>F21 key</summary>
        VK_F21 = 0x84,
        /// <inheritdoc cref="VK_F21" />
        F21 = VK_F21,
        
        /// <summary>F22 key</summary>
        VK_F22 = 0x85,
        /// <inheritdoc cref="VK_F22" />
        F22 = VK_F22,
        
        /// <summary>F23 key</summary>
        VK_F23 = 0x86,
        /// <inheritdoc cref="VK_F23" />
        F23 = VK_F23,
        
        /// <summary>F24 key</summary>
        VK_F24 = 0x87,
        /// <inheritdoc cref="VK_F24" />
        F24 = VK_F24,
        
        /// <summary>NUM LOCK key</summary>
        VK_NUMLOCK = 0x90,
        /// <inheritdoc cref="VK_NUMLOCK" />
        NumLock = VK_NUMLOCK,
        
        /// <summary>SCROLL LOCK key</summary>
        VK_SCROLL = 0x91,
        /// <inheritdoc cref="VK_SCROLL" />
        ScrollLock = VK_SCROLL,
        
        /// <summary>Left SHIFT key</summary>
        VK_LSHIFT = 0xA0,
        /// <inheritdoc cref="VK_LSHIFT" />
        LeftShift = VK_LSHIFT,
        
        /// <summary>Right SHIFT key</summary>
        VK_RSHIFT = 0xA1,
        /// <inheritdoc cref="VK_RSHIFT" />
        RightShift = VK_RSHIFT,
        
        /// <summary>Left CONTROL key</summary>
        VK_LCONTROL = 0xA2,
        /// <inheritdoc cref="VK_LCONTROL" />
        LeftControl = VK_LCONTROL,
        
        /// <summary>Right CONTROL key</summary>
        VK_RCONTROL = 0xA3,
        /// <inheritdoc cref="VK_RCONTROL" />
        RightControl = VK_RCONTROL,
        
        /// <summary>Left ALT key</summary>
        VK_LMENU = 0xA4,
        /// <inheritdoc cref="VK_LMENU" />
        LeftAlt = VK_LMENU,
     
        /// <summary>Right ALT key</summary>
        VK_RMENU = 0xA5,
        /// <inheritdoc cref="VK_RMENU" />
        RightAlt = VK_RMENU,
        
        /// <summary>Browser Back key</summary>
        VK_BROWSER_BACK = 0xA6,
        /// <inheritdoc cref="VK_BROWSER_BACK" />
        BrowserBack = VK_BROWSER_BACK,
        
        /// <summary>Browser Forward key</summary>
        VK_BROWSER_FORWARD = 0xA7,
        /// <inheritdoc cref="VK_BROWSER_FORWARD" />
        BrowserForward = VK_BROWSER_FORWARD,
        
        /// <summary>Browser Refresh key</summary>
        VK_BROWSER_REFRESH = 0xA8,
        /// <inheritdoc cref="VK_BROWSER_REFRESH" />
        BrowserRefresh = VK_BROWSER_REFRESH,
        
        /// <summary>Browser Stop key</summary>
        VK_BROWSER_STOP = 0xA9,
        /// <inheritdoc cref="VK_BROWSER_STOP" />
        BrowserStop = VK_BROWSER_STOP,
        
        /// <summary>Browser Search key</summary>
        VK_BROWSER_SEARCH = 0xAA,
        /// <inheritdoc cref="VK_BROWSER_SEARCH" />
        BrowserSearch = VK_BROWSER_SEARCH,
        
        /// <summary>Browser Favorites key</summary>
        VK_BROWSER_FAVORITES = 0xAB,
        /// <inheritdoc cref="VK_BROWSER_FAVORITES" />
        BrowserFavorites = VK_BROWSER_FAVORITES,
        
        /// <summary>Browser Start and Home key</summary>
        VK_BROWSER_HOME = 0xAC,
        /// <inheritdoc cref="VK_BROWSER_HOME" />
        BrowserHome = VK_BROWSER_HOME,
        
        /// <summary>Volume Mute key</summary>
        VK_VOLUME_MUTE = 0xAD,
        /// <inheritdoc cref="VK_VOLUME_MUTE" />
        VolumeMute = VK_VOLUME_MUTE,
        
        /// <summary>Volume Down key</summary>
        VK_VOLUME_DOWN = 0xAE,
        /// <inheritdoc cref="VK_VOLUME_DOWN" />
        VolumeDown = VK_VOLUME_DOWN,
        
        /// <summary>Volume Up key</summary>
        VK_VOLUME_UP = 0xAF,
        /// <inheritdoc cref="VK_VOLUME_UP" />
        VolumeUp = VK_VOLUME_UP,
        
        /// <summary>Next Track key</summary>
        VK_MEDIA_NEXT_TRACK = 0xB0,
        /// <inheritdoc cref="VK_MEDIA_NEXT_TRACK" />
        MediaNextTrack = VK_MEDIA_NEXT_TRACK,
        
        /// <summary>Previous Track key</summary>
        VK_MEDIA_PREV_TRACK = 0xB1,
        /// <inheritdoc cref="VK_MEDIA_PREV_TRACK" />
        MediaPrevTrack = VK_MEDIA_PREV_TRACK,
        
        /// <summary>Stop Media key</summary>
        VK_MEDIA_STOP = 0xB2,
        /// <inheritdoc cref="VK_MEDIA_STOP" />
        MediaStop = VK_MEDIA_STOP,
        
        /// <summary>Play/Pause Media key</summary>
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        /// <inheritdoc cref="VK_MEDIA_PLAY_PAUSE" />
        MediaPlayPause = VK_MEDIA_PLAY_PAUSE,
        
        /// <summary>Start Mail key</summary>
        VK_LAUNCH_MAIL = 0xB4,
        /// <inheritdoc cref="VK_LAUNCH_MAIL" />
        LaunchMail = VK_LAUNCH_MAIL,

        /// <summary>Select Media key</summary>
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        /// <inheritdoc cref="VK_LAUNCH_MEDIA_SELECT" />
        LaunchMediaSelect = VK_LAUNCH_MEDIA_SELECT,

        /// <summary>Start Application 1 key</summary>
        VK_LAUNCH_APP1 = 0xB6,
        /// <inheritdoc cref="VK_LAUNCH_APP1" />
        LaunchApp1 = VK_LAUNCH_APP1,

        /// <summary>Start Application 2 key</summary>
        VK_LAUNCH_APP2 = 0xB7,
        /// <inheritdoc cref="VK_LAUNCH_APP2" />
        LaunchApp2 = VK_LAUNCH_APP2,

        /// <summary>For the US standard keyboard, the <c>;:</c> (semicolon/colon) key. Otherwise, it can vary by keyboard.</summary>
        VK_OEM_1 = 0xBA,
        /// <inheritdoc cref="VK_OEM_1" />
        Semicolon = VK_OEM_1,
        
        /// <summary>For any country/region, the <c>=+</c> (equals/plus) key.</summary>
        VK_OEM_PLUS = 0xBB,
        /// <inheritdoc cref="VK_OEM_PLUS" />
        Equals = VK_OEM_PLUS,
        /// <inheritdoc cref="VK_OEM_PLUS" />
        Plus = VK_OEM_PLUS,

        /// <summary>For any country/region, the <c>,</c> (comma) key.</summary>
        VK_OEM_COMMA = 0xBC,
        /// <inheritdoc cref="VK_OEM_COMMA" />
        Comma = VK_OEM_COMMA,

        /// <summary>For any country/region, the <c>-</c> (dash) key.</summary>
        VK_OEM_MINUS = 0xBD,
        /// <inheritdoc cref="VK_OEM_MINUS" />
        Dash = VK_OEM_MINUS,
        /// <inheritdoc cref="VK_OEM_MINUS" />
        Minus = VK_OEM_MINUS,

        /// <summary>For any country/region, the <c>.</c> (period) key.</summary>
        VK_OEM_PERIOD = 0xBE,
        /// <inheritdoc cref="VK_OEM_PERIOD" />
        Period = VK_OEM_PERIOD,

        /// <summary>For the US standard keyboard, the <c>/?</c> (slash/question mark) key. Otherwise, it can vary by keyboard.</summary>
        VK_OEM_2 = 0xBF,
        /// <inheritdoc cref="VK_OEM_2" />
        Slash = VK_OEM_2,
        /// <inheritdoc cref="VK_OEM_2" />
        QuestionMark = VK_OEM_2,

        /// <summary>The OEM tilde key on a US standard keyboard</summary>
        VK_OEM_3 = 0xC0,
        /// <inheritdoc cref="VK_OEM_3" />
        Grave = VK_OEM_3,
        /// <inheritdoc cref="VK_OEM_3" />
        Tilde = VK_OEM_3,

        /// <summary>For the US standard keyboard, the <c>[{</c> (open bracket) key. Otherwise, it can vary by keyboard.</summary>
        VK_OEM_4 = 0xDB,
        /// <inheritdoc cref="VK_OEM_4" />
        OpenBracket = VK_OEM_4,

        /// <summary>The OEM pipe key on a US standard keyboard</summary>
        VK_OEM_5 = 0xDC,
        /// <inheritdoc cref="VK_OEM_5" />
        Backslash = VK_OEM_5,
        /// <inheritdoc cref="VK_OEM_5" />
        Pipe = VK_OEM_5,

        /// <summary>For the US standard keyboard, the <c>]}</c> (close bracket) key. Otherwise, it can vary by keyboard.</summary>
        VK_OEM_6 = 0xDD,
        /// <inheritdoc cref="VK_OEM_6" />
        CloseBracket = VK_OEM_6,

        /// <summary>The OEM single/double quote key on a US standard keyboard.</summary>
        VK_OEM_7 = 0xDE,
        /// <inheritdoc cref="VK_OEM_7" />
        Quote = VK_OEM_7,

        /// <summary>Used for miscellaneous characters; it can vary by keyboard.</summary>
        VK_OEM_8 = 0xDF,

        /// <summary>The <c>&lt;&gt;</c> keys on the US standard keyboard, or the <c>\|</c> key on the non-US 102-key keyboard.</summary>
        VK_OEM_102 = 0xE2,

        /// <summary>IME PROCESS key</summary>
        VK_PROCESSKEY = 0xE5,
        /// <inheritdoc cref="VK_PROCESSKEY" />
        ImeProcessKey = VK_PROCESSKEY,

        /// <summary>
        /// Used to pass Unicode characters as if they were keystrokes.
        /// The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.
        /// For more information, see Remark in <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput">KEYBDINPUT</see>, <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput">SendInput</see>, <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown">WM_KEYDOWN</see>, and <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keyup">WM_KEYUP</see>.
        /// </summary>
        VK_PACKET = 0xE7,

        /// <summary>Attn key</summary>
        VK_ATTN = 0xF6,
        /// <inheritdoc cref="VK_ATTN" />
        Attn = VK_ATTN,

        /// <summary>CrSel key</summary>
        VK_CRSEL = 0xF7,
        /// <inheritdoc cref="VK_CRSEL" />
        CrSel = VK_CRSEL,

        /// <summary>ExSel key</summary>
        VK_EXSEL = 0xF8,
        /// <inheritdoc cref="VK_EXSEL" />
        ExSel = VK_EXSEL,

        /// <summary>Erase EOF key</summary>
        VK_EREOF = 0xF9,
        /// <inheritdoc cref="VK_EREOF" />
        EraseEOF = VK_EREOF,

        /// <summary>Play key</summary>
        VK_PLAY = 0xFA,
        /// <inheritdoc cref="VK_PLAY" />
        Play = VK_PLAY,

        /// <summary>Zoom key</summary>
        VK_ZOOM = 0xFB,
        /// <inheritdoc cref="VK_ZOOM" />
        Zoom = VK_ZOOM,

        /// <summary>PA1 key</summary>
        VK_PA1 = 0xFD,
        /// <inheritdoc cref="VK_PA1" />
        Pa1 = VK_PA1,

        /// <summary>Clear key</summary>
        VK_OEM_CLEAR = 0xFE,
        /// <inheritdoc cref="VK_OEM_CLEAR" />
        OemClear = VK_OEM_CLEAR,
    }
}
