using System.Collections.Generic;

namespace CodeEvaluator.Packages.Core.Interfaces
{
    public interface IProjectWrapper
    {
        string Kind { get; }
        string UniqueName { get; }
        IEnumerable<IProjectItemWrapper> ProjectItems { get; }

        IEnumerable<IReference> References { get; }
    }
}