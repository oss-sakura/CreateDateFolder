using System.Windows;

namespace CreateDateFolder
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            hyperlink.RequestNavigate += (sender, e) =>
            {
                _ = System.Diagnostics.Process.Start(e.Uri.ToString());
            };
        }
    }
}
