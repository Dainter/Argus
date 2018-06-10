﻿using System.Windows;
using GraphDB;
using GraphDB.Contract;
using GraphDB.Tool;

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
            ConfigWindow configWindow = new ConfigWindow("Semantic.xml");
            configWindow.ShowDialog();
        }

        private void CreateGraph(string name)
        {
            
        }
    }
}
