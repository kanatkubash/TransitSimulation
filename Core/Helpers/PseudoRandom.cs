namespace Core.Helpers
{
  using System;

  /// <summary>
  /// RNG based on System.Random
  /// </summary>
  public class PseudoRandom : IRandom
  {
    protected Random Random { get; set; }

    public PseudoRandom() => Random = new Random();

    public int[] GetNumbers(int nums, int min, int max)
    {
      var numArray = new int[nums];

      for (var i = 0; i < nums; i++)
        numArray[i] = Random.Next(max + 1);

      return numArray;
    }
  }
}
