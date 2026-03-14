using Booking.Domain.Entities;
using Booking.Domain.Enums;
using BookingSystem.Application.DTOs.Tenant;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BookingSystem.Application.Features.Tenants.Commands.CreateTenant;

public sealed class CreateTenantHandler
    : IRequestHandler<CreateTenantCommand, TenantDto>
{
    private readonly ITenantRepository _repo;
    private readonly IUserRepository _userRepository;
    private readonly IWorkingHoursCrudRepository _workingHoursCrudRepository;
    private readonly IUnitOfWork _uow;

    public CreateTenantHandler(ITenantRepository repo, IUserRepository userRepository, IWorkingHoursCrudRepository workingHoursCrudRepository,
    IUnitOfWork uow)
    {
        _repo = repo;
        _userRepository = userRepository;
        _workingHoursCrudRepository = workingHoursCrudRepository;
        _uow = uow;
    }

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken ct)
    {
        // მერე დავამტებ სლაგის შემოწმებას
        var slugExists = await _repo.ExistsBySlugAsync(request.Slug, ct);
        if (slugExists)
            throw new ValidationException("Slug already exists.");

        var tenant = new Booking.Domain.Entities.Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            TimeZone = "Asia/Tbilisi",
            IsActive = true
        };

        var user = new User
        {
            Tenant = tenant,
            Phone = request.Phone,
            Email = request.Email,
            PasswordHash = "12345678",
            Role = UserRole.Tenant,
            IsActive = true
        };

        var workingHours = new List<TenantWorkingHours>
        {
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Sunday,    StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = true  },
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Monday,    StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = false },
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Tuesday,   StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = false },
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = false },
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Thursday,  StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = false },
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Friday,    StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = false },
            new() { Id = Guid.NewGuid(), TenantId = tenant.Id, DayOfWeek = DayOfWeek.Saturday,  StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(18, 0), IsClosed = true  }
        };

        await _repo.AddAsync(tenant, ct);
        await _userRepository.AddAsync(user, ct);
        await _workingHoursCrudRepository.ReplaceTenantWeekAsync(tenant.Id, workingHours, ct);
        await _uow.SaveChangesAsync(ct);

        return new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.Description,
            tenant.Phone,
            tenant.Email,
            tenant.Address,
            tenant.TimeZone,
            tenant.IsActive);
    }
}