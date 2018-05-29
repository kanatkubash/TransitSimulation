namespace Core.Entities
{
  using GMap.NET;

  /// <summary>
  /// Represents vertex of graph i.e. City, Customs Post
  /// </summary>
  public class Node : SimItem
  {
    public string Name { get; set; }
    /// <summary>
    /// Number of incoming trucks
    /// </summary>
    public int In { get; set; }
    /// <summary>
    /// Number of outcoming trucks
    /// </summary>
    public int Out { get; set; }
    /// <summary>
    /// Number of passing by trucks
    /// </summary>
    public int Transit { get; set; }
    /// <summary>
    /// Position in map
    /// </summary>
    public PointLatLng LatLng { get; set; }
    /// <summary>
    /// Construct Node
    /// </summary>
    /// <param name="name">Name of node (city,customs post)</param>
    /// <param name="id">Id of node</param>
    public Node(string name, int id = 0) : base(id) => Name = name;

    public static bool operator ==(Node n1, Node n2) => n1.Name == n2.Name;
    public static bool operator !=(Node n1, Node n2) => n1.Name != n2.Name;

    public override string ToString() => this.Name;
  }
}
