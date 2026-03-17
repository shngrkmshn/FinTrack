using FinTrackPro.Domain.ValueObjects;

namespace FinTrackPro.Domain.Entities;

public sealed class Budget
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid UserId { get; private set; }
    public Money Amount { get; private set; }
    public DateRange Period { get; private set; }
    public Money SpentAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // For EF Core
    private Budget()
    {
        Name = string.Empty;
        Amount = null!;
        Period = null!;
        SpentAmount = null!;
    }

    public Budget(
        Guid id,
        string name,
        Guid categoryId,
        Guid userId,
        Money amount,
        DateRange period)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Budget id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Budget name cannot be empty.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Budget name cannot exceed 100 characters.", nameof(name));

        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category id cannot be empty.", nameof(categoryId));

        if (userId == Guid.Empty)
            throw new ArgumentException("User id cannot be empty.", nameof(userId));

        if (amount.Amount <= 0)
            throw new ArgumentException("Budget amount must be greater than zero.", nameof(amount));

        Id = id;
        Name = name.Trim();
        CategoryId = categoryId;
        UserId = userId;
        Amount = amount;
        Period = period;
        SpentAmount = Money.Zero(amount.Currency);
        CreatedAt = DateTime.UtcNow;
    }

    public static Budget Create(
        string name,
        Guid categoryId,
        Guid userId,
        Money amount,
        DateRange period)
    {
        return new Budget(Guid.NewGuid(), name, categoryId, userId, amount, period);
    }

    public void RecordSpending(Money spending)
    {
        if (spending.Currency != Amount.Currency)
            throw new InvalidOperationException(
                $"Cannot record {spending.Currency} spending against {Amount.Currency} budget.");

        SpentAmount = SpentAmount.Add(spending);
        UpdatedAt = DateTime.UtcNow;
    }
}
