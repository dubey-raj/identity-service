using System;
using System.Collections.Generic;

namespace IdentityService.DataStorage.DAL;

public partial class Client
{
    public int Id { get; set; }

    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string? ClientName { get; set; }

    public virtual ICollection<ClientScope> ClientScopes { get; set; } = new List<ClientScope>();
}
