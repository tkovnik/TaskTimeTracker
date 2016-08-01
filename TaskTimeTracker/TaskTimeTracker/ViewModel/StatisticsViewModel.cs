using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;
using TaskTimeTracker.Model;
using TaskTimeTracker.Storage;
using TaskTimeTracker.ViewModel.Base;

namespace TaskTimeTracker.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        #region Fields

        BasicStatistic _BasicStatistics;
        #endregion

        #region Constructors

        public StatisticsViewModel()
        {
            InitTestStatistics();

            // TODO: add property to UI for DefaultHourWorkDay
            DefaultHourWorkDay = new TimeSpan(8, 0, 0);
        }

        #endregion

        #region Public Properties

        public TimeSpan DefaultHourWorkDay { get; set; }

        public bool CheckSubFolder { get; set; }

        public ObservableCollection<BrowsedIteration> BrowsedIterations { get; private set; }

        public BasicStatistic BasicStatistics
        {
            get { return _BasicStatistics; }
            private set
            {
                _BasicStatistics = value;
                NotifyPropertyChanged(() => this.BasicStatistics);
            } 
        }

        #endregion

        #region Basic statistics

        #region Public Methods

        public async void LoadIterations(string directory)
        {
            LocalStorageProvider provider = new LocalStorageProvider();

            StorageResult result = await provider.LoadStoredIterationsAsync(directory, CheckSubFolder);


            if(result.Status == StorageStatus.Success || result.Status == StorageStatus.Warning)
            {
                BrowsedIterations = new ObservableCollection<BrowsedIteration>((List<BrowsedIteration>)result.Result);

                // Generate statistics
                GenerateBasicStatistics();
            }
        }

        #endregion

        private void GenerateBasicStatistics()
        {
            if(BrowsedIterations != null && BrowsedIterations.Count > 0)
            {
                BasicStatistic stat = new BasicStatistic();

                foreach (BrowsedIteration iteration in BrowsedIterations)
                {
                    stat.DaysCount++;

                    TimeSpan total = iteration.Iteration.TotalTime;
                    stat.HoursSpent += total;

                    if(total > DefaultHourWorkDay)
                    {
                        //we have overtime
                        TimeSpan overtime = total - DefaultHourWorkDay;
                        stat.Overtime += overtime;
                    }                  
                }

                BasicStatistics = stat;
                 
            }
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

    public class BasicStatistic
    {
        public BasicStatistic()
        {
            HoursSpent = new TimeSpan(0, 0, 0);
            Overtime = new TimeSpan(0, 0, 0);
        }

        public int DaysCount { get; set; }
        public TimeSpan HoursSpent { get; set; }
        public TimeSpan Overtime { get; set; }
        public WorkingDay LongestWorkingDay { get; set; }
    }

    public class WorkingDay
    {
        public TimeSpan TimeSpent { get; set; }
        public DateTime Date { get; set; }
    }

    public class DutyWrapper
    {
        public string Display { get; set; }
        public Duty Duty { get; set; }
        public int Number { get; set; }
    }
}
