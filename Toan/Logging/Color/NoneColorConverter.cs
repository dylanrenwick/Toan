namespace Toan.Logging.Color;

public class NoneColorConverter : ColorConverter
{
    public override string Convert(LogColor color)
        => string.Empty;
}

