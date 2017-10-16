using System;
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
            Assert.Throws<ArgumentException>(() => EnumIterator.For<int>());
            Assert.Throws<ArgumentException>(() => EnumIterator.For<long>());
            Assert.Throws<ArgumentException>(() => EnumIterator.For<double>());
            Assert.Throws<ArgumentException>(() => EnumIterator.For<float>());
            Assert.Throws<ArgumentException>(() => EnumIterator.For<DateTime>());

            Assert.Throws<ArgumentException>(() => EnumIterator.For(typeof(int)));
            Assert.Throws<ArgumentException>(() => EnumIterator.For(typeof(long)));
            Assert.Throws<ArgumentException>(() => EnumIterator.For(typeof(double)));
            Assert.Throws<ArgumentException>(() => EnumIterator.For(typeof(float)));
            Assert.Throws<ArgumentException>(() => EnumIterator.For(typeof(DateTime)));
        }

        [Fact]
        public void Ctor_should_succeed_if_enum_type_is_specified()
        {
            Assert.NotNull(EnumIterator.For<DayOfWeek>());
            Assert.NotNull(EnumIterator.For<BindingFlags>());

            Assert.NotNull(EnumIterator.For(typeof(DayOfWeek)));
            Assert.NotNull(EnumIterator.For(typeof(BindingFlags)));
        }

        [Fact]
        public void Enumerator_should_return_all_enum_values_in_correct_order()
        {
            List<DayOfWeek> daysOfWeek = EnumIterator.For<DayOfWeek>().ToList();
            Assert.Equal(7, daysOfWeek.Count);
            Assert.Equal(DayOfWeek.Sunday, daysOfWeek.ElementAt(0));
            Assert.Equal(DayOfWeek.Monday, daysOfWeek.ElementAt(1));
            Assert.Equal(DayOfWeek.Tuesday, daysOfWeek.ElementAt(2));
            Assert.Equal(DayOfWeek.Wednesday, daysOfWeek.ElementAt(3));
            Assert.Equal(DayOfWeek.Thursday, daysOfWeek.ElementAt(4));
            Assert.Equal(DayOfWeek.Friday, daysOfWeek.ElementAt(5));
            Assert.Equal(DayOfWeek.Saturday, daysOfWeek.ElementAt(6));
        }

        [Fact]
        public void Non_generic_enumerator_should_return_all_enum_values_in_correct_order()
        {
            List<DayOfWeek> daysOfWeek = EnumIterator.For(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList();
            Assert.Equal(7, daysOfWeek.Count);
            Assert.Equal(DayOfWeek.Sunday, daysOfWeek.ElementAt(0));
            Assert.Equal(DayOfWeek.Monday, daysOfWeek.ElementAt(1));
            Assert.Equal(DayOfWeek.Tuesday, daysOfWeek.ElementAt(2));
            Assert.Equal(DayOfWeek.Wednesday, daysOfWeek.ElementAt(3));
            Assert.Equal(DayOfWeek.Thursday, daysOfWeek.ElementAt(4));
            Assert.Equal(DayOfWeek.Friday, daysOfWeek.ElementAt(5));
            Assert.Equal(DayOfWeek.Saturday, daysOfWeek.ElementAt(6));
        }

        [Fact]
        public void Basic_linq_functions_work_on_the_iterator()
        {
            List<DayOfWeek> enumerable = EnumIterator.For<DayOfWeek>().ToList();
            Assert.Equal(DayOfWeek.Sunday, enumerable.First());
            Assert.Equal(DayOfWeek.Saturday, enumerable.Last());
            Assert.Equal(DayOfWeek.Wednesday, enumerable.Skip(3).Take(1).Single());
            Assert.True(enumerable.Any());
            Assert.Equal(7, enumerable.Count);
        }
    }
}