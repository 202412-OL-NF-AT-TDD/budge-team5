﻿namespace Budget_Team5;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0m;
        }

        var days = end.Subtract(start).Days + 1;
        var budgets = _budgetRepo.GetAll();

        if (start.Month == end.Month && start.Year == end.Year)
        {
            var budget = budgets.First(b => b.YearMonth == start.ToString("yyyyMM"));

            return (decimal)budget.Amount / DateTime.DaysInMonth(start.Year, start.Month) * days;
        }

        var monthKeys = new List<string>();
        var startPointer = new DateTime(start.Year, start.Month, 1);
        var endPointer = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

        while (startPointer < endPointer)
        {
            monthKeys.Add(startPointer.ToString("yyyyMM"));
            startPointer = startPointer.AddMonths(1);
        }

        var targetBudgets = budgets.Where(bu => monthKeys.Contains(bu.YearMonth)).ToList();
        var totalBudget = 0m;

        foreach (var targetBudget in targetBudgets)
        {
            int overlappingDays;
            DateTime overlappingEnd;
            DateTime overlappingStart;
            if (targetBudget.YearMonth == start.ToString("yyyyMM"))
            {
                overlappingEnd = targetBudget.LastDay();
                overlappingStart = start;
                // overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            }
            else if (targetBudget.YearMonth == end.ToString("yyyyMM"))
            {
                overlappingEnd = end;
                overlappingStart = targetBudget.FirstDay();
                // overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            }
            else
            {
                overlappingEnd = targetBudget.LastDay();
                overlappingStart = targetBudget.FirstDay();
                // overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            }

            overlappingDays = (overlappingEnd - overlappingStart).Days + 1;

            totalBudget += (decimal)overlappingDays * targetBudget.DailyAmount();
        }

        return totalBudget;
    }
}