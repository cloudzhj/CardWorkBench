using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CardWorkbench.Controls;
using CardWorkbench.CommonControls;
using CardWorkbench.MenuControls;
using DevExpress.Xpf.NavBar;

namespace CardWorkbench
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public BitSyncEyeControl BitSynceEye { get; set; }
        public FrameDump FrameDump { get; set; }
        public ReceiverChartControl ReceiverChart { get; set; }
        public BitSyncChart BitSyncChartPanel { get; set; }
        public DecomOutputPanel DecomPanel { get; set; }
        public ReceiverUC Receiveruc { get; set; }
        public BitSyncUC BitSyncuc { get; set; }
        public TimeUC Timeuc { get; set; }
        public FrameSyncUC FrameSyncuc { get; set; }

        bool isDragDropInEffect = false;
        Point pos = new Point();
        public MainWindow()
        {
            InitializeComponent();
            BitSynceEye = new BitSyncEyeControl();
            FrameDump = new FrameDump();
            ReceiverChart = new ReceiverChartControl();
            BitSyncChartPanel = new BitSyncChart();
            DecomPanel = new DecomOutputPanel();
            BitSyncuc = new BitSyncUC();
            Receiveruc = new ReceiverUC();
            Timeuc = new TimeUC();
            FrameSyncuc = new FrameSyncUC();
        }
        #region 拖动
        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                double xPos = e.GetPosition(null).X - pos.X + (double)currEle.GetValue(Canvas.LeftProperty);
                double yPos = e.GetPosition(null).Y - pos.Y + (double)currEle.GetValue(Canvas.TopProperty);
                currEle.SetValue(Canvas.LeftProperty, xPos);
                currEle.SetValue(Canvas.TopProperty, yPos);
                pos = e.GetPosition(null);
            }
        }

        private void AddDragElement(UserControl uiEle)
        {
           
                uiEle.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Element_MouseLeftButtonDown), true);
                uiEle.AddHandler(Button.MouseMoveEvent, new MouseEventHandler(Element_MouseMove), true);
                uiEle.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Element_MouseLeftButtonUp), true);
                    
                uiEle.MouseMove += new MouseEventHandler(Element_MouseMove);
                uiEle.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                uiEle.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
            
        }
        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement fEle = sender as FrameworkElement;
            isDragDropInEffect = true;
            pos = e.GetPosition(null);
            fEle.CaptureMouse();
            fEle.Cursor = Cursors.Hand;
        }

        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
            }
        }
        #endregion
        private void NavBarGroup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavBarGroup selectGroup = (NavBarGroup)this.navcontrol.View.GetNavBarGroup(e);
            NavBarItem item = (NavBarItem)this.navcontrol.View.GetNavBarItem(e);
            if (item == null || item.Content == null)
            {
                return;
            }
            string value = (string)item.Content;
        }

        //点击位同步眼图
        private void barButtonItem6_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }


        private void barButtonItem4_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (!this.mainControl.Items.Contains(this.BitSynceEye.mainPanel))
            {
                this.mainControl.Items.Add(this.BitSynceEye.mainPanel);
                //AddDragElement(this.bitSynceEye);
            }
            this.BitSynceEye.mainPanel.Focus();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (!this.mainControl.Items.Contains(this.FrameDump.mainPanel))
            {
                this.mainControl.Items.Add(this.FrameDump.mainPanel);
                //AddDragElement(this.frameDump);
            }

            this.FrameDump.mainPanel.Focus();
        }

        private void barButtonItem4_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (!this.mainControl.Items.Contains(this.ReceiverChart.mainPanel))
            {
                this.mainControl.Items.Add(this.ReceiverChart.mainPanel);
                //AddDragElement(this.frameDump);
            }

            this.ReceiverChart.mainPanel.Focus();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (!this.mainControl.Items.Contains(this.BitSyncChartPanel.mainPanel))
            {
                this.mainControl.Items.Add(this.BitSyncChartPanel.mainPanel);
                //AddDragElement(this.frameDump);
            }

            this.BitSyncChartPanel.mainPanel.Focus();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (!this.mainControl.Items.Contains(this.DecomPanel.mainPanel))
            {
                this.mainControl.Items.Add(this.DecomPanel.mainPanel);
                //AddDragElement(this.frameDump);
            }

            this.DecomPanel.mainPanel.Focus();
        }

        //工作灯控件
        private void barButtonItem13_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.mainControl.Items.Add(new LampControl().mainPanel);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.mainControl.Items.Add(new TimeControl().mainPanel);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.mainControl.Items.Add(new CirleControl().mainPanel);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.mainControl.Items.Add(new ChartControl().mainPanel);
        }

        private void NavBarItem_Select(object sender, EventArgs e)
        {
            if (!this.mainDockManager.FloatGroups.Contains(BitSyncuc.mainPanel))
            {
                this.mainDockManager.FloatGroups.Add(BitSyncuc.mainPanel);
            }
        }

        private void NavBarItem_Select_1(object sender, EventArgs e)
        {
            if (!this.mainDockManager.FloatGroups.Contains(Receiveruc.mainPanel))
            {
                this.mainDockManager.FloatGroups.Add(Receiveruc.mainPanel);
            }
        }

        private void NavBarItem_Select_2(object sender, EventArgs e)
        {
            if (!this.mainDockManager.FloatGroups.Contains(Timeuc.mainPanel))
            {
                this.mainDockManager.FloatGroups.Add(Timeuc.mainPanel);
            }
        }

        private void NavBarItem_Select_3(object sender, EventArgs e)
        {
            if (!this.mainDockManager.FloatGroups.Contains(FrameSyncuc.mainPanel))
            {
                this.mainDockManager.FloatGroups.Add(FrameSyncuc.mainPanel);
            }
        }
    }
}
