using BookingSystem.Application.Interfaces;
using BookingSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // repositories
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        // unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // customers
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // services
        services.AddScoped<IServiceRepository, ServiceRepository>();


        // working hours 
        services.AddScoped<IWorkingHoursRepository, WorkingHoursRepository>();

        // working crud
        services.AddScoped<IWorkingHoursCrudRepository, WorkingHoursCrudRepository>();


        // block time crud
        services.AddScoped<IBlockedTimeCrudRepository, BlockedTimeCrudRepository>();


        // blocked time 
        services.AddScoped<IBlockedTimeRepository, BlockedTimeRepository>();

        // staff
        services.AddScoped<IStaffRepository, StaffRepository>();

        // service crud
        services.AddScoped<IServiceCrudRepository, ServiceCrudRepository>();


        // staff service set
        services.AddScoped<IStaffServiceRepository, StaffServiceRepository>();


        services.AddScoped<IStaffWorkingHoursRepository, StaffWorkingHoursRepository>();



        return services;
    }
}
