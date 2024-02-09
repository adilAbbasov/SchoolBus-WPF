using SchoolBusWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SchoolBusWPF.Views
{
    public partial class AttendanceView : Page
    {
        public AttendanceView()
        {
            InitializeComponent();
            DataContext = new AttendanceViewModel();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var button = (sender as Button)!;
            var context = button.DataContext!;
            var viewModel = (DataContext as AttendanceViewModel)!;

            viewModel.DeleteEntity(context);
        }
    }
}
