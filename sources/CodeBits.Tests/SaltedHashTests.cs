using System;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class SaltedHashTests
    {
        [Fact]
        public void DevTests()
        {
            SaltedHash saltedHash = SaltedHash.Compute("CodeBits");
            Console.WriteLine(saltedHash.Hash);
            Console.WriteLine(saltedHash.Salt);
        }

        private static readonly byte[] Salt = new byte[] {
            96, 58, 235, 87, 24, 66, 65, 139, 131, 144, 106, 202, 160, 54, 94, 33, 34, 33, 246, 26, 126,
            131, 222, 158, 37, 182, 104, 92, 94, 199, 177, 32
        };
    }
}