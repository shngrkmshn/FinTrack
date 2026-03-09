using FinTrackPro.Domain.Enums;
using FinTrackPro.Domain.ValueObjects;

namespace FinTrackPro.Tests.Domain.Entities;

public class RecurrenceSchedule
{
    private static readonly FinTrackPro.Domain.ValueObjects.Money ValidAmount =
        new FinTrackPro.Domain.ValueObjects.Money(100m, Currency.USD);

    private static readonly RecurrencePeriod ValidPeriod = RecurrencePeriod.Monthly();

    private static readonly Guid ValidAccountId = Guid.NewGuid();
    private static readonly Guid ValidCategoryId = Guid.NewGuid();
    private static readonly Guid ValidUserId = Guid.NewGuid();
    private static readonly DateTime ValidStartDate = new DateTime(2026, 1, 1);

    // ── Constructor / factory validation ──────────────────────────────────────

    [Fact]
    public void Constructor_WithTransferType_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Transfer,
                ValidAmount,
                "desc",
                ValidAccountId,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithEmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.Empty,
                TransactionType.Income,
                ValidAmount,
                "desc",
                ValidAccountId,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithEmptyAccountId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                ValidAmount,
                "desc",
                Guid.Empty,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithEmptyCategoryId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                ValidAmount,
                "desc",
                ValidAccountId,
                Guid.Empty,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithEmptyUserId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                ValidAmount,
                "desc",
                ValidAccountId,
                ValidCategoryId,
                Guid.Empty,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithZeroAmount_Throws()
    {
        var zeroAmount = new FinTrackPro.Domain.ValueObjects.Money(0m, Currency.USD);

        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                zeroAmount,
                "desc",
                ValidAccountId,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithEndDateBeforeStartDate_Throws()
    {
        var endDate = ValidStartDate.AddDays(-1);

        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                ValidAmount,
                "desc",
                ValidAccountId,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod,
                endDate));
    }

    [Fact]
    public void Constructor_WithEmptyDescription_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                ValidAmount,
                "   ",
                ValidAccountId,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void Constructor_WithDescriptionOver500Chars_Throws()
    {
        var longDescription = new string('x', 501);

        Assert.Throws<ArgumentException>(() =>
            new FinTrackPro.Domain.Entities.RecurrenceSchedule(
                Guid.NewGuid(),
                TransactionType.Income,
                ValidAmount,
                longDescription,
                ValidAccountId,
                ValidCategoryId,
                ValidUserId,
                ValidStartDate,
                ValidPeriod));
    }

    [Fact]
    public void CreateIncome_WithValidParameters_CreatesActiveIncomeSchedule()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Equal(TransactionType.Income, schedule.TransactionType);
        Assert.True(schedule.IsActive);
        Assert.True(schedule.IsIncome);
        Assert.False(schedule.IsExpense);
        Assert.Null(schedule.EndDate);
        Assert.Null(schedule.LastGeneratedDate);
    }

    [Fact]
    public void CreateExpense_WithValidParameters_CreatesActiveExpenseSchedule()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateExpense(
            ValidAmount, "Rent", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Equal(TransactionType.Expense, schedule.TransactionType);
        Assert.True(schedule.IsActive);
        Assert.True(schedule.IsExpense);
        Assert.False(schedule.IsIncome);
    }

    // ── GenerateTransaction ────────────────────────────────────────────────────

    [Fact]
    public void GenerateTransaction_ForIncomeSchedule_ReturnsIncomeTransaction()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        var transaction = schedule.GenerateTransaction(ValidStartDate);

        Assert.Equal(TransactionType.Income, transaction.TransactionType);
        Assert.Equal(ValidAmount, transaction.Amount);
        Assert.Equal(schedule.Id, transaction.RecurrenceScheduleId);
        Assert.Equal(ValidAccountId, transaction.AccountId);
        Assert.Equal(ValidCategoryId, transaction.CategoryId);
        Assert.Equal(ValidUserId, transaction.UserId);
    }

    [Fact]
    public void GenerateTransaction_ForExpenseSchedule_ReturnsExpenseTransaction()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateExpense(
            ValidAmount, "Rent", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        var transaction = schedule.GenerateTransaction(ValidStartDate);

        Assert.Equal(TransactionType.Expense, transaction.TransactionType);
        Assert.Equal(schedule.Id, transaction.RecurrenceScheduleId);
    }

    [Fact]
    public void GenerateTransaction_WhenScheduleIsInactive_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);
        schedule.Deactivate();

        Assert.Throws<InvalidOperationException>(() => schedule.GenerateTransaction(ValidStartDate));
    }

    [Fact]
    public void GenerateTransaction_WithDateBeforeStartDate_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Throws<ArgumentException>(() => schedule.GenerateTransaction(ValidStartDate.AddDays(-1)));
    }

    [Fact]
    public void GenerateTransaction_WithDateAfterEndDate_Throws()
    {
        var endDate = ValidStartDate.AddMonths(3);
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod, endDate);

        Assert.Throws<ArgumentException>(() => schedule.GenerateTransaction(endDate.AddDays(1)));
    }

    [Fact]
    public void GenerateTransaction_DoesNotUpdateLastGeneratedDate()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        schedule.GenerateTransaction(ValidStartDate);

        Assert.Null(schedule.LastGeneratedDate);
    }

    // ── MarkAsGenerated ────────────────────────────────────────────────────────

    [Fact]
    public void MarkAsGenerated_UpdatesLastGeneratedDateAndUpdatedAt()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        schedule.MarkAsGenerated(ValidStartDate);

        Assert.Equal(ValidStartDate, schedule.LastGeneratedDate);
        Assert.NotNull(schedule.UpdatedAt);
    }

    // ── GetOccurrencesBetween ──────────────────────────────────────────────────

    [Fact]
    public void GetOccurrencesBetween_MonthlySchedule_ReturnsCorrectDates()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        var occurrences = schedule.GetOccurrencesBetween(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 3, 31)).ToList();

        Assert.Equal(3, occurrences.Count);
        Assert.Contains(new DateTime(2026, 1, 1), occurrences);
        Assert.Contains(new DateTime(2026, 2, 1), occurrences);
        Assert.Contains(new DateTime(2026, 3, 1), occurrences);
    }

    [Fact]
    public void GetOccurrencesBetween_RespectsScheduleEndDate()
    {
        var endDate = new DateTime(2026, 2, 1);
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod, endDate);

        var occurrences = schedule.GetOccurrencesBetween(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 6, 30)).ToList();

        Assert.Equal(2, occurrences.Count);
        Assert.Contains(new DateTime(2026, 1, 1), occurrences);
        Assert.Contains(new DateTime(2026, 2, 1), occurrences);
    }

    [Fact]
    public void GetOccurrencesBetween_WithStartAfterEnd_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Throws<ArgumentException>(() =>
            schedule.GetOccurrencesBetween(new DateTime(2026, 3, 1), new DateTime(2026, 1, 1)));
    }

    // ── Activate / Deactivate ──────────────────────────────────────────────────

    [Fact]
    public void Deactivate_WhenActive_DeactivatesSchedule()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        schedule.Deactivate();

        Assert.False(schedule.IsActive);
        Assert.NotNull(schedule.UpdatedAt);
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);
        schedule.Deactivate();

        Assert.Throws<InvalidOperationException>(() => schedule.Deactivate());
    }

    [Fact]
    public void Activate_WhenInactive_ActivatesSchedule()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);
        schedule.Deactivate();

        schedule.Activate();

        Assert.True(schedule.IsActive);
    }

    [Fact]
    public void Activate_WhenAlreadyActive_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Throws<InvalidOperationException>(() => schedule.Activate());
    }

    // ── Update methods ────────────────────────────────────────────────────────

    [Fact]
    public void UpdateEndDate_WithValidDate_UpdatesEndDate()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);
        var newEndDate = ValidStartDate.AddYears(1);

        schedule.UpdateEndDate(newEndDate);

        Assert.Equal(newEndDate.Date, schedule.EndDate);
    }

    [Fact]
    public void UpdateEndDate_ToNull_ClearsEndDate()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod,
            ValidStartDate.AddMonths(6));

        schedule.UpdateEndDate(null);

        Assert.Null(schedule.EndDate);
    }

    [Fact]
    public void UpdateEndDate_WithDateBeforeStartDate_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Throws<ArgumentException>(() => schedule.UpdateEndDate(ValidStartDate.AddDays(-1)));
    }

    [Fact]
    public void UpdateCategory_WithEmptyGuid_Throws()
    {
        var schedule = FinTrackPro.Domain.Entities.RecurrenceSchedule.CreateIncome(
            ValidAmount, "Salary", ValidAccountId, ValidCategoryId, ValidUserId, ValidStartDate, ValidPeriod);

        Assert.Throws<ArgumentException>(() => schedule.UpdateCategory(Guid.Empty));
    }
}
