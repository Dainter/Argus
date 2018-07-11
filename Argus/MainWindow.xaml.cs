using System.Collections.Generic;
using System.IO;
using System.Windows;
using Argus.Backend;
using Argus.Backend.Model.Nodes;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataStorage.GetStorage().OpenOrCreate("Workflow",
                Properties.Settings.Default.WorkflowDBPath,
                Directory.GetCurrentDirectory());
            //IEnumerable<UserGroup> userGroups = DataStorage.GetStorage().GetUserGroups("Workflow");
            //GraphConstructor graphConstructor =
            //    new GraphConstructor(Properties.Settings.Default.WorkflowDBPath,
            //        Directory.GetCurrentDirectory());
            //graphConstructor.CreateGraph();
            DataStorage.GetStorage().SaveAsJson("Workflow", @"C:\Users\z003hkns\Desktop\Workflow.json");
            
            //var users = DataStorage.GetStorage().GetUsers("Workflow");
            ConfigWindow configWindow = new ConfigWindow(Properties.Settings.Default.WorkflowDBPath, Directory.GetCurrentDirectory());
            configWindow.ShowDialog();
        }
    }
}
