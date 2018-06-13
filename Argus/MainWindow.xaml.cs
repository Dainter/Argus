using System.Windows;
using Argus.Backend;
using Argus.Backend.Model.Nodes;
using Argus.Backend.Utility;
using GraphDB;
using GraphDB.Contract;
using GraphDB.Core;
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

            var myGraphConstructor = new GraphConstructor(Properties.Settings.Default.WorkflowDBPath);

            myGraphConstructor.CreateGraph();

            ConfigWindow configWindow = new ConfigWindow(Properties.Settings.Default.WorkflowDBPath);
            configWindow.ShowDialog();
        }

        
    }
}
