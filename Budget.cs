namespace Budget_Team5;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public int DailyAmount()
    {
        var dailyAmount = Amount / Days();
        return dailyAmount;
    }

    public int Days()
    {
        var firstDay = FirstDay();
        return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
    }

    public DateTime FirstDay()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", null);
    }

    public DateTime LastDay()
    {
        var firstDay = FirstDay();
        return new DateTime(firstDay.Year, firstDay.Month, Days());
    }
}