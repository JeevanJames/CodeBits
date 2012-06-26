using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class EnumListTests
    {
        [Fact]
        public void Ctor_should_throw_if_non_enum_type_is_specified()
        {
            Assert.Throws<ArgumentException>(() => new EnumList<int>());
            Assert.Throws<ArgumentException>(() => new EnumList<long>());
            Assert.Throws<ArgumentException>(() => new EnumList<double>());
            Assert.Throws<ArgumentException>(() => new EnumList<float>());
            Assert.Throws<ArgumentException>(() => new EnumList<decimal>());
            Assert.Throws<ArgumentException>(() => new EnumList<DateTime>());
        }

        [Fact]
        public void Ctor_should_succeed_if_enum_type_is_specified()
        {
            Assert.NotNull(new EnumList<DayOfWeek>());
            Assert.NotNull(new EnumList<BindingFlags>());
        }

        [Fact]
        public void Enumerator_should_return_all_enum_values_in_correct_order()
        {
            List<DayOfWeek> daysOfWeek = new EnumList<DayOfWeek>().ToList();
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
            var enumerable = new EnumList<DayOfWeek>() as IEnumerable;
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
    }
}