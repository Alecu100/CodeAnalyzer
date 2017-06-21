using System;

namespace CodeEvaluator.Dto
{
    [Serializable]
    public class EvaluatedMemberDto
    {
        public EvaluatedMemberDto()
        {
            MemberFlags = 512;
        }

        /// <summary>
        ///     Gets or sets the identifier text.
        /// </summary>
        /// <value>
        ///     The identifier text.
        /// </value>
        public string IdentifierText { get; set; }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>
        ///     The full name.
        /// </value>
        public string FullIdentifierText { get; set; }

        public int MemberFlags { get; set; }
    }
}