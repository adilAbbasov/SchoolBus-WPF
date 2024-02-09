using MaterialDesignThemes.Wpf;
using SchoolBusWPF.Models.Concretes;
using SchoolBusWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchoolBusWPF.Views
{
    public partial class RideView : Page
    {
        public RideView()
        {
            InitializeComponent();
            DataContext = new RideViewModel();
        }

        private void StudentButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button)!;
            var context = (button.DataContext as Student)!;
            var viewModel = (DataContext as RideViewModel)!;
           
            var result = viewModel.UpdateStudent(context);

            switch (result)
            {
                case 0:
                    return;
                case 1:
                    {
                        if (button.Content is PackIcon icon)
                        {
                            icon.Kind = PackIconKind.RemoveBold;
                            icon.Foreground = new SolidColorBrush(Colors.OrangeRed);
                        }
                    }
                    break;
                case -1:
                    {
                        if (button.Content is PackIcon icon)
                        {
                            icon.Kind = PackIconKind.AddBold;
                            icon.Foreground = new SolidColorBrush(Colors.ForestGreen);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var button = (sender as Button)!;
            var context = button.DataContext!;
            var viewModel = (DataContext as RideViewModel)!;

            viewModel.DeleteEntity(context);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to start this ride?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var viewModel = (DataContext as RideViewModel)!;
            var canStart = viewModel.CanStartRide();

            if (!canStart)
            {
                MessageBox.Show("Ride cannot start on holidays!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //

            var context = (sender as Button)!.DataContext;
            viewModel.StartRide(context);
        }
    }
}
