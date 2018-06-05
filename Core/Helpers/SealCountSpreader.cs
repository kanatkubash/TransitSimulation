using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
  public class SealCountSpreader
  {
    public virtual int[] Spread(int count, int[] distributions)
    {
      var list = new List<int>(count);
      var total = 0;
      for (var i = 0; i < distributions.Length - 1; i++)
      {
        var percentCount = count * distributions[i] / 100;
        list.AddRange(Enumerable.Repeat(i + 1, percentCount));
        total += percentCount;
      }
      list.AddRange(Enumerable.Repeat(distributions.Length, count - total));

      var array = list.ToArray();
      new Random().Shuffle(array);

      return array;
    }
  }
}