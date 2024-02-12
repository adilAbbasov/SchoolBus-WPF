using SchoolBusWPF.ViewModels;
using SchoolBusWPF.Views;
using System.Windows;
using System.Windows.Controls;

namespace SchoolBusWPF
{
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new Uri("Views/RideView.xaml", UriKind.Relative));
        }

        private void Attendances_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if(mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/AttendanceView.xaml", UriKind.Relative));
        }

        private void Cars_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if (mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/CarView.xaml", UriKind.Relative));
        }

        private void Drivers_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if (mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/DriverView.xaml", UriKind.Relative));
        }

        private void Groups_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if (mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/GroupView.xaml", UriKind.Relative));
        }

        private void Holidays_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if (mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/HolidayView.xaml", UriKind.Relative));
        }

        private void Parents_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if (mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/ParentView.xaml", UriKind.Relative));
        }

        private void Rides_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            mainFrame.Navigate(new Uri("Views/RideView.xaml", UriKind.Relative));
        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBx.Text = string.Empty;

            if (mainFrame.Content is RideView content)
                content.RemoveRidePopup();

            mainFrame.Navigate(new Uri("Views/StudentView.xaml", UriKind.Relative));
        }

        private void SearchTxtBx_TextChanged(object sender, TextChangedEventArgs e)
        {
            var content = mainFrame.Content!;
            var pattern = (sender as TextBox)?.Text!;

            if (content is AttendanceView)
                ((content as AttendanceView)?.DataContext as AttendanceViewModel)?.SearchData(pattern);
            else if (content is CarView)
                ((content as CarView)?.DataContext as CarViewModel)?.SearchData(pattern);
            else if (content is DriverView)
                ((content as DriverView)?.DataContext as DriverViewModel)?.SearchData(pattern);
            else if (content is GroupView)
                ((content as GroupView)?.DataContext as GroupViewModel)?.SearchData(pattern);
            else if (content is HolidayView)
                ((content as HolidayView)?.DataContext as HolidayViewModel)?.SearchData(pattern);
            else if (content is ParentView)
                ((content as ParentView)?.DataContext as ParentViewModel)?.SearchData(pattern);
            else if (content is RideView)
                ((content as RideView)?.DataContext as RideViewModel)?.SearchData(pattern);
            else if (content is StudentView)
                ((content as StudentView)?.DataContext as StudentViewModel)?.SearchData(pattern);
        }
    }
}