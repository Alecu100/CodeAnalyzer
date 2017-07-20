namespace CodeEvaluator.Evaluation.Extensions
{
    using CodeEvaluator.Evaluation.Members;

    public static class EvaluatedMemberExtensions
    {
        public static bool IsStatic(this EvaluatedMember member)
        {
            return (member.MemberFlags & EMemberFlags.Static) != 0;
        }
    }
}