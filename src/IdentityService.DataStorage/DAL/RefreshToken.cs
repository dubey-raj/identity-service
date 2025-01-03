using System;
using System.Collections.Generic;

namespace IdentityService.DataStorage.DAL;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual User? User { get; set; }
}
