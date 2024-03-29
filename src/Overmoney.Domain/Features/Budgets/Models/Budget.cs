namespace Overmoney.Domain.Features.Budgets.Models;

public sealed class Budget
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int UserId { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public IEnumerable<BudgetLine> BudgetLines { get => _budgetLines; }

    private readonly List<BudgetLine> _budgetLines = [];

    public Budget(int id, int userId, string name, int year, int month, List<BudgetLine> budgetLines)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Year = year;
        Month = month;
        _budgetLines = budgetLines;
    }

    public Budget(int userId, string name, int year, int month, List<BudgetLine>? budgetLines = null)
    {
        Name = name;
        Year = year;
        UserId = userId;
        Month = month;
        _budgetLines = budgetLines ?? [];
    }

    public void Update(string name, int year, int month)
    {
        Name = name;
        Year = year;
        Month = month;
    }

    public void UpsertBudgetLine(BudgetLine budgetLine)
    {
        if (budgetLine.Id is 0)
        {
            _budgetLines.Add(budgetLine);
            return;
        }

        var line = _budgetLines.FirstOrDefault(x => x.Id == budgetLine.Id);

        if (line is null)
        {
            _budgetLines.Add(budgetLine);
            return;
        }

        line.Update(budgetLine.Amount);
    }

    public void RemoveBudgetLine(long id)
    {
        var line = _budgetLines.FirstOrDefault(x => x.Id == id);

        if (line is not null)
        {
            _budgetLines.Remove(line);
        }
    }

    public void RemoveBudgetLine(BudgetLine budgetLine)
    {
        _budgetLines.Remove(budgetLine);
    }
}
