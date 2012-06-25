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
    }
}