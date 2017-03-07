//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : StaticWorkflowEvaluatorContext.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 22:32
//  
// 
//  Contains             : Implementation of the StaticWorkflowEvaluatorContext.cs class.
//  Classes              : StaticWorkflowEvaluatorContext.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="StaticWorkflowEvaluatorContext.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    public class StaticWorkflowEvaluatorContext
    {
        #region Fields

        private StaticWorkflowEvaluatorParameters _parameters;

        private readonly List<StaticWorkflowEvaluatorExecutionFrame> _staticWorkflowEvaluatorExecutionFrames =
            new List<StaticWorkflowEvaluatorExecutionFrame>();

        private readonly List<ISyntaxNodeEvaluator> _syntaxNodeEvaluators = new List<ISyntaxNodeEvaluator>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current execution frame.
        /// </summary>
        /// <value>
        ///     The current execution frame.
        /// </value>
        public StaticWorkflowEvaluatorExecutionFrame CurrentExecutionFrame
        {
            get
            {
                if (_staticWorkflowEvaluatorExecutionFrames.Count > 0)
                {
                    return _staticWorkflowEvaluatorExecutionFrames[_staticWorkflowEvaluatorExecutionFrames.Count - 1];
                }

                return null;
            }
        }

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        /// <value>
        ///     The parameters.
        /// </value>
        public StaticWorkflowEvaluatorParameters Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        /// <summary>
        ///     Gets the static workflow evaluator execution frames.
        /// </summary>
        /// <value>
        ///     The static workflow evaluator execution frames.
        /// </value>
        public IReadOnlyList<StaticWorkflowEvaluatorExecutionFrame> StaticWorkflowEvaluatorExecutionFrames
        {
            get
            {
                return _staticWorkflowEvaluatorExecutionFrames;
            }
        }

        /// <summary>
        ///     Gets the syntax node evaluators.
        /// </summary>
        /// <value>
        ///     The syntax node evaluators.
        /// </value>
        public IReadOnlyList<ISyntaxNodeEvaluator> SyntaxNodeEvaluators
        {
            get
            {
                return _syntaxNodeEvaluators;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Pops the execution frame.
        /// </summary>
        public void PopExecutionFramePassingReturningParameters()
        {
            var currentExecutionFrame = _staticWorkflowEvaluatorExecutionFrames.Last();

            if (_staticWorkflowEvaluatorExecutionFrames.Count >= 2)
            {
                var previousExecutionFrame =
                    _staticWorkflowEvaluatorExecutionFrames[_staticWorkflowEvaluatorExecutionFrames.Count - 2];

                foreach (var returnedMethodParameter in previousExecutionFrame.ReturnedMethodParameters)
                {
                    returnedMethodParameter.Dispose();
                }

                previousExecutionFrame.ReturnedMethodParameters.Clear();

                foreach (var returningMethodParameter in currentExecutionFrame.ReturningMethodParameters)
                {
                    previousExecutionFrame.ReturnedMethodParameters.Add(returningMethodParameter.Move());
                }
            }

            currentExecutionFrame.Dispose();

            _staticWorkflowEvaluatorExecutionFrames.RemoveAt(_staticWorkflowEvaluatorExecutionFrames.Count - 1);
        }

        /// <summary>
        ///     Pops the syntax node evaluator.
        /// </summary>
        public void PopSyntaxNodeEvaluator()
        {
            _syntaxNodeEvaluators.RemoveAt(_syntaxNodeEvaluators.Count - 1);
        }

        /// <summary>
        ///     Pushes the execution frame.
        /// </summary>
        /// <param name="executionFrame">The execution frame.</param>
        public void PushExecutionFramePassingInputs(StaticWorkflowEvaluatorExecutionFrame executionFrame)
        {
            if (_staticWorkflowEvaluatorExecutionFrames.Count > 0)
            {
                executionFrame.PassedMethodParameters.Clear();
                var previousExecutionFrame = _staticWorkflowEvaluatorExecutionFrames.Last();
                foreach (var storedInputVariable in previousExecutionFrame.PassedMethodParameters)
                {
                    executionFrame.PassedMethodParameters.Add(storedInputVariable.Key, storedInputVariable.Value.Move());
                }
            }

            _staticWorkflowEvaluatorExecutionFrames.Add(executionFrame);
        }

        /// <summary>
        ///     Pushes the syntax node evaluator.
        /// </summary>
        /// <param name="syntaxNodeEvaluator">The syntax node evaluator.</param>
        public void PushSyntaxNodeEvaluator(ISyntaxNodeEvaluator syntaxNodeEvaluator)
        {
            _syntaxNodeEvaluators.Add(syntaxNodeEvaluator);
        }

        #endregion
    }
}