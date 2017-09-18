using System;
using System.Collections.Generic;
using CodeEvaluator.Evaluation.Members;

namespace CodeEvaluator.Evaluation.Extensions
{
    public static class EvaluatedMemberExtensions
    {
        public static bool IsStatic(this EvaluatedMember member)
        {
            return (member.MemberFlags & EMemberFlags.Static) != 0;
        }

        public static bool IsVirtualOrAbstract(this EvaluatedMethodBase method)
        {
            return (method.MemberFlags & EMemberFlags.Abstract) != 0 ||
                   (method.MemberFlags & EMemberFlags.Virtual) != 0;
        }

        public static bool IsOverride(this EvaluatedMethodBase method)
        {
            return (method.MemberFlags & EMemberFlags.Override) != 0;
        }

        public static bool IsNew(this EvaluatedMethodBase method)
        {
            return (method.MemberFlags & EMemberFlags.New) != 0;
        }

        public static bool IsWellKnown(this EvaluatedTypeInfo evaluatedTypeInfo)
        {
            return (evaluatedTypeInfo.MemberFlags & EMemberFlags.External) == 0;
        }


        public static void ForEach(
            this IReadOnlyCollection<EvaluatedObject> evaluatedObjects,
            Action<EvaluatedObject> actionToExecuteForEachObject)
        {
            foreach (var evaluatedObject in evaluatedObjects)
                actionToExecuteForEachObject(evaluatedObject);
        }
    }
}