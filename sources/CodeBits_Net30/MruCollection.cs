#region --- License & Copyright Notice ---
/*
CodeBits Code Snippets
Copyright (c) 2012-2017 Jeevan James
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
    public partial class MruCollection<T> : Collection<T>
    {
        private int _capacity;
        private readonly IEqualityComparer<T> _comparer;

        public MruCollection(int capacity)
        {
            _capacity = capacity;

            Type equatableType = typeof(IEquatable<>).MakeGenericType(typeof(T));
            if (!equatableType.IsAssignableFrom(typeof(T))) 
                throw new ArgumentException("Generic type should implement IEquatable<>");
            _comparer = new EquatableEqualityComparer<T>();
        }

        public MruCollection(int capacity, IEqualityComparer<T> comparer)
        {
            _capacity = capacity;
            _comparer = comparer;
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                _capacity = value;
                TrimExcess();
            }
        }

        protected override void InsertItem(int index, T item)
        {
            RemoveExisting(item);
            base.InsertItem(0, item);
            TrimExcess();
        }

        protected override void SetItem(int index, T item)
        {
            InsertItem(index, item);
        }

        private void RemoveExisting(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_comparer.Equals(item, this[i]))
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        private void TrimExcess()
        {
            if (Count <= _capacity)
                return;
            for (int i = Count - 1; i >= _capacity; i--)
                RemoveAt(i);
        }
    }

    public partial class MruCollection<T>
    {
        private sealed class EquatableEqualityComparer<TItem> : IEqualityComparer<TItem>
        {
            bool IEqualityComparer<TItem>.Equals(TItem x, TItem y)
            {
                return x.Equals(y);
            }

            int IEqualityComparer<TItem>.GetHashCode(TItem obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}