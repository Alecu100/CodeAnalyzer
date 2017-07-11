using System.Windows;
using System.Windows.Controls;

namespace CodeEvaluator.UserInterface.Controls.Base.Adorners
{
    #region Using

    

    #endregion

    public class ResizeChrome : Control
    {
        #region Constructors and Destructors

        static ResizeChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ResizeChrome),
                new FrameworkPropertyMetadata(typeof(ResizeChrome)));
        }

        #endregion
    }
}