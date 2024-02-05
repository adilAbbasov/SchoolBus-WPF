using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SchoolBusWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            mainFrame.Navigate(new Uri("Views/DriverView.xaml", UriKind.Relative));

            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Split("bin")[0];

            var schoolBusFullPath = Path.Combine(outPutDirectory, "Images\\schoolbus.jpg");
            var userFullPath = Path.Combine(outPutDirectory, "Images\\user.jpg");

            var schoolImage = new BitmapImage();
            schoolImage.BeginInit();
            schoolImage.UriSource = new Uri(schoolBusFullPath);
            schoolImage.EndInit();

            var userImage = new BitmapImage();
            userImage.BeginInit();
            userImage.UriSource = new Uri(userFullPath);
            userImage.EndInit();

            this.schoolImage.Source = schoolImage;
            this.userImage.Source = userImage;
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