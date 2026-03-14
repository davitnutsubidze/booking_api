using Booking.Domain.Entities;
using Booking.Domain.Enums;
using BookingSystem.Application.Interfaces;
using MediatR;
using BookingSystem.Application.Common.Exceptions;

namespace BookingSystem.Application.Features.Appointments.Commands.CreateAppointment;

public sealed class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _appointments;
    private readonly ICustomerRepository _customers;
    private readonly IServiceRepository _services;
    private readonly IUserRepository _users;
    private readonly ICustomerTenantRepository _customerTenants;
    private readonly IUnitOfWork _uow;

    public CreateAppointmentHandler(
        IAppointmentRepository appointments,
        ICustomerRepository customers,
        IUserRepository users,
        IServiceRepository services,
        ICustomerTenantRepository customerTenant,
        IUnitOfWork uow)
    {
        _appointments = appointments;
        _customers = customers;
        _services = services;
        _users = users;
        _customerTenants = customerTenant;
        _uow = uow;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken ct)
    {
        // 1) Service -> duration
        var service = await _services.GetByIdAsync(request.ServiceId, ct)
                      ?? throw new NotFoundException("Service not found.");

        if (service.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        var start = request.StartDateTimeUtc;
        var end = start.AddMinutes(service.DurationMinutes);

        // 2) overlap check
        var hasOverlap = await _appointments.HasOverlapAsync(request.StaffId, start, end, ct);
        if (hasOverlap)
            throw new InvalidOperationException("Selected time slot is not available.");

        // 3) customer (find by phone, else create)
        var customer = await _customers.FindByPhoneAsync(request.TenantId, request.CustomerPhone, ct);

        if (customer is null)
        {
            // 3.1) ჯერ user ვიპოვოთ tenant-ის ფარგლებში
            var user = await _users.FindByTenantAndEmailOrPhoneAsync(
                request.TenantId,
                request.CustomerEmail,
                request.CustomerPhone,
                ct);

            // 3.2) თუ user არ არსებობს -> შევქმნათ
            if (user is null)
            {
                user = new User
                {
                    TenantId = request.TenantId,
                    Phone = request.CustomerPhone,
                    Email = request.CustomerEmail ?? string.Empty,
                    PasswordHash = "12345678",
                    Role = UserRole.Customer,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _users.AddAsync(user, ct);
                await _uow.SaveChangesAsync(ct); // რომ user.Id მივიღოთ
            }

            // 3.3) customer შევქმნათ user-ზე მიბმით
            customer = new Customer
            {
                UserId = user.Id,
                FirstName = request.CustomerFirstName,
                LastName = request.CustomerLastName,
                Phone = request.CustomerPhone,
                Email = request.CustomerEmail,
                CreatedAt = DateTime.UtcNow
            };

            await _customers.AddAsync(customer, ct);
            await _uow.SaveChangesAsync(ct); // რომ customer.Id მივიღოთ


        }
        // 4) CustomerTenant კავშირიც შევქმნათ თუ არ არსებობს
        var customerTenantExists = await _customerTenants.ExistsAsync(customer.Id, request.TenantId, ct);

        if (!customerTenantExists)
        {
            var customerTenant = new CustomerTenant
            {
                CustomerId = customer.Id,
                TenantId = request.TenantId,
                FirstVisitAt = DateTime.UtcNow,
                LastVisitAt = DateTime.UtcNow,
                IsBlocked = false,
                IsDeleted = false
            };

            await _customerTenants.AddAsync(customerTenant, ct);
            await _uow.SaveChangesAsync(ct);
        }
        // 5) create appointment
        var appointment = new Appointment
        {
            TenantId = request.TenantId,
            ServiceId = request.ServiceId,
            StaffId = request.StaffId,
            CustomerId = customer.Id,
            StartDateTime = start,
            EndDateTime = end,
            Status = AppointmentStatus.Pending,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _appointments.AddAsync(appointment, ct);
        await _uow.SaveChangesAsync(ct);

        return appointment.Id;
    }
}
