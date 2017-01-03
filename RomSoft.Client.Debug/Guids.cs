// Guids.cs
// MUST match guids.h

namespace RomSoft.Client.Debug
{
    using System;

    static class GuidList
    {
        public const string guidRomSoft_Client_DebugPkgString = "99a8ee3a-13db-4bea-b139-af41a28b9caa";
        public const string guidRomSoft_Client_DebugCmdSetString = "4a87a1ef-776d-4502-a7f5-68bb34db4114";
        public const string guidToolWindowPersistanceString = "37352c55-6861-4212-a9d3-51af10e7dc1f";

        public static readonly Guid guidRomSoft_Client_DebugCmdSet = new Guid(guidRomSoft_Client_DebugCmdSetString);
    };
}