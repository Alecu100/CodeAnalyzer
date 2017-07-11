using System.Windows;
using CodeEvaluator.UserInterface.Controls.Base.Enums;

namespace CodeEvaluator.UserInterface.Controls.Base
{

    #region Using

    #endregion

    public struct WorkflowConnectorInfo
    {
        #region Public Properties

        public double DesignerItemLeft { get; set; }

        public Size DesignerItemSize { get; set; }

        public double DesignerItemTop { get; set; }

        public EConnectorOrientation Orientation { get; set; }

        public Point Position { get; set; }

        #endregion
    }
}