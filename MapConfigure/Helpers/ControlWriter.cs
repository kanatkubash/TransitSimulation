namespace MapConfigure.Helpers
{
  using System.IO;
  using System.Text;
  using System.Windows.Controls;

  public class ControlWriter : TextWriter
  {
    private readonly TextBox textbox;

    public ControlWriter(TextBox control)
    {
      textbox = control;
      textbox.AcceptsReturn = true;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(string value)
    {
      textbox?.Dispatcher.Invoke(() =>
      {
        textbox.AppendText(value);
        if (textbox.IsVisible)
          textbox.ScrollToEnd();
      });
    }

    public override void Write(char value)
    {
      textbox?.Dispatcher.Invoke(() =>
      {
        textbox.AppendText(value.ToString());
        if (textbox.IsVisible)
          textbox.ScrollToEnd();
      });
    }
  }
}
