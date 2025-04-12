using System.Globalization;
using HotChocolate.Language;

public class DateOnlyType : ScalarType<DateOnly, StringValueNode>
{
    public DateOnlyType(string name, BindingBehavior bind = BindingBehavior.Explicit) : base(name, bind)
    {
        
    }

    public override IValueNode ParseResult(object? resultValue) =>
        ParseValue(resultValue);

    protected override DateOnly ParseLiteral(StringValueNode valueSyntax)
    {
        return DateOnly.Parse(valueSyntax.Value, CultureInfo.InvariantCulture);
    }

    protected override StringValueNode ParseValue(DateOnly runtimeValue)
    {
        return new StringValueNode(runtimeValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}