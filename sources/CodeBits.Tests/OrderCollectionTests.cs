using Xunit;

namespace CodeBits.Tests
{
    public sealed class OrderCollectionTests
    {
        [Fact]
        public void DevTest()
        {
            var coll = new OrderedCollection<string>(false);
            coll.Add("Merina");
            coll.Add("Jeevan");
            coll.Add("Ryan");
            coll.Add("James");
            coll.Add("Kkkkk");

            Assert.Equal(5, coll.Count);
            //Assert.Equal("Jeevan", coll[0]);
            //Assert.Equal("Merina", coll[1]);
        }
    }
}