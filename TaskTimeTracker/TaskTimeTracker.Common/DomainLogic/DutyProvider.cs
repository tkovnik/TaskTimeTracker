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
            private set
            {
                _Iteration = value;
                NotifyPropertyChanged("Iteration");
            }
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
            //first we have to pause current ongoing duty
            PauseCurrentDuty();

            Duty duty = new Duty();
            duty.Status = (int)DutyStatus.Ongoing;

            _Iteration.Duties.Add(duty);
            OngoingDuty = duty;
        }

        /// <summary>
        /// Method finishes given duty and unapuses prevous duty if exists
        /// </summary>
        /// <param name="duty"></param>
        public void FinishDutyAndUnpausePrevious()
        {
            FinishDuty(OngoingDuty);

            Duty previousDuty = _Iteration.Duties.LastOrDefault(a => a.Status == (int)DutyStatus.Paused);

            if (previousDuty != null)
                UnpauseDuty(previousDuty);
            else
                StartNewDuty();
        }

        public void FinishDutyAndStartNew()
        {
            FinishDuty(OngoingDuty);

            StartNewDuty();
        }

        public void UnpauseDuty(Duty duty)
        {
            if(_OngoingDuty != duty)
            {
                //first we have to pause current ongoing duty
                PauseCurrentDuty();

                duty.TimeFrames.Add(new DutyTimeFrame() { From = DateTime.Now });
                duty.Status = (int)DutyStatus.Ongoing;
                _OngoingDuty = duty;
            }
        }

        /// <summary>
        /// Method finishes ongoing duty and finishes iteration (all duties are completed)
        /// </summary>
        public void EndIteration()
        {
            FinishDuty(OngoingDuty);
        }


        public void StartNewIteration()
        {
            //we have to prepaire provider for new iteration
            Iteration = new Iteration();
            OngoingDuty = null;
        }

        #endregion

        private void FinishDuty(Duty duty)
        {
            if (duty != null)
            {
                if (duty.Status != (int)DutyStatus.Completed)
                {
                    //lets close last time frame
                    DutyTimeFrame lastTimeFrame = duty.TimeFrames.Last();

                    lastTimeFrame.To = DateTime.Now;

                    duty.Status = (int)DutyStatus.Completed;
                }
            }
        }

        private void PauseCurrentDuty()
        {
            if(_Iteration.Duties.Count > 0)
            {
                Duty currentlyOngoingDuty = _Iteration.Duties.FirstOrDefault(a => a.Status == (int)DutyStatus.Ongoing);

                if(currentlyOngoingDuty != null)
                {
                    currentlyOngoingDuty.Status = (int)DutyStatus.Paused;
                    //we must end last time frame for this duty
                    DutyTimeFrame tf = currentlyOngoingDuty.TimeFrames.Last();
                    tf.To = DateTime.Now;
                }
            }
        }

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
