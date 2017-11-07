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

/* Documentation: http://codebits.codeplex.com/wikipage?title=CollectionExtensions */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBits
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (items == null)
                return;

            foreach (T item in items)
                collection.Add(item);
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (items == null)
                return;

            collection.AddRange(items.Where(predicate));
        }

        public static void AddRange<TDest, TSource>(this ICollection<TDest> collection, IEnumerable<TSource> items,
            Converter<TSource, TDest> converter)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (converter == null)
                throw new ArgumentNullException("converter");
            if (items == null)
                return;

            collection.AddRange(items.Select(item => converter(item)));
        }

        public static void AddRange<TDest, TSource>(this ICollection<TDest> collection, IEnumerable<TSource> items,
            Func<TSource, bool> predicate,
            Converter<TSource, TDest> converter)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (converter == null)
                throw new ArgumentNullException("converter");
            if (items == null)
                return;

            collection.AddRange(items.Where(predicate).Select(item => converter(item)));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return;
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (T item in source)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null)
                return;
            if (action == null)
                throw new ArgumentNullException("action");

            var index = 0;
            foreach (T item in source)
                action(item, index++);
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

        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
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

        public static bool RemoveLast<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public static TOutput[] ToArray<TInput, TOutput>(this IEnumerable<TInput> source,
            Converter<TInput, TOutput> converter)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (converter == null)
                throw new ArgumentNullException("converter");

            var array = new TOutput[source.Count()];
            var counter = 0;
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
