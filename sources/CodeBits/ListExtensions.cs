using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBits
{
    public static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (items == null)
                return;

            foreach (T item in items)
                list.Add(item);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (items == null)
                return;

            list.AddRange(items.Where(predicate));
        }

        public static void AddRange<TList, TItem>(this IList<TList> list, IEnumerable<TItem> items, Converter<TItem, TList> converter)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (converter == null)
                throw new ArgumentNullException("converter");
            if (items == null)
                return;

            list.AddRange(items.Select(item => converter(item)));
        }

        public static void AddRange<TList, TItem>(this IList<TList> list, IEnumerable<TItem> items, Func<TItem, bool> predicate,
            Converter<TItem, TList> converter)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (converter == null)
                throw new ArgumentNullException("converter");
            if (items == null)
                return;

            list.AddRange(items.Where(predicate).Select(item => converter(item)));
        }

        public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
        }

        public static IEnumerable<int> IndexOfAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    yield return i;
            }
        }

        public static bool Remove<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public static int RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            int count = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }

        public static TOutput[] ToArray<TInput, TOutput>(this IEnumerable<TInput> source, Converter<TInput, TOutput> converter)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (converter == null)
                throw new ArgumentNullException("converter");

            var array = new TOutput[source.Count()];
            int counter = 0;
            foreach (TInput item in source)
                array[counter++] = converter(item);
            return array;
        }

        public static T[] ToArray<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).ToArray();
        }

        public static TOutput[] ToArray<TInput, TOutput>(this IEnumerable<TInput> source, Func<TInput, bool> predicate,
            Converter<TInput, TOutput> converter)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (converter == null)
                throw new ArgumentNullException("converter");

            return source.Where(predicate).Select(item => converter(item)).ToArray();
        }
    }
}