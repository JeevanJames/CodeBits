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

        [Fact]
        public void Bytes_test()
        {
            string friendlyName = ByteSizeFriendlyName.Build(0);
            Assert.Equal("0 bytes", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(0, new FriendlyNameOptions { UnitDisplayMode = UnitDisplayMode.AlwaysHide });
            Assert.Equal("0", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(1);
            Assert.Equal("1 byte", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(2);
            Assert.Equal("2 bytes", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(1023);
            Assert.Equal("1023 bytes", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(1023, new FriendlyNameOptions { GroupDigits = true });
            Assert.Equal("1,023 bytes", friendlyName);
        }

        [Fact]
        public void Unit_display_mode_hide_only_for_bytes_test()
        {
            var options = new FriendlyNameOptions {
                UnitDisplayMode = UnitDisplayMode.HideOnlyForBytes
            };

            string friendlyName = ByteSizeFriendlyName.Build(1024, options);
            Assert.Equal("1 KB", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(1023, options);
            Assert.Equal("1023", friendlyName);

            options.GroupDigits = true;

            friendlyName = ByteSizeFriendlyName.Build(1023, options);
            Assert.Equal("1,023", friendlyName);
        }

        [Fact]
        public void Gigabyte_values()
        {
            const long Size = 1024L * 1024 * 1024 * 10;
            
            string friendlyName = ByteSizeFriendlyName.Build(Size, new FriendlyNameOptions { DecimalPlaces = 10 });;
            Assert.Equal("10 GB", friendlyName);

            friendlyName = ByteSizeFriendlyName.Build(Size, new FriendlyNameOptions { UnitDisplayMode = UnitDisplayMode.AlwaysHide });
            Assert.Equal("10", friendlyName);
        }
    }
}