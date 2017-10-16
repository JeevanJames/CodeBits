using System;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class ByteArrayHelperTests
    {
        private readonly byte[] _nullArray = null;
        private readonly byte[] _emptyArray = new byte[0];
        private readonly byte[] _zeroedArray = { 0, 0, 0, 0 };

        [Fact]
        public void Fill_tests()
        {
            Assert.Throws<ArgumentNullException>(() => _nullArray.Fill(0));
            _emptyArray.Fill(0);

            var bytes = new byte[4];
            bytes.Fill(5);
            Assert.True(bytes.IsEqualTo(5, 5, 5, 5));
        }

        [Fact]
        public void Null_and_empty_tests()
        {
            Assert.True(_nullArray.IsEqualTo(_nullArray));
            Assert.True(_nullArray.IsEqualTo(null));

            Assert.False(_emptyArray.IsEqualTo(null));
            Assert.False(_emptyArray.IsEqualTo(_nullArray));
            Assert.True(_emptyArray.IsEqualTo(_emptyArray));
            Assert.True(_emptyArray.IsEqualTo(new byte[0]));
            Assert.False(_emptyArray.IsEqualTo(0));

            Assert.False(_zeroedArray.IsEqualTo(null));
            Assert.False(_zeroedArray.IsEqualTo(_nullArray));
            Assert.True(_zeroedArray.IsEqualTo(_zeroedArray));
            Assert.True(_zeroedArray.IsEqualTo(0, 0, 0, 0));
        }

        [Fact]
        public void Value_tests()
        {
            Assert.False(new byte[] { 1, 2, 3, 4 }.IsEqualTo(4, 3, 2, 1));
            Assert.True(new byte[] { 1, 2, 3, 4 }.IsEqualTo(1, 2, 3, 4));

            Assert.False(new byte[] { 1, 2, 3, 4 }.IsEqualTo(1, 2, 3));
            Assert.False(new byte[] { 1, 2, 3, 4 }.IsEqualTo(1, 2, 3, 4, 5));
        }

        [Fact]
        public void IsNullOrEmpty_tests()
        {
            Assert.True(_nullArray.IsNullOrEmpty());
            Assert.True(_emptyArray.IsNullOrEmpty());
            Assert.False(new byte[] { 0 }.IsNullOrEmpty());
            Assert.False(new byte[] { 0, 0, 0, 0 }.IsNullOrEmpty());
            Assert.False(new byte[] { 1, 2, 3, 4 }.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrZeroed_tests()
        {
            Assert.True(_nullArray.IsNullOrZeroed());
            Assert.True(_emptyArray.IsNullOrZeroed());
            Assert.True(new byte[] { 0 }.IsNullOrZeroed());
            Assert.True(new byte[] { 0, 0, 0, 0 }.IsNullOrZeroed());
            Assert.False(new byte[] { 1, 2, 3, 4 }.IsNullOrZeroed());
        }

        [Fact]
        public void ToString_tests()
        {
            Assert.Null(_nullArray.ToString(null));
            Assert.Equal(string.Empty, _emptyArray.ToString(null));

            Assert.Throws<ArgumentNullException>(() => _zeroedArray.ToString(null));
            Assert.Equal("0000", _zeroedArray.ToString(string.Empty));
            Assert.Equal("0, 0, 0, 0", _zeroedArray.ToString(", "));

            var bytes = new byte[] { 255, 255, 255, 255 };
            Assert.Equal("255255255255", bytes.ToString(string.Empty));
            Assert.Equal("255 | 255 | 255 | 255", bytes.ToString(" | "));
        }
    }

    public sealed class ByteArrayHelperSequenceTests
    {

    }
}