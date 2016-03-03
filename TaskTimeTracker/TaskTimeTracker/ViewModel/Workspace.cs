using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TaskTimeTracker.Command;
using TaskTimeTracker.Common.DomainLogic;
using TaskTimeTracker.Common.Model;
using TaskTimeTracker.Storage;
using TaskTimeTracker.ViewModel.Base;

namespace TaskTimeTracker.ViewModel
{
    /// <summary>
    /// This is the underlying view model which holds any potencial "sub"
    /// </summary>
    public class Workspace : ViewModelBase
    {
        #region Fields

        private DutyProvider _Provider;

        DispatcherTimer _Timer;

        private TimeSpan _Elapsed;

        private string _ElapsedTime;

        private bool _IsSettingsOpen;

        private StorageResult _StorageResult;

        private ObservableCollection<DutyGroup> _AvailableGroups;

        private ObservableCollection<string> _AvailableKeywords;

        bool _LoadedFromTemp;

        private string _PrettyIterationPrint;

        #endregion

        #region Constructors

        public Workspace()
        {
            Provider = new DutyProvider();
            _Timer = new DispatcherTimer();
            _Timer.Interval = TimeSpan.FromSeconds(1);
            _Timer.Tick += TimerTick;

            _AvailableGroups = new ObservableCollection<DutyGroup>();
            _AvailableKeywords = new ObservableCollection<string>();

            LoadDutyGroups();
            LoadKeywords();
            LoadTempIteration();

            PropertyChanged += Workspace_PropertyChanged;
        }

        private void Workspace_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentDuty")
            {
                StoreTempIteration();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _Elapsed = _Elapsed.Add(TimeSpan.FromSeconds(1));
            ElapsedTime = _Elapsed.ToString("hh\\:mm\\:ss");
        }

        #endregion

        #region Static

        private static Workspace _Workspace;

        public static Workspace This
        {
            get
            {
                if (_Workspace == null)
                {
                    _Workspace = new Workspace();
                }

                return _Workspace;
            }
        }

        #endregion

        #region Public Properties

        public DutyProvider Provider
        {
            get { return _Provider; }
            private set
            {
                _Provider = value;
                NotifyPropertyChanged(() => Provider);
            }
        }

        public string ElapsedTime
        {
            get { return _ElapsedTime; }
            set
            {
                _ElapsedTime = value;
                NotifyPropertyChanged(() => ElapsedTime);
            }
        }

        /// <summary>
        /// Gets or sets duty which is currently in progress
        /// </summary>
        public Duty CurrentDuty
        {
            get { return _Provider.OngoingDuty; }
            //set
            //{
            //    _CurrentDuty = value;
            //    NotifyPropertyChanged(() => this.CurrentDuty);
            //}
        }

        public DutyGroup CurrentDutyGroup
        {
            get
            {
                if (_Provider.OngoingDuty != null)
                    return _Provider.OngoingDuty.Group;

                return null;
            }
        }

        public bool IsSettingsOpen
        {
            get { return _IsSettingsOpen; }

            set
            {
                if (_IsSettingsOpen != value)
                {
                    _IsSettingsOpen = value;
                    NotifyPropertyChanged(() => IsSettingsOpen);
                }
            }
        }

        public StorageResult StorageResult
        {
            get { return _StorageResult; }
            set
            {
                _StorageResult = value;
                NotifyPropertyChanged(() => StorageResult);
            }
        }

        public ObservableCollection<DutyGroup> AvailableGroups
        {
            get { return _AvailableGroups; }
            set
            {
                _AvailableGroups = value;
                NotifyPropertyChanged(() => AvailableGroups);
            }
        }

        public ObservableCollection<string> AvailableKeywords
        {
            get { return _AvailableKeywords; }
            set
            {
                _AvailableKeywords = value;
                NotifyPropertyChanged(() => AvailableKeywords);
            }
        }

        public string PrettyIterationPrint
        {
            get { return _PrettyIterationPrint; }
            set
            {
                _PrettyIterationPrint = value;
                NotifyPropertyChanged(() => PrettyIterationPrint);
            }
        }

        #endregion

        #region Private Methods

        private void SetPrettyIterationPrint()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Duty duty in Provider.Iteration.Duties)
            {
                var startTime = duty.TimeFrames.First();
                var endTime = duty.TimeFrames.Last();

                sb.AppendLine(string.Format("{0} - {1}    {2}; {3}", startTime.From.ToString("HH\\:mm"), endTime.To.Value.ToString("HH\\:mm"), duty.Name, duty.Description));
            }

            PrettyIterationPrint = sb.ToString();
        }

        private async void LoadTempIteration()
        {
            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations", "Common"));

            LocalStorageProvider provider = new LocalStorageProvider();
            StorageResult result = await provider.LoadTempIteration(directory.FullName);

            if (result.Result != null)
            {
                Iteration iteration = JsonConvert.DeserializeObject<Iteration>((string)result.Result);

                if (iteration != null && iteration.Duties != null && iteration.Duties.Count > 0)
                {
                    Provider.SetIteration(iteration);
                    NotifyPropertyChanged(() => CurrentDuty);
                    NotifyPropertyChanged(() => CurrentDutyGroup);

                    _LoadedFromTemp = true;

                    SetAndStartTimer();
                }
            }
        }

        private async void StoreTempIteration()
        {
            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations", "Common"));

            LocalStorageProvider provider = new LocalStorageProvider();

            string json = JsonConvert.SerializeObject(Provider.Iteration);

            StorageResult result = await provider.StoreTempIteration(json, directory.FullName);
        }

        private void StartNewDuty()
        {
            _Provider.StartNewDuty();
            NotifyPropertyChanged(() => this.CurrentDuty);
            SetCurrentDutyGroup(new DutyGroup() { Name = "Add current group..." });
            NotifyPropertyChanged(() => CurrentDutyGroup);
            SetAndStartTimer();
        }

        private void FinishDutyAndUnpausePrevious()
        {
            _Provider.FinishDutyAndUnpausePrevious();
            NotifyPropertyChanged(() => this.CurrentDuty);
            NotifyPropertyChanged(() => CurrentDutyGroup);
            SetAndStartTimer();
        }

        private void FinishDutyAndStartNew()
        {
            _Provider.FinishDutyAndStartNew();
            NotifyPropertyChanged(() => this.CurrentDuty);
            NotifyPropertyChanged(() => CurrentDutyGroup);
            SetAndStartTimer();
        }

        private void UnpauseDuty(object duty)
        {
            Duty dut = duty as Duty;
            if (dut != null && dut != CurrentDuty)
            {
                _Provider.UnpauseDuty(dut);

                NotifyPropertyChanged(() => CurrentDuty);
                NotifyPropertyChanged(() => CurrentDutyGroup);

                SetAndStartTimer();
            }
        }

        private void SetAndStartTimer()
        {
            _Elapsed = CurrentDuty.TotalTimeSpent;
            if (_LoadedFromTemp)
            {
                DutyTimeFrame lastFrame = CurrentDuty.TimeFrames.Last();

                _Elapsed = DateTime.Now - lastFrame.From;
                _LoadedFromTemp = false;
            }

            ElapsedTime = CurrentDuty.TotalTimeSpent.ToString("hh\\:mm\\:ss");

            if (!_Timer.IsEnabled)
                _Timer.Start();
        }

        private void ResetAndStopTimer()
        {
            ElapsedTime = new TimeSpan(0, 0, 0).ToString("hh\\:mm\\:ss");

            _Timer.Stop();
        }

        private void OpenSettings()
        {
            IsSettingsOpen = true;
        }

        private async void FinishIterationAndStoreIt()
        {
            _Provider.EndIteration();

            SetPrettyIterationPrint();

            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations"));

            //TODO: add proper factory infrastructure
            LocalStorageProvider storage = new LocalStorageProvider();
            StorageResult = await storage.StoreIteration(_Provider.Iteration, directory.FullName);

            _Provider.StartNewIteration();

            NotifyPropertyChanged(() => CurrentDuty);
            NotifyPropertyChanged(() => CurrentDutyGroup);
            ResetAndStopTimer();

            var directoryTemp = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations", "Common"));
            await storage.DeleteTempIteration(directoryTemp.FullName);
        }

        private void AddNewDutyGroup(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                AvailableGroups.Add(new DutyGroup() { Name = name });

                SyncDutyGroups();
            }
        }

        private void SetCurrentDutyGroup(object dutyGroup)
        {
            DutyGroup dg = dutyGroup as DutyGroup;

            if (dg != null && _Provider.OngoingDuty != null)
            {
                //Todo: maybe put this logic to provider;
                _Provider.OngoingDuty.Group = dg;
                NotifyPropertyChanged(() => CurrentDutyGroup);
            }
        }



        private void AddKeyword(object keyword)
        {
            if (CurrentDuty != null && keyword != null)
            {
                if (string.IsNullOrEmpty(CurrentDuty.Keywords))
                {
                    CurrentDuty.Keywords = keyword.ToString();
                }
                else
                {
                    string val = keyword.ToString();

                    if (!CurrentDuty.Keywords.Contains(val))
                    {
                        if (CurrentDuty.Keywords[CurrentDuty.Keywords.Length - 1] == ',')
                        {
                            CurrentDuty.Keywords += val;
                        }
                        else
                            CurrentDuty.Keywords += string.Format(",{0}", val);
                    }
                }
            }
        }

        private void AddLastKeywordToList(object val)
        {
            if (val != null)
            {
                string[] tmp = val.ToString().Split(',');
                if (tmp.Length > 0)
                {
                    string kwd = tmp[tmp.Length - 1].Trim();
                    if (!_AvailableKeywords.Any(a => a.Equals(kwd, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        AvailableKeywords.Add(kwd);
                    }
                }

                if (CurrentDuty != null)
                    CurrentDuty.Keywords = val + ",";
            }
        }


        #endregion

        #region Commands

        #region Duty Commands

        RelayCommand _StartNewDutyCommand;

        public RelayCommand StartNewDutyCommand
        {
            get
            {
                if (_StartNewDutyCommand == null)
                {
                    _StartNewDutyCommand = new RelayCommand((p) => StartNewDuty());
                }

                return _StartNewDutyCommand;
            }
        }

        RelayCommand _FinishDutyAndUnpausePreviousCommand;

        public RelayCommand FinishDutyAndUnpausePreviousCommand
        {
            get
            {
                if (_FinishDutyAndUnpausePreviousCommand == null)
                {
                    _FinishDutyAndUnpausePreviousCommand = new RelayCommand((p) => FinishDutyAndUnpausePrevious());
                }

                return _FinishDutyAndUnpausePreviousCommand;
            }
        }

        RelayCommand _FinishDutyAndStartNewCommand;

        public RelayCommand FinishDutyAndStartNewCommand
        {
            get
            {
                if (_FinishDutyAndStartNewCommand == null)
                {
                    _FinishDutyAndStartNewCommand = new RelayCommand((p) => FinishDutyAndStartNew());
                }

                return _FinishDutyAndStartNewCommand;
            }
        }

        RelayCommand _UnpauseDutyCommand;

        public RelayCommand UnpauseDutyCommand
        {
            get
            {
                if (_UnpauseDutyCommand == null)
                {
                    _UnpauseDutyCommand = new RelayCommand((p) => UnpauseDuty(p));
                }

                return _UnpauseDutyCommand;
            }
        }

        RelayCommand _OpenSettingsCommand;

        public RelayCommand OpenSettingsCommand
        {
            get
            {
                if (_OpenSettingsCommand == null)
                {
                    _OpenSettingsCommand = new RelayCommand((p) => OpenSettings());
                }

                return _OpenSettingsCommand;
            }
        }

        RelayCommand _FinishIteration;

        public RelayCommand FinishIteration
        {
            get
            {
                if (_FinishIteration == null)
                {
                    _FinishIteration = new RelayCommand((p) => FinishIterationAndStoreIt());
                }

                return _FinishIteration;
            }
        }

        RelayCommand _NewGroupCommand;

        public RelayCommand NewGroupCommand
        { get
            {
                if (_NewGroupCommand == null)
                {
                    _NewGroupCommand = new RelayCommand((p) => AddNewDutyGroup(p != null ? p.ToString() : null));
                }

                return _NewGroupCommand;
            }
        }

        RelayCommand _SetDutyGroupCommand;

        public RelayCommand SetDutyGroupCommand
        {
            get
            {
                if (_SetDutyGroupCommand == null)
                {
                    _SetDutyGroupCommand = new RelayCommand((p) => SetCurrentDutyGroup(p));
                }

                return _SetDutyGroupCommand;
            }
        }

        RelayCommand _AddKeywordCommand;

        public RelayCommand AddKeywordCommand
        {
            get
            {
                if (_AddKeywordCommand == null)
                {
                    _AddKeywordCommand = new RelayCommand((p) => AddKeyword(p));
                }

                return _AddKeywordCommand;
            }
        }

        RelayCommand _AddKeywordToListCommand;

        public RelayCommand AddKeywordToListCommand
        {
            get
            {
                if (_AddKeywordToListCommand == null)
                {
                    _AddKeywordToListCommand = new RelayCommand((p) => AddLastKeywordToList(p));
                }

                return _AddKeywordToListCommand;
            }
        }

        #endregion

        #endregion

        #region Storage Methods

        private async void SyncDutyGroups()
        {
            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations", "Common"));

            //TODO: add proper factory infrastructure
            LocalStorageProvider storage = new LocalStorageProvider();

            StorageResult = await storage.StoreGroups(AvailableGroups.ToList(), directory.FullName);
        }

        private async void LoadDutyGroups()
        {
            LocalStorageProvider storage = new LocalStorageProvider();

            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations", "Common"));

            StorageResult = await storage.LoadGroups(directory.FullName);

            if (StorageResult.Result != null)
            {
                string json = (string)StorageResult.Result;

                List<DutyGroup> lst = JsonConvert.DeserializeObject<List<DutyGroup>>(json);
                if (lst != null && lst.Count > 0)
                {
                    AvailableGroups = new ObservableCollection<DutyGroup>(lst);
                }
                else
                {
                    InitDummyGroups();
                }
            }
            else
            {
                InitDummyGroups();
            }
        }

        private async void LoadKeywords()
        {
            LocalStorageProvider storage = new LocalStorageProvider();

            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations", "Common"));

            StorageResult = await storage.LoadKeywords(directory.FullName);

            if (StorageResult.Result != null)
            {
                string val = StorageResult.Result.ToString();
                ObservableCollection<string> col = new ObservableCollection<string>();
                string[] tmp = val.Split(',');

                foreach (string kword in tmp)
                    col.Add(kword.Trim());

                AvailableKeywords = col;
            }
        }

        #endregion

        #region Dummy data

        private void InitDummyGroups()
        {
            if (_AvailableGroups == null)
            {
                AvailableGroups = new ObservableCollection<DutyGroup>();

                AvailableGroups.Add(new DutyGroup() { Name = "Programiranje" });
                AvailableGroups.Add(new DutyGroup() { Name = "Support" });
                AvailableGroups.Add(new DutyGroup() { Name = "Malica" });

            }
        }

        #endregion
    }
}
