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

        // var days = end.Subtract(start).Days + 1;
        // var budgets = _budgetRepo.GetAll();
        //
        // if (start.Month == end.Month && start.Year == end.Year)
        // {
        //     var budget = budgets.First(b => b.YearMonth == start.ToString("yyyyMM"));
        //
        //     return (decimal)budget.Amount / DateTime.DaysInMonth(start.Year, start.Month) * days;
        // }
        //
        // var monthKeys = new List<string>();
        // var startPointer = new DateTime(start.Year, start.Month, 1);
        // var endPointer = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));
        //
        // while (startPointer < endPointer)
        // {
        //     monthKeys.Add(startPointer.ToString("yyyyMM"));
        //     startPointer = startPointer.AddMonths(1);
        // }
        //
        // var targetBudgets = budgets.Where(bu => monthKeys.Contains(bu.YearMonth)).ToList();

        var period = new Period(start, end);

        return _budgetRepo.GetAll().Sum(targetBudget => targetBudget.OverlappingAmount(period));
        // return targetBudgets.Sum(targetBudget => targetBudget.OverlappingAmount(period));
    }
}