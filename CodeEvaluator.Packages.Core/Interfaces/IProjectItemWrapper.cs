using System.Collections.Generic;

namespace CodeEvaluator.Packages.Core.Interfaces
{
    public interface IProjectItemWrapper
    {
        string Name { get; }
        string Kind { get; }
        IEnumerable<IProjectItemWrapper> ProjectItems { get; }
        IEnumerable<string> FileNames { get; }
    }
}