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

using Xunit;

namespace CodeBits.Tests
{
    public sealed class SaltedHashTests
    {
        [Fact]
        public void Verify_salted_password_is_verified()
        {
            for (int i = 0; i < 50; i++)
            {
                string password = PasswordGenerator.Generate(32);
                SaltedHash saltedHash = SaltedHash.Compute(password);
                Assert.True(SaltedHash.Verify(password, saltedHash.PasswordHash, saltedHash.Salt));
            }
        }
    }
}
