namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using global::CodeEvaluator.Evaluation.Interfaces;

    #region Using

    #endregion

    public class CodeEvaluatorExecutionStack
    {
        #region SpecificFields

        private CodeEvaluatorParameters _parameters;

        private readonly List<CodeEvaluatorExecutionFrame> _staticWorkflowEvaluatorExecutionFrames =
            new List<CodeEvaluatorExecutionFrame>();

        private readonly List<ISyntaxNodeEvaluator> _syntaxNodeEvaluators = new List<ISyntaxNodeEvaluator>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current execution frame.
        /// </summary>
        /// <value>
        ///     The current execution frame.
        /// </value>
        public CodeEvaluatorExecutionFrame CurrentExecutionFrame
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
        public CodeEvaluatorParameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        ///     Gets the static workflow evaluator execution frames.
        /// </summary>
        /// <value>
        ///     The static workflow evaluator execution frames.
        /// </value>
        public IReadOnlyList<CodeEvaluatorExecutionFrame> StaticWorkflowEvaluatorExecutionFrames
        {
            get { return _staticWorkflowEvaluatorExecutionFrames; }
        }

        /// <summary>
        ///     Gets the syntax node evaluators.
        /// </summary>
        /// <value>
        ///     The syntax node evaluators.
        /// </value>
        public IReadOnlyList<ISyntaxNodeEvaluator> SyntaxNodeEvaluators
        {
            get { return _syntaxNodeEvaluators; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Pops the execution frame.
        /// </summary>
        public void PopFramePassingReturnedObjectsToPreviousFrame()
        {
            var currentExecutionFrame = _staticWorkflowEvaluatorExecutionFrames.Last();

            if (_staticWorkflowEvaluatorExecutionFrames.Count >= 2)
            {
                var previousExecutionFrame =
                    _staticWorkflowEvaluatorExecutionFrames[_staticWorkflowEvaluatorExecutionFrames.Count - 2];

                previousExecutionFrame.MemberAccessReference = currentExecutionFrame.ReturningMethodParameters;
            }

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
        public void PushFramePassingParametersFromPreviousFrame(CodeEvaluatorExecutionFrame executionFrame)
        {
            if (_staticWorkflowEvaluatorExecutionFrames.Count > 0)
            {
                executionFrame.PassedMethodParameters.Clear();

                var previousExecutionFrame = _staticWorkflowEvaluatorExecutionFrames.Last();
                foreach (var storedInputVariable in previousExecutionFrame.PassedMethodParameters)
                {
                    executionFrame.PassedMethodParameters.Add(storedInputVariable.Key, storedInputVariable.Value);
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