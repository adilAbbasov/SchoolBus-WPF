using SchoolBusWPF.ViewModels;
using System.Windows.Controls;

namespace SchoolBusWPF.Views
{
	public partial class HolidayView : Page
    {
        public HolidayView()
        {
            InitializeComponent();
            DataContext = new HolidayViewModel();
        }
	}
}
