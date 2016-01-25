using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Translation;

namespace TaskTimeTracker.Common.Model
{
    public class Task : INotifyPropertyChanged
    {
        #region Fields

        private string _Name;
        private string _Description;
        private string _Keywords;
        private int _TaskStatus;

        private TaskGroup _Group;
        private List<TaskTimeFrame> TimeFrames;

        #endregion

        #region Constructors

        public Task()
            :this(null)
        {
        }

        public Task(string name)
        {
            TimeFrames = new List<TaskTimeFrame>();

            //Adding the first timeframe
            TaskTimeFrame ttf = new TaskTimeFrame();
            TimeFrames.Add(ttf);

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

        public TaskGroup Group
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

        public int TaskStatus
        {
            get { return _TaskStatus; }
            set
            {
                if (_TaskStatus != value)
                {
                    _TaskStatus = value;
                    NotifyPropertyChanged("TaskStatus");
                }
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

    public enum TaskStatus
    {
        Ongoing = 0,
        Paused = 1,
        Completed = 2
    }
}
