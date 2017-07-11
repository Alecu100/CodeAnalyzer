namespace CodeEvaluator.UserInterface.Interfaces
{
    public interface IWorkflowDiagramSizes
    {
        #region Public Properties

        /// <summary>
        ///     Gets the width of the column.
        /// </summary>
        /// <value>
        ///     The width of the column.
        /// </value>
        double ColumnWidth { get; }

        /// <summary>
        ///     Gets the column spacing.
        /// </summary>
        /// <value>
        ///     The column spacing.
        /// </value>
        double MinimumColumnSpacing { get; }

        /// <summary>
        ///     Gets the margin.
        /// </summary>
        /// <value>
        ///     The margin.
        /// </value>
        double MinimumMargin { get; }

        /// <summary>
        ///     Gets the height of the row.
        /// </summary>
        /// <value>
        ///     The height of the row.
        /// </value>
        double RowHeight { get; }

        /// <summary>
        ///     Gets the row spacing.
        /// </summary>
        /// <value>
        ///     The row spacing.
        /// </value>
        double RowSpacing { get; }

        #endregion
    }
}