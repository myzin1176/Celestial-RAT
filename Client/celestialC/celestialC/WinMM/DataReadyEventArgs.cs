using System;

namespace WinMM
{
    // Token: 0x02000015 RID: 21
    public class DataReadyEventArgs : EventArgs
    {
        // Token: 0x060000D8 RID: 216 RVA: 0x00004F40 File Offset: 0x00003140
        public DataReadyEventArgs(byte[] data)
        {
            this.data = data;
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x060000D9 RID: 217 RVA: 0x00004F50 File Offset: 0x00003150
        public byte[] Data
        {
            get
            {
                return this.data;
            }
        }

        // Token: 0x04000066 RID: 102
        private byte[] data;
    }
}
