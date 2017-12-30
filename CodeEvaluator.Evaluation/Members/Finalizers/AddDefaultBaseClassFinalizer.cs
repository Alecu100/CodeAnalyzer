using System.Linq;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Interfaces;
using StructureMap;

namespace CodeEvaluator.Evaluation.Members.Finalizers
{
    public class AddDefaultBaseClassFinalizer : IEvaluatedTypeInfoFinalizer
    {
        public void FinalizeTypeInfo(EvaluatedTypeInfo evaluatedTypeInfo)
        {
            if (evaluatedTypeInfo.IsValueType &&
                evaluatedTypeInfo.FullIdentifierText != CodeEvaluatorConstants.ObjectTypeName &&
                evaluatedTypeInfo.FullIdentifierText != CodeEvaluatorConstants.ValueTypeName)
            {
                var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

                var valueTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(CodeEvaluatorConstants.ValueTypeName);

                evaluatedTypeInfo.BaseTypeInfos.Add(valueTypeInfo);
            }
            else if ((evaluatedTypeInfo.IsReferenceType &&
                     evaluatedTypeInfo.FullIdentifierText != CodeEvaluatorConstants.ObjectTypeName) ||
                     evaluatedTypeInfo.FullIdentifierText == CodeEvaluatorConstants.ValueTypeName)
            {
                if (evaluatedTypeInfo.BaseTypeInfos.All(baseType => !baseType.IsReferenceType))
                {
                    var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

                    var valueTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(CodeEvaluatorConstants.ObjectTypeName);

                    evaluatedTypeInfo.BaseTypeInfos.Add(valueTypeInfo);
                }
            }
        }

        public int Priority
        {
            get { return 0; }
        }
    }
}