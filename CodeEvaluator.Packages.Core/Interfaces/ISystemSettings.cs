using System.Collections.Generic;

namespace CodeEvaluator.Packages.Core.Interfaces
{
    #region Using

    

    #endregion

    public interface ISystemSettings
    {
        #region Public Properties

        /// <summary>
        ///     Gets the selected projects for past solutions.
        /// </summary>
        /// <value>
        ///     The selected projects for past solutions.
        /// </value>
        IReadOnlyList<ProjectSettings> ProjectSettings { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the or replace project settings.
        /// </summary>
        /// <param name="projectSettings">The project settings.</param>
        /// <returns></returns>
        void AddOrReplaceProjectSettings(ProjectSettings projectSettings);

        /// <summary>
        ///     Gets the name of the settings by full solution.
        /// </summary>
        /// <param name="fullSolutionName">Full name of the solution.</param>
        /// <returns></returns>
        ProjectSettings GetProjectSettingsByFullSolutionName(string fullSolutionName);

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        void Save();

        #endregion
    }
}