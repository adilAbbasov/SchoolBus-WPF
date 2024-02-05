using MaterialDesignThemes.Wpf;
using SchoolBusWPF.Models.Concretes;
using SchoolBusWPF.ViewModels;
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

        private void StudentButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not Models.Concretes.Student student)
                return;

            if (DataContext is not RideViewModel viewModel)
                return;

            var result = viewModel.ModifyStudent(student);

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
    }
}
