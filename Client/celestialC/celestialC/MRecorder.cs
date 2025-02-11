using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinMM;

namespace celestialC
{
    public class MRecorder
    {
        public virtual WaveIn a
        {
            get
            {
                return this._a;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler<DataReadyEventArgs> value2 = new EventHandler<DataReadyEventArgs>(this.a_DataReady);
                if (this._a != null)
                {
                    this._a.DataReady -= value2;
                }
                this._a = value;
                if (this._a != null)
                {
                    this._a.DataReady += value2;
                }
            }
        }

        public MRecorder(int i, int bs, int Q)
        {
            this.bfrs = new List<byte[]>();
            this.IsRecord = true;
            this.Q = 0;
            this.a = new WaveIn(i);
            this.Q = Q;
            this.a.BufferQueueSize = 200;
            this.a.BufferSize = bs;
            if (Q == 0)
            {
                if (this.a.SupportsFormat(WaveFormat.Pcm8Khz8BitMono))
                {
                    this.a.Open(WaveFormat.Pcm8Khz8BitMono);
                }
                else
                {
                    Q = 1;
                    WaveIn a = this.a;
                    a.BufferSize = (int)Math.Round(unchecked((double)a.BufferSize + (double)bs / 2.0));
                    this.a.Open(WaveFormat.Pcm44Khz16BitMono);
                }
            }
            else if (this.a.SupportsFormat(WaveFormat.Pcm44Khz16BitMono))
            {
                this.a.Open(WaveFormat.Pcm44Khz16BitMono);
            }
            else
            {
                Q = 0;
                this.a.BufferSize = (int)Math.Round((double)bs / 2.0);
                this.a.Open(WaveFormat.Pcm8Khz8BitMono);
            }
            this.a.Start();
            this.IsRecord = true;
        }

        public byte[] GETBF()
        {
            byte[] result;
            lock (this)
            {
                if (this.bfrs.Count > 0)
                {
                    byte[] array = this.bfrs[0];
                    this.bfrs.RemoveAt(0);
                    result = array;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public void Dispose()
        {
            this.IsRecord = false;
            try
            {
                this.a.Stop();
            }
            catch (Exception ex)
            {
            }
            try
            {
                this.a.Close();
            }
            catch (Exception ex2)
            {
            }
            try
            {
                this.a.Dispose();
            }
            catch (Exception ex3)
            {
            }
        }
        private void a_DataReady(object sender, DataReadyEventArgs e)
        {
            lock (this)
            {
                if (this.bfrs.Count < 2)
                {
                    this.bfrs.Add(e.Data);
                }
            }
        }

        public List<byte[]> bfrs;

        [AccessedThroughProperty("a")]
        private WaveIn _a;

        public bool IsRecord;

        public int Q;
    }
}
