﻿using System.Runtime.InteropServices;

namespace IdentityService.Model
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CustomClaimTypes
    {
        public const string Role = "Role";
    }
}