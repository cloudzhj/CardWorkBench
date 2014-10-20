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
      //工具panel名称、标题 
      public static readonly string PANEL_FRAMEDUMP_NAME = "frameDumpPanel";
      public static readonly string PANEL_FRAMEDUMP_CAPTION = "原始帧显示";
      public static readonly string PANEL_RECEIVERCHART_NAME = "receiverChartPanel";
      public static readonly string PANEL_RECEIVERCHART_CAPTION = "接收机波形显示";     
      public static readonly string PANEL_BITSYNCCHART_NAME = "bitSyncChartPanel";
      public static readonly string PANEL_BITSYNCCHART_CAPTION = "位同步波形显示";
      public static readonly string PANEL_DECOMOUTPUT_NAME = "decomOutputPanel";
      public static readonly string PANEL_DECOMOUTPUT_CAPTION = "解码输出";
      public static readonly string PANEL_CUSTOMCONTROL_NAME = "mainControl";  //自定义控件     
      //硬件设置菜单栏对话框名称
      public static readonly string DIALOG_RECEIVER_SETTING_NAME = "receiverSettingDialog";  //接收机设置
      public static readonly string DIALOG_BITSYNC_SETTING_NAME = "bitSyncSettingDialog";  //位同步设置
      public static readonly string DIALOG_FRAMESYNC_SETTING_NAME = "frameSyncSettingDialog"; //帧同步设置
      public static readonly string DIALOG_TIME_SETTING_NAME = "timeSettingDialog"; //时间设置
      public static readonly string DIALOG_PLAYBACK_SETTING_NAME = "playBackSettingDialog"; //回放设置

       
      //注册服务声明
      public IDialogService receiverSettingDialogService { get { return GetService<IDialogService>(DIALOG_RECEIVER_SETTING_NAME); } }  //获得接收机设置对话框服务

      public IDialogService bitSyncSettingDialogService { get { return GetService<IDialogService>(DIALOG_BITSYNC_SETTING_NAME); } }  //获得位同步设置对话框服务

      public IDialogService frameSyncSettingDialogService { get { return GetService<IDialogService>(DIALOG_FRAMESYNC_SETTING_NAME); } }  //获得帧同步设置对话框服务

      public IDialogService timeSettingDialogService { get { return GetService<IDialogService>(DIALOG_TIME_SETTING_NAME); } }  //获得时间同步设置对话框服务

      public IDialogService playBackSettingDialogService { get { return GetService<IDialogService>(DIALOG_PLAYBACK_SETTING_NAME); } }  //获得模拟回放设置对话框服务


      //参数实体类列表
      public List<Param> paramList { get; set; }
      public List<CalibrateType> calibrateTypeList { get; set; }
      public List<ParamSortType> paramSortTypeList { get; set; }

      //参数grid某行是否拖拽开始
      bool _dragStarted = false;
      //参数grid面板table view的名称
      public static readonly string PARAM_GRID_TABLEVIEW_NAME = "paramGridTabelView"; 

      public MainWindowViewModel() {
          //初始化参数数据
          SampleData.initData();
          paramList = SampleData.paramList;
          calibrateTypeList = SampleData.calibrateTypeList;
          paramSortTypeList = SampleData.paramSortTypeList;

          //
      }


      #region 顶部功能菜单命令绑定      
      /**
       *源码管理工具界面命令
       **/
      public ICommand frameDumpCommand {
          get { return new DelegateCommand<DockLayoutManager>(onFrameDumpClick, x => { return true; }); }
      }

      private void onFrameDumpClick(DockLayoutManager dockManager)
      {
          createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_FRAMEDUMP_NAME, PANEL_FRAMEDUMP_CAPTION, new FrameDump());          
      }

      /**
       *接收机波形显示工具界面命令
       **/
      public ICommand receiverChartCommand
      {
          get { return new DelegateCommand<DockLayoutManager>(onReceiverChartClick, x => { return true; }); }
      }

      private void onReceiverChartClick(DockLayoutManager dockManager)
      {
          createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_RECEIVERCHART_NAME, PANEL_RECEIVERCHART_CAPTION, new ReceiverChartControl());
      }

      /**
      *位同步波形显示工具界面命令
      **/
      public ICommand bitSyncChartCommand
      {
          get { return new DelegateCommand<DockLayoutManager>(onBitSyncChartClick, x => { return true; }); }
      }

      private void onBitSyncChartClick(DockLayoutManager dockManager)
      {
          createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_BITSYNCCHART_NAME, PANEL_BITSYNCCHART_CAPTION, new BitSyncChart());
      }

      /**
        *解码输出工具界面命令
        **/
      public ICommand decomOutputCommand
      {
          get { return new DelegateCommand<DockLayoutManager>(onDecomOutputClick, x => { return true; }); }
      }

      private void onDecomOutputClick(DockLayoutManager dockManager)
      {
          createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_DECOMOUTPUT_NAME, PANEL_DECOMOUTPUT_CAPTION, new DecomOutputPanel());
      }

      /**
       * 自定义控件显示命令
       */
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

      /**
       * 在工作区创建document panel
       **/
      private void createWorkDocumentPanel(DockLayoutManager dockManager, string documentGroupName, string addDocPanelName, string addDocPanelCaption, object panelContent)
      {
          DocumentGroup documentGroup = dockManager.GetItem(documentGroupName) as DocumentGroup;
          DocumentPanel docPanel = dockManager.GetItem(addDocPanelName) as DocumentPanel;
          if (docPanel == null)
          {
              docPanel = dockManager.DockController.AddDocumentPanel(documentGroup);
              docPanel.Caption = addDocPanelCaption;
              docPanel.Content = panelContent;
              docPanel.Name = addDocPanelName;
          }
          else if (docPanel.IsClosed)
          {
              dockManager.DockController.Restore(docPanel);
          }
          dockManager.DockController.Activate(docPanel);
      }
      #endregion

      #region 硬件设置对话框命令绑定
     
      /**
      声明弹出接收机设置界面命令
      * **/
      public ICommand receiverSettingCommand
      {
          get { return new DelegateCommand<object>(onReceiverSettingClick, x => { return true; }); }
      }

      private void onReceiverSettingClick(object context)
      {
          UICommand okCommand = new UICommand()
          {
              Caption = "确定",
              IsCancel = false,
              IsDefault = true,
              Command = new DelegateCommand<CancelEventArgs>(
                 x => { },
                 true
              ),
          };
          UICommand cancelCommand = new UICommand()
          {
              Id = MessageBoxResult.Cancel,
              Caption = "取消",
              IsCancel = true,
              IsDefault = false,
          };
          UICommand result = receiverSettingDialogService.ShowDialog(
              dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
              title: "接收机设置",
              viewModel: null
          );

          if (result == okCommand)
          {
              MessageBox.Show("successfull!!");
          }
      }

      /**
   声明弹出位同步设置界面命令
   * **/
      public ICommand bitSyncSettingCommand
      {
          get { return new DelegateCommand<object>(onBitSyncSettingClick, x => { return true; }); }
      }

      private void onBitSyncSettingClick(object context)
      {
          UICommand okCommand = new UICommand()
          {
              Caption = "确定",
              IsCancel = false,
              IsDefault = true,
              Command = new DelegateCommand<CancelEventArgs>(
                 x => { },
                 true
              ),
          };
          UICommand cancelCommand = new UICommand()
          {
              Id = MessageBoxResult.Cancel,
              Caption = "取消",
              IsCancel = true,
              IsDefault = false,
          };
          UICommand result = bitSyncSettingDialogService.ShowDialog(
              dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
              title: "位同步设置",
              viewModel: null
          );

          if (result == okCommand)
          {
              MessageBox.Show("successfull!!");
          }
      }

      /**
        声明弹出帧同步设置界面命令
        * **/
      public ICommand frameSyncSettingCommand
      {
          get { return new DelegateCommand<object>(onFrameSyncSettingClick, x => { return true; }); }
      }

      private void onFrameSyncSettingClick(object context)
      {
          UICommand okCommand = new UICommand()
          {
              Caption = "确定",
              IsCancel = false,
              IsDefault = true,
              Command = new DelegateCommand<CancelEventArgs>(
                 x => { },
                 true
              ),
          };
          UICommand cancelCommand = new UICommand()
          {
              Id = MessageBoxResult.Cancel,
              Caption = "取消",
              IsCancel = true,
              IsDefault = false,
          };
          UICommand result = frameSyncSettingDialogService.ShowDialog(
              dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
              title: "帧同步设置",
              viewModel: null
          );

          if (result == okCommand)
          {
              MessageBox.Show("successfull!!");
          }
      }

      /**
    声明弹出时间同步设置界面命令
    * **/
      public ICommand timeSettingCommand
      {
          get { return new DelegateCommand<object>(ontimeSyncSettingClick, x => { return true; }); }
      }
      private void ontimeSyncSettingClick(object context)
      {
          UICommand okCommand = new UICommand()
          {
              Caption = "确定",
              IsCancel = false,
              IsDefault = true,
              Command = new DelegateCommand<CancelEventArgs>(
                 x => { },
                 true
              ),
          };
          UICommand cancelCommand = new UICommand()
          {
              Id = MessageBoxResult.Cancel,
              Caption = "取消",
              IsCancel = true,
              IsDefault = false,
          };
          UICommand result = timeSettingDialogService.ShowDialog(
              dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
              title: "时间同步设置",
              viewModel: null
          );

          if (result == okCommand)
          {
              MessageBox.Show("successfull!!");
          }
      }

      /**
        声明弹出模拟回放设置界面命令
        * **/
      public ICommand playBackSettingCommand
      {
          get { return new DelegateCommand<object>(onPlayBackSettingClick, x => { return true; }); }
      }
      private void onPlayBackSettingClick(object context)
      {
          UICommand okCommand = new UICommand()
          {
              Caption = "确定",
              IsCancel = false,
              IsDefault = true,
              Command = new DelegateCommand<CancelEventArgs>(
                 x => { },
                 true
              ),
          };
          UICommand cancelCommand = new UICommand()
          {
              Id = MessageBoxResult.Cancel,
              Caption = "取消",
              IsCancel = true,
              IsDefault = false,
          };
          UICommand result = playBackSettingDialogService.ShowDialog(
              dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
              title: "模拟回放设置",
              viewModel: null
          );

          if (result == okCommand)
          {
              MessageBox.Show("successfull!!");
          }
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
                    //TODO ...

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
            int rowHandle = paramTableView.GetRowHandleByMouseEventArgs(e);
            if (_dragStarted)
            {
                FrameworkElement element = paramTableView.GetRowElementByMouseEventArgs(e);

                RowControl rowControl = paramTableView.GetRowElementByRowHandle(rowHandle) as RowControl;
                RowData rowData = null;
                if (rowControl != null)
                {
                    rowData = rowControl.DataContext as RowData;
                }
                DataObject data = CreateDataObject(rowData);

                if (element != null)
                    DragDrop.DoDragDrop(element, data, DragDropEffects.Move | DragDropEffects.Copy);
                _dragStarted = false;
            }

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
        public ICommand CommonControlKeyDownCommand
        {
            get { return new DelegateCommand<KeyEventArgs>(onCommonControlsDeleteKeyDown, x => { return true; }); }
        }

        private void onCommonControlsDeleteKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)  //删除按键
            {
                //UserControl control = LayoutHelper.FindParentObject<UserControl>(e.Source as DependencyObject);
               // FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
                Canvas workCanvas = e.Source as Canvas;
                var maxZ = UIControlHelper.getMaxZIndexOfContainer(workCanvas); //当前最顶层的控件

                if (workCanvas.Children.Count != 0)
                {
                    foreach (FrameworkElement childElement in workCanvas.Children)
                    {
                        Console.WriteLine("delete before" + Canvas.GetZIndex(childElement));
                        if (Canvas.GetZIndex(childElement) == maxZ)
                        {
                            workCanvas.Children.Remove(childElement); //删除
                        }

                    }

                }

               

                if (workCanvas.Children.Count != 0)
                {
                    foreach (FrameworkElement childElement in workCanvas.Children)
                    {
                        Console.WriteLine("delete after" + Canvas.GetZIndex(childElement));
                    }

                }
                Console.WriteLine(maxZ);


            }
        }
        #endregion
    }
}
