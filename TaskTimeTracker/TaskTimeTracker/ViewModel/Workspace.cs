using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TaskTimeTracker.Command;
using TaskTimeTracker.Common.DomainLogic;
using TaskTimeTracker.Common.Model;
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

        #endregion

        #region Constructors

        public Workspace()
        {
            Provider = new DutyProvider();
            _Timer = new DispatcherTimer();
            _Timer.Interval = TimeSpan.FromSeconds(1);
            _Timer.Tick += TimerTick;
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

        #endregion

        #region Private Methods

        private void StartNewDuty()
        {
            _Provider.StartNewDuty();
            NotifyPropertyChanged(() => this.CurrentDuty);
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

        private void OpenSettings()
        {
            IsSettingsOpen = true;
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

        #endregion

        #endregion
    }
}
