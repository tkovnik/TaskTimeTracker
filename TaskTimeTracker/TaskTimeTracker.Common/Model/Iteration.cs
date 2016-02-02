using System;
using System.Collections.Generic;
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

        private List<Task> _Tasks;

        #endregion

        #region Constructors

        public Iteration()
        {
            _Tasks = new List<Task>();
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

        public List<Task> Tasks
        {
            get { return _Tasks; }
            set
            {
                _Tasks = value;
                NotifyPropertyChanged("Description");
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
