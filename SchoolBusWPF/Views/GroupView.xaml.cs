using SchoolBusWPF.ViewModels;
using System.Windows.Controls;

namespace SchoolBusWPF.Views
{
	public partial class Group : Page
    {
        public Group()
        {
            InitializeComponent();
            DataContext = new GroupViewModel();
        }
	}
}
