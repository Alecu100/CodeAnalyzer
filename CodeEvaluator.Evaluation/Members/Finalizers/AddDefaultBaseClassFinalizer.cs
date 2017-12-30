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
            if (evaluatedTypeInfo.IsValueType)
            {
                var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

                var valueTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(CodeEvaluatorConstants.ValueTypeName);

                evaluatedTypeInfo.BaseTypeInfos.Add(valueTypeInfo);
            }
            else if (evaluatedTypeInfo.IsReferenceType &&
                     evaluatedTypeInfo.FullIdentifierText != CodeEvaluatorConstants.ObjectTypeName)
            {
                if (evaluatedTypeInfo.BaseTypeInfos.All(baseType => !baseType.IsReferenceType))
                {
                    var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

                    var valueTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(CodeEvaluatorConstants.ObjectTypeName);

                    evaluatedTypeInfo.BaseTypeInfos.Add(valueTypeInfo);
                }
            }
        }
    }
}