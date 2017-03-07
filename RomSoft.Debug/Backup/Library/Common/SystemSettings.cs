//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : SystemSettings.cs
//  Author               : Alecsandru
//  Last Updated         : 08/12/2015 at 20:56
//  
// 
//  Contains             : Implementation of the SystemSettings.cs class.
//  Classes              : SystemSettings.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="SystemSettings.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Common
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    [Serializable]
    public class SystemSettings : ISystemSettings
    {
        #region Static Fields

        private static bool mainInstance = true;

        private static string settingsPath;

        #endregion

        #region Fields

        [XmlElement("ProjectsSettings", typeof(ProjectSettings))]
        public List<ProjectSettings> ProjectsSettings = new List<ProjectSettings>();

        public string Version = "1.0.0.0";

        #endregion

        #region Constructors and Destructors

        public SystemSettings()
        {
            if (File.Exists(SettingsPath) && mainInstance)
            {
                try
                {
                    mainInstance = false;

                    var readAllBytes = File.ReadAllBytes(SettingsPath);
                    var memoryStream = new MemoryStream(readAllBytes);

                    var xmlSerializer = new XmlSerializer(typeof(SystemSettings));
                    var pastSystemSettings = (SystemSettings)xmlSerializer.Deserialize(memoryStream);

                    if (pastSystemSettings.Version == Version)
                    {
                        ProjectsSettings = pastSystemSettings.ProjectsSettings;
                    }
                }
                catch (Exception)
                {
                    File.Delete(SettingsPath);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the project settings.
        /// </summary>
        /// <value>
        ///     The project settings.
        /// </value>
        public IReadOnlyList<ProjectSettings> ProjectSettings
        {
            get
            {
                return ProjectsSettings;
            }
        }

        public static string SettingsPath
        {
            get
            {
                if (settingsPath == null)
                {
                    var localApplicationDataPath =
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    settingsPath = localApplicationDataPath + "\\" + "RomsoftSettings.xml";
                }

                return settingsPath;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the or replace project settings.
        /// </summary>
        /// <param name="projectSettings">The project settings.</param>
        /// <returns></returns>
        public void AddOrReplaceProjectSettings(ProjectSettings projectSettings)
        {
            var existingSettings =
                ProjectsSettings.FirstOrDefault(ps => ps.FullSolutionName == projectSettings.FullSolutionName);

            if (existingSettings != null)
            {
                ProjectsSettings.Remove(existingSettings);
            }

            ProjectsSettings.Add(projectSettings);
        }

        /// <summary>
        ///     Gets the name of the settings by full solution.
        /// </summary>
        /// <param name="fullSolutionName">Full name of the solution.</param>
        /// <returns></returns>
        public ProjectSettings GetProjectSettingsByFullSolutionName(string fullSolutionName)
        {
            return ProjectsSettings.FirstOrDefault(ps => ps.FullSolutionName == fullSolutionName);
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            var memoryStream = new MemoryStream();
            var xmlSerializer = new XmlSerializer(typeof(SystemSettings));
            xmlSerializer.Serialize(memoryStream, this);

            File.WriteAllBytes(SettingsPath, memoryStream.ToArray());
        }

        #endregion
    }
}