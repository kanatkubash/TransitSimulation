namespace Core.Helpers
{
  using System.Linq;

  /// <summary>
  /// Spreads given number to month days
  /// Used to simulate per day count based on monthly count
  /// </summary>
  public class NumberByDaysSpreader
  {
    protected IRandom Random { get; }

    public NumberByDaysSpreader() => Random = new PseudoRandom();

    public int[] Spread(int monthDays, int count)
    {
      var nums = Random.GetNumbers(count, 0, monthDays - 1);

      return nums
        .GroupBy(n => n)
        .Select(x => x.Count())
        .ToArray();
    }
  }
}
