namespace Budget_Team5;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    private DateTime End { get; set; }

    private DateTime Start { get; set; }

    public int OverlappingDays(Budget budget)
    {
        DateTime overlappingEnd = End < budget.LastDay()
            ? End
            : budget.LastDay();
        DateTime overlappingStart = Start > budget.FirstDay()
            ? Start
            : budget.FirstDay();
        if (budget.YearMonth == Start.ToString("yyyyMM"))
        {
            // overlappingEnd = budget.LastDay();
            // overlappingStart = Start;
        }
        else if (budget.YearMonth == End.ToString("yyyyMM"))
        {
            // overlappingEnd = End;
            // overlappingStart = budget.FirstDay();
        }
        else
        {
            // overlappingEnd = budget.LastDay();
            // overlappingStart = budget.FirstDay();
        }

        return (overlappingEnd - overlappingStart).Days + 1;
    }
}