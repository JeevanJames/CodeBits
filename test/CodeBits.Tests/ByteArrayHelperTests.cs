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

using Shouldly;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class ByteArrayHelperTests
    {
        private readonly byte[] _nullArray = null;
        private readonly byte[] _emptyArray = Array.Empty<byte>();
        private readonly byte[] _zeroedArray = { 0, 0, 0, 0 };

        [Fact]
        public void Fill_tests()
        {
            Should.Throw<ArgumentNullException>(() => _nullArray.Fill(0));
            Should.NotThrow(() => _emptyArray.Fill(0));

            var bytes = new byte[4];
            bytes.Fill(5);
            bytes.IsEqualTo(5, 5, 5, 5).ShouldBe(true);
        }

        [Fact]
        public void Null_and_empty_tests()
        {
            _nullArray.IsEqualTo(_nullArray).ShouldBeTrue();
            _nullArray.IsEqualTo(null).ShouldBeTrue();

            _emptyArray.IsEqualTo(null).ShouldBeFalse();
            _emptyArray.IsEqualTo(_nullArray).ShouldBeFalse();
            _emptyArray.IsEqualTo(_emptyArray).ShouldBeTrue();
            _emptyArray.IsEqualTo(Array.Empty<byte>()).ShouldBeTrue();
            _emptyArray.IsEqualTo(0).ShouldBeFalse();

            _zeroedArray.IsEqualTo(null).ShouldBeFalse();
            _zeroedArray.IsEqualTo(_nullArray).ShouldBeFalse();
            _zeroedArray.IsEqualTo(_zeroedArray).ShouldBeTrue();
            _zeroedArray.IsEqualTo(0, 0, 0, 0).ShouldBeTrue();
        }

        [Fact]
        public void Value_tests()
        {
            var sut = new byte[] { 1, 2, 3, 4 };

            sut.IsEqualTo(4, 3, 2, 1).ShouldBeFalse();
            sut.IsEqualTo(1, 2, 3, 4).ShouldBeTrue();

            sut.IsEqualTo(1, 2, 3).ShouldBeFalse();
            sut.IsEqualTo(1, 2, 3, 4, 5).ShouldBeFalse();
        }

        [Fact]
        public void IsNullOrEmpty_tests()
        {
            _nullArray.IsNullOrEmpty().ShouldBeTrue();
            _emptyArray.IsNullOrEmpty().ShouldBeTrue();
            new byte[] { 0 }.IsNullOrEmpty().ShouldBeFalse();
            new byte[] { 0, 0, 0, 0 }.IsNullOrEmpty().ShouldBeFalse();
            new byte[] { 1, 2, 3, 4 }.IsNullOrEmpty().ShouldBeFalse();
        }

        [Fact]
        public void IsNullOrZeroed_tests()
        {
            _nullArray.IsNullOrZeroed().ShouldBeTrue();
            _emptyArray.IsNullOrZeroed().ShouldBeTrue();
            new byte[] { 0 }.IsNullOrZeroed().ShouldBeTrue();
            new byte[] { 0, 0, 0, 0 }.IsNullOrZeroed().ShouldBeTrue();
            new byte[] { 1, 2, 3, 4 }.IsNullOrZeroed().ShouldBeFalse();
        }

        [Fact]
        public void ToString_tests()
        {
            Should.Throw<ArgumentNullException>(() => _nullArray.ToString(null));
            Should.Throw<ArgumentNullException>(() => _emptyArray.ToString(null));
            Should.Throw<ArgumentNullException>(() => _zeroedArray.ToString(null));

            _zeroedArray.ToString(string.Empty).ShouldBe("0000");
            _zeroedArray.ToString(", ").ShouldBe("0, 0, 0, 0");

            var bytes = new byte[] { 255, 255, 255, 255 };
            bytes.ToString(string.Empty).ShouldBe("255255255255");
            bytes.ToString(" | ").ShouldBe("255 | 255 | 255 | 255");
        }
    }

    public sealed class ByteArrayHelperSequenceTests
    {

    }
}
