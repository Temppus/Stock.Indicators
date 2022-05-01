namespace Skender.Stock.Indicators;

// NULLABLE SYSTEM OVERRIDES
internal static class NullMath
{
    internal static decimal? Abs(decimal? value)
        => (value is null)
        ? null
        : value < 0 ? (decimal)-value : (decimal)value;

    internal static double? Abs(double? value)
        => (value is null)
        ? null
        : value < 0 ? (double)-value : (double)value;

    internal static double? Atan(double? value)
        => (value is null)
        ? null
        : Math.Atan((double)value);

    internal static double? Log(double? value)
        => (value is null)
        ? null
        : Math.Log((double)value);

    internal static double? Max(double? d1, double? d2)
        => (d1 is null || d2 is null)
        ? null
        : d1 > d2 ? d1 : d2;

    internal static double? Min(double? d1, double? d2)
        => (d1 is null || d2 is null)
        ? null
        : d1 < d2 ? d1 : d2;

    internal static decimal? Round(decimal? value, int digits)
        => (value is null)
        ? null
        : Math.Round((decimal)value, digits);

    internal static double? Round(double? value, int digits)
        => (value is null)
        ? null
        : Math.Round((double)value, digits);

    internal static double? Sqrt(double? value)
        => (value is null)
        ? null
        : Math.Sqrt((double)value);
}
