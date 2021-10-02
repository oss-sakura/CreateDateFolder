using System.Windows;
using System.Windows.Input;

namespace CreateDateFolder
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            ReadProperties();
            InitControls();
        }

        private void ReadProperties()
        {
            SeqNoCheckBox.IsChecked = Properties.Settings.Default.AddSeqNo;
            SeqNoTextBox.Text = Properties.Settings.Default.ZeroPaddingLength.ToString();
            DelimiterComboBox.Text = Properties.Settings.Default.DelimiterChar;
        }

        private void SaveProperties()
        {
            Properties.Settings.Default.AddSeqNo = SeqNoCheckBox.IsChecked.Value;
            Properties.Settings.Default.ZeroPaddingLength = int.TryParse(SeqNoTextBox.Text, out int len) ? len : 1;
            Properties.Settings.Default.DelimiterChar = DelimiterComboBox.Text;
            Properties.Settings.Default.Save();
        }

        private void SeqNoValidation(object sender, TextCompositionEventArgs e)
        {
            int i = 0;
            e.Handled = true;
            if (int.TryParse(e.Text, out i) && i > 0)
            {
                e.Handled = false;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveProperties();
            Application.Current.Shutdown();
        }

        private void InitControls()
        {
            if (!SeqNoCheckBox.IsChecked.Value)
            {
                SeqNoTextBox.IsEnabled = false;
                DelimiterComboBox.IsEnabled = false;
            }
            else
            {
                SeqNoTextBox.IsEnabled = true;
                DelimiterComboBox.IsEnabled = true;
            }
        }

        private void SeqNoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            InitControls();
        }

        private void SeqNoTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(SeqNoTextBox.Text.Trim()))
            {
                SeqNoTextBox.Text = 1.ToString();
            }
         }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow w = new AboutWindow();
            w.ShowDialog();
        }

        private void QuitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveProperties();
            Application.Current.Shutdown();
        }
    }

}
