using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;

namespace TaskTimeTracker.Common.DomainLogic
{
    public class DutyProvider : INotifyPropertyChanged
    {
        #region Fields

        private Iteration _Iteration;

        private Duty _OngoingDuty;

        #endregion

        #region Constructors

        public DutyProvider()
        {
            _Iteration = new Iteration();
        }

        #endregion

        #region Public Properties

        public Iteration Iteration
        {
            get { return _Iteration; }
        }

        public Duty OngoingDuty
        {
            get
            {
                return _OngoingDuty;
            }

            set
            {
                if (_OngoingDuty != value)
                {
                    _OngoingDuty = value;
                    NotifyPropertyChanged("OngoingDuty");
                }
            }
        }

        #endregion

        #region Public Methods

        public void StartNewDuty()
        {
            //TODO: if we have current ongoing duty we have to pause it

            Duty duty = new Duty();
            duty.Status = (int)DutyStatus.Ongoing;

            _Iteration.Duties.Add(duty);
            OngoingDuty = duty;
        }

        public void FinishDuty(Duty duty)
        {
            //if we don't have no timeframe we should fall
            var lastTimeFrame = duty.TimeFrames.Last();
            lastTimeFrame.To = DateTime.Now;

            duty.Status = (int)DutyStatus.Completed;

            //TODO: Should we unpause previous ongoing duty?
            //TODO: should we start new duty if all previous duties are completed?
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
