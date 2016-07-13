using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;

namespace TaskTimeTracker.Model
{
    /// <summary>
    /// Wrapper around basic Iteration object for additional info
    /// </summary>
    public class BrowsedIteration : INotifyPropertyChanged
    {
        #region Fields

        private string _Source;
        private string _Name;
        private Iteration _Iteration;

        #endregion

        #region Constructors

        public BrowsedIteration()
        {

        }

        public string Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
                NotifyPropertyChanged("Source");
            }
        }

        public Iteration Iteration
        {
            get { return _Iteration; }
            set
            {
                _Iteration = value;
                NotifyPropertyChanged("Iteration");
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
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
