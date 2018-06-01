namespace MapConfigure.Components.Validations
{
  using System.Globalization;
  using System.Windows.Controls;
  using System.Windows.Data;
  using Core.Data;

  public class RouteFromToValidation : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      TransportStats stat = (value as BindingGroup).Items[0] as TransportStats;
      if (stat.From == null)
        return new ValidationResult(false, "Заполните пункт прибытия");
      else if (stat.To == null)
        return new ValidationResult(false, "Заполните пункт отправки");
      else if (stat.From == stat.To)
        return new ValidationResult(false, "Пункт прибытия и отправки одинаковы");
      return ValidationResult.ValidResult;
    }
  }
}
