namespace Core.Entities
{
  using Helpers;

  /// <summary>
  /// Base class for all simulation entities
  /// </summary>
  public class SimItem
  {
    public int Id { get; protected set; }
    /// <summary>
    /// Dictionary for storing meta info on entity
    /// </summary>
    public HashMap Meta { get; private set; }

    public SimItem() => Meta = new HashMap();

    public SimItem(int id, HashMap values = null)
    {
      if (values == null)
        values = new HashMap();
      Id = id;
      Meta = values;
    }

    public object this[string key]
    {
      get => Meta.ContainsKey(key) ? Meta[key] : null;
      set => Meta[key] = value;
    }
  }
}
