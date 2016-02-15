using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;

namespace TaskTimeTracker.Storage
{
    public class LocalStorageProvider : IStorageProvider
    {
        public async Task<StorageResult> StoreIteration(Iteration iteration, string uri)
        {
            StorageResult result = new StorageResult();

            try
            {
                string json = JsonConvert.SerializeObject(iteration);

                string filename = string.Format("{0}.json", DateTime.Now.ToString("yyyy_MM_dd_hhmmss"));

                string path = Path.Combine(uri, filename);

                using (StreamWriter file = File.CreateText(path))
                {
                    await file.WriteAsync(json);
                }

                result.Status = StorageStatus.Success;
                //TODO: put string in resource file
                result.Message = string.Format("File '{0}' was successfully created.", path);
            }
            catch (Exception ex)
            {
                result.Status = StorageStatus.Error;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
