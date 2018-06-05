using System.Linq;
using Core.Data;
using Core.Entities;
using MapConfigure.Data;

namespace MapConfigure.Helpers
{
  public class RoadStatsMemoryRepo
  {
    public static MemoryRepo<RoadStats> Make(IRepository<Road> roadRepo)
    {
      var roadStats = roadRepo.LoadAll().Select(r => new RoadStats()
      {
        From = r.From.Name,
        To = r.To.Name,
        Length = (int)r.Length,
      });

      var repo = new MemoryRepo<RoadStats>();
      repo.SaveAll(roadStats);
      return repo;
    }
  }
}