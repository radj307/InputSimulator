using InputSimulator;
using InputSimulator.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Example.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region SetMouse
        static readonly DataTable dt = new DataTable();
        private bool TryParseMathExpression(string expression, out int result)
        {
            try
            {
                result = Convert.ToInt32(dt.Compute(expression, null));
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        static readonly Action<UIElement> showNegativeFeedback = ActionFeedbackAdorner.MakeTemplate(TimeSpan.FromSeconds(1), Color.FromRgb(255, 0, 0));

        private void MouseCoordTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        { // prevent invalid input

            if (!e.Text.All(ch => char.IsDigit(ch)
                    || ch == '/'
                    || ch == '*'
                    || ch == '-'
                    || ch == '+'
                    || ch == '%'))
                e.Handled = true; //< prevent input
        }
        private void SetMouse_Click(object sender, RoutedEventArgs e)
        {
            POINT? cursorPos = null;

            // X axis
            int x = default;
            if (SetMouseX.Text.Length == 0)
            {
                cursorPos = NativeMethods.GetCursorPos();
                x = cursorPos.Value.x;
            }
            else if (!TryParseMathExpression(SetMouseX.Text, out x))
            {
                showNegativeFeedback(SetMouseX);
                return;
            }
            // Y axis
            int y = default;
            if (SetMouseY.Text.Length == 0)
            {
                cursorPos ??= NativeMethods.GetCursorPos();
                y = cursorPos.Value.y;
            }
            else if (!TryParseMathExpression(SetMouseY.Text, out y))
            {
                showNegativeFeedback(SetMouseY);
                return;
            }

            // set position
            MouseInputMethods.SetPosition(x, y);
        }
        #endregion SetMouse

        #region MoveMouse
        private static bool IsShiftDown(System.Windows.Input.KeyboardDevice keyboard) => keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift) || keyboard.IsKeyDown(System.Windows.Input.Key.RightShift);
        private static bool IsCtrlDown(System.Windows.Input.KeyboardDevice keyboard) => keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl) || keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl);
        private void MoveMouse_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
            case System.Windows.Input.Key.NumPad4:
            case System.Windows.Input.Key.Left:
                {
                    var x = -(int)MoveMouseX.Value;

                    bool shift = IsShiftDown(e.KeyboardDevice);
                    bool ctrl = IsCtrlDown(e.KeyboardDevice);
                    if (shift && ctrl)
                        x /= 8;
                    else if (ctrl)
                        x /= 4;
                    else if (shift)
                        x *= 4;

                    MouseInputMethods.Move(x, 0);
                    break;
                }
            case System.Windows.Input.Key.NumPad6:
            case System.Windows.Input.Key.Right:
                {
                    var x = (int)MoveMouseX.Value;

                    bool shift = IsShiftDown(e.KeyboardDevice);
                    bool ctrl = IsCtrlDown(e.KeyboardDevice);
                    if (shift && ctrl)
                        x /= 8;
                    else if (ctrl)
                        x /= 4;
                    else if (shift)
                        x *= 4;

                    MouseInputMethods.Move(x, 0);
                    break;
                }
            case System.Windows.Input.Key.NumPad8:
            case System.Windows.Input.Key.Up:
                {
                    var y = -(int)MoveMouseY.Value;

                    bool shift = IsShiftDown(e.KeyboardDevice);
                    bool ctrl = IsCtrlDown(e.KeyboardDevice);
                    if (shift && ctrl)
                        y /= 8;
                    else if (ctrl)
                        y /= 4;
                    else if (shift)
                        y *= 4;

                    MouseInputMethods.Move(0, y);
                    break;
                }
            case System.Windows.Input.Key.NumPad2:
            case System.Windows.Input.Key.Down:
                {
                    var y = (int)MoveMouseY.Value;

                    bool shift = IsShiftDown(e.KeyboardDevice);
                    bool ctrl = IsCtrlDown(e.KeyboardDevice);
                    if (shift && ctrl)
                        y /= 8;
                    else if (ctrl)
                        y /= 4;
                    else if (shift)
                        y *= 4;

                    MouseInputMethods.Move(0, y);
                    break;
                }
            case System.Windows.Input.Key.NumPad5:
            case System.Windows.Input.Key.Enter:
                MouseInputMethods.LeftButtonClick();
                break;
            }
            e.Handled = true;
            ((UIElement)sender).Focus(); //< refocus the textbox
        }
        private void MoveMouseAmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!e.Text.All(ch => char.IsDigit(ch)))
                e.Handled = true;
        }
        #endregion MoveMouse

        #region Keyboard

        #region KeyButton
        private void KeyButton_Click(object sender, RoutedEventArgs e)
        { // key press
            var vk = (EVirtualKeyCode)((FrameworkElement)sender).Tag;

            KeyboardOutput.Focus();

            KeyboardInputMethods.KeyPress(vk);
        }
        #endregion KeyButton

        #region KeyToggleButton
        private void KeyToggleButton_Checked(object sender, RoutedEventArgs e)
        { // key down
            KeyboardOutput.Focus();

            var vk = (EVirtualKeyCode)((FrameworkElement)sender).Tag;
            KeyboardInputMethods.KeyDown(vk);
        }
        private void KeyToggleButton_Unchecked(object sender, RoutedEventArgs e)
        { // key up
            KeyboardOutput.Focus();

            var vk = (EVirtualKeyCode)((FrameworkElement)sender).Tag;
            KeyboardInputMethods.KeyUp(vk);
        }
        #endregion KeyToggleButton

        #region LockKeyToggleButton
        private void LockKeyToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var vk = (EVirtualKeyCode)((FrameworkElement)sender).Tag;
            if (NativeMethods.GetKeyState(vk) == EKeyStates.Up)
            {
                KeyboardInputMethods.KeyPress(vk);
            }
        }
        private void LockKeyToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var vk = (EVirtualKeyCode)((FrameworkElement)sender).Tag;
            if (NativeMethods.GetKeyState(vk) == EKeyStates.Toggled)
            {
                KeyboardInputMethods.KeyPress(vk);
            }
        }
        private void LockKeyToggleButton_Loaded(object sender, RoutedEventArgs e)
        {
            var tb = (ToggleButton)sender;
            var vk = (EVirtualKeyCode)tb.Tag;

            if (NativeMethods.GetKeyState(vk) == EKeyStates.Toggled)
            {
                tb.IsChecked = true;
            }
        }
        #endregion LockKeyToggleButton

        #endregion Keyboard

        private void AutoTypeButton_Click(object sender, RoutedEventArgs e)
        {
            AutoTypeOutput.Focus();

            KeyboardInputMethods.WriteText(AutoTypeInput.Text);
        }
    }
}
