using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;

namespace TaskTimeTracker.Storage
{
    /// <summary>
    /// Storing shit can be very specific (specially for win phone and store apps
    /// So for the time beeing we will implement everything right here in wpf app
    /// we could probably put interface and result in PCL
    /// </summary>
    public interface IStorageProvider
    {
        Task<StorageResult> StoreIteration(Iteration iteration, string uri);
        Task<StorageResult> StoreGroups(List<DutyGroup> groups, string uri);
        Task<StorageResult> LoadGroups(string uri);
        Task<StorageResult> LoadKeywords(string uri);
        Task<StorageResult> StoreTempIteration(string json, string uri);
        Task<StorageResult> LoadTempIteration(string uri);
        Task<StorageResult> DeleteTempIteration(string uri);
        Task<StorageResult> LoadStoredIterations(string uri); 
    }
}
