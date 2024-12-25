﻿namespace Budget_Team5;

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
        var firstDay = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
    }

    public DateTime FirstDay()
    {
        var firstDay = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return firstDay;
    }

    public DateTime LastDay()
    {
        var firstDay = DateTime.ParseExact(YearMonth, "yyyyMM", null);
        return new DateTime(firstDay.Year, firstDay.Month, Days());
    }
}