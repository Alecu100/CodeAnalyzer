//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedMember.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 15:38
//  
// 
//  Contains             : Implementation of the TrackedMember.cs class.
//  Classes              : TrackedMember.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedMember.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using System;

    using Microsoft.CodeAnalysis;

    #endregion

    public class TrackedMember : IDisposable
    {
        #region Fields

        protected bool _isDisposed;

        #endregion

        #region Constructors and Destructors

        ~TrackedMember()
        {
            DisposeInternal(false);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the declaration.
        /// </summary>
        /// <value>
        ///     The declaration.
        /// </value>
        public SyntaxNode Declaration { get; set; }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>
        ///     The full name.
        /// </value>
        public string FullIdentifierText { get; set; }

        /// <summary>
        ///     Gets or sets the name of the identifier.
        /// </summary>
        /// <value>
        ///     The name of the identifier.
        /// </value>
        public SyntaxToken Identifier { get; set; }

        /// <summary>
        ///     Gets or sets the identifier text.
        /// </summary>
        /// <value>
        ///     The identifier text.
        /// </value>
        public string IdentifierText { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is external.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is external; otherwise, <c>false</c>.
        /// </value>
        public bool IsExternal { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is static.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is static; otherwise, <c>false</c>.
        /// </value>
        public bool IsStatic { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DisposeInternal(true);
        }

        #endregion

        #region Protected Methods and Operators

        protected virtual void DisposeInternal(bool isDisposing)
        {
        }

        #endregion
    }
}