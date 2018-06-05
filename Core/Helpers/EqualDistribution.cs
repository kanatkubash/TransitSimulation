using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
  public class EqualDistribution : IRandom
  {
    public int[] GetNumbers(int nums, int min, int max)
    {
      var count = nums / max;
      var ret = new List<int>(nums);
      var total = 0;

      for (int i = min; i < max; i++)
      {
        ret.AddRange(Enumerable.Repeat(i, count));
        total += count;
      }
      ret.AddRange(Enumerable.Repeat(max, nums - total));

      return ret.ToArray();
    }
  }
}