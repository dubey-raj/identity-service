namespace IdentityService.Model
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = RequestMode.Password;
        public string Mode {  get; set; } = string.Empty;
    }
}
