namespace Core.Entities
{
  using System;

  /// <summary>
  /// Truck is truck
  /// </summary>
  public class Truck : SimItem, ISimable
  {
    /// <summary>
    /// Road to drive
    /// </summary>
    public Road Road { get; }
    public int AvgSpeed { get; }
    /// <summary>
    /// Seals (plombas)
    /// </summary>
    public int Seals { get; }
    public double RoadPercentage => DrivenKms / Road.Length * 100d;
    /// <summary>
    /// Kms already driven
    /// </summary>
    public double DrivenKms { get; private set; } = 0;
    /// <summary>
    /// Is arrived at destination
    /// </summary>
    public bool Arrived
    {
      get => arrived;
      private set => arrived = value;
    }
    private bool arrived = false;
    /// <summary>
    /// Passed seconds from start of driving
    /// </summary>
    public double PassedSeconds { get; private set; } = 0;

    public Truck(Road road, int speed, int seals) : base() => (Road, AvgSpeed, Seals) = (road, speed, seals);

    public void Run(long t, long dtMs)
    {
      if (Arrived)
        return;
      var road = Road
                 ?? throw new TypeInitializationException("Truck", new Exception("Road is NULL"));
      /// x = km/3.6*s/1000
      var deltaKm = AvgSpeed * 1.0 / 3.6 * dtMs / 1000 / 1000;
      DrivenKms += deltaKm;
      Arrived = RoadPercentage >= 100.0d;
      PassedSeconds += dtMs * 1.0 / 1000;
    }
  }
}
