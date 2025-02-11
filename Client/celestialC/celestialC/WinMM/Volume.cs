namespace WinMM
{
    // Token: 0x0200000F RID: 15
    public struct Volume
    {
        // Token: 0x060000C9 RID: 201 RVA: 0x00004DEC File Offset: 0x00002FEC
        public Volume(float leftVolume, float rightVolume)
        {
            this.left = leftVolume;
            this.right = rightVolume;
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00004DFC File Offset: 0x00002FFC
        public Volume(float volume)
        {
            this.left = volume;
            this.right = volume;
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x060000CB RID: 203 RVA: 0x00004E0C File Offset: 0x0000300C
        // (set) Token: 0x060000CC RID: 204 RVA: 0x00004E14 File Offset: 0x00003014
        public float Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x060000CD RID: 205 RVA: 0x00004E20 File Offset: 0x00003020
        // (set) Token: 0x060000CE RID: 206 RVA: 0x00004E28 File Offset: 0x00003028
        public float Right
        {
            get
            {
                return this.right;
            }
            set
            {
                this.right = value;
            }
        }

        // Token: 0x060000CF RID: 207 RVA: 0x00004E34 File Offset: 0x00003034
        public static bool operator ==(Volume volume1, Volume volume2)
        {
            return volume1.Equals(volume2);
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00004E4C File Offset: 0x0000304C
        public static bool operator !=(Volume volume1, Volume volume2)
        {
            return !volume1.Equals(volume2);
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00004E64 File Offset: 0x00003064
        public override bool Equals(object obj)
        {
            if (obj == null || base.GetType() != obj.GetType())
            {
                return false;
            }
            Volume volume = (Volume)obj;
            return this.Left == volume.Left && this.Right == volume.Right;
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x00004EC4 File Offset: 0x000030C4
        public override int GetHashCode()
        {
            return 0;
        }

        // Token: 0x0400003C RID: 60
        private float left;

        // Token: 0x0400003D RID: 61
        private float right;
    }
}
