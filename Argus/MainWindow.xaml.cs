using System.IO;
using System.Windows;
using Argus.Backend;
using GraphDB.Tool;


namespace Argus
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataStorage.GetStorage().OpenOrCreate("Workflow",  
                Properties.Settings.Default.WorkflowDBPath,
                Directory.GetCurrentDirectory());
            var users = DataStorage.GetStorage().GetUsers("Workflow");
            ConfigWindow configWindow = new ConfigWindow(Properties.Settings.Default.WorkflowDBPath, Directory.GetCurrentDirectory());
            configWindow.ShowDialog();
        }

        
    }
}
