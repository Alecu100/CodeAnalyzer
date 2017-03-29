/***************************************************************************
 
Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;

namespace CodeEvaluator.Packages.Vs2015
{
    static class GuidList
    {
        public const string guidComboBoxPkgString = "40d9f297-25fb-4264-99ed-7785f8331c94";
        public const string guidComboBoxCmdSetString = "B2C8E135-0E7A-4696-963E-BD3280F8578C";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Guid guidComboBoxPkg = new Guid(guidComboBoxPkgString);
        public static readonly Guid guidComboBoxCmdSet = new Guid(guidComboBoxCmdSetString);

        public const string guidRomSoft_Client_DebugPkgString = "99a8ee3a-13db-4bea-b139-af41a28b9caa";
        public const string guidRomSoft_Client_DebugCmdSetString = "4a87a1ef-776d-4502-a7f5-68bb34db4114";
        public const string guidToolWindowPersistanceString = "37352c55-6861-4212-a9d3-51af10e7dc1f";

        public static readonly Guid guidRomSoft_Client_DebugCmdSet = new Guid(guidRomSoft_Client_DebugCmdSetString);
    };
}