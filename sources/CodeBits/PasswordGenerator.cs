#region --- License & Copyright Notice ---
/*
CodeBits Code Snippets
Copyright (c) 2012 Jeevan James
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

/* Documentation: http://codebits.codeplex.com/wikipage?title=PasswordGenerator&referringTitle=Home */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CodeBits
{
    public static class PasswordGenerator
    {
        public static string Generate(int length, PasswordCharacters allowedCharacters = PasswordCharacters.All,
            IEnumerable<char> excludeCharacters = null)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "Password length must be greater than zero");

            var randomBytes = new byte[length];
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(randomBytes);

            string allowedCharactersString = GenerateAllowedCharactersString(allowedCharacters, excludeCharacters);
            int allowedCharactersCount = allowedCharactersString.Length;

            var characters = new char[length];
            for (int i = 0; i < length; i++)
                characters[i] = allowedCharactersString[randomBytes[i] % allowedCharactersCount];
            return new string(characters);
        }

        public static SecureString GenerateSecure(int length, PasswordCharacters allowedCharacters = PasswordCharacters.All,
            IEnumerable<char> excludedCharacters = null)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "Password length must be greater than zero");

            var randomBytes = new byte[length];
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(randomBytes);

            string allowedCharactersString = GenerateAllowedCharactersString(allowedCharacters, excludedCharacters);
            int allowedCharactersCount = allowedCharactersString.Length;

            var password = new SecureString();
            for (int i = 0; i < length; i++)
                password.AppendChar(allowedCharactersString[randomBytes[i] % allowedCharactersCount]);
            password.MakeReadOnly();
            return password;
        }

        private static string GenerateAllowedCharactersString(PasswordCharacters characters, IEnumerable<char> excludeCharacters)
        {
            var allowedCharactersString = new StringBuilder();
            foreach (KeyValuePair<PasswordCharacters, string> type in AllowedPasswordCharacters)
            {
                if ((characters & type.Key) != type.Key)
                    continue;
                if (excludeCharacters == null)
                    allowedCharactersString.Append(type.Value);
                else
                    allowedCharactersString.Append(type.Value.Where(c => !excludeCharacters.Contains(c)).ToArray());
            }
            return allowedCharactersString.ToString();
        }

        private static readonly Dictionary<PasswordCharacters, string> AllowedPasswordCharacters =
            new Dictionary<PasswordCharacters, string>(4) {
                { PasswordCharacters.LowercaseLetters, "abcdefghijklmnopqrstuvwxyz" },
                { PasswordCharacters.UppercaseLetters, "ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                { PasswordCharacters.Numbers, "0123456789" },
                { PasswordCharacters.Punctuations, @"~`!@#$%^&*()_-+={[}]|\:;""'<,>.?/" },
                { PasswordCharacters.Space, " " },
            };
    }

    [Flags]
    public enum PasswordCharacters
    {
        LowercaseLetters = 0x01,
        UppercaseLetters = 0x02,
        Numbers = 0x04,
        Punctuations = 0x08,
        Space = 0x10,
        AllLetters = LowercaseLetters | UppercaseLetters,
        AlphaNumeric = AllLetters | Numbers,
        All = AllLetters | Numbers | Punctuations | Space,
    }
}