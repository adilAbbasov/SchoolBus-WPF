using SchoolBusWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SchoolBusWPF.Views
{
    public partial class Student : Page
    {
        public Student()
        {
            InitializeComponent();
            DataContext = new StudentViewModel();
        }
	}
}
