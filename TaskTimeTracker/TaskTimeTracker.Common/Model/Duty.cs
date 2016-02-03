using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Translation;

namespace TaskTimeTracker.Common.Model
{
    public class Duty : INotifyPropertyChanged
    {
        #region Fields

        private string _Name;
        private string _Description;
        private string _Keywords;
        private int _Status;

        private DutyGroup _Group;
        private List<DutyTimeFrame> _TimeFrames;

        #endregion

        #region Constructors

        public Duty()
            :this(null)
        {
        }

        public Duty(string name)
        {
            _TimeFrames = new List<DutyTimeFrame>();

            //should we set here status of duty or let the business logic take care of this?
            //_Status = (int)DutyStatus.Ongoing;

            //Adding the first timeframe
            DutyTimeFrame ttf = new DutyTimeFrame();
            _TimeFrames.Add(ttf);

            //setting default name if it's not provided
            if (string.IsNullOrWhiteSpace(name))
                Name = string.Format("{0}: {1}", Lang.TaskName_Default, ttf.ToString());
            else
                Name = name;
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
                if(_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }               
            }
        }

        public DutyGroup Group
        {
            get { return _Group; }
            set
            {
                if (_Group != value)
                {
                    _Group = value;
                    NotifyPropertyChanged("Group");
                }
            }
        }

        public string Keywords
        {
            get { return _Keywords; }
            set
            {
                if (_Keywords != value)
                {
                    _Keywords = value;
                    NotifyPropertyChanged("Keywords");
                }
            }
        }

        public int Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        public List<DutyTimeFrame> TimeFrames
        {
            get { return _TimeFrames; }
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

    public enum DutyStatus
    {
        Ongoing = 0,
        Paused = 1,
        Completed = 2
    }
}
