#region --- License & Copyright Notice ---
/*
Useful code blocks that can included in your C# projects through NuGet
Copyright (c) 2012-2021 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.Globalization;
using System.Threading;

using Shouldly;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class ByteSizeFriendlyNameTests
    {
        public ByteSizeFriendlyNameTests()
        {
            // Set current thread culture for predictable digit grouping
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-100)]
        public void Negative_byte_values_should_throw_exception(int bytes)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => ByteSizeFriendlyName.Build(bytes));
        }

        [Theory]
        [InlineData(0, "0 bytes")]
        [InlineData(1, "1 byte")]
        [InlineData(2, "2 bytes")]
        [InlineData(1023, "1023 bytes")]
        public void Bytes_test(int bytes, string expectedFriendlyName)
        {
            string friendlyName = ByteSizeFriendlyName.Build(bytes);
            friendlyName.ShouldBe(expectedFriendlyName);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(1023, "1023")]
        [InlineData(1024, "1 KB")]
        public void Unit_display_mode_hide_only_for_bytes_test(int bytes, string expectedFriendlyName)
        {
            var options = new FriendlyNameOptions
            {
                UnitDisplayMode = UnitDisplayMode.HideOnlyForBytes
            };
            string friendlyName = ByteSizeFriendlyName.Build(bytes, options);
            friendlyName.ShouldBe(expectedFriendlyName);
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
            friendlyName.ShouldBe(expectedFriendlyName);
        }

        [Fact]
        public void Megabyte_values()
        {
            const long size = 1024L * 1024 * 10;
            
            string friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions { DecimalPlaces = 10 });
            friendlyName.ShouldBe("10 MB");

            friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions
            {
                UnitDisplayMode = UnitDisplayMode.AlwaysHide
            });
            friendlyName.ShouldBe("10");

            friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions
            {
                UnitDisplayMode = UnitDisplayMode.HideOnlyForBytes
            });
            friendlyName.ShouldBe("10 MB");
        }

        [Fact]
        public void Gigabyte_values()
        {
            const long size = 1024L * 1024 * 1024 * 10;
            
            string friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions { DecimalPlaces = 10 });
            friendlyName.ShouldBe("10 GB");

            friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions
            {
                UnitDisplayMode = UnitDisplayMode.AlwaysHide
            });
            friendlyName.ShouldBe("10");

            friendlyName = ByteSizeFriendlyName.Build(size, new FriendlyNameOptions
            {
                UnitDisplayMode = UnitDisplayMode.HideOnlyForBytes
            });
            friendlyName.ShouldBe("10 GB");
        }
    }
}
