using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;
using TaskTimeTracker.Model;

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

                string filename = string.Format("{0}.json", DateTime.Now.ToString("yyyy_MM_ddTHHmmss"));

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

        public async Task<StorageResult> StoreGroups(List<DutyGroup> groups, string uri)
        {
            StorageResult result = new StorageResult();

            try
            {
                string json = JsonConvert.SerializeObject(groups);

                string filename = string.Format("{0}.json", "AvailableGroups");

                string path = Path.Combine(uri, filename);

                using (StreamWriter file = File.CreateText(path))
                {
                    await file.WriteAsync(json);
                }

                result.Status = StorageStatus.Success;
                //TODO: put string in resource file
                result.Message = string.Format("Groups were successfully synced.");
            }
            catch (Exception ex)
            {
                result.Status = StorageStatus.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<StorageResult> LoadGroups(string uri)
        {
            StorageResult result = new StorageResult();

            try
            {
                string filename = string.Format("{0}.json", "AvailableGroups");

                string path = Path.Combine(uri, filename);

                string json = await Task<string>.Run(() => File.ReadAllText(path));

                result.Result = json;
                result.Status = StorageStatus.Success;
                //TODO: put string in resource file
                result.Message = string.Format("Groups were successfully loaded.");
            }
            catch (Exception ex)
            {
                result.Status = StorageStatus.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<StorageResult> LoadKeywords(string uri)
        {
            StorageResult result = new StorageResult();

            try
            {
                string filename = string.Format("{0}.txt", "AvailableKeywords");

                string path = Path.Combine(uri, filename);

                string txt = await Task<string>.Run(() => File.ReadAllText(path));

                result.Result = txt;
                result.Status = StorageStatus.Success;
                //TODO: put string in resource file
                result.Message = string.Format("Keywords were successfully loaded.");
            }
            catch (Exception ex)
            {
                result.Status = StorageStatus.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<StorageResult> StoreTempIteration(string json, string uri)
        {
            StorageResult result = new StorageResult();

            try
            {
                string filename = string.Format("{0}.json", "IterationTemp");

                string path = Path.Combine(uri, filename);

                using (StreamWriter file = File.CreateText(path))
                {
                    await file.WriteAsync(json);
                }

                result.Status = StorageStatus.Success;
                //TODO: put string in resource file
                result.Message = string.Format("Temp iteration was saved.");
            }
            catch (Exception ex)
            {
                result.Status = StorageStatus.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<StorageResult> LoadTempIteration(string uri)
        {
            StorageResult result = new StorageResult();

            try
            {
                string filename = string.Format("{0}.json", "IterationTemp");

                string path = Path.Combine(uri, filename);

                if (File.Exists(path))
                {

                    string txt = await Task<string>.Run(() => File.ReadAllText(path));

                    result.Result = txt;
                    result.Status = StorageStatus.Success;
                    //TODO: put string in resource file
                    result.Message = "Temp iteration was loaded.";
                }
                else
                {
                    result.Result = null;
                    result.Status = StorageStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = StorageStatus.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<StorageResult> LoadStoredIterationsAsync(string uri, bool checkSubFolders = false)
        {
            SearchOption so = checkSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var fileNames = Directory.EnumerateFiles(uri, "*.*", so).Where(a => a.EndsWith(".json"));
            StorageResult result = new StorageResult();
            result.Status = StorageStatus.Success;

            StringBuilder sbErrors = new StringBuilder();

            try
            {
                List<BrowsedIteration> browsedIterations = new List<BrowsedIteration>();
                foreach (string fileName in fileNames)
                {
                    string txt = await Task<string>.Run(() => File.ReadAllText(fileName));

                    if(!string.IsNullOrEmpty(txt))
                    {
                        Iteration iteration = JsonConvert.DeserializeObject<Iteration>(txt);

                        if(iteration != null)
                        {
                            browsedIterations.Add(new BrowsedIteration()
                            {
                                Source = fileName,
                                Name = new FileInfo(fileName).Name,
                                Iteration = iteration
                            });
                            
                        }
                        else
                        {
                            sbErrors.AppendLine(string.Format("File: {0} does not contain iteration", fileName));
                            result.Status = StorageStatus.Warning;
                        }

                    }
                    else
                    {
                        sbErrors.AppendLine(string.Format("File: {0} does not contain iteration", fileName));
                        result.Status = StorageStatus.Warning;
                    }
                }
                result.Result = browsedIterations;
                result.Message = "Files loaded";
                
            }
            catch (Exception ex)
            {
                result.Message = string.Format("Error: {0}", ex.Message);
                result.Status = StorageStatus.Error;
            }


            string errorWarnings = sbErrors.ToString();
            if(!string.IsNullOrEmpty(errorWarnings))
            {
                result.Status = StorageStatus.Warning;
                result.Message = errorWarnings;
            }
            return result;
        }

        public async Task<StorageResult> DeleteTempIteration(string uri)
        {
            StorageResult result = new StorageResult();

            string filename = string.Format("{0}.json", "IterationTemp");

            string path = Path.Combine(uri, filename);

            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));

                result.Message = "Temp Iteration File deleted";
                result.Status = StorageStatus.Success;
            }

            return result;
        }


    }
}
