using System;
using System.Linq;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class GetUniqueItemTests
    {
        [Fact]
        public void Ctor_parameter_validations_test()
        {
            Assert.Throws<ArgumentNullException>(() => UniqueItemGenerator.GetNextUniqueItem(null, "Test"));
            Assert.Throws<ArgumentNullException>(() => UniqueItemGenerator.GetNextUniqueItem(Enumerable.Empty<string>(), null));
            Assert.Throws<ArgumentNullException>(() => UniqueItemGenerator.GetNextUniqueItem(Enumerable.Empty<string>(), "Test", null, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Special_collection_size_test()
        {
            Assert.Equal("Test", UniqueItemGenerator.GetNextUniqueItem(Enumerable.Empty<string>(), "Test"));

            var items = new[] { "One", "Two", "Three" };
            Assert.Equal("Test", UniqueItemGenerator.GetNextUniqueItem(items, "Test"));

            items = new[] { "One", "Two", "Test123", "Three" };
            Assert.Equal("Test", UniqueItemGenerator.GetNextUniqueItem(items, "Test"));
        }

        [Fact]
        public void Generates_correct_sequence_test()
        {
            var items = new[] { "Test - 1", "Test - 2", "Test - 3" };
            Assert.Equal("Test", UniqueItemGenerator.GetNextUniqueItem(items, "Test"));

            items = new[] { "Test", "Test - 1", "Test - 4", "Test - 5" };
            Assert.Equal("Test - 2", UniqueItemGenerator.GetNextUniqueItem(items, "Test"));
        }
    }
}