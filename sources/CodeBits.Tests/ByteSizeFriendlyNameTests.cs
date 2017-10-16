using System;
using System.Globalization;
using System.Threading;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class ByteSizeFriendlyNameTests
    {
        public ByteSizeFriendlyNameTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-100)]
        public void Negative_byte_values_should_throw_exception(int bytes)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ByteSizeFriendlyName.Build(bytes));
        }

        [Theory]
        [InlineData(0, "0 bytes")]
        [InlineData(1, "1 byte")]
        [InlineData(2, "2 bytes")]
        [InlineData(1023, "1023 bytes")]
        public void Bytes_test(int bytes, string expectedFriendlyName)
        {
            string friendlyName = ByteSizeFriendlyName.Build(bytes);
            Assert.Equal(expectedFriendlyName, friendlyName);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(1023, "1023")]
        [InlineData(1024, "1 KB")]
        public void Unit_display_mode_hide_only_for_bytes_test(int bytes, string expectedFriendlyName)
        {
            var options = new FriendlyNameOptions {
                UnitDisplayMode = UnitDisplayMode.HideOnlyForBytes
            };
            string friendlyName = ByteSizeFriendlyName.Build(bytes, options);
            Assert.Equal(expectedFriendlyName, friendlyName);
        }

        [Theory]
        [InlineData(1023, "1,023 bytes")]
        [InlineData(1024 * 1023, "1,023 KB")]
        public void Unit_display_mode_group_digits_test(int bytes, string expectedFriendlyName)
        {
            var options = new FriendlyNameOptions {
                GroupDigits = true
            };
            string friendlyName = ByteSizeFriendlyName.Build(bytes, options);
            Assert.Equal(expectedFriendlyName, friendlyName);
        }

        [Fact]
        public void Gigabyte_values()
        {
            const long size = 1024L * 1024 * 1024 * 10;
            
            string friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions { DecimalPlaces = 10 });;
            Assert.Equal("10 GB", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions { UnitDisplayMode = UnitDisplayMode.AlwaysHide });
            Assert.Equal("10", friendlyName);
        }
    }
}