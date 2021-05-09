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
using System.IO;
using System.Text;

using Shouldly;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class ChecksumTests
    {
        [Theory]
        [ClassData(typeof(ChecksumAlgorithmTestData))]
        public void Should_throw_for_null_values(IChecksumImplementation impl)
        {
            Should.Throw<ArgumentNullException>(() => impl.FromBytes(null));
            Should.Throw<ArgumentNullException>(() => impl.FromFile(null));
            Should.Throw<ArgumentNullException>(() => impl.FromFile(null, 4096));
            Should.Throw<ArgumentNullException>(() => impl.FromStream(null));
            Should.Throw<ArgumentNullException>(() => impl.FromString(null));
        }

        [Theory]
        [ClassData(typeof(ChecksumAlgorithmTestData))]
        public void Should_throw_if_file_not_found(IChecksumImplementation impl)
        {
            Should.Throw<FileNotFoundException>(() => impl.FromFile(string.Empty));
            Should.Throw<FileNotFoundException>(() => impl.FromFile(string.Empty, 4096));

            Should.Throw<FileNotFoundException>(() => impl.FromFile(@"Z:\should_not_exist.txt"));
            Should.Throw<FileNotFoundException>(() => impl.FromFile(@"Z:\should_not_exist.txt", 4096));
        }

        [Theory]
        [ClassData(typeof(ChecksumAlgorithmTestData))]
        public void Empty_byte_array_checksum_should_be_same_as_empty_string_checksum(IChecksumImplementation impl)
        {
            string emptyByteArray = impl.FromBytes(new byte[0]);
            string emptyString = impl.FromString(string.Empty);
            emptyByteArray.ShouldBe(emptyString);
        }

        [Theory]
        [ClassData(typeof(StringHashTestData))]
        public void Should_compute_for_string_values(IChecksumImplementation impl, string expectedChecksum)
        {
            string checksum = impl.FromString("CodeBits");
            checksum.ShouldBe(expectedChecksum);
        }

        [Theory]
        [ClassData(typeof(StringHashTestData))]
        public void Should_compute_for_stream_values(IChecksumImplementation impl, string expectedChecksum)
        {
            byte[] sourceBytes = Encoding.ASCII.GetBytes("CodeBits");
            var ms = new MemoryStream(sourceBytes);
            string checksum = impl.FromStream(ms);
            checksum.ShouldBe(expectedChecksum);
        }
    }

    public sealed class ChecksumAlgorithmTestData : TheoryData<IChecksumImplementation>
    {
        public ChecksumAlgorithmTestData()
        {
            Add(Checksum.Md5);
            Add(Checksum.Sha1);
            Add(Checksum.Sha256);
            Add(Checksum.Sha384);
            Add(Checksum.Sha512);
        }
    }

    public sealed class StringHashTestData : TheoryData<IChecksumImplementation, string>
    {
        // https://defuse.ca/checksums.htm
        public StringHashTestData()
        {
            Add(Checksum.Md5, "7c238bd6ea71d8ec3939121f329fc37a");
            Add(Checksum.Sha1, "58150d1473bf1ec207fa801e262b6c920c8e33ae");
            Add(Checksum.Sha256, "5be9e1147abf9d7b3b2d06f673b5071a12b22fe3af16a79b12643dcb9f4462d6");
            Add(Checksum.Sha384, "a2e64235029bf16b247791e7dadf88f8193acdc982701c53a68b30926b59cc4462abd71cd51c104a666be192e063aa38");
            Add(Checksum.Sha512, "ab768cdcb551a08231b32d2e4731d496c4d8a9cd02ce780208056f307e13b0f7051216897b33e43b2b3fd10de0c389849085f6fd2cd3dff7f0cbf6a3e8b9a5ce");
        }
    }
}
