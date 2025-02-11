using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WinMM.Properties
{
    // Token: 0x02000010 RID: 16
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    internal class Resources
    {
        // Token: 0x060000D3 RID: 211 RVA: 0x00004EC8 File Offset: 0x000030C8
        internal Resources()
        {
        }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060000D4 RID: 212 RVA: 0x00004ED0 File Offset: 0x000030D0
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(Resources.resourceMan, null))
                {
                    ResourceManager resourceManager = new ResourceManager("WinMM.Properties.Resources", typeof(Resources).Assembly);
                    Resources.resourceMan = resourceManager;
                }
                return Resources.resourceMan;
            }
        }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x060000D5 RID: 213 RVA: 0x00004F18 File Offset: 0x00003118
        // (set) Token: 0x060000D6 RID: 214 RVA: 0x00004F20 File Offset: 0x00003120
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return Resources.resourceCulture;
            }
            set
            {
                Resources.resourceCulture = value;
            }
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x060000D7 RID: 215 RVA: 0x00004F28 File Offset: 0x00003128
        internal static string Devices
        {
            get
            {
                return Resources.ResourceManager.GetString("Devices", Resources.resourceCulture);
            }
        }

        // Token: 0x0400003E RID: 62
        private static ResourceManager resourceMan;

        // Token: 0x0400003F RID: 63
        private static CultureInfo resourceCulture;
    }
}
