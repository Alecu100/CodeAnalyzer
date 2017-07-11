using System.Collections.Generic;

namespace CodeEvaluator.Packages.Core.Interfaces
{

    #region Using

    #endregion

    public interface IProjectFilesProvider
    {
        #region Public Methods and Operators

        IList<IProjectItemWrapper> GetAllSourceFileNamesFromProjects(IList<IProjectWrapper> searchLocations);

        IList<IReference> GetAllReferencesFromProjects(IList<IProjectWrapper> searchLocations);

        #endregion
    }
}