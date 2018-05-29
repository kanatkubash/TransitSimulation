namespace Core.Data
{
  using System.Collections.Generic;

  /// <summary>
  /// Used for persisting simulation entity data
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IDataProvider<T>
  {
    List<T> GetAll();
    void SaveAll(List<T> data);
  }
}
