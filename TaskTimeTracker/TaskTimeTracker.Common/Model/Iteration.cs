using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Common.Model
{
    /// <summary>
    /// Holds information about task which are created in current iteration (eg. workday)
    /// </summary>
    public class Iteration : INotifyPropertyChanged
    {
        #region Fields

        private string _Name;
        private string _Description;

        private ObservableCollection<Duty> _Duties;

        #endregion

        #region Constructors

        public Iteration()
        {
            _Duties = new ObservableCollection<Duty>();
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<Duty> Duties
        {
            get { return _Duties; }
            set
            {
                _Duties = value;
                NotifyPropertyChanged("Duties");
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
