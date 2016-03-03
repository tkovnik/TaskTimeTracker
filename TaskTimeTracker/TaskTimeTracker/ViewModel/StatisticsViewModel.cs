using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;
using TaskTimeTracker.ViewModel.Base;

namespace TaskTimeTracker.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        #region Constructors

        public StatisticsViewModel()
        {
            InitTestStatistics();
        }

        #endregion

        #region Test Data

        private ObservableCollection<DutyWrapper> _Statistics;
        public ObservableCollection<DutyWrapper> Statistics
        {
            get { return _Statistics; }
            set
            {
                _Statistics = value;
                NotifyPropertyChanged(() => Statistics);
            }
        }

        private void InitTestStatistics()
        {
            Random rnd = new Random();
            Statistics = new ObservableCollection<DutyWrapper>();

            List<DutyGroup> listGroup = new List<DutyGroup>();
            listGroup.Add(new DutyGroup() { Name = "Programiranje" });
            listGroup.Add(new DutyGroup() { Name = "Support" });
            listGroup.Add(new DutyGroup() { Name = "Testiranje" });
            listGroup.Add(new DutyGroup() { Name = "Dokumentacija" });

            List<DutyWrapper> listDuties = new List<DutyWrapper>();

            for (int i = 0; i < 100; i++)
            {
                Duty dut = new Duty("Task " + i);
                dut.TimeFrames.Clear();
                DutyTimeFrame frame = new DutyTimeFrame();
                frame.From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
                frame.To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 0, 0);
                dut.TimeFrames.Add(frame);
                int r = rnd.Next(4);
                dut.Group = listGroup[r];

                int s = rnd.Next(5);
                listDuties.Add(new DutyWrapper() { Duty = dut, Number = s, Display = dut.Group.Name });
                //dut.TotalTimeSpent
            }

            foreach (DutyWrapper dutWrap in listDuties)
            {
                DutyWrapper realWrapper = Statistics.FirstOrDefault(a => a.Display == dutWrap.Duty.Group.Name);

                if (realWrapper != null)
                {
                    realWrapper.Number += dutWrap.Number;
                }
                else
                {
                    Statistics.Add(dutWrap);
                }
            }
        }

        #endregion
    }

    public class DutyWrapper
    {
        public string Display { get; set; }
        public Duty Duty { get; set; }
        public int Number { get; set; }
    }
}
