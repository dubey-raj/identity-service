using System;
using System.Collections.Generic;

namespace IdentityService.DataStorage.DAO;

public partial class UserAddress
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
