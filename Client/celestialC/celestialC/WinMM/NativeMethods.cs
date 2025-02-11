using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinMM
{
    // Token: 0x02000009 RID: 9
    internal sealed class NativeMethods
    {
        // Token: 0x06000071 RID: 113 RVA: 0x00004490 File Offset: 0x00002690
        private NativeMethods()
        {
        }

        // Token: 0x06000072 RID: 114 RVA: 0x00004498 File Offset: 0x00002698
        public static void Throw(NativeMethods.MMSYSERROR error, NativeMethods.ErrorSource source)
        {
        }

        // Token: 0x06000073 RID: 115
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint PlaySound(string lpszSound, IntPtr hmod, NativeMethods.PLAYSOUNDFLAGS fuSound);

        // Token: 0x06000074 RID: 116
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInAddBuffer(WaveInSafeHandle hwi, IntPtr pwh, uint cbwh);

        // Token: 0x06000075 RID: 117
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInClose(WaveInSafeHandle hwi);

        // Token: 0x06000076 RID: 118
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInGetDevCaps(UIntPtr uDeviceID, ref NativeMethods.WAVEINCAPS pwic, uint cbwic);

        // Token: 0x06000077 RID: 119
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInGetErrorText(NativeMethods.MMSYSERROR mmrError, StringBuilder pszText, uint cchText);

        // Token: 0x06000078 RID: 120
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint waveInGetNumDevs();

        // Token: 0x06000079 RID: 121
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInOpen(ref IntPtr phwi, uint uDeviceID, ref NativeMethods.WAVEFORMATEX pwfx, NativeMethods.waveInProc dwCallback, IntPtr dwCallbackInstance, NativeMethods.WAVEOPENFLAGS fdwOpen);

        // Token: 0x0600007A RID: 122
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInPrepareHeader(WaveInSafeHandle hwi, IntPtr pwh, uint cbwh);

        // Token: 0x0600007B RID: 123
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInReset(WaveInSafeHandle hwi);

        // Token: 0x0600007C RID: 124
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInStart(WaveInSafeHandle hwi);

        // Token: 0x0600007D RID: 125
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveInUnprepareHeader(WaveInSafeHandle hwi, IntPtr pwh, uint cbwh);

        // Token: 0x0600007E RID: 126
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutClose(WaveOutSafeHandle hwo);

        // Token: 0x0600007F RID: 127
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutGetDevCaps(UIntPtr uDeviceID, ref NativeMethods.WAVEOUTCAPS pwoc, uint cbwoc);

        // Token: 0x06000080 RID: 128
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutGetErrorText(NativeMethods.MMSYSERROR mmrError, StringBuilder pszText, uint cchText);

        // Token: 0x06000081 RID: 129
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint waveOutGetNumDevs();

        // Token: 0x06000082 RID: 130
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutGetPitch(WaveOutSafeHandle hwo, ref uint pdwPitch);

        // Token: 0x06000083 RID: 131
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutGetPlaybackRate(WaveOutSafeHandle hwo, ref uint pdwRate);

        // Token: 0x06000084 RID: 132
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutGetVolume(WaveOutSafeHandle hwo, ref uint pdwVolume);

        // Token: 0x06000085 RID: 133
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutGetVolume(UIntPtr uDeviceID, ref uint pdwVolume);

        // Token: 0x06000086 RID: 134
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutOpen(ref IntPtr phwo, uint uDeviceID, ref NativeMethods.WAVEFORMATEX pwfx, NativeMethods.waveOutProc dwCallback, IntPtr dwCallbackInstance, NativeMethods.WAVEOPENFLAGS dwFlags);

        // Token: 0x06000087 RID: 135
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutPause(WaveOutSafeHandle hwo);

        // Token: 0x06000088 RID: 136
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutPrepareHeader(WaveOutSafeHandle hwo, IntPtr pwh, uint cbwh);

        // Token: 0x06000089 RID: 137
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutReset(WaveOutSafeHandle hwo);

        // Token: 0x0600008A RID: 138
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutRestart(WaveOutSafeHandle hwo);

        // Token: 0x0600008B RID: 139
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutSetPitch(WaveOutSafeHandle hwo, uint pdwPitch);

        // Token: 0x0600008C RID: 140
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutSetPlaybackRate(WaveOutSafeHandle hwo, uint dwRate);

        // Token: 0x0600008D RID: 141
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutSetVolume(WaveOutSafeHandle hwo, uint dwVolume);

        // Token: 0x0600008E RID: 142
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutSetVolume(UIntPtr uDeviceID, uint dwVolume);

        // Token: 0x0600008F RID: 143
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutUnprepareHeader(WaveOutSafeHandle hwo, IntPtr pwh, uint cbwh);

        // Token: 0x06000090 RID: 144
        [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern NativeMethods.MMSYSERROR waveOutWrite(WaveOutSafeHandle hwo, IntPtr pwh, uint cbwh);

        // Token: 0x02000016 RID: 22
        // (Invoke) Token: 0x060000DB RID: 219
        public delegate void waveOutProc(IntPtr hwo, NativeMethods.WAVEOUTMESSAGE uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

        // Token: 0x02000017 RID: 23
        // (Invoke) Token: 0x060000DF RID: 223
        public delegate void waveInProc(IntPtr hwi, NativeMethods.WAVEINMESSAGE uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

        // Token: 0x02000018 RID: 24
        [Flags]
        public enum WAVEOPENFLAGS
        {
            // Token: 0x04000068 RID: 104
            CALLBACK_NULL = 0,
            // Token: 0x04000069 RID: 105
            CALLBACK_WINDOW = 65536,
            // Token: 0x0400006A RID: 106
            CALLBACK_THREAD = 131072,
            // Token: 0x0400006B RID: 107
            [Obsolete]
            CALLBACK_TASK = 131072,
            // Token: 0x0400006C RID: 108
            CALLBACK_FUNCTION = 196608,
            // Token: 0x0400006D RID: 109
            WAVE_FORMAT_QUERY = 1,
            // Token: 0x0400006E RID: 110
            WAVE_ALLOWSYNC = 2,
            // Token: 0x0400006F RID: 111
            WAVE_MAPPED = 4,
            // Token: 0x04000070 RID: 112
            WAVE_FORMAT_DIRECT = 8
        }

        // Token: 0x02000019 RID: 25
        public enum WAVEHDRFLAGS
        {
            // Token: 0x04000072 RID: 114
            WHDR_BEGINLOOP = 4,
            // Token: 0x04000073 RID: 115
            WHDR_DONE = 1,
            // Token: 0x04000074 RID: 116
            WHDR_ENDLOOP = 8,
            // Token: 0x04000075 RID: 117
            WHDR_INQUEUE = 16,
            // Token: 0x04000076 RID: 118
            WHDR_PREPARED = 2
        }

        // Token: 0x0200001A RID: 26
        public enum WAVEOUTMESSAGE
        {
            // Token: 0x04000078 RID: 120
            WOM_OPEN = 955,
            // Token: 0x04000079 RID: 121
            WOM_CLOSE,
            // Token: 0x0400007A RID: 122
            WOM_DONE
        }

        // Token: 0x0200001B RID: 27
        public enum WAVEINMESSAGE
        {
            // Token: 0x0400007C RID: 124
            MM_WIM_OPEN = 958,
            // Token: 0x0400007D RID: 125
            MM_WIM_CLOSE,
            // Token: 0x0400007E RID: 126
            MM_WIM_DATA
        }

        // Token: 0x0200001C RID: 28
        public enum MMSYSERROR
        {
            // Token: 0x04000080 RID: 128
            MMSYSERR_NOERROR,
            // Token: 0x04000081 RID: 129
            MMSYSERR_ERROR,
            // Token: 0x04000082 RID: 130
            MMSYSERR_BADDEVICEID,
            // Token: 0x04000083 RID: 131
            MMSYSERR_NOTENABLED,
            // Token: 0x04000084 RID: 132
            MMSYSERR_ALLOCATED,
            // Token: 0x04000085 RID: 133
            MMSYSERR_INVALHANDLE,
            // Token: 0x04000086 RID: 134
            MMSYSERR_NODRIVER,
            // Token: 0x04000087 RID: 135
            MMSYSERR_NOMEM,
            // Token: 0x04000088 RID: 136
            MMSYSERR_NOTSUPPORTED,
            // Token: 0x04000089 RID: 137
            MMSYSERR_BADERRNUM,
            // Token: 0x0400008A RID: 138
            MMSYSERR_INVALFLAG,
            // Token: 0x0400008B RID: 139
            MMSYSERR_INVALPARAM,
            // Token: 0x0400008C RID: 140
            MMSYSERR_HANDLEBUSY,
            // Token: 0x0400008D RID: 141
            MMSYSERR_INVALIDALIAS,
            // Token: 0x0400008E RID: 142
            MMSYSERR_BADDB,
            // Token: 0x0400008F RID: 143
            MMSYSERR_KEYNOTFOUND,
            // Token: 0x04000090 RID: 144
            MMSYSERR_READERROR,
            // Token: 0x04000091 RID: 145
            MMSYSERR_WRITEERROR,
            // Token: 0x04000092 RID: 146
            MMSYSERR_DELETEERROR,
            // Token: 0x04000093 RID: 147
            MMSYSERR_VALNOTFOUND,
            // Token: 0x04000094 RID: 148
            MMSYSERR_NODRIVERCB,
            // Token: 0x04000095 RID: 149
            MMSYSERR_MOREDATA,
            // Token: 0x04000096 RID: 150
            WAVERR_BADFORMAT = 32,
            // Token: 0x04000097 RID: 151
            WAVERR_STILLPLAYING,
            // Token: 0x04000098 RID: 152
            WAVERR_UNPREPARED,
            // Token: 0x04000099 RID: 153
            WAVERR_SYNC,
            // Token: 0x0400009A RID: 154
            MIDIERR_UNPREPARED = 64,
            // Token: 0x0400009B RID: 155
            MIDIERR_STILLPLAYING,
            // Token: 0x0400009C RID: 156
            MIDIERR_NOMAP,
            // Token: 0x0400009D RID: 157
            MIDIERR_NOTREADY,
            // Token: 0x0400009E RID: 158
            MIDIERR_NODEVICE,
            // Token: 0x0400009F RID: 159
            MIDIERR_INVALIDSETUP,
            // Token: 0x040000A0 RID: 160
            MIDIERR_BADOPENMODE,
            // Token: 0x040000A1 RID: 161
            MIDIERR_DONT_CONTINUE,
            // Token: 0x040000A2 RID: 162
            TIMERR_NOCANDO = 97,
            // Token: 0x040000A3 RID: 163
            TIMERR_STRUCT = 129,
            // Token: 0x040000A4 RID: 164
            JOYERR_PARMS = 165,
            // Token: 0x040000A5 RID: 165
            JOYERR_NOCANDO,
            // Token: 0x040000A6 RID: 166
            JOYERR_UNPLUGGED,
            // Token: 0x040000A7 RID: 167
            MCIERR_INVALID_DEVICE_ID = 257,
            // Token: 0x040000A8 RID: 168
            MCIERR_UNRECOGNIZED_KEYWORD = 259,
            // Token: 0x040000A9 RID: 169
            MCIERR_UNRECOGNIZED_COMMAND = 261,
            // Token: 0x040000AA RID: 170
            MCIERR_HARDWARE,
            // Token: 0x040000AB RID: 171
            MCIERR_INVALID_DEVICE_NAME,
            // Token: 0x040000AC RID: 172
            MCIERR_OUT_OF_MEMORY,
            // Token: 0x040000AD RID: 173
            MCIERR_DEVICE_OPEN,
            // Token: 0x040000AE RID: 174
            MCIERR_CANNOT_LOAD_DRIVER,
            // Token: 0x040000AF RID: 175
            MCIERR_MISSING_COMMAND_STRING,
            // Token: 0x040000B0 RID: 176
            MCIERR_PARAM_OVERFLOW,
            // Token: 0x040000B1 RID: 177
            MCIERR_MISSING_STRING_ARGUMENT,
            // Token: 0x040000B2 RID: 178
            MCIERR_BAD_INTEGER,
            // Token: 0x040000B3 RID: 179
            MCIERR_PARSER_INTERNAL,
            // Token: 0x040000B4 RID: 180
            MCIERR_DRIVER_INTERNAL,
            // Token: 0x040000B5 RID: 181
            MCIERR_MISSING_PARAMETER,
            // Token: 0x040000B6 RID: 182
            MCIERR_UNSUPPORTED_FUNCTION,
            // Token: 0x040000B7 RID: 183
            MCIERR_FILE_NOT_FOUND,
            // Token: 0x040000B8 RID: 184
            MCIERR_DEVICE_NOT_READY,
            // Token: 0x040000B9 RID: 185
            MCIERR_INTERNAL,
            // Token: 0x040000BA RID: 186
            MCIERR_DRIVER,
            // Token: 0x040000BB RID: 187
            MCIERR_CANNOT_USE_ALL,
            // Token: 0x040000BC RID: 188
            MCIERR_MULTIPLE,
            // Token: 0x040000BD RID: 189
            MCIERR_EXTENSION_NOT_FOUND,
            // Token: 0x040000BE RID: 190
            MCIERR_OUTOFRANGE,
            // Token: 0x040000BF RID: 191
            MCIERR_FLAGS_NOT_COMPATIBLE = 284,
            // Token: 0x040000C0 RID: 192
            MCIERR_FILE_NOT_SAVED = 286,
            // Token: 0x040000C1 RID: 193
            MCIERR_DEVICE_TYPE_REQUIRED,
            // Token: 0x040000C2 RID: 194
            MCIERR_DEVICE_LOCKED,
            // Token: 0x040000C3 RID: 195
            MCIERR_DUPLICATE_ALIAS,
            // Token: 0x040000C4 RID: 196
            MCIERR_BAD_CONSTANT,
            // Token: 0x040000C5 RID: 197
            MCIERR_MUST_USE_SHAREABLE,
            // Token: 0x040000C6 RID: 198
            MCIERR_MISSING_DEVICE_NAME,
            // Token: 0x040000C7 RID: 199
            MCIERR_BAD_TIME_FORMAT,
            // Token: 0x040000C8 RID: 200
            MCIERR_NO_CLOSING_QUOTE,
            // Token: 0x040000C9 RID: 201
            MCIERR_DUPLICATE_FLAGS,
            // Token: 0x040000CA RID: 202
            MCIERR_INVALID_FILE,
            // Token: 0x040000CB RID: 203
            MCIERR_NULL_PARAMETER_BLOCK,
            // Token: 0x040000CC RID: 204
            MCIERR_UNNAMED_RESOURCE,
            // Token: 0x040000CD RID: 205
            MCIERR_NEW_REQUIRES_ALIAS,
            // Token: 0x040000CE RID: 206
            MCIERR_NOTIFY_ON_AUTO_OPEN,
            // Token: 0x040000CF RID: 207
            MCIERR_NO_ELEMENT_ALLOWED,
            // Token: 0x040000D0 RID: 208
            MCIERR_NONAPPLICABLE_FUNCTION,
            // Token: 0x040000D1 RID: 209
            MCIERR_ILLEGAL_FOR_AUTO_OPEN,
            // Token: 0x040000D2 RID: 210
            MCIERR_FILENAME_REQUIRED,
            // Token: 0x040000D3 RID: 211
            MCIERR_EXTRA_CHARACTERS,
            // Token: 0x040000D4 RID: 212
            MCIERR_DEVICE_NOT_INSTALLED,
            // Token: 0x040000D5 RID: 213
            MCIERR_GET_CD,
            // Token: 0x040000D6 RID: 214
            MCIERR_SET_CD,
            // Token: 0x040000D7 RID: 215
            MCIERR_SET_DRIVE,
            // Token: 0x040000D8 RID: 216
            MCIERR_DEVICE_LENGTH,
            // Token: 0x040000D9 RID: 217
            MCIERR_DEVICE_ORD_LENGTH,
            // Token: 0x040000DA RID: 218
            MCIERR_NO_INTEGER,
            // Token: 0x040000DB RID: 219
            MCIERR_WAVE_OUTPUTSINUSE = 320,
            // Token: 0x040000DC RID: 220
            MCIERR_WAVE_SETOUTPUTINUSE,
            // Token: 0x040000DD RID: 221
            MCIERR_WAVE_INPUTSINUSE,
            // Token: 0x040000DE RID: 222
            MCIERR_WAVE_SETINPUTINUSE,
            // Token: 0x040000DF RID: 223
            MCIERR_WAVE_OUTPUTUNSPECIFIED,
            // Token: 0x040000E0 RID: 224
            MCIERR_WAVE_INPUTUNSPECIFIED,
            // Token: 0x040000E1 RID: 225
            MCIERR_WAVE_OUTPUTSUNSUITABLE,
            // Token: 0x040000E2 RID: 226
            MCIERR_WAVE_SETOUTPUTUNSUITABLE,
            // Token: 0x040000E3 RID: 227
            MCIERR_WAVE_INPUTSUNSUITABLE,
            // Token: 0x040000E4 RID: 228
            MCIERR_WAVE_SETINPUTUNSUITABLE,
            // Token: 0x040000E5 RID: 229
            MCIERR_SEQ_DIV_INCOMPATIBLE = 336,
            // Token: 0x040000E6 RID: 230
            MCIERR_SEQ_PORT_INUSE,
            // Token: 0x040000E7 RID: 231
            MCIERR_SEQ_PORT_NONEXISTENT,
            // Token: 0x040000E8 RID: 232
            MCIERR_SEQ_PORT_MAPNODEVICE,
            // Token: 0x040000E9 RID: 233
            MCIERR_SEQ_PORT_MISCERROR,
            // Token: 0x040000EA RID: 234
            MCIERR_SEQ_TIMER,
            // Token: 0x040000EB RID: 235
            MCIERR_SEQ_PORTUNSPECIFIED,
            // Token: 0x040000EC RID: 236
            MCIERR_SEQ_NOMIDIPRESENT,
            // Token: 0x040000ED RID: 237
            MCIERR_NO_WINDOW = 346,
            // Token: 0x040000EE RID: 238
            MCIERR_CREATEWINDOW,
            // Token: 0x040000EF RID: 239
            MCIERR_FILE_READ,
            // Token: 0x040000F0 RID: 240
            MCIERR_FILE_WRITE,
            // Token: 0x040000F1 RID: 241
            MCIERR_NO_IDENTITY,
            // Token: 0x040000F2 RID: 242
            MIXERR_INVALLINE = 1024,
            // Token: 0x040000F3 RID: 243
            MIXERR_INVALCONTROL,
            // Token: 0x040000F4 RID: 244
            MIXERR_INVALVALUE,
            // Token: 0x040000F5 RID: 245
            MIXERR_LASTERROR = 1026
        }

        // Token: 0x0200001D RID: 29
        [Flags]
        public enum WAVECAPS
        {
            // Token: 0x040000F7 RID: 247
            WAVECAPS_PITCH = 1,
            // Token: 0x040000F8 RID: 248
            WAVECAPS_PLAYBACKRATE = 2,
            // Token: 0x040000F9 RID: 249
            WAVECAPS_VOLUME = 4,
            // Token: 0x040000FA RID: 250
            WAVECAPS_LRVOLUME = 8,
            // Token: 0x040000FB RID: 251
            WAVECAPS_SYNC = 16,
            // Token: 0x040000FC RID: 252
            WAVECAPS_SAMPLEACCURATE = 32,
            // Token: 0x040000FD RID: 253
            WAVECAPS_DIRECTSOUND = 64
        }

        // Token: 0x0200001E RID: 30
        [Flags]
        public enum WAVEFORMATS
        {
            // Token: 0x040000FF RID: 255
            WAVE_FORMAT_1M08 = 1,
            // Token: 0x04000100 RID: 256
            WAVE_FORMAT_1S08 = 2,
            // Token: 0x04000101 RID: 257
            WAVE_FORMAT_1M16 = 4,
            // Token: 0x04000102 RID: 258
            WAVE_FORMAT_1S16 = 8,
            // Token: 0x04000103 RID: 259
            WAVE_FORMAT_2M08 = 16,
            // Token: 0x04000104 RID: 260
            WAVE_FORMAT_2S08 = 32,
            // Token: 0x04000105 RID: 261
            WAVE_FORMAT_2M16 = 64,
            // Token: 0x04000106 RID: 262
            WAVE_FORMAT_2S16 = 128,
            // Token: 0x04000107 RID: 263
            WAVE_FORMAT_4M08 = 256,
            // Token: 0x04000108 RID: 264
            WAVE_FORMAT_4S08 = 512,
            // Token: 0x04000109 RID: 265
            WAVE_FORMAT_4M16 = 1024,
            // Token: 0x0400010A RID: 266
            WAVE_FORMAT_4S16 = 2048,
            // Token: 0x0400010B RID: 267
            WAVE_FORMAT_48M08 = 4096,
            // Token: 0x0400010C RID: 268
            WAVE_FORMAT_48S08 = 8192,
            // Token: 0x0400010D RID: 269
            WAVE_FORMAT_48M16 = 16384,
            // Token: 0x0400010E RID: 270
            WAVE_FORMAT_48S16 = 32768,
            // Token: 0x0400010F RID: 271
            WAVE_FORMAT_96M08 = 65536,
            // Token: 0x04000110 RID: 272
            WAVE_FORMAT_96S08 = 131072,
            // Token: 0x04000111 RID: 273
            WAVE_FORMAT_96M16 = 262144,
            // Token: 0x04000112 RID: 274
            WAVE_FORMAT_96S16 = 524288
        }

        // Token: 0x0200001F RID: 31
        public enum WAVEFORMATTAG
        {
            // Token: 0x04000114 RID: 276
            WAVE_FORMAT_PCM = 1,
            // Token: 0x04000115 RID: 277
            WAVE_FORMAT_ADPCM
        }

        // Token: 0x02000020 RID: 32
        [Flags]
        public enum PLAYSOUNDFLAGS
        {
            // Token: 0x04000117 RID: 279
            SND_SYNC = 0,
            // Token: 0x04000118 RID: 280
            SND_ASYNC = 1,
            // Token: 0x04000119 RID: 281
            SND_NODEFAULT = 2,
            // Token: 0x0400011A RID: 282
            SND_MEMORY = 4,
            // Token: 0x0400011B RID: 283
            SND_LOOP = 8,
            // Token: 0x0400011C RID: 284
            SND_NOSTOP = 16,
            // Token: 0x0400011D RID: 285
            SND_PURGE = 64,
            // Token: 0x0400011E RID: 286
            SND_APPLICATION = 128,
            // Token: 0x0400011F RID: 287
            SND_NOWAIT = 8192,
            // Token: 0x04000120 RID: 288
            SND_ALIAS = 65536,
            // Token: 0x04000121 RID: 289
            SND_FILENAME = 131072,
            // Token: 0x04000122 RID: 290
            SND_RESOURCE = 262148,
            // Token: 0x04000123 RID: 291
            SND_ALIAS_ID = 1114112
        }

        // Token: 0x02000021 RID: 33
        public enum ErrorSource
        {
            // Token: 0x04000125 RID: 293
            WaveIn,
            // Token: 0x04000126 RID: 294
            WaveOut
        }

        // Token: 0x02000022 RID: 34
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WAVEOUTCAPS
        {
            // Token: 0x04000127 RID: 295
            public ushort wMid;

            // Token: 0x04000128 RID: 296
            public ushort wPid;

            // Token: 0x04000129 RID: 297
            public uint vDriverVersion;

            // Token: 0x0400012A RID: 298
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;

            // Token: 0x0400012B RID: 299
            public NativeMethods.WAVEFORMATS dwFormats;

            // Token: 0x0400012C RID: 300
            public ushort wChannels;

            // Token: 0x0400012D RID: 301
            public ushort wReserved1;

            // Token: 0x0400012E RID: 302
            public NativeMethods.WAVECAPS dwSupport;
        }

        // Token: 0x02000023 RID: 35
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WAVEINCAPS
        {
            // Token: 0x0400012F RID: 303
            public ushort wMid;

            // Token: 0x04000130 RID: 304
            public ushort wPid;

            // Token: 0x04000131 RID: 305
            public uint vDriverVersion;

            // Token: 0x04000132 RID: 306
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;

            // Token: 0x04000133 RID: 307
            public uint dwFormats;

            // Token: 0x04000134 RID: 308
            public ushort wChannels;

            // Token: 0x04000135 RID: 309
            public ushort wReserved1;
        }

        // Token: 0x02000024 RID: 36
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WAVEHDR
        {
            // Token: 0x04000136 RID: 310
            public IntPtr lpData;

            // Token: 0x04000137 RID: 311
            public uint dwBufferLength;

            // Token: 0x04000138 RID: 312
            public uint dwBytesRecorded;

            // Token: 0x04000139 RID: 313
            public IntPtr dwUser;

            // Token: 0x0400013A RID: 314
            public NativeMethods.WAVEHDRFLAGS dwFlags;

            // Token: 0x0400013B RID: 315
            public uint dwLoops;

            // Token: 0x0400013C RID: 316
            public IntPtr lpNext;

            // Token: 0x0400013D RID: 317
            public uint reserved;
        }

        // Token: 0x02000025 RID: 37
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WAVEFORMATEX
        {
            // Token: 0x0400013E RID: 318
            public short wFormatTag;

            // Token: 0x0400013F RID: 319
            public short nChannels;

            // Token: 0x04000140 RID: 320
            public int nSamplesPerSec;

            // Token: 0x04000141 RID: 321
            public int nAvgBytesPerSec;

            // Token: 0x04000142 RID: 322
            public short nBlockAlign;

            // Token: 0x04000143 RID: 323
            public short wBitsPerSample;

            // Token: 0x04000144 RID: 324
            public short cbSize;
        }

        // Token: 0x02000026 RID: 38
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WAVEFORMATEXTENSIBLE
        {
            // Token: 0x04000145 RID: 325
            public short wFormatTag;

            // Token: 0x04000146 RID: 326
            public short nChannels;

            // Token: 0x04000147 RID: 327
            public int nSamplesPerSec;

            // Token: 0x04000148 RID: 328
            public int nAvgBytesPerSec;

            // Token: 0x04000149 RID: 329
            public short nBlockAlign;

            // Token: 0x0400014A RID: 330
            public short wBitsPerSample;

            // Token: 0x0400014B RID: 331
            public short cbSize;

            // Token: 0x0400014C RID: 332
            public ushort wValidBitsPerSample;

            // Token: 0x0400014D RID: 333
            public uint dwChannelMask;

            // Token: 0x0400014E RID: 334
            public Guid SubFormat;
        }

        // Token: 0x02000027 RID: 39
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MMTIME
        {
            // Token: 0x0400014F RID: 335
            public uint wType;

            // Token: 0x04000150 RID: 336
            public uint wData1;

            // Token: 0x04000151 RID: 337
            public uint wData2;
        }
    }
}
