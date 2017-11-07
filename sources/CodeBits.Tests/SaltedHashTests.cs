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