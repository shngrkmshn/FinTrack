using FinTrackPro.Domain.Enums;
using FinTrackPro.Domain.ValueObjects;

namespace FinTrackPro.Domain.Entities;

public class RecurrenceSchedule
{
    public Guid Id { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public Money Amount { get; private set; }
    public string Description { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public RecurrencePeriod RecurrencePeriod { get; private set; }
    public DateTime? LastGeneratedDate { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public bool IsIncome => TransactionType == TransactionType.Income;
    public bool IsExpense => TransactionType == TransactionType.Expense;

    private RecurrenceSchedule()
    {
        Description = string.Empty;
        Amount = null!;
        RecurrencePeriod = null!;
    }

    public RecurrenceSchedule(
        Guid id,
        TransactionType transactionType,
        Money amount,
        string description,
        Guid accountId,
        Guid categoryId,
        Guid userId,
        DateTime startDate,
        RecurrencePeriod recurrencePeriod,
        DateTime? endDate = null)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Recurrence schedule id cannot be empty.", nameof(id));
        }

        if (transactionType == TransactionType.Transfer)
        {
            throw new ArgumentException(
                "Recurrence schedules do not support Transfer type. Use Income or Expense.",
                nameof(transactionType));
        }

        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account id cannot be empty.", nameof(accountId));
        }

        if (categoryId == Guid.Empty)
        {
            throw new ArgumentException("Category id cannot be empty.", nameof(categoryId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        ValidateAmount(amount);
        ValidateDescription(description);

        if (recurrencePeriod == null)
        {
            throw new ArgumentNullException(nameof(recurrencePeriod), "Recurrence period cannot be null.");
        }

        if (endDate.HasValue && endDate.Value.Date < startDate.Date)
        {
            throw new ArgumentException("End date must be greater than or equal to start date.", nameof(endDate));
        }

        Id = id;
        TransactionType = transactionType;
        Amount = amount;
        Description = description.Trim();
        AccountId = accountId;
        CategoryId = categoryId;
        UserId = userId;
        StartDate = startDate.Date;
        EndDate = endDate?.Date;
        RecurrencePeriod = recurrencePeriod;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static RecurrenceSchedule CreateIncome(
        Money amount,
        string description,
        Guid accountId,
        Guid categoryId,
        Guid userId,
        DateTime startDate,
        RecurrencePeriod recurrencePeriod,
        DateTime? endDate = null)
    {
        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Income amount must be positive.", nameof(amount));
        }

        return new RecurrenceSchedule(
            Guid.NewGuid(),
            TransactionType.Income,
            amount,
            description,
            accountId,
            categoryId,
            userId,
            startDate,
            recurrencePeriod,
            endDate);
    }

    public static RecurrenceSchedule CreateExpense(
        Money amount,
        string description,
        Guid accountId,
        Guid categoryId,
        Guid userId,
        DateTime startDate,
        RecurrencePeriod recurrencePeriod,
        DateTime? endDate = null)
    {
        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Expense amount must be positive.", nameof(amount));
        }

        return new RecurrenceSchedule(
            Guid.NewGuid(),
            TransactionType.Expense,
            amount,
            description,
            accountId,
            categoryId,
            userId,
            startDate,
            recurrencePeriod,
            endDate);
    }

    public Transaction GenerateTransaction(DateTime occurrenceDate)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Cannot generate a transaction from an inactive schedule.");
        }

        if (occurrenceDate.Date < StartDate)
        {
            throw new ArgumentException(
                "Occurrence date cannot be before the schedule start date.",
                nameof(occurrenceDate));
        }

        if (EndDate.HasValue && occurrenceDate.Date > EndDate.Value)
        {
            throw new ArgumentException(
                "Occurrence date cannot be after the schedule end date.",
                nameof(occurrenceDate));
        }

        if (TransactionType == TransactionType.Income)
        {
            return Transaction.CreateIncome(
                Amount,
                occurrenceDate,
                Description,
                AccountId,
                UserId,
                CategoryId,
                recurrenceScheduleId: Id);
        }

        return Transaction.CreateExpense(
            Amount,
            occurrenceDate,
            Description,
            AccountId,
            UserId,
            CategoryId,
            recurrenceScheduleId: Id);
    }

    public void MarkAsGenerated(DateTime date)
    {
        LastGeneratedDate = date;
        UpdatedAt = DateTime.UtcNow;
    }

    public DateTime CalculateNextOccurrence(DateTime fromDate)
    {
        if (fromDate.Date < StartDate)
        {
            return StartDate;
        }

        return RecurrencePeriod.CalculateNextOccurrence(fromDate);
    }

    public IEnumerable<DateTime> GetOccurrencesBetween(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before or equal to end date.", nameof(startDate));
        }

        var occurrences = new List<DateTime>();
        var effectiveEnd = EndDate.HasValue && EndDate.Value < endDate.Date ? EndDate.Value : endDate.Date;
        var currentDate = StartDate;

        while (currentDate <= effectiveEnd)
        {
            if (currentDate >= startDate.Date)
            {
                occurrences.Add(currentDate);
            }

            currentDate = RecurrencePeriod.CalculateNextOccurrence(currentDate);
        }

        return occurrences;
    }

    public void Activate()
    {
        if (IsActive)
        {
            throw new InvalidOperationException("Recurrence schedule is already active.");
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Recurrence schedule is already deactivated.");
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string newDescription)
    {
        ValidateDescription(newDescription);

        Description = newDescription.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAmount(Money newAmount)
    {
        ValidateAmount(newAmount);

        Amount = newAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRecurrencePeriod(RecurrencePeriod newRecurrencePeriod)
    {
        if (newRecurrencePeriod == null)
        {
            throw new ArgumentNullException(nameof(newRecurrencePeriod), "Recurrence period cannot be null.");
        }

        RecurrencePeriod = newRecurrencePeriod;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEndDate(DateTime? newEndDate)
    {
        if (newEndDate.HasValue && newEndDate.Value.Date < StartDate)
        {
            throw new ArgumentException("End date must be greater than or equal to start date.", nameof(newEndDate));
        }

        EndDate = newEndDate?.Date;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCategory(Guid newCategoryId)
    {
        if (newCategoryId == Guid.Empty)
        {
            throw new ArgumentException("Category id cannot be empty.", nameof(newCategoryId));
        }

        CategoryId = newCategoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Recurrence schedule description cannot be empty.", nameof(description));
        }

        if (description.Length > 500)
        {
            throw new ArgumentException(
                "Recurrence schedule description cannot exceed 500 characters.",
                nameof(description));
        }
    }

    private static void ValidateAmount(Money amount)
    {
        if (amount == null)
        {
            throw new ArgumentNullException(nameof(amount), "Amount cannot be null.");
        }

        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Recurrence schedule amount must be positive.", nameof(amount));
        }
    }
}
