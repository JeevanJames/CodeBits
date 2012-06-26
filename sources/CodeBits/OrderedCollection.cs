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
                int comparison = _comparer.Compare(existingItem, item);
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
                int comparison = _comparer.Compare(item, Items[pivotIndex]);
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