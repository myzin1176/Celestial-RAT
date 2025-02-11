namespace WinMM
{
    // Token: 0x02000008 RID: 8
    public class WaveInDeviceCaps
    {
        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000064 RID: 100 RVA: 0x00004410 File Offset: 0x00002610
        // (set) Token: 0x06000065 RID: 101 RVA: 0x00004418 File Offset: 0x00002618
        public int DeviceId
        {
            get
            {
                return this.deviceId;
            }
            set
            {
                this.deviceId = value;
            }
        }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000066 RID: 102 RVA: 0x00004424 File Offset: 0x00002624
        // (set) Token: 0x06000067 RID: 103 RVA: 0x0000442C File Offset: 0x0000262C
        public string Manufacturer
        {
            get
            {
                return this.manufacturer;
            }
            set
            {
                this.manufacturer = value;
            }
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000068 RID: 104 RVA: 0x00004438 File Offset: 0x00002638
        // (set) Token: 0x06000069 RID: 105 RVA: 0x00004440 File Offset: 0x00002640
        public int ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = value;
            }
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x0600006A RID: 106 RVA: 0x0000444C File Offset: 0x0000264C
        // (set) Token: 0x0600006B RID: 107 RVA: 0x00004454 File Offset: 0x00002654
        public int DriverVersion
        {
            get
            {
                return this.driverVersion;
            }
            set
            {
                this.driverVersion = value;
            }
        }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x0600006C RID: 108 RVA: 0x00004460 File Offset: 0x00002660
        // (set) Token: 0x0600006D RID: 109 RVA: 0x00004468 File Offset: 0x00002668
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x0600006E RID: 110 RVA: 0x00004474 File Offset: 0x00002674
        // (set) Token: 0x0600006F RID: 111 RVA: 0x0000447C File Offset: 0x0000267C
        public int Channels
        {
            get
            {
                return this.channels;
            }
            set
            {
                this.channels = value;
            }
        }

        // Token: 0x04000031 RID: 49
        private int deviceId;

        // Token: 0x04000032 RID: 50
        private string manufacturer;

        // Token: 0x04000033 RID: 51
        private int productId;

        // Token: 0x04000034 RID: 52
        private int driverVersion;

        // Token: 0x04000035 RID: 53
        private string name;

        // Token: 0x04000036 RID: 54
        private int channels;
    }
}
