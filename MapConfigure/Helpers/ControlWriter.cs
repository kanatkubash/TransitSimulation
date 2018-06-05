namespace MapConfigure.Helpers
{
  using System.IO;
  using System.Text;
  using System.Windows.Controls;

  public class ControlWriter : TextWriter
  {
    private readonly TextBox textbox;

    public ControlWriter(TextBox control) => this.textbox = control;

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(string value)
    {
      textbox?.Dispatcher.Invoke(() => textbox.Text += value);
    }

    public override void Write(char value)
    {
      textbox?.Dispatcher.Invoke(() => textbox.Text += value);
    }
  }
}
