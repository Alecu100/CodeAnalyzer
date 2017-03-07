//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : IXamlResourcesRepository.cs
//  Author               : Alecsandru
//  Last Updated         : 10/02/2016 at 16:31
//  
// 
//  Contains             : Implementation of the IXamlResourcesRepository.cs class.
//  Classes              : IXamlResourcesRepository.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="IXamlResourcesRepository.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace CodeAnalyzer.UserInterface.Interfaces
{
    public interface IXamlResourcesRepository
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Finds the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        object FindResource(object resourceKey);

        #endregion
    }
}