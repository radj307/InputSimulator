using InputSimulator;
using InputSimulator.Native;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Example.WPF
{
    /// <summary>
    /// Interaction logic for MouseTestWindow.xaml
    /// </summary>
    public partial class MouseTestWindow : Window
    {
        public MouseTestWindow()
        {
            InitializeComponent();

            DynamicButtonMatrix = new Button[UGrid.Rows][];

            for (int y = 0, y_max = UGrid.Rows; y < y_max; ++y)
            {
                var row = new Button[UGrid.Columns];
                for (int x = 0, x_max = UGrid.Columns; x < x_max; ++x)
                {
                    var dynamicButton = new Button();

                    dynamicButton.SetValue(TagProperty, new Point(x, y));
                    dynamicButton.SetValue(ContentProperty, $"({x}, {y})");
                    dynamicButton.AddHandler(ButtonBase.ClickEvent, DynamicButton_Click);

                    row[x] = dynamicButton;
                    UGrid.Children.Add(dynamicButton);
                }
                DynamicButtonMatrix[y] = row;
            }
        }

        private readonly Button[][] DynamicButtonMatrix;
        static readonly Action<UIElement> showNegativeFeedback = ActionFeedbackAdorner.MakeTemplate(TimeSpan.FromSeconds(1), Color.FromRgb(255, 0, 0));

        private Point GetTargetPos() => new(int.Parse(PosX.Text), int.Parse(PosY.Text));
        private Button? GetDynamicButton(Point pos)
        {
            if (pos.Y < 0 || pos.Y >= DynamicButtonMatrix.Length
                || pos.X < 0 || pos.X >= DynamicButtonMatrix[Convert.ToInt32(pos.Y)].Length)
                return null;

            return DynamicButtonMatrix[Convert.ToInt32(pos.Y)][Convert.ToInt32(pos.X)];
        }

        private void DynamicButton_Click(object sender, RoutedEventArgs e)
        {
            var gridPos = (Point)((FrameworkElement)sender).Tag;

            Output.Text = gridPos.ToString();
        }
        private void Pos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.All(ch => char.IsDigit(ch)))
                e.Handled = true;
        }

        private void MoveTo_Click(object sender, RoutedEventArgs e)
        {
            var targetPos = GetTargetPos();

            if (GetDynamicButton(targetPos) is not Button targetButton)
            {
                showNegativeFeedback((UIElement)sender);
                return;
            }

            var buttonWindowPos = targetButton.TransformToAncestor(this).Transform(new(0, 0));
            var buttonScreenPos = this.PointToScreen(buttonWindowPos);
            var (x, y) = buttonScreenPos;

            MouseInput.SetPosition((int)x, (int)y);
        }
        private void ClickAt_Click(object sender, RoutedEventArgs e)
        {
            var targetPos = GetTargetPos();

            if (GetDynamicButton(targetPos) is not Button targetButton)
            {
                showNegativeFeedback((UIElement)sender);
                return;
            }

            var buttonPosRelativeToWindow = targetButton.TransformToAncestor(this);
            // TODO
        }
    }
}
