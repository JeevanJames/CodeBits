using System;
using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class MruCollectionTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void Create_instance_with_valid_capacities(int capacity)
        {
            var sut1 = new MruCollection<int>(capacity);
            var sut2 = new MruCollection<NonEquatableReference>(capacity, new MruCollectionOptions<NonEquatableReference>
            {
                EqualityComparer = new NonEquatableEqualityComparer()
            });

            sut1.ShouldNotBeNull();
            sut2.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Create_instance_with_invalid_capacities(int capacity)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new MruCollection<int>(capacity));
            Should.Throw<ArgumentOutOfRangeException>(() => new MruCollection<NonEquatableReference>(capacity, new MruCollectionOptions<NonEquatableReference>
            {
                EqualityComparer = new NonEquatableEqualityComparer()
            }));
        }

        [Fact]
        public void Create_instance_with_type_not_implementing_IEquatable()
        {
            Should.Throw<ArgumentException>(() => new MruCollection<NonEquatableReference>(10));
            Should.Throw<ArgumentException>(() => new MruCollection<NonEquatableStruct>(10));
        }

        [Fact]
        public void Accessing_an_item_moves_it_to_the_top()
        {
            var stringCollection = new MruCollection<string>(12, new MruCollectionOptions<string> {
                InitialData = StringData
            });
            var june = stringCollection[5];
            june.ShouldBe("June");
            stringCollection.Peek(0).ShouldBe("June");
        }

        [Fact]
        public void Accessing_an_item_should_not_trigger()
        {
            var stringCollection = new MruCollection<string>(12, new MruCollectionOptions<string>
            {
                InitialData = StringData,
                Triggers = MruTriggers.ItemSetOrInserted
            });
            var june = stringCollection[5];
            june.ShouldBe("June");
            stringCollection.Peek(0).ShouldBe("January");
        }

        public static IEnumerable<string> StringData => new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static IEnumerable<int> NumberData => new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    public class NonEquatableReference
    {
        public int SomeProp { get; set; }
    }

    public class NonEquatableStruct
    {
        public int SomeProp { get; set; }
    }

    public sealed class NonEquatableEqualityComparer : IEqualityComparer<NonEquatableReference>
    {

        public bool Equals(NonEquatableReference x, NonEquatableReference y)
        {
            return Equals(x.SomeProp, y.SomeProp);
        }

        public int GetHashCode(NonEquatableReference obj)
        {
            return obj.SomeProp.GetHashCode();
        }
    }
}
