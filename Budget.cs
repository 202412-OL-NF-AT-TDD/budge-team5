﻿namespace Budget_Team5;

public class Budget
{
    public int Amount { get; set; }
    public string YearMonth { get; set; }

    public int Days()
    {
        var firstDay = DateTime.ParseExact(YearMonth, "yyyyMM",null);
        return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
    }
}