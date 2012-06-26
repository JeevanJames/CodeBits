using System;
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
        private readonly IComparer<T> _comparer;
        private readonly bool _allowDuplicates;

        public OrderedCollection()
            : this(false, null)
        {
        }

        public OrderedCollection(bool allowDuplicates = false, IComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                Type comparableType = typeof(IComparable<>).MakeGenericType(typeof(T));
                if (comparableType.IsAssignableFrom(typeof(T)))
                    _comparer = new ComparableComparer<T>();
            } else
                _comparer = comparer;
            _allowDuplicates = allowDuplicates;
        }

        public OrderedCollection(Comparison<T> comparison, bool allowDuplicates = false)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");
            _comparer = new ComparisonComparer<T>(comparison);
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
            int insertIndex = GetInsertIndex(item);
            if (insertIndex < 0)
                throw new ArgumentException("Attempting to insert duplicate value in collection");
            base.InsertItem(insertIndex, item);
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
            for (int i = 0; i < Items.Count; i++)
            {
                T existingItem = Items[i];
                int comparison = _comparer.Compare(item, existingItem);
                if (comparison == 0 && !_allowDuplicates)
                    return -1;
                if (comparison > 0)
                    return i;
            }
            return 0;
        }

        private int GetInsertIndexComplex(T item)
        {
            throw new System.NotImplementedException();
        }

        private const int SimpleAlgorithmThreshold = 10;

        private sealed class ComparableComparer<TItem> : IComparer<TItem>
        {
            int IComparer<TItem>.Compare(TItem x, TItem y)
            {
                return ((IComparable<TItem>)x).CompareTo(y);
            }
        }

        private sealed class ComparisonComparer<TItem> : IComparer<TItem>
        {
            private readonly Comparison<TItem> _comparison;

            internal ComparisonComparer(Comparison<TItem> comparison)
            {
                _comparison = comparison;
            }

            int IComparer<TItem>.Compare(TItem x, TItem y)
            {
                return _comparison(x, y);
            }
        }
    }
}