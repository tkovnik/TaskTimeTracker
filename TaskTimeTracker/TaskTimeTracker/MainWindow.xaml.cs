using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TaskTimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Workspace workspace;
        public MainWindow()
        {
            InitializeComponent();

            //apply main view model
            workspace = new Workspace();
            this.DataContext = workspace;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            workspace.NewDuty();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //workspace.ListTest.Add("D");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            workspace.Provider.FinishDutyAndUnpausePrevious();
        }
    }
}
