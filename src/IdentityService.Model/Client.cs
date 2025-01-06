namespace IdentityService.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] Scopes { get; set; } = [];
    }
}
