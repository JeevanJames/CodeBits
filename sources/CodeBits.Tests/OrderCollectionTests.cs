﻿using System;

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
            Assert.Equal(collection.AllowDuplicates, false);
        }

        [Fact]
        public void Verify_simple_insertions()
        {
            var collection = new OrderedCollection<string>();
            collection.Add("Spiderman");
            AssertCollection(collection, "Spiderman");
            collection.Add("Ironman");
            AssertCollection(collection, "Ironman", "Spiderman");
            collection.Add("Thor");
            AssertCollection(collection, "Ironman", "Spiderman", "Thor");
            collection.Add("Hawkeye");
            AssertCollection(collection, "Hawkeye", "Ironman", "Spiderman", "Thor");
            collection.Add("Black Widow");
            AssertCollection(collection, "Black Widow", "Hawkeye", "Ironman", "Spiderman", "Thor");
            collection.Add("Captain America");
            AssertCollection(collection, "Black Widow", "Captain America", "Hawkeye", "Ironman", "Spiderman", "Thor");
            Assert.Throws<ArgumentException>(() => collection.Add("Thor"));
        }

        private void AssertCollection(OrderedCollection<string> collection, params string[] values)
        {
            Assert.Equal(collection.Count, values.Length);
            for (int i = 0; i < collection.Count; i++)
                Assert.Equal(collection[i], values[i]);
        }

        [Fact]
        public void DevTest()
        {
            var coll = new OrderedCollection<string>(false);
            coll.Add("Merina");
            coll.Add("Jeevan");
            coll.Add("Ryan");
            coll.Add("James");
            coll.Add("Kkkkk");

            Assert.Equal(5, coll.Count);
            Assert.Equal("James", coll[0]);
            Assert.Equal("Jeevan", coll[1]);
            Assert.Equal("Kkkkk", coll[2]);
            Assert.Equal("Merina", coll[3]);
            Assert.Equal("Ryan", coll[4]);
        }
    }
}