using Core.Data;

namespace MapConfigure.Data
{
  using System.Collections.Generic;
  using System.IO;
  using Newtonsoft.Json;

  public class JsonRepo<T> : IRepository<T>, IDataProvider<T>
  {
    public List<T> LoadAll() => Load();

    public void SaveAll(IEnumerable<T> items) => Save(items);

    protected void Save(IEnumerable<T> items)
    {
      var filename = typeof(T).Name + ".json";
      File.WriteAllText(filename, JsonConvert.SerializeObject(items));
    }

    protected List<T> Load()
    {
      var filename = typeof(T).Name + ".json";
      if (!File.Exists(filename))
        File.Create(filename).Close();
      var json = File.ReadAllText(filename);
      return string.IsNullOrWhiteSpace(json)
        ? new List<T>()
        : JsonConvert.DeserializeObject<List<T>>(json);
    }

    public List<T> GetAll() => Load();

    public void SaveAll(List<T> data) => Save(data);
  }
}
