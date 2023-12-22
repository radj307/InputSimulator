# InputSimulator

Keyboard, mouse, & hardware device input simulation library for C#, targeting .NET Standard 2.1.  
It is both a thin & thick wrapper of the [`SendInput`](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput) function. You can use entirely high-level methods or use the Win32 types directly if you want to follow the MSDN documentation *(Most of the type/function names are identical)*.  

MSDN documentation is included in XMLDoc format, so it's integrated with IntelliSense.

```csharp
using InputSimulator;
```

## Examples

- [**Keyboard**](#keyboard)
- [**Mouse**](#mouse)

### Keyboard

The `KeyboardInput` static class contains high-level methods for synthesizing keyboard input events.

#### Key Press (▼▲)
```csharp
KeyboardInput.KeyPress(EVirtualKeyCode.X);
// For typable characters, you can use a char:
KeyboardInput.KeyPress('X');
```

#### Key Down (▼) & Key Up (▲)
```csharp
KeyboardInput.KeyDown(EVirtualKeyCode.LeftControl);
// ...
KeyboardInput.KeyUp(EVirtualKeyCode.LeftControl);
```

#### Key Stroke (▼... ▲...)
```csharp
KeyboardInput.KeyStroke(EModifierKeys.Ctrl, EVirtualKeyCode.V);
KeyboardInput.KeyStroke(EModifierKeys.Ctrl, 'V');
```

#### Chars & Strings

```csharp
KeyboardInput.FromText('?');
KeyboardInput.FromText("Hello World!");
// newlines, tabs, and backspaces are also accepted:
KeyboardInput.FromText(
    "New\nLines",
    "\t\tTabs",
    "Backspaces\b\b\b\b\b\b\b\b\b"
);
```

### Mouse

The `MouseInput` static class contains high-level methods for synthesizing mouse input events.  
All methods have full multi-monitor support by default.

#### Move To Position (Pixels)

```csharp

```
