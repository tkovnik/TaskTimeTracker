using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Duty _CurrentDuty;

        private DutyProvider _Provider;

        public DutyProvider Provider
        {
            get { return _Provider; }
            set
            {
                _Provider = value;
                NotifyPropertyChanged(() => this.Provider);
            }
        }

        #endregion

        #region Constructors

        public Workspace()
        {
            Provider = new DutyProvider();
        }

        #endregion

        #region Iteration

        /// <summary>
        /// Gets or sets duty which is currently in progress
        /// </summary>
        public Duty CurrentDuty
        {
            get { return Provider.OngoingDuty; }
            //set
            //{
            //    _CurrentDuty = value;
            //    NotifyPropertyChanged(() => this.CurrentDuty);
            //}
        }

        #endregion

        public void NewDuty()
        {
            Provider.StartNewDuty();
            NotifyPropertyChanged(() => this.CurrentDuty);
        }
    }
}
