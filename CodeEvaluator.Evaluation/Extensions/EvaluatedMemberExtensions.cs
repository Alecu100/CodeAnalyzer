namespace CodeEvaluator.Evaluation.Extensions
{
    using System;
    using System.Collections.Generic;

    using CodeEvaluator.Evaluation.Members;

    public static class EvaluatedMemberExtensions
    {
        public static bool IsStatic(this EvaluatedMember member)
        {
            return (member.MemberFlags & EMemberFlags.Static) != 0;
        }

        public static void ForEach(
            this IReadOnlyCollection<EvaluatedObject> evaluatedObjects,
            Action<EvaluatedObject> actionToExecuteForEachObject)
        {
            foreach (var evaluatedObject in evaluatedObjects)
            {
                actionToExecuteForEachObject(evaluatedObject);
            }
        }
    }
}