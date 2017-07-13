namespace CodeEvaluator.Packages.Core.Interfaces
{
    using System.Collections.Generic;

    public interface ISolutionWrapper
    {
        string FullName { get; }

        IEnumerable<IProjectWrapper> Projects { get; }
    }
}