using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Common.Model
{
    /// <summary>
    /// Task can be interrupted with other task taking priority (new ongoing task)
    /// So every task can have multiple timeframes
    /// </summary>
    public class DutyTimeFrame : INotifyPropertyChanged
    {
        #region Fields

        private DateTime _From;
        private DateTime? _To;

        #endregion

        #region Constructors

        public DutyTimeFrame()
        {
            From = DateTime.Now;
        }

        #endregion

        #region Public Properties

        public DateTime From
        {
            get { return _From; }
            set
            {
                if (_From != value)
                {
                    _From = value;
                    NotifyPropertyChanged("From");
                }
            }
        }

        public DateTime? To
        {
            get { return _To; }
            set
            {
                if (_To != value)
                {
                    _To = value;
                    NotifyPropertyChanged("To");
                }
            }
        }

        /// <summary>
        /// Returns time spent within this timeframe
        /// If the time frame has not end time property returns null
        /// </summary>
        public TimeSpan? SpentTime
        {
            get
            {
                if(_To != null)
                {
                    var st = this._To - this._From;
                    return st;
                }

                return null;
            }
        }

        #endregion

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyInfo)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyInfo));
            }
        }

        #endregion
    }
}
