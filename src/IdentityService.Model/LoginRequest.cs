namespace IdentityService.Model
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public string GrantType {  get; set; } = AppConstants.GrantType_Password;
    }
}
