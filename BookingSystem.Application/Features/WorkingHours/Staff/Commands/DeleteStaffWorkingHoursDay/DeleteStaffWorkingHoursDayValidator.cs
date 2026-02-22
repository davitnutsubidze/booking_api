using FluentValidation;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.DeleteStaffWorkingHoursDay;

public sealed class DeleteStaffWorkingHoursDayValidator : AbstractValidator<DeleteStaffWorkingHoursDayCommand>
{
    public DeleteStaffWorkingHoursDayValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
    }
}