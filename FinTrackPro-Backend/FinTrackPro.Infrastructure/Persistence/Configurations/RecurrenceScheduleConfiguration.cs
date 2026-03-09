using FinTrackPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrackPro.Infrastructure.Persistence.Configurations;

public class RecurrenceScheduleConfiguration : IEntityTypeConfiguration<RecurrenceSchedule>
{
    public void Configure(EntityTypeBuilder<RecurrenceSchedule> builder)
    {
        builder.HasKey(schedule => schedule.Id);

        builder.Property(schedule => schedule.TransactionType)
            .IsRequired();

        builder.OwnsOne(schedule => schedule.Amount, amountBuilder =>
        {
            amountBuilder.Property(money => money.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            amountBuilder.Property(money => money.Currency)
                .HasColumnName("Currency")
                .IsRequired();
        });

        builder.Property(schedule => schedule.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(schedule => schedule.AccountId)
            .IsRequired();

        builder.Property(schedule => schedule.CategoryId)
            .IsRequired();

        builder.Property(schedule => schedule.UserId)
            .IsRequired();

        builder.Property(schedule => schedule.StartDate)
            .IsRequired();

        builder.Property(schedule => schedule.EndDate);

        builder.OwnsOne(schedule => schedule.RecurrencePeriod, periodBuilder =>
        {
            periodBuilder.Property(period => period.RecurrenceType)
                .HasColumnName("RecurrenceType")
                .IsRequired();

            periodBuilder.Property(period => period.Interval)
                .HasColumnName("Interval")
                .IsRequired();
        });

        builder.Property(schedule => schedule.LastGeneratedDate);

        builder.Property(schedule => schedule.IsActive)
            .IsRequired();

        builder.Property(schedule => schedule.CreatedAt)
            .IsRequired();

        builder.Property(schedule => schedule.UpdatedAt);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(schedule => schedule.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(schedule => schedule.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(schedule => schedule.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
