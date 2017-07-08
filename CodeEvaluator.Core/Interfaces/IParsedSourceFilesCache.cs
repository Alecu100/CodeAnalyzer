﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeAnalysis.Core.Interfaces
{

    #region Using

    #endregion

    public interface IParsedSourceFilesCache : IList<SyntaxTree>
    {
        #region Public Methods and Operators

        void RebuildFromCodeFiles(IList<string> codeFileNames);

        #endregion
    }
}