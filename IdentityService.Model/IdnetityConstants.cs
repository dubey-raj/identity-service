using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Model
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CustomClaimTypes
    {
        public const string Role = "Role";
    }
}
