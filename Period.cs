namespace Budget_Team5;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime End { get; private set; }

    public DateTime Start { get; private set; }

    public int OverlappingDays(Budget targetBudget)
    {
        DateTime overlappingEnd;
        DateTime overlappingStart;
        if (targetBudget.YearMonth == Start.ToString("yyyyMM"))
        {
            overlappingEnd = targetBudget.LastDay();
            overlappingStart = Start;
        }
        else if (targetBudget.YearMonth == End.ToString("yyyyMM"))
        {
            overlappingEnd = End;
            overlappingStart = targetBudget.FirstDay();
        }
        else
        {
            overlappingEnd = targetBudget.LastDay();
            overlappingStart = targetBudget.FirstDay();
        }

        var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
        return overlappingDays;
    }
}