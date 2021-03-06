﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GraphDB.Contract.Enum;
using GraphDB.Core;
using GraphDB.Tool.Drawing;
using Microsoft.Win32;
using Microsoft.Windows.Controls.Ribbon;

//&lt; < 小于号 
//&gt; > 大于号 
//&amp; & 和 
//&apos; ' 单引号 
//&quot; " 双引号 
//(&#x0020;)  空格 
//(&#x0009;) Tab 
//(&#x000D;) 回车 
//(&#x000A;) 换行 

namespace GraphDB.Tool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow
    {
        Graph myGdb;
        private GraphRenderer myGraphRenderer;
        //MainDataSet DataSet;
        private readonly string myDataBasePath;
        private readonly string myAssemblyPath;

        bool myIsDbAvailable;
        bool myIsModified;
        DispatcherTimer myStatusUpadteTimer;
        Node myCurModifyNode;
        Edge myCurModifyEdge;
        
        public ConfigWindow(string dbPath, string assemblePath)
        {
            InitializeComponent();
            myDataBasePath = dbPath;
            myAssemblyPath = assemblePath;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowReset();
        }

        private void WindowReset()
        {
            AllReset();
            myGraphRenderer = new GraphRenderer();
            ChangeStyle("默认样式");
            StatusUpdateTimer_Init();
            try
            {
                myGdb = new Graph("Database", myDataBasePath, myAssemblyPath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Open Failed.");
                return;
            }
            FillNodeList();
            myIsModified = false;
            myIsDbAvailable = true;
        }

        private void FillNodeList()
        {
            NodeListBox.Items.Clear();
            int index = 0;
            foreach (var curItem in myGdb.Nodes)
            {
                NodeListBox.Items.Add( $"{index} Name:{curItem.Value.Name} Type:{curItem.Value.GetType().Name}" );
                index++;
            }
        }

        //完全重置
        private void AllReset()
        {
            myGdb = null;
            myIsDbAvailable = false;
            NodeListBox.Items.Clear();
            ClearArrows(DrawingSurface);
            DrawingSurface.ClearVisuals();

            ResetRibbonControls();
        }

        private void ResetRibbonControls()
        {
            myCurModifyNode = null;
            myCurModifyEdge = null;

            EndNodeName.ItemsSource = null;
            EdgeAttributeBox.ItemsSource = null;
            EdgeValueBox.Text = "";
        }

        //节点更新
        private void GraphNodeUpdate()
        {
            NodeListBox.Items.Clear();
            ClearArrows(DrawingSurface);
            DrawingSurface.ClearVisuals();

            ResetRibbonControls();
            FillNodeList();
        }
        //连边更新
        private void GraphEdgeUpdate()
        {
            myCurModifyEdge = null;
            ClearArrows(DrawingSurface);
            DrawingSurface.ClearVisuals();
            SelectNode(NodeListBox.SelectedIndex);
            UpdateCustomNode(myCurModifyNode.Name);
        }

        #region StatusTimer
        private void StatusUpdateTimer_Init()
        {
            myStatusUpadteTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 3)
            };
            myStatusUpadteTimer.Tick += StatusUpdateTimer_Tick;
            myStatusUpadteTimer.IsEnabled = false;
        }

        private void StatusUpdateTimer_Tick(object sender, EventArgs e)
        {
            StatusLabel.Content = "Ready";
            myStatusUpadteTimer.IsEnabled = false;
        }

        public void ShowStatus(string sStatus)
        {
            StatusLabel.Content = sStatus;
            myStatusUpadteTimer.Start();
        }
        #endregion
        
        #region FileCommand
        //新建命令执行函数
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (myIsDbAvailable)
            {
                var choice = MessageBox.Show("Save current graph database to file？", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (choice == MessageBoxResult.Yes)
                {
                    //保存网络
                    try
                    {
                        myGdb.SaveDataBase();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ShowStatus("Save Failed.");
                        return;
                    }
                }
                else if (choice == MessageBoxResult.No)
                {
                }
                else if (choice == MessageBoxResult.Cancel)
                {
                    return;
                }
                AllReset();
            }
            //初始化对话框，文件类型，过滤器，初始路径等设置
            var savedialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            //成功选取文件后，根据文件类型执行读取函数
            if (savedialog.ShowDialog() != true)
            {
                return;
            }
            Cursor = Cursors.Wait;
            var strPath = savedialog.FileName;
            try
            {
                myGdb = new Graph("Database", strPath, myAssemblyPath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Create Failed.");
                return;
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            ShowStatus("Create Success.");
            myIsDbAvailable = true;
        }

        //打开文件命令执行函数
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (myIsDbAvailable)
            {
                var choice = MessageBox.Show("Save current graph database to file？", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (choice == MessageBoxResult.Yes)
                {
                    //保存网络
                    try
                    {
                        myGdb.SaveDataBase();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ShowStatus("Save Failed.");
                        return;
                    }
                }
                else if (choice == MessageBoxResult.No)
                {
                }
                else if (choice == MessageBoxResult.Cancel)
                {
                    return;
                }
                AllReset();
            }
            //初始化对话框，文件类型，过滤器，初始路径等设置
            var opendialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*|XML files (*.xml)|*.xml",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            //成功选取文件后，根据文件类型执行读取函数
            if (opendialog.ShowDialog() != true)
            {
                return;
            }
            Cursor = Cursors.Wait;
            var strPath = opendialog.FileName;
            try
            {
                myGdb = new Graph("Database", strPath, myAssemblyPath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Open Failed.");
                return;
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            FillNodeList();
            ShowStatus("Open Success.");
            myIsDbAvailable = true;
        }

        //保存命令执行函数
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            //保存网络
            try
            {
                myGdb.SaveDataBase();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Save Failed.");
                return;
            }
            myIsModified = false;
            ShowStatus("Save Success.");
        }
        
        //另存为命令执行函数
        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            //调出另存为对话框
            //初始化对话框，文件类型，过滤器，初始路径等设置
            var savedialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                FilterIndex = 0,
                RestoreDirectory = true
            };
            //成功选取文件后，根据文件类型执行读取函数
            if (savedialog.ShowDialog() != true)
            {
                return;
            }
            Cursor = Cursors.Wait;
            var strPath = savedialog.FileName;
            //切换IO句柄中的目标地址,并保存
            try
            {
                myGdb.SaveAsDataBase(strPath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Save As Failed.");
                return;
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            ShowStatus("Save As Success.");
        }
        
        //快速打印命令执行函数
        private void QuickPrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
        
        //打印预览命令执行函数
        private void PrintPreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
        
        //打印命令执行函数
        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
        
        //关闭数据库执行函数
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (myIsDbAvailable)
            {
                var choice = MessageBox.Show("Save current graph database to file？", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (choice == MessageBoxResult.Yes)
                {
                    //保存网络
                    try
                    {
                        myGdb.SaveDataBase();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ShowStatus("Save Failed.");
                        return;
                    }
                }
                else if (choice == MessageBoxResult.No)
                {
                }
                else if (choice == MessageBoxResult.Cancel)
                {
                    return;
                }
                ShowStatus("Database Closed.");
                AllReset();
            }
        }
        
        //退出程序执行函数
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        
        //关闭窗体前检查
        private void RibbonWindow_Closing(object sender, CancelEventArgs e)
        {
            if(myIsModified == false)
            {
                return;
            }
            if (myIsDbAvailable)
            {
                var choice = MessageBox.Show("Save current graph database to file？", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (choice == MessageBoxResult.Yes)
                {
                    //保存网络
                    try
                    {
                        myGdb.SaveDataBase();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ShowStatus("Save Failed.");
                        return;
                    }
                }
                else if (choice == MessageBoxResult.No)
                {
                }
                AllReset();
            }
        }
        
        //保存命令使能
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //另存为命令使能
        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //快速打印命令使能
        private void QuickPrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //打印预览命令使能
        private void PrintPreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //打印命令使能
        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //关闭数据库命令使能
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //刷新命令使能
        private void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myIsDbAvailable;
        }
        
        //刷新命令执行
        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (myIsDbAvailable == false)
            {
                return;
            }
            var choice = MessageBox.Show("Reload Database without Save?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (choice == MessageBoxResult.No)
            {
                return;
            }
            Refresh();
        }

        private void Refresh()
        {
            AllReset();
            myGdb = new Graph("Database", myDataBasePath, myAssemblyPath);
            FillNodeList();
            myIsDbAvailable = true;
        }

        #endregion

        #region Drawing
        private Graph mySubGraph;
        private bool myBolScrolltoCenter;

        private void BuildSubGraph(Node curSelNode)
        {

            var drawNodes = new List<Node>();
            var neibourNodes = new List<Node>();
            try
            {
                drawNodes.Add(curSelNode);
                mySubGraph = new Graph("SubGraph");
                mySubGraph.AddNode(new Node(curSelNode));
                foreach (Edge edge in curSelNode.OutBound)
                {
                    neibourNodes.Add(edge.To);
                    drawNodes.Add(edge.To);
                    try
                    {
                        mySubGraph.AddNode(new Node(edge.To));
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    Edge newEdge = new Edge(edge.Attribute);
                    mySubGraph.AddEdgeByGuid(curSelNode.Guid, edge.To.Guid, newEdge);
                }
                foreach (Node node in neibourNodes)
                {
                    foreach (Edge edge in node.InBound)
                    {
                        if (drawNodes.IndexOf(edge.From) < 0)
                        {
                            drawNodes.Add(edge.From);
                            mySubGraph.AddNode(new Node(edge.From));
                        }
                        if ((edge.From.Name != curSelNode.Name)
                            && (neibourNodes.IndexOf(edge.From) < 0))
                        {
                            Edge newEdge = new Edge(edge.Attribute);
                            mySubGraph.AddEdgeByGuid(edge.From.Guid, edge.To.Guid, newEdge);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Error Occured.");
            }
        }

        //获取visual索引
        private string GetVisualIndex(DrawingVisual visual)
        {
            if (visual == null)
            {
                return "";
            }
            foreach (var curItem in myGraphRenderer.Visuals)
            {
                if (curItem.Value.Equals(visual))
                {
                    return curItem.Key;
                }
            }
            return "";
        }
        
        //构造新的ToolTip
        private ToolTip BuildNewTip(string guid)
        {
            ToolTip nodeTip = new ToolTip
            {
                Content = mySubGraph.Nodes[guid].DataOutput()
            };

            return nodeTip;
        }
        
        //显示节点细节信息
        private void LoadNodeInfo(Node curNode)
        {
            int intRow = 0;
            if(curNode == null)
            {
                return;
            }
            PropertyInfo[] pInfos = curNode.GetType().GetProperties();
            NodeInfoGrid.Children.Clear();
            NodeInfoGrid.RowDefinitions.Clear();
            foreach (PropertyInfo pInfo in pInfos)
            {
                //TitleLabel
                var newRow = new RowDefinition { Height = new GridLength(20, GridUnitType.Auto) }; 
                NodeInfoGrid.RowDefinitions.Add(newRow);
                var curTitle = new Label
                {
                    Content = pInfo.Name + ":"
                };
                NodeInfoGrid.Children.Add(curTitle);
                curTitle.SetValue(Grid.RowProperty, intRow);
                curTitle.SetValue(Grid.ColumnProperty, 0);
                curTitle.HorizontalContentAlignment = HorizontalAlignment.Right;
                //Content
                var curContent = GetWidget(pInfo, curNode);
                if(curContent == null)
                {
                    curContent = new TextBox
                    {
                        Margin = new Thickness(2),
                        Width = 200
                    };
                }
                NodeInfoGrid.Children.Add(curContent);
                curContent.SetValue(Grid.RowProperty, intRow);
                curContent.SetValue(Grid.ColumnProperty, 1);
                intRow++;
            }
        }

        //生成控件
        private Control GetWidget(PropertyInfo pInfo, object curNode)
        {
            
            if(pInfo.PropertyType.Name == "List`1" || pInfo.PropertyType.Name == "IEnumerable`1")
            {
                ListBox contentlistBox = new ListBox
                {
                    Margin = new Thickness(2),
                };
                dynamic x = pInfo.GetValue(curNode, null);
                foreach (var item in x)
                {
                    if( pInfo.Name == "OutBound" )
                    {
                        contentlistBox.Items.Add((Edge)item + " "+ ((Edge)item).To.Name);
                    }
                    else if(pInfo.Name == "InBound")
                    {
                        contentlistBox.Items.Add(((Edge)item).From.Name + " " + (Edge)item);
                    }
                    else
                    {
                        contentlistBox.Items.Add(item.ToString());
                    }
                }
                return contentlistBox;
            }
            var contentBox = new TextBox();
            var newLabel = new Label();
            contentBox.Margin = new Thickness(2);
            contentBox.IsReadOnly = true;
            newLabel.Content = pInfo.GetValue(curNode, null);
            if(newLabel.Content == null)
            {
                newLabel.Content = "Null";
            }
            contentBox.Text = newLabel.Content.ToString();
            return contentBox;
        }

        //清除连边形状
        private void ClearArrows(UIElement element)
        {
            BackCanvas.Children.Clear();
            BackCanvas.Children.Add(element);
        }
        #endregion

        #region UICommand
        //节点列表框选中事件处理函数
        private void NodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectNode(NodeListBox.SelectedIndex);
            MainScroll.ScrollToBottom();
            MainScroll.ScrollToRightEnd();
            myBolScrolltoCenter = true;
        }
        //主滚动框自动居中
        private void MainScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (myBolScrolltoCenter)
            {
                var bottom = MainScroll.VerticalOffset;
                var right = MainScroll.HorizontalOffset;
                MainScroll.ScrollToVerticalOffset(bottom / 2);
                MainScroll.ScrollToHorizontalOffset(right / 2);
                myBolScrolltoCenter = false;
            }
        }
        //画布鼠标移动事件-节点标签显示
        private void DrawingSurfaceMouseMove(object sender, MouseEventArgs e)
        {
            string visualGuid = GetVisualIndex(DrawingSurface.GetVisual(e.GetPosition(DrawingSurface)));
            if (visualGuid == "")
            {
                return;
            }

            ToolTip nodeTip = BuildNewTip(visualGuid);
            PointLabel.Content = nodeTip.Content;
            nodeTip.Placement = PlacementMode.Mouse;
            DrawingSurface.ToolTip = nodeTip;
        }
        //画布鼠标点击事件-切换选中节点并重新绘图
        private void DrawingSurfaceMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string visualGuid = GetVisualIndex(DrawingSurface.GetVisual(e.GetPosition(DrawingSurface)));
            if (visualGuid == "")
            {
                return;
            }
            int intNode = myGdb.IndexOf( visualGuid );
            if (intNode == -1)
            {
                return;
            }
            NodeListBox.SelectedIndex = intNode;
        }
        //清除命令框内容命令执行
        private void ClearCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }
        //清除命令框按钮使能
        private void ClearCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
        }
        //清除结果框内容命令执行
        private void ClearResultCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }
        //清除结果框按钮使能
        private void ClearResultCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
        }
        //样式选择框
        private void NodeStyleSelection_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var newItem = (RibbonGalleryItem)e.NewValue;
            ChangeStyle(newItem.ToolTipTitle);
            if (NodeListBox.SelectedIndex == -1)
            {
                return;
            }
            myGraphRenderer.DrawGraph();
        }

        private void ChangeStyle(string style)
        {
            switch (style)
            {
                case "默认样式":
                    style = "DefaultNodeStyle";
                    break;
                case "深邃星空":
                    style = "PurpleNodeStyle";
                    break;
                case "底比斯之水":
                    style = "BlueNodeStyle";
                    break;
                case "千本樱":
                    style = "PinkNodeStyle";
                    break;
                default:
                    style = "DefaultNodeStyle";
                    break;
            }
            var curStyle = (Style)TryFindResource(style);
            myGraphRenderer.ChangeStyle( curStyle );
        }
        #endregion

        #region Ribbon Linkage
        //选择节点
        private void SelectNode(int index)
        {
            if (index < 0)
            {
                return;
            }
            ResetRibbonControls();
            Node newNode = myGdb.Nodes.ElementAt(index).Value;
            if (newNode == null)
            {
                return;
            }
            if (myCurModifyNode != null && newNode.Guid == myCurModifyNode.Guid)
            {
                return;
            }
            myCurModifyNode = newNode;
            UpdateEndNodeComboBox();
            //绑定Ribbon文本框
            StatusNameBox.Text = newNode.Name;
            LoadNodeInfo(myCurModifyNode);
            BuildSubGraph(myCurModifyNode);
            ClearArrows(DrawingSurface);
            int neibour = myCurModifyNode.OutBound.Select( x => x.To.Name ).Distinct().Count();
            myGraphRenderer.DrawNewGraph(DrawingSurface, neibour, mySubGraph);
        }

        //名称文本框值改变
        private void StatusNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newName = ((TextBox)sender).Text;
            if( !myIsDbAvailable )
            {
                return;
            }
            UpdateCustomNode( newName );
        }

        //查找用户指定节点
        private void UpdateCustomNode(string newName)
        {
            Node newNode = myGdb.GetNodeByName(newName);
            if (newNode == null)
            {
                return;
            }
            if (myCurModifyNode != null && newNode.Guid == myCurModifyNode.Guid)
            {
                return;
            }
            myCurModifyNode = newNode;
            UpdateEndNodeComboBox();
            //绑定NodeList
            NodeListBox.SelectedIndex =  myGdb.IndexOf( newNode.Guid );
        }

        private void UpdateEndNodeComboBox()
        {
            EndNodeName.ItemsSource = myCurModifyNode.OutBound.Select(x => x.To.Name).Distinct();
            if (EndNodeName.ItemsSource == null || EndNodeName.Items.Count <= 0)
            {
                return;
            }
            EndNodeName.SelectedIndex = 0;
        }

        //目标节点名称选项改变
        private void EndNodeNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }
            FillAttributeList(e.AddedItems[0].ToString());
        }

        //填充特性列表内容
        private void FillAttributeList(string endName)
        {
            EdgeAttributeBox.ItemsSource = null;
            if (myCurModifyNode == null)
            {
                return;
            }
            EdgeAttributeBox.ItemsSource = myCurModifyNode.GetEdgesByName( endName, EdgeDirection.Out ).Select(x => x.Attribute).Distinct();
            EdgeValueBox.Text = "";
        }

        //连边列表特性选项改变
        private void EdgeAttributeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }
            FindCustomEdge(e.AddedItems[0].ToString());
        }

        //查找目标连边
        private void FindCustomEdge(string attribute)
        {
            if (myCurModifyNode == null)
            {
                return;
            }
            string endName = EndNodeName.Text;
            var edges = myCurModifyNode.GetEdgesByName( endName, EdgeDirection.Out ).Where( x => x.Attribute == attribute );
            if( !edges.Any() )
            {
                return;
            }
            myCurModifyEdge = edges.First();
            EdgeValueBox.Text = myCurModifyEdge.Value;
        }
        #endregion

        #region Graph Operation
        //移除节点命令执行函数
        private void RemoveNodeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                myGdb.RemoveNode(RemoveNodeName.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Remove Node Failed.");
                return;
            }
            ShowStatus("Remove Node Success.");
            GraphNodeUpdate();
        }

        //加入连边命令执行函数
        private void AddEdgeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (StartNodeName.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Start node shouldn't be empty.");
                return;
            }
            string fromName = StartNodeName.Text.Trim();
            if (EndNodeName.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Target node shouldn't be empty.");
                return;
            }
            string toName = EndNodeName.Text.Trim();
            if (EdgeAttributeBox.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Attribute of Edge shouldn't be empty.");
                return;
            }
            string edgeAttrib = EdgeAttributeBox.Text.Trim();
            if ( EdgeValueBox.Text.Trim() == "" )
            {
                ShowStatus("Add Edge Failed. Value of Edge shouldn't be empty.");
                return;
            }
            string edgeValue = EdgeValueBox.Text.Trim();
            Edge newEdge = new Edge( edgeAttrib, edgeValue );
            try
            {
                myGdb.AddEdge(fromName, toName, newEdge);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Add Edge Failed.");
                return;
            }
            GraphEdgeUpdate();
            ShowStatus("Add Edge Successed.");
        }

        //修改连边命令执行函数
        private void ModifyEdgeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (StartNodeName.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Start node shouldn't be empty.");
                return;
            }
            string fromName = StartNodeName.Text.Trim();
            if (EndNodeName.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Target node shouldn't be empty.");
                return;
            }
            string toName = EndNodeName.Text.Trim();
            if (EdgeAttributeBox.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Attribute of Edge shouldn't be empty.");
                return;
            }
            string edgeAttrib = EdgeAttributeBox.Text.Trim();
            if (EdgeValueBox.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Value of Edge shouldn't be empty.");
                return;
            }
            string edgeValue = EdgeValueBox.Text.Trim();

            myCurModifyEdge = myGdb.GetEdgeByType( fromName, toName, edgeAttrib );
            if (myCurModifyEdge == null)
            {
                ShowStatus("Modify Edge Failed, no Edge be selected.");
                return;
            }
            myCurModifyEdge.Value = edgeValue;
            ShowStatus("Modify Edge Success.");
        }

        //移除连边命令执行函数
        private void RemoveEdgeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (StartNodeName.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Start node shouldn't be empty.");
                return;
            }
            string fromName = StartNodeName.Text.Trim();
            if (EndNodeName.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Target node shouldn't be empty.");
                return;
            }
            string toName = EndNodeName.Text.Trim();
            if (EdgeAttributeBox.Text.Trim() == "")
            {
                ShowStatus("Add Edge Failed. Attribute of Edge shouldn't be empty.");
                return;
            }
            string edgeAttrib = EdgeAttributeBox.Text.Trim();

            try
            {
                myGdb.RemoveEdge(fromName, toName, edgeAttrib);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowStatus("Remove Edge Failed.");
                return;
            }
            ShowStatus("Remove Edge Success.");
            GraphEdgeUpdate();
        }

        private void RemoveNodeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (myIsDbAvailable == false)
            {
                e.CanExecute = false;
                return;
            }
            if (myCurModifyNode == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }

        private void AddEdgeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (myIsDbAvailable == false)
            {
                e.CanExecute = false;
                return;
            }
            if (myCurModifyNode == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }

        private void ModifyEdgeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (myIsDbAvailable == false)
            {
                e.CanExecute = false;
                return;
            }
            if (myCurModifyNode == null)
            {
                e.CanExecute = false;
                return;
            }
            if( myCurModifyEdge == null )
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }

        private void RemoveEdgeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (myIsDbAvailable == false)
            {
                e.CanExecute = false;
                return;
            }
            if (myCurModifyNode == null)
            {
                e.CanExecute = false;
                return;
            }
            if (myCurModifyEdge == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }
        #endregion
    }
}
