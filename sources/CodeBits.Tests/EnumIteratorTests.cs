using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class EnumIteratorTests
    {
        [Fact]
        public void Ctor_should_throw_if_non_enum_type_is_specified()
        {
            Assert.Throws<ArgumentException>(() => new EnumIterator<int>());
            Assert.Throws<ArgumentException>(() => new EnumIterator<long>());
            Assert.Throws<ArgumentException>(() => new EnumIterator<double>());
            Assert.Throws<ArgumentException>(() => new EnumIterator<float>());
            Assert.Throws<ArgumentException>(() => new EnumIterator<decimal>());
            Assert.Throws<ArgumentException>(() => new EnumIterator<DateTime>());
        }

        [Fact]
        public void Ctor_should_succeed_if_enum_type_is_specified()
        {
            Assert.NotNull(new EnumIterator<DayOfWeek>());
            Assert.NotNull(new EnumIterator<BindingFlags>());
        }

        [Fact]
        public void Enumerator_should_return_all_enum_values_in_correct_order()
        {
            List<DayOfWeek> daysOfWeek = new EnumIterator<DayOfWeek>().ToList();
            Assert.Equal(daysOfWeek.Count, 7);
            Assert.Equal(daysOfWeek[0], DayOfWeek.Sunday);
            Assert.Equal(daysOfWeek[1], DayOfWeek.Monday);
            Assert.Equal(daysOfWeek[2], DayOfWeek.Tuesday);
            Assert.Equal(daysOfWeek[3], DayOfWeek.Wednesday);
            Assert.Equal(daysOfWeek[4], DayOfWeek.Thursday);
            Assert.Equal(daysOfWeek[5], DayOfWeek.Friday);
            Assert.Equal(daysOfWeek[6], DayOfWeek.Saturday);
        }

        [Fact]
        public void Non_generic_enumerator_should_return_all_enum_values_in_correct_order()
        {
            var enumerable = new EnumIterator<DayOfWeek>() as IEnumerable;
            List<DayOfWeek> daysOfWeek = enumerable.OfType<DayOfWeek>().ToList();
            Assert.Equal(daysOfWeek.Count, 7);
            Assert.Equal(daysOfWeek[0], DayOfWeek.Sunday);
            Assert.Equal(daysOfWeek[1], DayOfWeek.Monday);
            Assert.Equal(daysOfWeek[2], DayOfWeek.Tuesday);
            Assert.Equal(daysOfWeek[3], DayOfWeek.Wednesday);
            Assert.Equal(daysOfWeek[4], DayOfWeek.Thursday);
            Assert.Equal(daysOfWeek[5], DayOfWeek.Friday);
            Assert.Equal(daysOfWeek[6], DayOfWeek.Saturday);
        }

        [Fact]
        public void Basic_linq_functions_work_on_the_iterator()
        {
            var enumerable = new EnumIterator<DayOfWeek>();
            Assert.Equal(DayOfWeek.Sunday, enumerable.First());
            Assert.Equal(DayOfWeek.Saturday, enumerable.Last());
            Assert.Equal(DayOfWeek.Wednesday, enumerable.Skip(3).Take(1).Single());
            Assert.True(enumerable.Any());
            Assert.Equal(7, enumerable.Count());
        }
    }
}