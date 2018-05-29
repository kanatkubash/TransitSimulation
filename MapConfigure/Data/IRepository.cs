namespace MapConfigure.Data
{
  using System.Collections.Generic;

  public interface IRepository<T>
  {
    List<T> LoadAll();
    void SaveAll(IEnumerable<T> items);
  }
}
