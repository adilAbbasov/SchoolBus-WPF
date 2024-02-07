using SchoolBusWPF.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolBusWPF.Views
{
	public partial class ParentWindow : Page
	{
		public ParentWindow()
		{
			InitializeComponent();
			DataContext = new ParentViewModel();
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

			if(textBox.Text.Length == 9 && (e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Left && e.Key != Key.Right))
			{
				e.Handled = true;
				return;
			}
		}

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = sender as PasswordBox;
			var viewModel = DataContext as ParentViewModel;
			viewModel.Password = passwordBox.Password;
		}

		private void DeleteButtonClick(object sender, RoutedEventArgs e)
		{
			if (sender is null || sender is not Button button)
				return;

			var result = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				var viewModel = DataContext as ParentViewModel;
				var context = button.DataContext;

				if (viewModel is null || context is null)
					return;

				viewModel.DeleteEntity(context);
			}
		}
	}
}
