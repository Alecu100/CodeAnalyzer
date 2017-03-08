//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IParsedSourceFilesCache.cs
//  Author               : Alecsandru
//  Last Updated         : 30/11/2015 at 18:31
//  
// 
//  Contains             : Implementation of the IParsedSourceFilesCache.cs class.
//  Classes              : IParsedSourceFilesCache.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IParsedSourceFilesCache.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
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