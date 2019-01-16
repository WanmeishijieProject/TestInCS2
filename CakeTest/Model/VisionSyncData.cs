using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeTest.Model
{
    public class VisionSyncData
    {
        public VisionSyncData()
        {
            IsNewSizing = false;
            IsOldSizing = false;
            VisionLock = new object();
        }
        public bool IsNewSizing{get;set;}
        public bool IsOldSizing { get; set; }
        public object VisionLock { get; set; }
    }
}
