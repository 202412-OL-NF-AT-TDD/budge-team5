using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Budget_Team5;

public class BudgetServiceTests
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    [Test]
    public void InvalidStartEnd()
    {
        var result = _budgetService.Query(new DateTime(2024, 11, 30), new DateTime(2024, 11, 1));

        result.Should().Be(0m);
    }

    [Test]
    public void Query_Cross_More_Than_Two_Months()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
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
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2024, 12, 1));

        result.Should().Be(3300m);
    }


    [Test]
    public void Query_Cross_Two_Months()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
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
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2024, 11, 1));

        result.Should().Be(300m);
    }

    [Test]
    public void Query_Partial_Oct()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202410",
                    Amount = 6200
                }
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 20), new DateTime(2024, 10, 30));

        result.Should().Be(2200m);
    }

    [Test]
    public void Query_When_Db_Budget_Is_Zero_For_Start()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
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
                new Budget
                {
                    YearMonth = "202502",
                    Amount = 93000
                },
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2025, 1, 1));

        result.Should().Be(6400m);
    }

    [Test]
    public void Query_When_Db_No_Budget_For_Not_Start_And_Not_End()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
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
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2025, 1, 1));

        result.Should().Be(3500m);
    }

    [Test]
    public void Query_When_Db_No_Budget_For_Start()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
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
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 31), new DateTime(2025, 1, 1));

        result.Should().Be(6400m);
    }


    [Test]
    public void Query_Whole_Oct()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
                new Budget
                {
                    YearMonth = "202410",
                    Amount = 6200
                }
            });

        var result = _budgetService.Query(new DateTime(2024, 10, 01), new DateTime(2024, 10, 31));

        result.Should().Be(6200m);
    }

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }
}