namespace Core.Data
{
  using Newtonsoft.Json;
  using System.Collections.Generic;
  using System.IO;

  /// <summary>
  /// Persists data to JSON file named after type
  /// </summary>
  /// <typeparam name="T">Type of entity</typeparam>
  public class JsonDataProvider<T> : IDataProvider<T>
  {
    private string GetFileName() => $"{typeof(T).Name}.json";

    public List<T> GetAll()
    {
      var filename = GetFileName();
      if (!File.Exists(filename))
        File.Create(filename).Close();
      var text = File.ReadAllText(filename);

      return text == ""
        ? new List<T>()
        : new List<T>(JsonConvert.DeserializeObject<T[]>(text));
    }

    public void SaveAll(List<T> data)
    {
      var filename = GetFileName();
      File.WriteAllText(filename, JsonConvert.SerializeObject(data.ToArray()));
    }
  }
}
