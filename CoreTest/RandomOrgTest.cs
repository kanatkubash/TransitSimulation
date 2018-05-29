using Core.Helpers;
using NUnit.Framework;
using System.Linq;

namespace CoreTest
{
  [TestFixture]
  public class RandomOrgTest
  {
    [Test]
    public void TestGetRandomNumbers()
    {
      var r = new RandomOrgGetter();
      var nums = r.GetNumbers(1666, 0, 30);
      Assert.AreEqual(nums.Length, 1666);
      Assert.That(nums.Min() >= 0);
      Assert.That(nums.Max() <= 30);
      Assert.That(nums.Where(n => n > 30).Count() == 0);
      Assert.That(nums.Where(n => n < 0).Count() == 0);
    }
  }
}
