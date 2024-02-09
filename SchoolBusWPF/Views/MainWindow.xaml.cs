using SchoolBusWPF.ViewModels;
using SchoolBusWPF.Views;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SchoolBusWPF
{
    public partial class MainWindow : Window
    {
        private Dictionary<Type, Func<object>> viewModelFactories;

        public MainWindow()
        {
            InitializeComponent();

            mainFrame.Navigate(new Uri("Views/RideView.xaml", UriKind.Relative));

            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!.Split("bin")[0];
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

        private void Attendances_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/AttendanceView.xaml", UriKind.Relative));
        }

        private void Cars_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/CarView.xaml", UriKind.Relative));
        }

        private void Drivers_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/DriverView.xaml", UriKind.Relative));
        }

        private void Groups_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/GroupView.xaml", UriKind.Relative));
        }

        private void Holidays_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/HolidayView.xaml", UriKind.Relative));
        }

        private void Parents_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/ParentView.xaml", UriKind.Relative));
        }

        private void Rides_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/RideView.xaml", UriKind.Relative));
        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/StudentView.xaml", UriKind.Relative));
        }

        private void SearchTxtBx_TextChanged(object sender, TextChangedEventArgs e)
        {
            var pattern = (sender as TextBox)?.Text!;
            var content = mainFrame.Content!;

            if (content is CarView)
            {
                ((content as CarView)?.DataContext as CarViewModel)?.SearchData(pattern);
            }
            else if (content is DriverView)
            {
                ((content as DriverView)?.DataContext as DriverViewModel)?.SearchData(pattern);
            }
            else if (content is GroupView)
            {
                ((content as GroupView)?.DataContext as GroupViewModel)?.SearchData(pattern);
            }
            else if (content is HolidayView)
            {
                ((content as HolidayView)?.DataContext as HolidayViewModel)?.SearchData(pattern);
            }
            else if (content is ParentView)
            {
                ((content as ParentView)?.DataContext as ParentViewModel)?.SearchData(pattern);
            }
            else if (content is RideView)
            {
                ((content as RideView)?.DataContext as RideViewModel)?.SearchData(pattern);
            }
            else if (content is StudentView)
            {
                ((content as StudentView)?.DataContext as StudentViewModel)?.SearchData(pattern);
            }
        }
    }
}