﻿#region --- License & Copyright Notice ---
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

<auto-generated>
    This code is downloaded from a CodeBits NuGet package.
</auto-generated>
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;

using Shouldly;

using Xunit;
using Xunit.Abstractions;

namespace CodeBits.NetStandard21.Tests
{
    public sealed class ShortGuidTests
    {
        private readonly ITestOutputHelper _log;

        public ShortGuidTests(ITestOutputHelper log)
        {
            _log = log;
        }

        [Fact]
        public void Can_create_using_ctor()
        {
            Should.NotThrow(() => new ShortGuid(Guid.NewGuid()));
        }

        [Fact]
        public void Can_create_valid_short_guid()
        {
            ShortGuid shortGuid = new ShortGuid(Guid.NewGuid());
            string shortGuidStr = shortGuid.ToString();

            shortGuidStr.Length.ShouldBe(22);
        }

        [Theory, MemberData(nameof(GetValidShortGuidStrings))]
        public void Can_use_implicit_converters(string str)
        {
            ShortGuid shortGuid = str;
            shortGuid.ToString().ShouldBeValidShortGuid();

            Guid guid = shortGuid;
            guid.ShouldNotBe(Guid.Empty);

            ShortGuid shortGuidFromGuid = guid;
            shortGuidFromGuid.ToString().ShouldBeValidShortGuid();
        }

        [Theory]
        [MemberData(nameof(GetValidShortGuidStrings))]
        public void Can_parse_valid_short_guids(string str)
        {
            Should.NotThrow(() => ShortGuid.Parse(str));
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("CodeBits")] public void Parse_throws_for_invalid_short_guids(string str)
        {
            Should.Throw<ArgumentException>(() => ShortGuid.Parse(str));
        }

        [Theory, MemberData(nameof(GetValidShortGuidStrings))]
        public void Can_convert_to_string_using_type_converter(string input)
        {
            ShortGuid shortGuid = ShortGuid.Parse(input);

            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(ShortGuid));
            string converted = (string)typeConverter.ConvertTo(shortGuid, typeof(string));

            typeConverter.CanConvertTo(typeof(string)).ShouldBe(true);
            converted.ShouldBe(input);
            converted.ShouldBeValidShortGuid();
        }

        [Theory, MemberData(nameof(GetValidShortGuidStrings))]
        public void Can_convert_from_string_using_type_converter(string str)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(ShortGuid));
            ShortGuid shortGuid = (ShortGuid)typeConverter.ConvertFrom(str);

            typeConverter.CanConvertFrom(typeof(string)).ShouldBe(true);
            shortGuid.ToString().ShouldBe(str);
        }

        [Theory, MemberData(nameof(GetValidShortGuidStrings))]
        public void Can_serialize_to_and_from_json(string str)
        {
            RecordWithShortGuid data = new() { SGValue = str };
            string serialized = JsonSerializer.Serialize(data);
            serialized.ShouldBe($"{{\"SGValue\":\"{str}\"}}");

            RecordWithShortGuid deserialized = JsonSerializer.Deserialize<RecordWithShortGuid>(serialized);
            deserialized.ShouldNotBeNull();
            deserialized.SGValue.ToString().ShouldBe(str);
        }

        [Fact]
        public void Can_generate_many_random_short_guids()
        {
            for (int i = 0; i < 1000; i++)
            {
                Guid guid = Guid.NewGuid();
                ShortGuid shortGuid = new ShortGuid(guid);

                _log.WriteLine($"{shortGuid} <==> {guid:D}");

                Guid guidClone = shortGuid;
                guidClone.ShouldBe(guid);
            }
        }

        public static IEnumerable<object[]> GetValidShortGuidStrings()
        {
            yield return new object[] { "OSt75kKga0KPsHXNG0zVOw" };
            yield return new object[] { "STVUhVGZ8k-LEKO8AHRQtg" };
            yield return new object[] { "Nik5UuodyUawej_BUENQNw" };
            yield return new object[] { "4xcB0na1oE6h5xYhF5mr-A" };
        }
    }

    internal sealed record RecordWithShortGuid
    {
        public ShortGuid SGValue { get; set; }
    }

    internal static class ShortGuidShouldyExtensions
    {
        internal static void ShouldBeValidShortGuid(this ShortGuid shortGuid)
        {
            string shortGuidStr = shortGuid.ToString();

        }

        internal static string ShouldBeValidShortGuid(this string shortGuidStr)
        {
            shortGuidStr.ShouldNotBeNull();
            shortGuidStr.ShouldMatch(@"^[\w-]{22}$");
            return shortGuidStr;
        }
    }
}
