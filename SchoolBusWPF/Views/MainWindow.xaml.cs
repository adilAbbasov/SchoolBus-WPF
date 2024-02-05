using System.Windows;

namespace SchoolBusWPF
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			mainFrame.Navigate(new Uri("Views/DriverView.xaml", UriKind.Relative));
		}

		private void Rides_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/RideView.xaml", UriKind.Relative));
		}

		private void Groups_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/GroupView.xaml", UriKind.Relative));
		}

		private void Students_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/StudentView.xaml", UriKind.Relative));
		}

		private void Parents_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/ParentView.xaml", UriKind.Relative));
		}

		private void Drivers_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/DriverView.xaml", UriKind.Relative));
		}

		private void Cars_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/CarView.xaml", UriKind.Relative));
		}

		private void Holidays_Click(object sender, RoutedEventArgs e)
		{
			mainFrame.Navigate(new Uri("Views/HolidayView.xaml", UriKind.Relative));
		}
	}
}