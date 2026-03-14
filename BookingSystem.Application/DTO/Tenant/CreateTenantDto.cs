using System;
using System.Collections.Generic;
using System.Text;

public sealed record CreateTenantDto(
    string Name,
    string? Description,
    string Phone,
    string Email,
    string Address
);
