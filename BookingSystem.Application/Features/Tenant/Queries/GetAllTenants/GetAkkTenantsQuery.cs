using Booking.Domain.Entities;
using MediatR;

public record GetAllTenantsQuery() : IRequest<List<Tenant>>;