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
using System.Runtime.InteropServices;
using System.Security;

using Shouldly;

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
            Should.Throw<ArgumentOutOfRangeException>(() => PasswordGenerator.Generate(length));

            Should.Throw<ArgumentOutOfRangeException>(() => PasswordGenerator.GenerateSecure(length));
        }

        [Fact]
        public void Generated_password_lengths_equals_input_length()
        {
            for (int i = 1; i <= 50; i++)
            {
                string password = PasswordGenerator.Generate(i);
                password.Length.ShouldBe(i);

                SecureString securePassword = PasswordGenerator.GenerateSecure(i);
                securePassword.Length.ShouldBe(i);
            }
        }

        [Theory]
        [InlineData(PasswordCharacters.Numbers, "13579")]
        [InlineData(PasswordCharacters.AllLetters, "JjMmRrEe")]
        [InlineData(PasswordCharacters.AlphaNumeric, "OlIS015")]
        public void Excluded_characters_are_excluded(PasswordCharacters allowedCharacters, string excludeCharacters)
        {
            for (int i = 0; i < 50; i++)
            {
                string password = PasswordGenerator.Generate(40, allowedCharacters, excludeCharacters);
                foreach (char excludeChar in excludeCharacters)
                    password.ShouldNotContain(excludeChar);

                SecureString securePassword = PasswordGenerator.GenerateSecure(40, allowedCharacters, excludeCharacters);
                foreach (char excludeChar in excludeCharacters)
                    SecureStringToString(securePassword).ShouldNotContain(excludeChar);
            }
        }

        // Converts a SecureString to a string
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
