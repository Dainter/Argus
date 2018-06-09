using System.Windows;
using GraphDB;

namespace Argus
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphDatabase gdb;
        public MainWindow()
        {
            InitializeComponent();
            gdb = new GraphDatabase("Argus");
        }

        
    }
}
