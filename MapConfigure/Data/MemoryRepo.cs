using System.Collections.Generic;
using System.Linq;
using Core.Data;

namespace MapConfigure.Data
{
  public class MemoryRepo<T> : IRepository<T>, IDataProvider<T>
  {
    protected IEnumerable<T> data;

    public List<T> LoadAll() => Load().ToList();

    public void SaveAll(IEnumerable<T> items) => Save(items);

    public List<T> GetAll() => Load().ToList();

    public void SaveAll(List<T> data) => Save(data);

    protected void Save(IEnumerable<T> items) => data = items;

    protected IEnumerable<T> Load() => data ?? new List<T>();
  }
}