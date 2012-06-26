using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeBits
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection</typeparam>
    public class OrderedCollection<T> : Collection<T>
    {
        private readonly bool _allowDuplicates;

        /// <summary>
        /// 
        /// </summary>
        public OrderedCollection() : this(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowDuplicates"></param>
        public OrderedCollection(bool allowDuplicates)
        {
            _allowDuplicates = allowDuplicates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="allowDuplicates"></param>
        public OrderedCollection(IList<T> list, bool allowDuplicates = false)
            : base(list)
        {
            _allowDuplicates = allowDuplicates;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowDuplicates
        {
            get { return _allowDuplicates; }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);
        }

        private int GetInsertIndex(T item)
        {
            if (Count == 0)
                return 0;
            return Count <= SimpleAlgorithmThreshold ? GetInsertIndexSimple(item) : GetInsertIndexComplex(item);
        }

        private int GetInsertIndexSimple(T item)
        {
            throw new System.NotImplementedException();
        }

        private int GetInsertIndexComplex(T item)
        {
            throw new System.NotImplementedException();
        }

        private const int SimpleAlgorithmThreshold = 10;
    }
}