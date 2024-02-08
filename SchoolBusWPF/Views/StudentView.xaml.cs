using SchoolBusWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SchoolBusWPF.Views
{
    public partial class StudentView : Page
    {
        public StudentView()
        {
            InitializeComponent();
            DataContext = new StudentViewModel();
        }

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is null || sender is not Button button)
				return;

			var result = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				var viewModel = DataContext as StudentViewModel;
				var context = button.DataContext;

				if (viewModel is null || context is null)
					return;

				viewModel.DeleteEntity(context);
			}
		}
	}
}
