//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedObject.cs
//  Author               : Alecsandru
//  Last Updated         : 14/12/2015 at 18:00
//  
// 
//  Contains             : Implementation of the EvaluatedObject.cs class.
//  Classes              : EvaluatedObject.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedObject.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Interfaces;
using Microsoft.CodeAnalysis;
using StructureMap;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    public class EvaluatedObject
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Pushes the history.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="executionFrame">The execution frame.</param>
        public void PushHistory(SyntaxNode expression, CodeEvaluatorExecutionFrame executionFrame)
        {
            var evaluatedObjectHistory = new EvaluatedObjectHistory
            {
                SyntaxNode = executionFrame.CurrentSyntaxNode
            };

            _history.Add(evaluatedObjectHistory);
        }

        #endregion

        #region Constructors and Destructors

        public EvaluatedObject(EvaluatedTypeInfo typeInfo, List<EvaluatedObjectReference> fields)
        {
            ObjectFactory.BuildUp(this);

            _typeInfo = typeInfo;
            _fields.AddRange(fields);
        }

        protected EvaluatedObject(EvaluatedTypeInfo typeInfo)
        {
            ObjectFactory.BuildUp(this);

            _typeInfo = typeInfo;
        }

        #endregion

        #region SpecificFields

        protected readonly List<EvaluatedObjectHistory> _history = new List<EvaluatedObjectHistory>();

        protected readonly List<EvaluatedObjectReference> _fields = new List<EvaluatedObjectReference>();

        protected EvaluatedTypeInfo _typeInfo;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the member variables.
        /// </summary>
        /// <value>
        ///     The member variables.
        /// </value>
        public virtual IReadOnlyList<EvaluatedObjectReference> Fields
        {
            get { return _fields.Union(_typeInfo.SharedStaticObject._fields).ToList(); }
        }


        /// <summary>
        ///     Gets the history.
        /// </summary>
        /// <value>
        ///     The history.
        /// </value>
        public virtual List<EvaluatedObjectHistory> History
        {
            get { return _history; }
        }

        /// <summary>
        ///     Gets or sets the parent heap.
        /// </summary>
        /// <value>
        ///     The parent heap.
        /// </value>
        public IEvaluatedObjectsHeap ParentHeap { get; set; }

        /// <summary>
        ///     Gets or sets the type information.
        /// </summary>
        /// <value>
        ///     The type information.
        /// </value>
        public virtual EvaluatedTypeInfo TypeInfo
        {
            get { return _typeInfo; }
        }

        #endregion
    }
}