using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.Model;
using TaskTimeTracker.Model;
using TaskTimeTracker.Storage;
using TaskTimeTracker.ViewModel.Base;

namespace TaskTimeTracker.ViewModel
{
    class IterationBrowserViewModel : ViewModelBase
    {
        private ObservableCollection<BrowsedIteration> _BrowsedIterations;
        private Iteration _CurrentIteration;

        #region Constructors

        public IterationBrowserViewModel()
        {
            _BrowsedIterations = new ObservableCollection<BrowsedIteration>();
        }

        #endregion

        #region Public Properties

        public ObservableCollection<BrowsedIteration> BrowsedIterations
        {
            get
            {
                return _BrowsedIterations;
            }

            set
            {
                _BrowsedIterations = value;
                NotifyPropertyChanged(() => BrowsedIterations);
            }
        }

        public Iteration CurrentIteration
        {
            get
            {
                return _CurrentIteration;
            }

            set
            {
                _CurrentIteration = value;
                NotifyPropertyChanged(() => CurrentIteration);
            }
        }



        #endregion

        public async void LoadIterations(string directory)
        {
            LocalStorageProvider provider = new LocalStorageProvider();

            StorageResult result = await provider.LoadStoredIterations(directory);

            if((result.Status == StorageStatus.Success || result.Status == StorageStatus.Warning) && result.Result != null)
            {
                BrowsedIterations = new ObservableCollection<BrowsedIteration>((List<BrowsedIteration>)result.Result);
            }
        }
    }
}
