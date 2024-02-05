using SchoolBusWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolBusWPF.Views
{
	public partial class Driver : Page
    {
        public Driver()
        {
            InitializeComponent();
			DataContext = new DriverViewModel();
        }

		private static bool IsAllowedKey(Key key)
		{
			return (key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9) || key == Key.Back || key == Key.Delete || key == Key.Left || key == Key.Right;
		}

		private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;

			if (!IsAllowedKey(e.Key))
			{
				e.Handled = true;
				return;
			}

			if (textBox.Text.Length == 9 && (e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Left && e.Key != Key.Right))
			{
				e.Handled = true;
				return;
			}
		}

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = sender as PasswordBox;
			var viewModel = DataContext as DriverViewModel;

			viewModel.Password = passwordBox.Password;
		}
	}
}
