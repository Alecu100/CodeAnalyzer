using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using CodeEvaluator.UserInterface.Controls.Base.Adorners;

namespace CodeEvaluator.UserInterface.Controls.Base
{
    #region Using

    

    #endregion

    public class ResizeDecorator : Control
    {
        #region Static SpecificFields

        public static readonly DependencyProperty ShowDecoratorProperty = DependencyProperty.Register(
            "ShowDecorator",
            typeof(bool),
            typeof(ResizeDecorator),
            new FrameworkPropertyMetadata(false, ShowDecoratorProperty_Changed));

        #endregion

        #region SpecificFields

        private Adorner _adorner;

        #endregion

        #region Constructors and Destructors

        public ResizeDecorator()
        {
            Unloaded += ResizeDecorator_Unloaded;
        }

        #endregion

        #region Public Properties

        public bool ShowDecorator
        {
            get
            {
                return (bool)GetValue(ShowDecoratorProperty);
            }
            set
            {
                SetValue(ShowDecoratorProperty, value);
            }
        }

        #endregion

        #region Private Methods and Operators

        private static void ShowDecoratorProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var decorator = (ResizeDecorator)d;
            var showDecorator = (bool)e.NewValue;

            if (showDecorator)
            {
                decorator.ShowAdorner();
            }
            else
            {
                decorator.HideAdorner();
            }
        }

        private void HideAdorner()
        {
            if (_adorner != null)
            {
                _adorner.Visibility = Visibility.Hidden;
            }
        }

        private void ResizeDecorator_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(_adorner);
                }

                _adorner = null;
            }
        }

        private void ShowAdorner()
        {
            if (_adorner == null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if (adornerLayer != null)
                {
                    var designerItem = this.FindParent<WorkflowItem>();
                    var canvas = VisualTreeHelper.GetParent(designerItem) as Canvas;
                    _adorner = new ResizeAdorner(designerItem);
                    adornerLayer.Add(_adorner);

                    if (ShowDecorator)
                    {
                        _adorner.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _adorner.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                _adorner.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}