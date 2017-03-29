using CodeAnalyzer.UserInterface.Interfaces;
using EnvDTE;

namespace CodeEvaluator.Packages.Vs2015.Wrappers
{
    public class SolutionWrapper : ISolutionWrapper
    {
        private readonly Solution _solution;

        public SolutionWrapper(Solution solution)
        {
            _solution = solution;
        }

        public string FullName
        {
            get { return _solution.FullName; }
        }
    }
}