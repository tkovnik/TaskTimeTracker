using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;
using TaskTimeTracker.View.Base;

namespace TaskTimeTracker.View
{
    /// <summary>
    /// This is the underlying view model which holds any potencial "sub"
    /// </summary>
    public class Workspace : ViewModelBase
    {
        #region Fields

        private Duty _CurrentDuty;

        #endregion

        #region Constructors

        public Workspace()
        {

        }

        #endregion

        #region Iteration

        /// <summary>
        /// Gets or sets duty which is currently in progress
        /// </summary>
        public Duty CurrentDuty
        {
            get { return _CurrentDuty; }
            set
            {
                _CurrentDuty = value;
                NotifyPropertyChanged(() => this.CurrentDuty);
            }
        }

        #endregion
    }
}
