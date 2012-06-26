using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeBits
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection</typeparam>
    public class OrderedObservableCollection<T> : ObservableCollection<T>
    {
        private readonly bool _allowDuplicates;

        public OrderedObservableCollection() : this(false)
        {
        }

        public OrderedObservableCollection(bool allowDuplicates)
        {
            _allowDuplicates = allowDuplicates;
        }

        public OrderedObservableCollection(List<T> list, bool allowDuplicates = false)
            : base(list)
        {
            _allowDuplicates = allowDuplicates;
        }

        public OrderedObservableCollection(IEnumerable<T> collection, bool allowDuplicates = false)
            : base(collection)
        {
            _allowDuplicates = allowDuplicates;
        }

        public bool AllowDuplicates
        {
            get { return _allowDuplicates; }
        }
    }
}