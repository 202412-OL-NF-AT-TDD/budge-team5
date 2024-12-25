using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Budget_Team5;

public class Tests
{
    private BudgetService _budgetService;
    private IBudgetRepo _budgetRepo;

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void InvalidStartEnd()
    {
        var result = _budgetService.Query(new DateTime(2024, 11, 30), new DateTime(2024, 11, 1));

        result.Should().Be(0m);
    }

    [Test]
    public void Query_Partial_Oct()
    {
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202410",
                Amount = 6200
            }
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 20), new DateTime(2024, 10, 30));

        result.Should().Be(2200m);
    }


    [Test]
    public void Query_Whole_Oct()
    {
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202410",
                Amount = 6200
            }
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 01), new DateTime(2024, 10, 31));

        result.Should().Be(6200m);
    }


    [Test]
    public void Query_Cross_Two_Months()
    {
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202410",
                Amount = 6200
            },
            new Budget
            {
                YearMonth = "202411",
                Amount = 3000
            },
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2024, 11, 1));

        result.Should().Be(300m);
    }

    [Test]
    public void Query_Cross_More_Than_Two_Months()
    {
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202410",
                Amount = 6200
            },
            new Budget
            {
                YearMonth = "202411",
                Amount = 3000
            },
            new Budget
            {
                YearMonth = "202412",
                Amount = 3100
            },
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2024, 12, 1));

        result.Should().Be(3300m);
    }

    [Test]
    public void Query_When_Db_No_Budget_For_Not_Start_And_Not_End()
    {
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202410",
                Amount = 6200
            },
            new Budget
            {
                YearMonth = "202411",
                Amount = 3000
            },
            // new Budget
            // {
            //     YearMonth = "202412",
            //     Amount = 3100
            // },
            new Budget
            {
                YearMonth = "202501",
                Amount = 9300
            },
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2025, 1, 1));

        result.Should().Be(3500m);
    }
    
    [Test]
    public void Query_When_Db_No_Budget_For_Start()
    {
        _budgetRepo.GetAll().Returns(
        [
            // new Budget
            // {
            //     YearMonth = "202410",
            //     Amount = 6200
            // },
            new Budget
            {
                YearMonth = "202411",
                Amount = 3000
            },
            new Budget
            {
                YearMonth = "202412",
                Amount = 3100
            },
            new Budget
            {
                YearMonth = "202501",
                Amount = 9300
            },
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2025, 1, 1));

        result.Should().Be(6400m);
    }
    
    [Test]
    public void Query_When_Db_Budget_Is_Zero_For_Start()
    {
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202410",
                Amount = 0
            },
            new Budget
            {
                YearMonth = "202411",
                Amount = 3000
            },
            new Budget
            {
                YearMonth = "202412",
                Amount = 3100
            },
            new Budget
            {
                YearMonth = "202501",
                Amount = 9300
            },
        ]);

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2025, 1, 1));

        result.Should().Be(6400m);
    }
}

public interface IBudgetRepo
{
    List<Budget> GetAll();
}

public class Budget
{
    public string YearMonth { get; set; }
    public int Amount { get; set; }
}

public class BudgetService(IBudgetRepo budgetRepo)
{
    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0m;
        }

        var days = end.Subtract(start).Days + 1;
        var budgets = budgetRepo.GetAll();

        if (start.Month == end.Month && start.Year == end.Year)
        {
            var budget = budgets.First(b => b.YearMonth == start.ToString("yyyyMM"));

            return (decimal)budget.Amount / DateTime.DaysInMonth(start.Year, start.Month) * days;
        }
        else
        {
            var monthKeys = new List<string>();

            var a = new DateTime(start.Year, start.Month, 1);
            var b = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

            while (a < b)
            {
                monthKeys.Add(a.ToString("yyyyMM"));
                a = a.AddMonths(1);
            }

            var targetBudgets = budgets.Where(bu => monthKeys.Contains(bu.YearMonth)).ToList();
            var totalBudget = 0m;

            foreach (var targetBudget in targetBudgets)
            {
                if (targetBudget.YearMonth == start.ToString("yyyyMM"))
                {
                    var daysInStart = DateTime.DaysInMonth(start.Year, start.Month);
                    totalBudget += (decimal)(daysInStart - start.Day + 1) * targetBudget.Amount / daysInStart;
                }

                else if (targetBudget.YearMonth == end.ToString("yyyyMM"))
                {
                    var daysInEnd = DateTime.DaysInMonth(end.Year, end.Month);
                    totalBudget += (decimal)end.Day * targetBudget.Amount / daysInEnd;
                }
                else
                {
                    totalBudget += targetBudget.Amount;
                }
            }

            return totalBudget;
        }
    }
}