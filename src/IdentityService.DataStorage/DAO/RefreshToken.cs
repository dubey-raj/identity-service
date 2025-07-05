using System;
using System.Collections.Generic;

namespace IdentityService.DataStorage.DAO;

public partial class RefreshToken
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual User? User { get; set; }
}
