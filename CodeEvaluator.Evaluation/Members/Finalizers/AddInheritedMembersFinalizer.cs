using System;
using System.Linq;
using CodeEvaluator.Evaluation.Interfaces;
using StructureMap;

namespace CodeEvaluator.Evaluation.Members.Finalizers
{
    public class AddInheritedMembersFinalizer : EvaluatedTypeInfoFinalizer
    {
        private readonly IMethodSignatureComparer _methodSignatureComparer;

        public AddInheritedMembersFinalizer()
        {
            _methodSignatureComparer = ObjectFactory.GetInstance<IMethodSignatureComparer>();
        }

        public override int Priority
        {
            get { return int.MaxValue; }
        }

        public override void FinalizeTypeInfo(EvaluatedTypeInfo evaluatedTypeInfo)
        {
            AddMembersFromBaseType(evaluatedTypeInfo, evaluatedTypeInfo);
        }

        private void AddMembersFromBaseType(EvaluatedTypeInfo evaluatedTypeInfo, EvaluatedTypeInfo baseTypeInfo)
        {
            if (!baseTypeInfo.IsInterfaceType)
            {
                var newMethods =
                    baseTypeInfo.SpecificMethods.Where(
                        baseMethod =>
                            evaluatedTypeInfo.AccesibleMethods.All(
                                specificMethod =>
                                    !_methodSignatureComparer.HaveSameSignature(baseMethod, specificMethod)));

                evaluatedTypeInfo.AccesibleMethods.AddRange(newMethods);

                var newFields = baseTypeInfo.SpecificFields.Where(
                    baseField =>
                        evaluatedTypeInfo.AccesibleFields.All(
                            specificField =>
                                !string.Equals(
                                    baseField.IdentifierText,
                                    specificField.IdentifierText,
                                    StringComparison.Ordinal)));

                evaluatedTypeInfo.AccesibleFields.AddRange(newFields);

                var newProperties =
                    baseTypeInfo.SpecificProperties.Where(
                        baseProperty =>
                            evaluatedTypeInfo.AccesibleProperties.All(
                                specificProperty =>
                                    !string.Equals(
                                        baseProperty.IdentifierText,
                                        specificProperty.IdentifierText,
                                        StringComparison.Ordinal)));

                evaluatedTypeInfo.AccesibleProperties.AddRange(newProperties);

                foreach (var secondBaseTypeInfo in baseTypeInfo.BaseTypeInfos)
                    AddMembersFromBaseType(evaluatedTypeInfo, secondBaseTypeInfo);
            }
        }
    }
}