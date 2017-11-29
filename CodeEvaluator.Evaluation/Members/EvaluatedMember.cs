namespace CodeEvaluator.Evaluation.Members
{
    using System;

    using CodeEvaluator.Evaluation.Exceptions;

    using Microsoft.CodeAnalysis;

    #region Using

    #endregion

    [Serializable]
    public abstract class EvaluatedMember
    {
        private SyntaxNode _declaration;

        private string _fullIdentifierText;

        private SyntaxToken _identifier;

        private string _identifierText;

        private bool _isFinalized;

        private EMemberFlags _memberFlags;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the declaration.
        /// </summary>
        /// <value>
        ///     The declaration.
        /// </value>
        public SyntaxNode Declaration
        {
            get
            {
                return _declaration;
            }
            set
            {
                ThrowExceptionIfFinalized();

                _declaration = value;
            }
        }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>
        ///     The full name.
        /// </value>
        public string FullIdentifierText
        {
            get
            {
                return _fullIdentifierText;
            }
            set
            {
                ThrowExceptionIfFinalized();

                _fullIdentifierText = value;
            }
        }

        /// <summary>
        ///     Gets or sets the name of the identifier.
        /// </summary>
        /// <value>
        ///     The name of the identifier.
        /// </value>
        public SyntaxToken Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                ThrowExceptionIfFinalized();

                _identifier = value;
            }
        }

        /// <summary>
        ///     Gets or sets the identifier text.
        /// </summary>
        /// <value>
        ///     The identifier text.
        /// </value>
        public string IdentifierText
        {
            get
            {
                return _identifierText;
            }
            set
            {
                ThrowExceptionIfFinalized();

                _identifierText = value;
            }
        }

        public EMemberFlags MemberFlags
        {
            get
            {
                return _memberFlags;
            }
            set
            {
                ThrowExceptionIfFinalized();

                _memberFlags = value;
            }
        }

        public bool IsFinalized
        {
            get
            {
                return _isFinalized;
            }
            set
            {
                ThrowExceptionIfFinalized();

                _isFinalized = value;
            }
        }

        protected void ThrowExceptionIfFinalized()
        {
            if (_isFinalized)
            {
                throw new TypeInfoFinalizedException("EvaluatedTypeInfo is already finalized!");
            }
        }

        #endregion
    }
}