using FluentValidation;

public sealed class UpsertStaffLunchBreakValidator
    : AbstractValidator<UpsertStaffLunchBreakCommand>
{
    public UpsertStaffLunchBreakValidator()
    {
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);

        RuleFor(x => x.Body.StartTime)
            .Matches(@"^\d{2}:\d{2}$");

        RuleFor(x => x.Body.EndTime)
            .Matches(@"^\d{2}:\d{2}$");
    }
}