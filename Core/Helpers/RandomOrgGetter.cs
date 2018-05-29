namespace Core.Helpers
{
  using System.Linq;
  using System.Net;

  /// <summary>
  /// RNG based on random.org data
  /// <remarks>Ceases to work sometimes. Use PseudoRandom in that cases</remarks>
  /// </summary>
  public class RandomOrgGetter : IRandom
  {
    protected WebClient Client { get; set; }
    public RandomOrgGetter() => Client = new WebClient();

    public int[] GetNumbers(int nums, int min, int max)
    {
      var url = "https://www.random.org/integers/" +
                $"?num={nums}&min={min}&max={max}&col=1&base=10&format=plain&rnd=new";
      var numsText = Client.DownloadString(url);

      return numsText
        .TrimEnd(new[] { '\n' })
        .Split(new[] { '\n' })
        .Select(str => int.Parse(str))
        .ToArray();
    }
  }
}
