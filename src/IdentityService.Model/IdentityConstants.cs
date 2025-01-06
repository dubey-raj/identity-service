using System.Runtime.InteropServices;

namespace IdentityService.Model
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CustomClaimTypes
    {
        public const string Role = "role";
        public const string RefreshToken = "refresh_token";
        public const string Scope = "role";
    }
}
