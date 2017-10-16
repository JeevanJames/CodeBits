using System;
using System.Collections.Generic;

using Xunit;
using Xunit.Extensions;

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

            Assert.Throws<ArgumentException>(() => new OrderedCollection<DayOfWeek>());
        }

        [Fact]
        public void Verify_non_default_ctors()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderedCollection<string>((IComparer<string>)null, null));
            Assert.Throws<ArgumentNullException>(() => new OrderedCollection<string>((Comparison<string>)null, null));
        }

        [Theory]
        [PropertyData(nameof(Items))]
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
        [PropertyData(nameof(Items))]
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