﻿using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using Overmoney.Domain.Features.Users.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Budgets.Models;

[JsonConverter(typeof(IntIdentityJsonConverter))]
public sealed class BudgetId : Identity<int>
{
    public BudgetId(int id) : base(id)
    { }
}

public sealed class Budget
{
    public BudgetId? Id { get; private set; }
    public string Name { get; private set; } = null!;
    public UserProfileId UserId { get; private set; } = null!;
    public int Year { get; private set; }
    public int Month { get; private set; }
    public IEnumerable<BudgetLine> BudgetLines { get => _budgetLines; }

    private readonly List<BudgetLine> _budgetLines = [];

    public Budget(BudgetId id, UserProfileId userId, string name, int year, int month, List<BudgetLine> budgetLines)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Year = year;
        Month = month;
        _budgetLines = budgetLines;
    }

    public Budget(UserProfileId userId, string name, int year, int month, List<BudgetLine>? budgetLines = null)
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
        if (budgetLine.Id == new BudgetLineId())
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

    public void RemoveBudgetLine(BudgetLineId id)
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
