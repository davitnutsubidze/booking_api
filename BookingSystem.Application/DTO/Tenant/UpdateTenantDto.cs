using System;
using System.Collections.Generic;
using System.Text;

public sealed record UpdateTenantDto(
    string Name,
    string Slug,
    string? Description,
    string Phone,
    string Email,
    string Address,
    string TimeZone,
    bool IsActive
);

