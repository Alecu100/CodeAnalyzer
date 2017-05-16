using CodeEvaluator.Packages.Core.Interfaces;
using VSLangProj;

namespace CodeEvaluator.Packages.Vs2015.Wrappers
{
    public class ReferenceWrapper : IReference
    {
        private readonly Reference _reference;

        public ReferenceWrapper(Reference reference)
        {
            _reference = reference;
        }

        public string Name
        {
            get { return _reference.Name; }
        }

        public string Path
        {
            get { return _reference.Path; }
        }
    }
}