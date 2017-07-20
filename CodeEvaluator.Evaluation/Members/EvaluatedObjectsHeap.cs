namespace CodeEvaluator.Evaluation.Members
{
    using System.Collections.Generic;

    using CodeEvaluator.Evaluation.Interfaces;

    #region Using

    

    #endregion

    public class EvaluatedObjectsHeap : List<EvaluatedObject>, IEvaluatedObjectsHeap
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///     read-only.
        /// </exception>
        public void Add(EvaluatedObject item)
        {
            base.Add(item);

            item.ParentHeap = this;
        }

        #endregion
    }
}