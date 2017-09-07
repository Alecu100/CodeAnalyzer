﻿namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    public class EvaluatedDelegate : EvaluatedObject
    {
        public EvaluatedDelegate(
            EvaluatedTypeInfo referenceType,
            EvaluatedObject evaluatedObject,
            IEnumerable<EvaluatedMethod> methodGroups,
            EvaluatedMethodBase method)
        {
            var evaluatedObjectReference = new EvaluatedObjectDirectReference();
            evaluatedObjectReference.AssignEvaluatedObject(evaluatedObject);

            _fields.Add(evaluatedObjectReference);
            Method = method;
            TypeInfo = referenceType;
            TargetMethodGroup.AddRange(methodGroups);
        }

        public EvaluatedDelegate(EvaluatedTypeInfo referenceType, EvaluatedMethodBase method)
        {
            Method = method;
            TypeInfo = referenceType;
        }

        public EvaluatedMethodBase Method { get; }

        public List<EvaluatedMethod> TargetMethodGroup { get; } = new List<EvaluatedMethod>();

        public EvaluatedObject TargetObject
        {
            get
            {
                return _fields[0].EvaluatedObjects[0];
            }
        }

        public override IReadOnlyList<EvaluatedObjectReferenceBase> Fields
        {
            get
            {
                return _fields;
            }
        }

        public override EvaluatedTypeInfo TypeInfo { get; }

        public override List<EvaluatedObjectHistory> History
        {
            get
            {
                return _fields[0].EvaluatedObjects[0].History;
            }
        }
    }
}