using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Storage
{
    public class StorageResult
    {
        public StorageStatus Status { get; set; }
        public string Message { get; set; }
    }

    public enum StorageStatus
    {
        Success = 1,
        Error = 2,
        Warning = 3
    }
}
