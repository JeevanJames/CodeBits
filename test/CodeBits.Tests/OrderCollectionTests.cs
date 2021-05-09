#region --- License & Copyright Notice ---
/*
Useful code blocks that can included in your C# projects through NuGet
Copyright (c) 2012-2021 Jeevan James
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

using Xunit;

namespace CodeBits.Tests
{
    public sealed class OrderCollectionTests
    {
        [Fact]
        public void Default_ctor_works()
        {
            var collection = new OrderedCollection<string>();
            Assert.NotNull(collection);
            Assert.Empty(collection);
            Assert.False(collection.AllowDuplicates);
            Assert.False(collection.ReverseOrder);
        }

        [Fact]
        public void Default_ctor_throws_when_type_is_not_IComparable()
        {
            Assert.Throws<ArgumentException>(() => new OrderedCollection<DayOfWeek>());
        }

        [Fact]
        public void Verify_non_default_ctors()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderedCollection<string>((IComparer<string>)null, null));
            Assert.Throws<ArgumentNullException>(() => new OrderedCollection<string>((Comparison<string>)null, null));
        }

        [Theory]
        [MemberData(nameof(Items))]
        public void Verify_inserts(string[] items)
        {
            var collection = new OrderedCollection<string>();
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                collection.Add(item);
                Assert.Equal(i + 1, collection.Count);
                AssertCollection(collection, (x, y) => string.CompareOrdinal(x, y) <= 0);
            }
        }

        [Theory]
        [MemberData(nameof(Items))]
        public void Verify_inserts_reverse(string[] items)
        {
            var collection = new OrderedCollection<string>(new OrderedCollectionOptions(false, true));
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                collection.Add(item);
                Assert.Equal(i + 1, collection.Count);
                AssertCollection(collection, (x, y) => string.CompareOrdinal(x, y) >= 0);
            }
        }

        private static void AssertCollection(OrderedCollection<string> collection, Func<string, string, bool> compareLogic)
        {
            for (int i = 0; i < collection.Count - 1; i++)
                Assert.True(compareLogic(collection[i], collection[i + 1]));
        }

        public static IEnumerable<object[]> Items
        {
            get
            {
                yield return
                    new object[] {
                        new[] {
                            "Thor", "Hulk", "Captain America", "Ironman", "Black Window", "Hawkeye", "Black Panther", "Quake", "Protector",
                            "Vision", "Red Hulk", "Spider-Woman", "Ant-Man", "Wasp"
                        }
                    };
                yield return new object[] { new[] { "Flash", "Superman", "Batman", "Wonder Woman", "Aquaman", "Hawkgirl" } };
            }
        }
    }
}
