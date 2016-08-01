using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskTimeTracker.ViewModel;

namespace TaskTimeTracker.View
{
    /// <summary>
    /// Interaction logic for BasicStatisticsView.xaml
    /// </summary>
    public partial class BasicStatisticsView : UserControl
    {
        //TODO: this is temp Add proper IOC support (statistics view model will also be used for charts and shit
        StatisticsViewModel _ViewModel;

        public BasicStatisticsView()
        {
            InitializeComponent();

            _ViewModel = new StatisticsViewModel();
            DataContext = _ViewModel;
        }

        private void LoadIterations(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
            {
                _ViewModel.LoadIterations(dlg.SelectedPath);
            }
        }
    }
}
