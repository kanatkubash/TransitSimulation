using Core.Helpers;
using NUnit.Framework;
using System.Linq;

namespace CoreTest
{
  [TestFixture]
  public class NumberSpreadTest
  {
    [Test]
    public void TestNumberSpreadAcrossMonth()
    {
      var r = new NumberByDaysSpreader();
      var nums = r.Spread(31, 1666);
      Assert.AreEqual(nums.Length, 31);
      Assert.AreEqual(nums.Sum(), 1666);
    }
  }
}
