namespace CodeEvaluator.Evaluation.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::CodeEvaluator.Evaluation.Exceptions;
    using global::CodeEvaluator.Evaluation.Members;

    public class EvaluatedMembersList<T> : IList<T>
    {
        private readonly int count;

        private readonly List<T> _evaluatedMembers = new List<T>();

        private bool _isFinalized;

        public bool IsFinalized
        {
            get
            {
                return _isFinalized;
            }
            set
            {
                ThrowExceptionIfAlreadyFinalized();

                _isFinalized = value;

                if (_isFinalized)
                {
                    var evaluatedMembers = _evaluatedMembers.Where(member => member is EvaluatedMember).Cast<EvaluatedMember>().Where(member => !member.IsFinalized);

                    foreach (var evaluatedMember in evaluatedMembers)
                    {
                        evaluatedMember.IsFinalized = true;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _evaluatedMembers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _evaluatedMembers.GetEnumerator();
        }

        public void Add(T item)
        {
            ThrowExceptionIfAlreadyFinalized();

            _evaluatedMembers.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            ThrowExceptionIfAlreadyFinalized();

            _evaluatedMembers.AddRange(items);
        }

        public void Clear()
        {
            ThrowExceptionIfAlreadyFinalized();

            _evaluatedMembers.Clear();
        }

        public bool Contains(T item)
        {
            return _evaluatedMembers.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _evaluatedMembers.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            ThrowExceptionIfAlreadyFinalized();

            return _evaluatedMembers.Remove(item);
        }

        public int Count
        {
            get
            {
                return _evaluatedMembers.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return IsFinalized;
            }
        }

        public int IndexOf(T item)
        {
            return _evaluatedMembers.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            ThrowExceptionIfAlreadyFinalized();

            _evaluatedMembers.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ThrowExceptionIfAlreadyFinalized();

            _evaluatedMembers.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return _evaluatedMembers[index];
            }
            set
            {
                ThrowExceptionIfAlreadyFinalized();

                _evaluatedMembers[index] = value;
            }
        }

        private void ThrowExceptionIfAlreadyFinalized()
        {
            if (_isFinalized)
            {
                throw new TypeInfoFinalizedException("EvaluatedTypeInfo is already finalized!");
            }
        }
    }
}