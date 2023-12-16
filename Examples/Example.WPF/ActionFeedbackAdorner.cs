using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Example.WPF
{
    public class ActionFeedbackAdorner : Adorner
    {
        #region Constructors
        private ActionFeedbackAdorner(UIElement adornedElement, TimeSpan time, Pen pen) : base(adornedElement)
        {
            _brush = pen.Brush;
            _thickness = pen.Thickness;
            _pen = pen;

            _animation = new(0.0, new(time));
            _animation.Completed += Animation_Completed;
        }
        private ActionFeedbackAdorner(UIElement adornedElement, TimeSpan time, Color color, double thickness = 1.0) : this(adornedElement, time, new Pen(new SolidColorBrush(color), thickness)) { }
        private ActionFeedbackAdorner(UIElement adornedElement, TimeSpan time, Brush brush, double thickness = 1.0) : this(adornedElement, time, new Pen(brush, thickness)) { }
        #endregion Constructors

        #region Fields
        private Pen _pen = null!;
        private readonly DoubleAnimation _animation;
        #endregion Fields

        #region Properties
        public Brush Brush
        {
            get => _brush;
            set
            {
                _brush = value;
                _pen = new(Brush, Thickness);
            }
        }
        private Brush _brush;
        public double Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                _pen = new(Brush, Thickness);
            }
        }
        private double _thickness;
        #endregion Properties

        #region Methods

        #region (Static) Show
        public static void Show(UIElement uiElement, TimeSpan time, Pen pen)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
            adornerLayer.Add(new ActionFeedbackAdorner(uiElement, time, pen));
        }
        public static void Show(UIElement uiElement, TimeSpan time, Color color, double thickness = 1.0)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
            adornerLayer.Add(new ActionFeedbackAdorner(uiElement, time, color, thickness));
        }
        public static void Show(UIElement uiElement, TimeSpan time, Brush brush, double thickness = 1.0)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
            adornerLayer.Add(new ActionFeedbackAdorner(uiElement, time, brush, thickness));
        }
        #endregion (Static) Show

        #region (Static) MakeTemplate
        public static Action<UIElement> MakeTemplate(TimeSpan time, Pen pen) => (UIElement adornedElement) => Show(adornedElement, time, pen);
        public static Action<UIElement> MakeTemplate(TimeSpan time, Color color, double thickness = 1.0) => (UIElement adornedElement) => Show(adornedElement, time, color, thickness);
        public static Action<UIElement> MakeTemplate(TimeSpan time, Brush brush, double thickness = 1.0) => (UIElement adornedElement) => Show(adornedElement, time, brush, thickness);
        #endregion (Static) MakeTemplate

        #region (Protected) OnRender
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(null, _pen, new(AdornedElement.RenderSize));
            this.BeginAnimation(Adorner.OpacityProperty, _animation);
        }
        #endregion (Protected) OnRender

        #region (Private) RemoveSelf
        private void RemoveSelf()
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(AdornedElement);
            adornerLayer.Remove(this);
        }
        #endregion (Private) RemoveSelf

        #endregion Methods

        #region EventHandlers
        private void Animation_Completed(object? sender, EventArgs e) => RemoveSelf();
        #endregion EventHandlers
    }
}
