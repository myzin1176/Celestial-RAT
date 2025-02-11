using System;

namespace WinMM
{
    // Token: 0x02000014 RID: 20
    [Flags]
    public enum WaveFormats
    {
        // Token: 0x04000052 RID: 82
        Mono8Bit11Khz = 1,
        // Token: 0x04000053 RID: 83
        Stereo8Bit11Khz = 2,
        // Token: 0x04000054 RID: 84
        Mono16Bit11Khz = 4,
        // Token: 0x04000055 RID: 85
        Stereo16Bit11Khz = 8,
        // Token: 0x04000056 RID: 86
        Mono8Bit22Khz = 16,
        // Token: 0x04000057 RID: 87
        Stereo8Bit22Khz = 32,
        // Token: 0x04000058 RID: 88
        Mono16Bit22Khz = 64,
        // Token: 0x04000059 RID: 89
        Stereo16Bit22Khz = 128,
        // Token: 0x0400005A RID: 90
        Mono8Bit44Khz = 256,
        // Token: 0x0400005B RID: 91
        Stereo8Bit44Khz = 512,
        // Token: 0x0400005C RID: 92
        Mono16Bit44Khz = 1024,
        // Token: 0x0400005D RID: 93
        Stereo16Bit44Khz = 2048,
        // Token: 0x0400005E RID: 94
        Mono8Bit48Khz = 4096,
        // Token: 0x0400005F RID: 95
        Stereo8Bit48Khz = 8192,
        // Token: 0x04000060 RID: 96
        Mono16Bit48Khz = 16384,
        // Token: 0x04000061 RID: 97
        Stereo16Bit48Khz = 32768,
        // Token: 0x04000062 RID: 98
        Mono8Bit96Khz = 65536,
        // Token: 0x04000063 RID: 99
        Stereo8Bit96Khz = 131072,
        // Token: 0x04000064 RID: 100
        Mono16Bit96Khz = 262144,
        // Token: 0x04000065 RID: 101
        Stereo16Bit96Khz = 524288
    }
}
