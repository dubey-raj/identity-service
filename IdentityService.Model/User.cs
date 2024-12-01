namespace IdentityService.Model
{
    public class User
    {
        public string UserName { get; set; } = string.Empty;
        public string[] Roles { get; set; } = [];
    }
}
