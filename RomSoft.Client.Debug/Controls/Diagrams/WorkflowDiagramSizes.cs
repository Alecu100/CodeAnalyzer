//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : WorkflowDiagramSizes.cs
//  Author               : Alecsandru
//  Last Updated         : 10/02/2016 at 12:21
//  
// 
//  Contains             : Implementation of the WorkflowDiagramSizes.cs class.
//  Classes              : WorkflowDiagramSizes.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="WorkflowDiagramSizes.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Controls.Diagrams
{
    #region Using

    using RomSoft.Client.Debug.Controls.Diagrams.Interfaces;

    #endregion

    public class WorkflowDiagramSizes : IWorkflowDiagramSizes
    {
        #region Public Properties

        /// <summary>
        ///     Gets the width of the column.
        /// </summary>
        /// <value>
        ///     The width of the column.
        /// </value>
        public double ColumnWidth
        {
            get
            {
                return 400;
            }
        }

        /// <summary>
        ///     Gets the margin.
        /// </summary>
        /// <value>
        ///     The margin.
        /// </value>
        public double MinimumMargin
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        ///     Gets the column spacing.
        /// </summary>
        /// <value>
        ///     The column spacing.
        /// </value>
        public double MinimumColumnSpacing
        {
            get
            {
                return 60;
            }
        }

        /// <summary>
        ///     Gets the height of the row.
        /// </summary>
        /// <value>
        ///     The height of the row.
        /// </value>
        public double RowHeight
        {
            get
            {
                return 200;
            }
        }

        /// <summary>
        ///     Gets the row spacing.
        /// </summary>
        /// <value>
        ///     The row spacing.
        /// </value>
        public double RowSpacing
        {
            get
            {
                return 60;
            }
        }

        #endregion
    }
}