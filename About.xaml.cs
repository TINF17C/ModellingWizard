
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Aml.Editor.Plugin
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            this.DataContext = this;
            InitializeComponent();

            var assembly = Assembly.GetCallingAssembly();

            txtVersion.Text = "Version "+ assembly.GetName().Version.ToString();
            
            
        }       

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public string Version
        {
            get;
            set;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
