using FinTrackPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrackPro.Infrastructure.Persistence.Configurations;

public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(budget => budget.Id);

        builder.Property(budget => budget.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(budget => budget.CategoryId)
            .IsRequired();

        builder.Property(budget => budget.UserId)
            .IsRequired();

        builder.OwnsOne(budget => budget.Amount, amountBuilder =>
        {
            amountBuilder.Property(money => money.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            amountBuilder.Property(money => money.Currency)
                .HasColumnName("Currency")
                .IsRequired();
        });

        builder.OwnsOne(budget => budget.SpentAmount, spentBuilder =>
        {
            spentBuilder.Property(money => money.Amount)
                .HasColumnName("SpentAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            spentBuilder.Property(money => money.Currency)
                .HasColumnName("SpentCurrency")
                .IsRequired();
        });

        builder.OwnsOne(budget => budget.Period, periodBuilder =>
        {
            periodBuilder.Property(dateRange => dateRange.StartDate)
                .HasColumnName("PeriodStartDate")
                .IsRequired();

            periodBuilder.Property(dateRange => dateRange.EndDate)
                .HasColumnName("PeriodEndDate")
                .IsRequired();
        });

        builder.Property(budget => budget.CreatedAt)
            .IsRequired();

        builder.Property(budget => budget.UpdatedAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(budget => budget.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(budget => budget.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
