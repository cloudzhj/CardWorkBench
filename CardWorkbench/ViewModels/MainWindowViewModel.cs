using CardWorkbench.Views.CommonControls;
using CardWorkbench.Views.CommonTools;
using CardWorkbench.Utils;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CardWorkbench.Models;
using DevExpress.Xpf.Editors;
using System.Windows.Threading;
using System.Windows.Media;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Bars;
using CardWorkbench.Views.MenuControls;
using System.Threading;
using DevExpress.Xpf.Core;
using CardWorkbench.ViewModels.CommonTools;
using System.Collections.ObjectModel;
using CardWorkbench.ViewModels.MenuControls;
using System.Windows.Media.Imaging;
using System.Collections;

namespace CardWorkbench.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
      //控件名称
      public static readonly string TOOLBOX_TEXTCONTROL_NAME = "toolbox_textCtrl"; //文本
      public static readonly string TOOLBOX_LINECONTROL_NAME = "toolbox_lineCtrl"; //二维曲线
      public static readonly string TOOLBOX_METERCONTROL_NAME = "toolbox_meterCtrl"; //仪表
      public static readonly string TOOLBOX_LIGHTCONTROL_NAME = "toolbox_lightCtrl"; //工作灯
      public static readonly string TOOLBOX_TIMECONTROL_NAME = "toolbox_timeCtrl"; //时间
      //工作区document group 名称
      public static readonly string DOCUMENTGROUP_NAME = "documentContainer";
      //工作区自定义控件Canvas名称
      public static readonly string CANVAS_CUSTOM_CONTROL_NAME = "workCanvas";
      //Ribbon标签栏及其组件名称
      public static readonly string RIBBONCONTROL_NAME = "ribbonControl";
      public static readonly string RIBBONPAGE_TOOLS_NAME = "toolsRibbonPage";  
      //工具panel名称、标题 
      public static readonly string PANEL_FRAMEDUMP_NAME = "frameDumpPanel";
      public static readonly string PANEL_FRAMEDUMP_CAPTION = "原始帧显示";

      public static readonly string PANEL_CUSTOMCONTROL_NAME = "mainControl";  //自定义控件     
      public static readonly string DOCUMENTPANEL_WORKSTATE_NAME = "document1"; //配置区panel名称
      //硬件设置菜单栏对话框名称
      public static readonly string DIALOG_HARDWAR_RECOGNITION_NAME = "hardwareRecognitionDialog";
      //public static readonly string DIALOG_RECEIVER_SETTING_NAME = "receiverSettingDialog";  //接收机设置
      //public static readonly string DIALOG_FRAMESYNC_SETTING_NAME = "frameSyncSettingDialog"; //帧同步设置
       
      //注册服务声明
      public IDialogService hardwareRecognitionDialogService { get { return GetService<IDialogService>(DIALOG_HARDWAR_RECOGNITION_NAME); } }  //获得硬件识别对话框服务
      public IOpenFileDialogService OpenFileDialogService { get { return GetService<IOpenFileDialogService>() ; } }  //获得文件选择对话框服务
      public ISplashScreenService SplashScreenService { get { return GetService<ISplashScreenService>(); } } //LOADING splash screen服务


      //参数grid某行是否拖拽开始
      bool _dragStarted = false;
      //参数grid面板table view的名称
      public static readonly string PARAM_GRID_TABLEVIEW_NAME = "paramGridTabelView";

      private HardwareRecognitionViewModel hardwareViewModel = null;

      public MainWindowViewModel() {
         
      }


      #region 顶部功能菜单命令绑定  
 
      /// <summary>
      /// 硬件识别对话框命令
      /// </summary>
      public ICommand hardwareRecognitionCommand
      {
          get { return new DelegateCommand<LayoutPanel>(onHardwareRecognitionClick, x => { return true; }); }
      }

      private void onHardwareRecognitionClick(LayoutPanel cardMenuPanel)
      {   
          UICommand okCommand = new UICommand()
          {
              Caption = "添加",
              IsCancel = false,
              IsDefault = true,
              Command = new DelegateCommand<CancelEventArgs>(
                 x => { },
                 x => {
                     if (HardwareRecognitionViewModel.grid != null 
                         && HardwareRecognitionViewModel.grid.SelectedItems.Count != 0)
                     {
                         return true;
                     }
                     return false;
                 }
              ),
          };
          UICommand cancelCommand = new UICommand()
          {
              Id = MessageBoxResult.Cancel,
              Caption = "取消",
              IsCancel = true,
              IsDefault = false,
          };
          UICommand result = hardwareRecognitionDialogService.ShowDialog(
              dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
              title: "设备识别",
              viewModel: null
          );

          if (result == okCommand)
          {
              IList devicelist = null;
              if (HardwareRecognitionViewModel.grid != null)
              {
                  devicelist = HardwareRecognitionViewModel.grid.SelectedItems as IList;
              }

              onSelectHardwareLoad(cardMenuPanel, devicelist);
          }
      }

      /// <summary>
      /// 加载选中板卡显示配置菜单命令
      /// </summary>
      /// <param name="cardMenuPanel">配置菜单layout panel</param>
      /// <param name="devicelist">设备清单列表</param>
      private void onSelectHardwareLoad(LayoutPanel cardMenuPanel, IList devicelist)
      {
          CardMenuConfig cardMenu = new CardMenuConfig();
          //CardMenuConfigViewModel cardMenuViewModel = cardMenu.DataContext as CardMenuConfigViewModel;
          NavBarControl menuNavBarControl = cardMenu.FindName("menuNavcontrol") as NavBarControl;
          if (devicelist != null)
          {
              foreach (var obj in devicelist)
              {
                  Device device = obj as Device;
                  //构建设备菜单组
                  NavBarGroup deviceGroup = new NavBarGroup();
                  deviceGroup.Name = "deviceNavBar" + device.deviceID;
                  deviceGroup.Header = device.deviceModel;
                  BitmapImage myBitmapImage = new BitmapImage(new Uri("pack://application:,,,/Images/hardware_32.png"));
                  deviceGroup.ImageSource = myBitmapImage;
                  ImageSettings deviceItemImageSetting = new ImageSettings();
                  deviceItemImageSetting.Height = 32;
                  deviceItemImageSetting.Width = 32;
                  deviceGroup.ItemImageSettings = deviceItemImageSetting;
                  //构建设备菜单组下通道菜单
                  if (device.channelList != null)
	              {
		            foreach (Channel channel in device.channelList)
	                {
                        NavBarItem channelItem = new NavBarItem();
                        channelItem.Name = "channelNavItem" + channel.channelID;

                    }
	              }

                  menuNavBarControl.Groups.Add(deviceGroup);
              }

          }

          cardMenuPanel.Content = cardMenu;
          FrameworkElement root = LayoutHelper.GetRoot(cardMenuPanel);
          //TEST///////////////////////////////////////////////

          ////////////////////////////////////////////////////
          //开启ribbon工具标签页
          RibbonControl ribbonControl = (RibbonControl)LayoutHelper.FindElementByName(root, RIBBONCONTROL_NAME);
          RibbonPage ribbonPage = ribbonControl.Manager.FindName(RIBBONPAGE_TOOLS_NAME) as RibbonPage;
          ribbonPage.IsEnabled = true;
      }

      /// <summary>
      /// 打开读取本地硬件配置文件对话框  TODO:应修改为读取通道配置文件信息
      /// </summary>
      public ICommand openHardwareConfigCommand {
          get { return new DelegateCommand<LayoutPanel>(onOpenHardwareConfigClick, x => { return true; }); }
      }

      private void onOpenHardwareConfigClick(LayoutPanel cardMenuPanel)
      {
          OpenFileDialogService.Filter = "配置文件|*.xml";
          OpenFileDialogService.FilterIndex = 1;
          bool DialogResult = OpenFileDialogService.ShowDialog();
          if (!DialogResult)
          {
              //ResultFileName = string.Empty;
          }
          else
          {
              //onSelectHardwareLoad(cardMenuPanel);

              //IFileInfo file = OpenFileDialogService.Files.First();
              //ResultFileName = file.Name;
              //using (var stream = file.OpenText())
              //{
              //    FileBody = stream.ReadToEnd();
              //}
          }
      }

      /// <summary>
      /// ”原始帧显示“ 按钮Command
      /// </summary>
      public ICommand frameDumpCommand {
          get { return new DelegateCommand<DockLayoutManager>(onFrameDumpClick, x => { return true; }); }
      }

      private void onFrameDumpClick(DockLayoutManager dockManager)
      {
          UIControlHelper.createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_FRAMEDUMP_NAME, PANEL_FRAMEDUMP_CAPTION, new FrameDump());          
      }


      /// <summary>
      /// "自定义控件" 按钮Command
      /// </summary>
      public ICommand customControlCommand {
          get { return new DelegateCommand<DockLayoutManager>(onCustomControlClick, x => { return true; }); }
      }
      private void onCustomControlClick(DockLayoutManager dockManager)
      {
          DocumentGroup documentGroup = dockManager.GetItem(DOCUMENTGROUP_NAME) as DocumentGroup;
          DocumentPanel docPanel = dockManager.GetItem(PANEL_CUSTOMCONTROL_NAME) as DocumentPanel;
          if (docPanel != null && docPanel.IsClosed)
          {
              dockManager.DockController.Restore(docPanel);
          }
          dockManager.DockController.Activate(docPanel);
      }

    
      #endregion

    

      #region 控件栏拖拽命令绑定
        public ICommand dropToolBoxCommand
        {
            get { return new DelegateCommand<DragEventArgs>(onDropToolBoxNavItem, x => { return true; }); }
        }


        //拖拽事件
        private void onDropToolBoxNavItem(DragEventArgs e)
        {
            object originalSource = e.OriginalSource;
            NavBarItemControl item = LayoutHelper.FindParentObject<NavBarItemControl>(originalSource as DependencyObject);
            NavBarGroupHeader header = LayoutHelper.FindParentObject<NavBarGroupHeader>(originalSource as DependencyObject);
            NavBarItem data = e.Data.GetData(NavBarDragDropHelper.FormatName) as NavBarItem;
            if (data != null && data.Name != null)
            {
                string navItemName = data.Name;
                FrameworkElement root = LayoutHelper.GetTopLevelVisual(originalSource as DependencyObject);
                Canvas workCanvas = (Canvas)LayoutHelper.FindElementByName(root, CANVAS_CUSTOM_CONTROL_NAME);
                UserControl commonControl = null;
                int maxZindex = UIControlHelper.getMaxZIndexOfContainer(workCanvas);
                if (TOOLBOX_TEXTCONTROL_NAME.Equals(navItemName))   //文本控件
                {
                    commonControl = new TextControl();

                }
                else if (TOOLBOX_LINECONTROL_NAME.Equals(navItemName)) //曲线控件
                {
                    commonControl = new ChartControl();
                }
                else if (TOOLBOX_LIGHTCONTROL_NAME.Equals(navItemName)) //工作灯控件
                {
                    commonControl = new LampControl();
                }
                else if (TOOLBOX_METERCONTROL_NAME.Equals(navItemName))  //仪表控件
                {
                    commonControl = new CirleControl();
                }
                else if (TOOLBOX_TIMECONTROL_NAME.Equals(navItemName)) //时间控件
                {
                    commonControl = new TimeControl();
                }
                if (commonControl != null)
                {
                    Canvas.SetZIndex(commonControl, maxZindex + 1);
                    workCanvas.Children.Add(commonControl);                   
                }

            }
        }
      #endregion

      #region 参数Grid面板命令绑定
        public ICommand ParamGridMouseDownCommand
        {
            get { return new DelegateCommand<MouseButtonEventArgs>(onParamGridMouseDown, x => { return true; }); }
        }

        private void onParamGridMouseDown(MouseButtonEventArgs e)
        {
            FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
            TableView paramTableView = (TableView)LayoutHelper.FindElementByName(root, PARAM_GRID_TABLEVIEW_NAME);
            int rowHandle = paramTableView.GetRowHandleByMouseEventArgs(e);
            if (rowHandle != GridDataController.InvalidRow)
                _dragStarted = true;
        }

        public ICommand ParamGridMouseMoveCommand
        {
            get { return new DelegateCommand<MouseEventArgs>(onParamGridMouseMove, x => { return true; }); }
        }

        private void onParamGridMouseMove(MouseEventArgs e)
        {
            FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
            TableView paramTableView = (TableView)LayoutHelper.FindElementByName(root, PARAM_GRID_TABLEVIEW_NAME);
            //int rowHandle = paramTableView.GetRowHandleByMouseEventArgs(e);
            //RowControl rowControl = paramTableView.GetRowElementByRowHandle(rowHandle) as RowControl;

            //此处必须新建控件的dipatcher线程做处理，否则在拖动参数时会出现界面假死的情况
            paramTableView.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_dragStarted)
                {
                    //Console.WriteLine("start..............");
                    FrameworkElement element = paramTableView.GetRowElementByMouseEventArgs(e);
                    RowData rowData = null;
                    if (element != null)
                    {
                        rowData = element.DataContext as RowData;
                        if (rowData == null)
                        {
                            return;
                        }
                        DataObject data = CreateDataObject(rowData);
                        DragDrop.DoDragDrop(element, data, DragDropEffects.Move | DragDropEffects.Copy);
                    }
                    _dragStarted = false;
                    //Console.WriteLine("end..............");
                }
            }));

        }

        private DataObject CreateDataObject(RowData rowData)
        {
            DataObject data = new DataObject();
            //data.SetData(typeof(int), rowHandle);
            data.SetData(typeof(RowData), rowData);
            return data;
        }

        #endregion

      #region 自定义控件Panel按钮命令绑定
        public ICommand resetCommonControlsPanelCommand
        {
            get { return new DelegateCommand<Canvas>(onResetCommonControlsBtnClick, x => { return true; }); }
        }

        private void onResetCommonControlsBtnClick(Canvas commonControlsCanvas)
        {
            if (commonControlsCanvas != null)
            {
                commonControlsCanvas.Children.Clear();
            }
        }

        /// <summary>
        /// 控件键盘按键响应事件命令
        /// </summary>
        public ICommand CanvasDeleteKeyDownCommand
        {
            get { return new DelegateCommand<KeyEventArgs>(onCanvasDeleteKeyDown, x => { return true; }); }
        }

        private void onCanvasDeleteKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)  //删除按键
            {
                Canvas workCanvas = e.Source as Canvas;
                if (workCanvas == null)
                {
                    FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
                    workCanvas = (Canvas)LayoutHelper.FindElementByName(root, CANVAS_CUSTOM_CONTROL_NAME);

                }
                var maxZ = UIControlHelper.getMaxZIndexOfContainer(workCanvas); //当前最顶层的控件

                if (workCanvas.Children.Count != 0)
                {
                    foreach (FrameworkElement childElement in workCanvas.Children)
                    {
                        if (Canvas.GetZIndex(childElement) == maxZ)
                        {
                            workCanvas.Children.Remove(childElement); //删除
                            break;
                        }

                    }

                }
            }
        }
        #endregion
    }
}
