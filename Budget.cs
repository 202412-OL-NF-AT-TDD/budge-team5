namespace Budget_Team5;

public class Budget
{
    public int Amount { get; init; }
    public string YearMonth { get; init; }

    public decimal OverlappingAmount(Period period)
    {
        var overlappingDays =
            period.OverlappingDays(CreatePeriod());

        return (decimal)overlappingDays * DailyAmount();
    }

    private Period CreatePeriod()
    {
        return new Period(FirstDay(), LastDay());
    }

    private int DailyAmount()
    {
        return Amount / Days();
    }

    private int Days()
    {
        var firstDay = FirstDay();
        return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
    }

    private DateTime FirstDay()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", null);
    }

    private DateTime LastDay()
    {
        var firstDay = FirstDay();
        return new DateTime(firstDay.Year, firstDay.Month, Days());
    }
}