using MaterialDesignThemes.Wpf;
using SchoolBusWPF.Models.Concretes;
using SchoolBusWPF.ViewModels;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolBusWPF.Views
{
	public partial class RideView : Page
	{
		private DispatcherTimer timer;
		private int progressValue;
		private int count = 0;

		public RideView()
		{
			InitializeComponent();
			DataContext = new RideViewModel();

			//var mapControl = new Mapconrol

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

			var context = (sender as Button)!.DataContext;
			ShowRidePopup(viewModel, context);
		}

		private void ShowRidePopup(RideViewModel viewModel, object context)
		{
			ridePopup.IsOpen = true;

			timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.04)
			};

			timer.Tick += (sender, e) => Timer_Tick(sender!, e, viewModel, context);
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e, RideViewModel viewModel, object context)
		{
			rideProgressBar.Value = progressValue++;
			var value = rideProgressBar.Value / 10;

			if ((value % 1) == 0)
				UpdateDisplayString();

			if (progressValue > 100)
			{
				viewModel.StartRide(context);

				rideBorder.CornerRadius = new CornerRadius(0);
				closeButton.IsEnabled = true;
                rideStart.Visibility = Visibility.Collapsed;
				map.Visibility = Visibility.Visible;

				timer.Stop();
			}
		}

		private void UpdateDisplayString()
		{
			if (count < 3)
			{
				rideTextBlock.Text += ".";
				count++;
			}
			else
			{
				rideTextBlock.Text = "Ride is starting ";
				count = 0;
			}
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			ridePopup.IsOpen = false;

		}

		public void RemoveRidePopup()
		{
            if (ridePopup.Parent is Panel parent)
            {
                parent.Children.Remove(ridePopup);
            }
        }
    }
}
