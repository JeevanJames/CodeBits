using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Shouldly;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class EnumIteratorTests
    {
        [Fact]
        public void Ctor_should_throw_if_null_is_specified()
        {
            Should.Throw<ArgumentNullException>(() => EnumIterator.For(null));
        }

        [Fact]
        public void Ctor_should_throw_if_non_enum_type_is_specified()
        {
            Should.Throw<ArgumentException>(() => EnumIterator.For<int>());
            Should.Throw<ArgumentException>(() => EnumIterator.For<long>());
            Should.Throw<ArgumentException>(() => EnumIterator.For<double>());
            Should.Throw<ArgumentException>(() => EnumIterator.For<float>());
            Should.Throw<ArgumentException>(() => EnumIterator.For<DateTime>());

            Should.Throw<ArgumentException>(() => EnumIterator.For(typeof(int)));
            Should.Throw<ArgumentException>(() => EnumIterator.For(typeof(long)));
            Should.Throw<ArgumentException>(() => EnumIterator.For(typeof(double)));
            Should.Throw<ArgumentException>(() => EnumIterator.For(typeof(float)));
            Should.Throw<ArgumentException>(() => EnumIterator.For(typeof(DateTime)));
        }

        [Fact]
        public void Ctor_should_succeed_if_enum_type_is_specified()
        {
            EnumIterator.For<DayOfWeek>().ShouldNotBeNull();
            EnumIterator.For<BindingFlags>().ShouldNotBeNull();

            EnumIterator.For(typeof(DayOfWeek)).ShouldNotBeNull();
            EnumIterator.For(typeof(BindingFlags)).ShouldNotBeNull();
        }

        [Fact]
        public void Enumerator_should_return_all_enum_values_in_correct_order()
        {
            List<DayOfWeek> daysOfWeek = EnumIterator.For<DayOfWeek>().ToList();
            daysOfWeek.Count.ShouldBe(7);
            daysOfWeek[0].ShouldBe(DayOfWeek.Sunday);
            daysOfWeek[1].ShouldBe(DayOfWeek.Monday);
            daysOfWeek[2].ShouldBe(DayOfWeek.Tuesday);
            daysOfWeek[3].ShouldBe(DayOfWeek.Wednesday);
            daysOfWeek[4].ShouldBe(DayOfWeek.Thursday);
            daysOfWeek[5].ShouldBe(DayOfWeek.Friday);
            daysOfWeek[6].ShouldBe(DayOfWeek.Saturday);
        }

        [Fact]
        public void Non_generic_enumerator_should_return_all_enum_values_in_correct_order()
        {
            List<DayOfWeek> daysOfWeek = EnumIterator.For(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList();
            daysOfWeek.Count.ShouldBe(7);
            daysOfWeek[0].ShouldBe(DayOfWeek.Sunday);
            daysOfWeek[1].ShouldBe(DayOfWeek.Monday);
            daysOfWeek[2].ShouldBe(DayOfWeek.Tuesday);
            daysOfWeek[3].ShouldBe(DayOfWeek.Wednesday);
            daysOfWeek[4].ShouldBe(DayOfWeek.Thursday);
            daysOfWeek[5].ShouldBe(DayOfWeek.Friday);
            daysOfWeek[6].ShouldBe(DayOfWeek.Saturday);
        }

        [Fact]
        public void Basic_linq_functions_work_on_the_iterator()
        {
            EnumIterator.For<DayOfWeek>().First().ShouldBe(DayOfWeek.Sunday);
            EnumIterator.For<DayOfWeek>().Last().ShouldBe(DayOfWeek.Saturday);
            EnumIterator.For<DayOfWeek>().Skip(3).Take(1).Single().ShouldBe(DayOfWeek.Wednesday);
            EnumIterator.For<DayOfWeek>().Any().ShouldBeTrue();
            EnumIterator.For<DayOfWeek>().Count().ShouldBe(7);
        }
    }
}