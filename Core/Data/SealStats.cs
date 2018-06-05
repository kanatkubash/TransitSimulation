namespace Core.Data
{
  public class SealStats
  {
    public string Node { get; set; }
    public int Minimum { get; set; }
    public int[] Inquiries { get; set; }
    public int[] Stock { get; set; }
    public int[] Reuses { get; set; }
    public int[] Increments { get; set; }
  }
}
