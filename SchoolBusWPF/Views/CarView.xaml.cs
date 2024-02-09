using SchoolBusWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolBusWPF.Views
{
    public partial class CarView : Page
    {
        public CarView()
        {
            InitializeComponent();
            DataContext = new CarViewModel();
        }

        private void PlateNumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int caretIndex = textBox.CaretIndex;

            if (textBox.Text.Length == 7 && !IsControlKey(e.Key))
            {
                e.Handled = true;
            }
            else if (caretIndex == 0 || caretIndex == 1 || caretIndex == 4 || caretIndex == 5 || caretIndex == 6)
            {
                if (!(IsNumericKey(e.Key) || IsControlKey(e.Key)))
                    e.Handled = true;
            }
            else if (caretIndex == 2 || caretIndex == 3)
            {
                if (!(IsLetterKey(e.Key) || IsControlKey(e.Key)))
                    e.Handled = true;

                char keyChar = KeyToChar(e.Key);

                if (char.IsLower(keyChar))
                {
                    textBox.Text = textBox.Text.Insert(caretIndex, char.ToUpper(keyChar).ToString());
                    textBox.CaretIndex = caretIndex + 1;
                    e.Handled = true;
                }
            }
        }

        private void SeatCountTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(IsNumericKey(e.Key) || IsControlKey(e.Key)))
                e.Handled = true;
        }

        private static bool IsNumericKey(Key key)
        {
            return (key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9);
        }

        private static bool IsLetterKey(Key key)
        {
            return (key >= Key.A && key <= Key.Z);
        }

        private static bool IsControlKey(Key key)
        {
            return key == Key.Back || key == Key.Delete || key == Key.Left || key == Key.Right;
        }

        private static char KeyToChar(Key key)
        {
            return key >= Key.A && key <= Key.Z ? (char)('a' + (key - Key.A)) : '\0';
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var button = (sender as Button)!;
            var context = button.DataContext!;
            var viewModel = (DataContext as CarViewModel)!;

            viewModel.DeleteEntity(context);
        }
    }
}
