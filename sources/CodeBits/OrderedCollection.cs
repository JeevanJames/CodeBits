#region --- License & Copyright Notice ---
/*
Copyright (c) 2005-2011 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeBits
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection</typeparam>
    public partial class OrderedCollection<T> : Collection<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly bool _allowDuplicates;

        /// <summary>
        /// Initializes a new instance of the OrderedCollection.
        /// </summary>
        public OrderedCollection()
            : this((IComparer<T>)null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the OrderedCollection using an optional IComparer implementation.
        /// If the IComparer instance is not specified, then the collection attempts to use the type's
        /// IComparable implementation to perform comparisons.
        /// </summary>
        /// <param name="comparer">Optional IComparer implementation used for the item comparisons during ordering</param>
        /// <param name="allowDuplicates">True if the collection should allow duplicate values</param>
        public OrderedCollection(IComparer<T> comparer = null, bool allowDuplicates = false)
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

        /// <summary>
        /// Initializes a new instance of the OrderedCollection class using a Comparison delegate for
        /// the comparison logic.
        /// </summary>
        /// <param name="comparison">The comparison delegate used for the item comparisons during ordering</param>
        /// <param name="allowDuplicates">True if the collection should allow duplicate values</param>
        public OrderedCollection(Comparison<T> comparison, bool allowDuplicates = false)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");
            _comparer = new ComparisonComparer<T>(comparison);
            _allowDuplicates = allowDuplicates;
        }

        /// <summary>
        /// Specifies whether duplicate values are allowed in the collection
        /// </summary>
        public bool AllowDuplicates
        {
            get { return _allowDuplicates; }
        }

        protected override sealed void InsertItem(int index, T item)
        {
            int insertIndex = GetInsertIndex(item);
            if (insertIndex < 0)
                throw new ArgumentException("Attempting to insert duplicate value in collection", "item");
            base.InsertItem(insertIndex, item);
        }

        protected override sealed void SetItem(int index, T item)
        {
            RemoveItem(index);
            int insertIndex = GetInsertIndex(item);
            if (insertIndex < 0)
                throw new ArgumentException("Attempting to set duplicate value in collection", "item");
            base.InsertItem(insertIndex, item);
        }

        /// <summary>
        /// Performs a comparison between two item of type T. By default, this uses the IComparer implementation
        /// or the Comparison delegate specified in the constructor, but derived types can override
        /// this method to specify their own custom logic.
        /// </summary>
        /// <param name="x">The first item to compare</param>
        /// <param name="y">The second item to compare</param>
        /// <returns>A signed integer - zero if the items are equal, less than zero if x is less than y and greater than zero if x is greater than y</returns>
        protected virtual int Compare(T x, T y)
        {
            if (_comparer == null)
                throw new InvalidOperationException("No comparison logic available for the collection");
            return _comparer.Compare(x, y);
        }

        private int GetInsertIndex(T item)
        {
            if (Count == 0)
                return 0;
            return Count <= SimpleAlgorithmThreshold ? GetInsertIndexSimple(item) : GetInsertIndexComplex(item);
        }

        //Performs a simple left-to-right search for the best location to insert the new item.
        //This algorithm is used while the collection size is small, i.e. less than or equal to the
        //value specified by the SimpleAlgorithmThreshold constant.
        private int GetInsertIndexSimple(T item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                T existingItem = Items[i];
                int comparison = Compare(existingItem, item);
                if (comparison == 0)
                    return _allowDuplicates ? i : -1;
                if (comparison > 0)
                    return i;
            }
            return Count;
        }

        //Performs a divide-and-conquer search for the best location to insert the new item.
        //Since the list is already sorted, this is the fastest algorithm after the collection size
        //crosses a certain threshold.
        private int GetInsertIndexComplex(T item)
        {
            int minIndex = 0, maxIndex = Count - 1;
            while (minIndex < maxIndex)
            {
                int pivotIndex = (maxIndex + minIndex) / 2;
                int comparison = Compare(item, Items[pivotIndex]);
                if (comparison == 0)
                    return _allowDuplicates ? pivotIndex : -1;
                if (comparison < 0)
                    maxIndex = pivotIndex - 1;
                else
                    minIndex = pivotIndex + 1;
            }
            return minIndex;
        }

        private const int SimpleAlgorithmThreshold = 10;
    }

    public partial class OrderedCollection<T>
    {
        //Comparer that uses the type's IComparable implementation to compare two values.
        private sealed class ComparableComparer<TItem> : IComparer<TItem>
        {
            int IComparer<TItem>.Compare(TItem x, TItem y)
            {
                return ((IComparable<TItem>)x).CompareTo(y);
            }
        }
    }

    public partial class OrderedCollection<T>
    {
        //Comparer that uses a Comparison delegate to perform the comparison logic.
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