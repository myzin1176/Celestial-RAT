namespace WinMM
{
    // Token: 0x0200000A RID: 10
    public class WaveFormat
    {
        // Token: 0x17000021 RID: 33
        // (get) Token: 0x06000091 RID: 145 RVA: 0x0000449C File Offset: 0x0000269C
        public static WaveFormat Pcm44Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 44100,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x06000092 RID: 146 RVA: 0x000044D8 File Offset: 0x000026D8
        public static WaveFormat Pcm44Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 44100,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x06000093 RID: 147 RVA: 0x00004514 File Offset: 0x00002714
        public static WaveFormat Pcm44Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 44100,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x06000094 RID: 148 RVA: 0x0000454C File Offset: 0x0000274C
        public static WaveFormat Pcm44Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 44100,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x06000095 RID: 149 RVA: 0x00004584 File Offset: 0x00002784
        public static WaveFormat Pcm32Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 32000,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000026 RID: 38
        // (get) Token: 0x06000096 RID: 150 RVA: 0x000045C0 File Offset: 0x000027C0
        public static WaveFormat Pcm32Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 32000,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000027 RID: 39
        // (get) Token: 0x06000097 RID: 151 RVA: 0x000045FC File Offset: 0x000027FC
        public static WaveFormat Pcm32Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 32000,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x06000098 RID: 152 RVA: 0x00004634 File Offset: 0x00002834
        public static WaveFormat Pcm32Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 32000,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x06000099 RID: 153 RVA: 0x0000466C File Offset: 0x0000286C
        public static WaveFormat Pcm24Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 24000,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x0600009A RID: 154 RVA: 0x000046A8 File Offset: 0x000028A8
        public static WaveFormat Pcm24Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 24000,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x0600009B RID: 155 RVA: 0x000046E4 File Offset: 0x000028E4
        public static WaveFormat Pcm24Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 24000,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x0600009C RID: 156 RVA: 0x0000471C File Offset: 0x0000291C
        public static WaveFormat Pcm24Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 24000,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x0600009D RID: 157 RVA: 0x00004754 File Offset: 0x00002954
        public static WaveFormat Pcm22Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 22050,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x1700002E RID: 46
        // (get) Token: 0x0600009E RID: 158 RVA: 0x00004790 File Offset: 0x00002990
        public static WaveFormat Pcm22Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 22050,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x1700002F RID: 47
        // (get) Token: 0x0600009F RID: 159 RVA: 0x000047CC File Offset: 0x000029CC
        public static WaveFormat Pcm22Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 22050,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000030 RID: 48
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x00004804 File Offset: 0x00002A04
        public static WaveFormat Pcm22Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 22050,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x060000A1 RID: 161 RVA: 0x0000483C File Offset: 0x00002A3C
        public static WaveFormat Pcm16Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 16000,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x060000A2 RID: 162 RVA: 0x00004878 File Offset: 0x00002A78
        public static WaveFormat Pcm16Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 16000,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x060000A3 RID: 163 RVA: 0x000048B4 File Offset: 0x00002AB4
        public static WaveFormat Pcm16Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 16000,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x060000A4 RID: 164 RVA: 0x000048EC File Offset: 0x00002AEC
        public static WaveFormat Pcm16Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 16000,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x060000A5 RID: 165 RVA: 0x00004924 File Offset: 0x00002B24
        public static WaveFormat Pcm12Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 12000,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x060000A6 RID: 166 RVA: 0x00004960 File Offset: 0x00002B60
        public static WaveFormat Pcm12Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 12000,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x060000A7 RID: 167 RVA: 0x0000499C File Offset: 0x00002B9C
        public static WaveFormat Pcm12Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 12000,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x060000A8 RID: 168 RVA: 0x000049D4 File Offset: 0x00002BD4
        public static WaveFormat Pcm12Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 12000,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x060000A9 RID: 169 RVA: 0x00004A0C File Offset: 0x00002C0C
        public static WaveFormat Pcm11Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 11025,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x060000AA RID: 170 RVA: 0x00004A48 File Offset: 0x00002C48
        public static WaveFormat Pcm11Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 11025,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x060000AB RID: 171 RVA: 0x00004A84 File Offset: 0x00002C84
        public static WaveFormat Pcm11Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 11025,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x060000AC RID: 172 RVA: 0x00004ABC File Offset: 0x00002CBC
        public static WaveFormat Pcm11Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 11025,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x060000AD RID: 173 RVA: 0x00004AF4 File Offset: 0x00002CF4
        public static WaveFormat Pcm8Khz16BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 8000,
                    BitsPerSample = 16,
                    Channels = 2
                };
            }
        }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x060000AE RID: 174 RVA: 0x00004B30 File Offset: 0x00002D30
        public static WaveFormat Pcm8Khz16BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 8000,
                    BitsPerSample = 16,
                    Channels = 1
                };
            }
        }

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x060000AF RID: 175 RVA: 0x00004B6C File Offset: 0x00002D6C
        public static WaveFormat Pcm8Khz8BitStereo
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 8000,
                    BitsPerSample = 8,
                    Channels = 2
                };
            }
        }

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x060000B0 RID: 176 RVA: 0x00004BA4 File Offset: 0x00002DA4
        public static WaveFormat Pcm8Khz8BitMono
        {
            get
            {
                return new WaveFormat
                {
                    FormatTag = WaveFormatTag.Pcm,
                    SamplesPerSecond = 8000,
                    BitsPerSample = 8,
                    Channels = 1
                };
            }
        }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x060000B1 RID: 177 RVA: 0x00004BDC File Offset: 0x00002DDC
        // (set) Token: 0x060000B2 RID: 178 RVA: 0x00004BE4 File Offset: 0x00002DE4
        public WaveFormatTag FormatTag
        {
            get
            {
                return this.formatTag;
            }
            set
            {
                this.formatTag = value;
            }
        }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x060000B3 RID: 179 RVA: 0x00004BF0 File Offset: 0x00002DF0
        // (set) Token: 0x060000B4 RID: 180 RVA: 0x00004BF8 File Offset: 0x00002DF8
        public short Channels
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

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x060000B5 RID: 181 RVA: 0x00004C04 File Offset: 0x00002E04
        // (set) Token: 0x060000B6 RID: 182 RVA: 0x00004C0C File Offset: 0x00002E0C
        public int SamplesPerSecond
        {
            get
            {
                return this.samplesPerSecond;
            }
            set
            {
                this.samplesPerSecond = value;
            }
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x060000B7 RID: 183 RVA: 0x00004C18 File Offset: 0x00002E18
        // (set) Token: 0x060000B8 RID: 184 RVA: 0x00004C20 File Offset: 0x00002E20
        public short BitsPerSample
        {
            get
            {
                return this.bitsPerSample;
            }
            set
            {
                this.bitsPerSample = value;
            }
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x060000B9 RID: 185 RVA: 0x00004C2C File Offset: 0x00002E2C
        public short BlockAlign
        {
            get
            {
                return (short)(this.Channels * this.BitsPerSample / 8);
            }
        }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x060000BA RID: 186 RVA: 0x00004C40 File Offset: 0x00002E40
        public int AverageBytesPerSecond
        {
            get
            {
                return this.SamplesPerSecond * (int)this.BlockAlign;
            }
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00004C50 File Offset: 0x00002E50
        internal WaveFormat Clone()
        {
            return new WaveFormat
            {
                bitsPerSample = this.bitsPerSample,
                channels = this.channels,
                formatTag = this.formatTag,
                samplesPerSecond = this.samplesPerSecond
            };
        }

        // Token: 0x04000037 RID: 55
        private WaveFormatTag formatTag;

        // Token: 0x04000038 RID: 56
        private short channels;

        // Token: 0x04000039 RID: 57
        private int samplesPerSecond;

        // Token: 0x0400003A RID: 58
        private short bitsPerSample;
    }
}
