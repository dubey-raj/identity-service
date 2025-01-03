using System;
using System.Collections.Generic;

namespace IdentityService.DataStorage.DAL;

public partial class UserToken
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool? IsRevoked { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public virtual User? User { get; set; }
}
