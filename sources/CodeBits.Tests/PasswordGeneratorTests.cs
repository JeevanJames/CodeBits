using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class PasswordGeneratorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Zero_or_negative_password_length_throws_exception(int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.Generate(length));

            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordGenerator.GenerateSecure(length));
        }

        [Fact]
        public void Generated_password_lengths_equals_input_length()
        {
            for (int i = 1; i <= 50; i++)
            {
                string password = PasswordGenerator.Generate(i);
                Assert.Equal(i, password.Length);
            }
            for (int i = 1; i <= 50; i++)
            {
                SecureString password = PasswordGenerator.GenerateSecure(i);
                Assert.Equal(i, password.Length);
            }
        }

        [Theory]
        [InlineData(PasswordCharacters.Numbers, "13579")]
        [InlineData(PasswordCharacters.AllLetters, "JjMmRrEe")]
        public void Excluded_characters_are_excluded(PasswordCharacters allowedCharacters, string excludeCharacters)
        {
            char[] excludeChars = excludeCharacters.ToArray();

            for (int i = 0; i < 50; i++)
            {
                string password = PasswordGenerator.Generate(40, allowedCharacters, excludeChars);
                foreach (char excludeChar in excludeChars)
                    Assert.DoesNotContain(excludeChar, password);
            }

            for (int i = 0; i < 50; i++)
            {
                SecureString password = PasswordGenerator.GenerateSecure(40, allowedCharacters, excludeChars);
                foreach (char excludeChar in excludeChars)
                    Assert.DoesNotContain(excludeChar, SecureStringToString(password));
            }
        }

        private static string SecureStringToString(SecureString secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            } finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
