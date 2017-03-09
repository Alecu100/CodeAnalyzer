using System.Collections.Generic;
using CodeEvaluator.Packages.Core.Interfaces;

namespace CodeAnalyzer.UserInterface.Interfaces
{
    public interface IProjectWrapper
    {
        string Kind { get; }
        string UniqueName { get; }
        IEnumerable<IProjectItemWrapper> ProjectItems { get; }
    }
}