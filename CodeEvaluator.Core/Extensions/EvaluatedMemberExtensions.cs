using CodeAnalysis.Core.Enums;
using CodeAnalysis.Core.Members;

namespace CodeAnalysis.Core.Extensions
{
    public static class EvaluatedMemberExtensions
    {
        public static bool IsStatic(this EvaluatedMember member)
        {
            return (member.MemberFlags & EMemberFlags.Static) != 0;
        }
    }
}