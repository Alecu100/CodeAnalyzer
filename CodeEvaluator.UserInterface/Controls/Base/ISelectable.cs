namespace CodeEvaluator.UserInterface.Controls.Base
{
    // Common interface for items that can be selected
    // on the DesignerCanvas; used by DesignerItem and Connection
    public interface ISelectable
    {
        #region Public Properties

        bool IsSelected { get; set; }

        #endregion
    }
}