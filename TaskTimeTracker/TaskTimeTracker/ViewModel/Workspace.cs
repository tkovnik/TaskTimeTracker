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

        #endregion

        #region Constructors

        public Workspace()
        {
            Provider = new DutyProvider();
            _Timer = new DispatcherTimer();
            _Timer.Interval = TimeSpan.FromSeconds(1);
            _Timer.Tick += TimerTick;

            InitDummyGroups();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _Elapsed = _Elapsed.Add(TimeSpan.FromSeconds(1));
            ElapsedTime = _Elapsed.ToString("hh\\:mm\\:ss");
        }

        #endregion

        #region Public Properties

        public DutyProvider Provider
        {
            get { return _Provider; }
            private set
            {
                _Provider = value;
                NotifyPropertyChanged(() => this.Provider);
            }
        }

        public string ElapsedTime
        {
            get { return _ElapsedTime; }
            set
            {
                _ElapsedTime = value;
                NotifyPropertyChanged(() => this.ElapsedTime);
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
                if(_Provider.OngoingDuty != null)
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
                    NotifyPropertyChanged(() => this.IsSettingsOpen);
                }
            }
        }

        public StorageResult StorageResult
        {
            get { return _StorageResult; }
            set
            {
                _StorageResult = value;
                NotifyPropertyChanged(() => this.StorageResult);
            }
        }

        public ObservableCollection<DutyGroup> AvailableGroups
        {
            get{ return _AvailableGroups; }
            set
            {
                _AvailableGroups = value;
                NotifyPropertyChanged(() => AvailableGroups);
            }
        }

        #endregion

        #region Private Methods

        private void StartNewDuty()
        {
            _Provider.StartNewDuty();
            NotifyPropertyChanged(() => this.CurrentDuty);
            SetCurrentDutyGroup(new DutyGroup() { Name = "Add current group..." });
            SetAndStartTimer();
        }

        private void FinishDutyAndUnpausePrevious()
        {
            _Provider.FinishDutyAndUnpausePrevious();
            NotifyPropertyChanged(() => this.CurrentDuty);
            SetAndStartTimer();
        }

        private void FinishDutyAndStartNew()
        {
            _Provider.FinishDutyAndStartNew();
            NotifyPropertyChanged(() => this.CurrentDuty);
            SetAndStartTimer();
        }

        private void SetAndStartTimer()
        {
            _Elapsed = CurrentDuty.TotalTimeSpent;
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

            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iterations"));

            //TODO: add proper factory infrastructure
            LocalStorageProvider storage = new LocalStorageProvider();
            StorageResult = await storage.StoreIteration(_Provider.Iteration, directory.FullName);

            _Provider.StartNewIteration();

            NotifyPropertyChanged(() => CurrentDuty);
            ResetAndStopTimer();
        }

        private void AddNewDutyGroup(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                AvailableGroups.Add(new DutyGroup() { Name = name });

                //TODO: store here storage
            }
        }

        private void SetCurrentDutyGroup(object dutyGroup)
        {
            DutyGroup dg = dutyGroup as DutyGroup;

            if(dg != null && _Provider.OngoingDuty != null)
            {
                //Todo: maybe put this logic to provider;
                _Provider.OngoingDuty.Group = dg;
                NotifyPropertyChanged(() => CurrentDutyGroup);
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
                if(_StartNewDutyCommand == null)
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
                if(_FinishDutyAndUnpausePreviousCommand == null)
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
                if(_FinishDutyAndStartNewCommand == null)
                {
                    _FinishDutyAndStartNewCommand = new RelayCommand((p) => FinishDutyAndStartNew());
                }

                return _FinishDutyAndStartNewCommand;
            }
        }

        RelayCommand _OpenSettingsCommand;

        public RelayCommand OpenSettingsCommand
        {
            get
            {
                if(_OpenSettingsCommand == null)
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
                if(_FinishIteration == null)
                {
                    _FinishIteration = new RelayCommand((p) => FinishIterationAndStoreIt());
                }

                return _FinishIteration;
            }
        }

        RelayCommand _NewGroupCommand;

        public RelayCommand NewGroupCommand
        {   get
            {
                if(_NewGroupCommand == null)
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
                if(_SetDutyGroupCommand == null)
                {
                    _SetDutyGroupCommand = new RelayCommand((p) => SetCurrentDutyGroup(p));
                }

                return _SetDutyGroupCommand;
            }
        }

        #endregion

        #endregion

        #region Dummy data

        private void InitDummyGroups()
        {
            if(_AvailableGroups == null)
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
