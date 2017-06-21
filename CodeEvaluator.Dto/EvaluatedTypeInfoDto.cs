using System;
using System.Collections.Generic;

namespace CodeEvaluator.Dto
{
    [Serializable]
    public class EvaluatedTypeInfoDto : EvaluatedMemberDto
    {
        [NonSerialized] public List<Type> OriginalBaseTypes = new List<Type>();

        public List<EvaluatedMethodDto> Constructors { get; } = new List<EvaluatedMethodDto>();

        public List<EvaluatedTypedMemberDto> Fields { get; } = new List<EvaluatedTypedMemberDto>();

        public List<EvaluatedMethodDto> Methods { get; } = new List<EvaluatedMethodDto>();

        public List<EvaluatedPropertyDto> Properties { get; } = new List<EvaluatedPropertyDto>();

        public List<EvaluatedTypeInfoDto> BaseTypeInfos { get; } = new List<EvaluatedTypeInfoDto>();

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is interface type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is interface type; otherwise, <c>false</c>.
        /// </value>
        public bool IsInterfaceType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is reference type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is reference type; otherwise, <c>false</c>.
        /// </value>
        public bool IsReferenceType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is value type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is value type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueType { get; set; }
    }
}