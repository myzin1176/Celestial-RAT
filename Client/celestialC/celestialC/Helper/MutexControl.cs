using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class MutexControl
    {
        public static Mutex current;

        public static bool CreateMutex()
        {
            current = new Mutex(false,Settings.MutexName,out bool created);
            return created;
        }
        public static void CloseMutex()
        {
            if (current != null)
            {
                current.Close();
                current = null;
            }
        }
    }
}
