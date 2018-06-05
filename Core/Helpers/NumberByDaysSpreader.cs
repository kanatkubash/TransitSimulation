namespace Core.Helpers
{
  using System.Linq;

  /// <summary>
  /// Spreads given number to month days
  /// Used to simulate per day count based on monthly count
  /// </summary>
  public class NumberByDaysSpreader
  {
    protected virtual IRandom Random { get; }

    public NumberByDaysSpreader() => Random = new PseudoRandom();

    public NumberByDaysSpreader(IRandom random) => Random = random ?? new PseudoRandom();

    public virtual int[] Spread(int monthDays, int count)
    {
      if (count == 0)
        return new int[monthDays];

      var nums = Random.GetNumbers(count, 0, monthDays - 1);

      var spreads = nums
        .GroupBy(n => n)
        .Select(x => x.Count())
        .ToList();
      if (spreads.Count < monthDays)
        spreads.AddRange(new int[monthDays - spreads.Count]);

      return spreads.ToArray();
    }
  }
}
