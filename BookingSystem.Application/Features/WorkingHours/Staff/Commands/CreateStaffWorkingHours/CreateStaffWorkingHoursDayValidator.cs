using FluentValidation;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.CreateStaffWorkingHoursDay;

public sealed class CreateStaffWorkingHoursDayValidator : AbstractValidator<CreateStaffWorkingHoursDayCommand>
{
    public CreateStaffWorkingHoursDayValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.Body.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.Body.StartTime).Matches(@"^\d{2}:\d{2}$");
        RuleFor(x => x.Body.EndTime).Matches(@"^\d{2}:\d{2}$");
    }
}