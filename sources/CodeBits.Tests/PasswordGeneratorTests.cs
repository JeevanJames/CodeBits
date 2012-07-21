using System;
using System.Security;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class PasswordGeneratorTests
    {
        [Fact]
        public void Zero_or_negative_password_length_throws_exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.Generate(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.Generate(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.Generate(-100));

            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.GenerateSecure(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.GenerateSecure(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.GenerateSecure(-100));
        }

        [Fact]
        public void Generated_password_lengths_equals_input_length()
        {
            for (int i = 1; i <= 50; i++)
            {
                string password = PasswordGenerator.Generate(i);
                Assert.Equal(i, password.Length);
            }
        }

        [Fact]
        public void Generated_secure_password_lengths_equals_input_length()
        {
            for (int i = 1; i <= 50; i++)
            {
                SecureString password = PasswordGenerator.GenerateSecure(i);
                Assert.Equal(i, password.Length);
            }
        }
    }
}