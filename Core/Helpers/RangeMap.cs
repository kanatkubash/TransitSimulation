namespace Core.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Used to store subroad or subsomething info based on kms
  /// For example if route is A--50km--a'--100km--B
  /// Its stored as
  /// [50]=Road(A,a')
  /// [100]=Road(a',B)
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class RangeMap<T> : SortedDictionary<int, T>
  {
    public int MaxValue { get; }
    public int MinValue { get; }

    public RangeMap(int minValue, int maxValue) => (MinValue, MaxValue) = (minValue, maxValue);

    public new T this[int i]
    {
      get
      {
        if (i > MaxValue)
          throw new ArgumentOutOfRangeException("index");
        if (Keys.Count == 0)
          throw new InvalidOperationException("Range values not set");
        return base[Keys.Last(k => k <= i)];
      }
      set => base[i] = value;
    }
  }
}
