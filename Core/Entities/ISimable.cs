namespace Core.Entities
{
  /// <summary>
  /// Interface for non static sim.entites
  /// </summary>
  public interface ISimable
  {
    /// <summary>
    /// Runs simulation of given object
    /// </summary>
    /// <param name="t">Current time of simulation</param>
    /// <param name="dtMs">Delta t passed from last simulation</param>
    void Run(long t, long dtMs);
  }
}
