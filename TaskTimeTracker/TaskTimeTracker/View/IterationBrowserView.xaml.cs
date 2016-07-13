using Microsoft.Win32;
using System.Windows.Controls;
using TaskTimeTracker.ViewModel;

namespace TaskTimeTracker.View
{
    /// <summary>
    /// Interaction logic for IterationBrowserView.xaml
    /// </summary>
    public partial class IterationBrowserView : UserControl
    {
        IterationBrowserViewModel _ViewModel;

        public IterationBrowserView()
        {
            InitializeComponent();

            _ViewModel = new IterationBrowserViewModel();
            DataContext = _ViewModel;
        }

        private void LoadIterations(object sender, System.Windows.RoutedEventArgs e)
        {
            
            //System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            //if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
            //{
            //    //load iterations
            //    _ViewModel.LoadIterations(dlg.SelectedPath);
            //}
        }
    }
}
