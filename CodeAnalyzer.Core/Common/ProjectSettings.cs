//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : ProjectSettings.cs
//  Author               : Alecsandru
//  Last Updated         : 08/12/2015 at 20:47
//  
// 
//  Contains             : Implementation of the ProjectSettings.cs class.
//  Classes              : ProjectSettings.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="ProjectSettings.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CodeAnalysis.Core.Common
{
    #region Using

    

    #endregion

    [Serializable]
    public class ProjectSettings
    {
        #region Fields

        /// <summary>
        ///     Gets or sets the full name of the solutin.
        /// </summary>
        /// <value>
        ///     The full name of the solutin.
        /// </value>
        public string FullSolutionName;

        /// <summary>
        ///     Gets or sets the name of the selected class.
        /// </summary>
        /// <value>
        ///     The name of the selected class.
        /// </value>
        public string SelectedClassName;

        /// <summary>
        ///     Gets or sets the name of the selected file.
        /// </summary>
        /// <value>
        ///     The name of the selected file.
        /// </value>
        public string SelectedFileName;

        /// <summary>
        ///     Gets or sets the name of the selected method.
        /// </summary>
        /// <value>
        ///     The name of the selected method.
        /// </value>
        public string SelectedMethodName;

        /// <summary>
        ///     Gets or sets the name of the selected project.
        /// </summary>
        /// <value>
        ///     The name of the selected project.
        /// </value>
        public string SelectedProjectName;

        [XmlElement("SelectedProject", typeof(string))]
        public List<string> SelectedProjects = new List<string>();

        #endregion
    }
}