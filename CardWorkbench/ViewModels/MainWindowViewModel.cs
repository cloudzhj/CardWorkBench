using CardWorkbench.CommonControls;
using CardWorkbench.Controls;
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

namespace CardWorkbench.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
      //控件名称
      public readonly string TOOLBOX_TEXTCONTROL_NAME = "toolbox_textCtrl"; //文本
      public readonly string TOOLBOX_LINECONTROL_NAME = "toolbox_lineCtrl"; //二维曲线
      public readonly string TOOLBOX_METERCONTROL_NAME = "toolbox_meterCtrl"; //仪表
      public readonly string TOOLBOX_LIGHTCONTROL_NAME = "toolbox_lightCtrl"; //工作灯
      public readonly string TOOLBOX_TIMECONTROL_NAME = "toolbox_timeCtrl"; //时间
      //工作区document group 名称
      public readonly string DOCUMENTGROUP_NAME = "documentContainer";  
      //工具panel名称、标题 
      public readonly string PANEL_FRAMEDUMP_NAME = "frameDumpPanel";
      public readonly string PANEL_FRAMEDUMP_CAPTION = "原始帧显示";

      public readonly string PANEL_RECEIVERCHART_NAME = "receiverChartPanel";
      public readonly string PANEL_RECEIVERCHART_CAPTION = "接收机波形显示";
      
      public readonly string PANEL_BITSYNCCHART_NAME = "bitSyncChartPanel";
      public readonly string PANEL_BITSYNCCHART_CAPTION = "位同步波形显示";

      public readonly string PANEL_DECOMOUTPUT_NAME = "decomOutputPanel";
      public readonly string PANEL_DECOMOUTPUT_CAPTION = "解码输出";

      public readonly string PANEL_CUSTOMCONTROL_NAME = "mainControl";  //自定义控件     
      //硬件设置菜单栏对话框名称
      public readonly string DIALOG_RECEIVER_SETTING_NAME = "receiverSettingDialog";  //接收机设置
      public readonly string DIALOG_BITSYNC_SETTING_NAME = "bitSyncSettingDialog";  //位同步设置
      public readonly string DIALOG_FRAMESYNC_SETTING_NAME = "frameSyncSettingDialog"; //帧同步设置
      public readonly string DIALOG_TIME_SETTING_NAME = "timeSettingDialog"; //时间设置
       
      public IDialogService receiverSettingDialogService { get { return GetService<IDialogService>(DIALOG_RECEIVER_SETTING_NAME); } }  //获得接收机设置对话框服务

      public IDialogService bitSyncSettingDialogService { get { return GetService<IDialogService>(DIALOG_BITSYNC_SETTING_NAME); } }  //获得位同步设置对话框服务

      public IDialogService frameSyncSettingDialogService { get { return GetService<IDialogService>(DIALOG_FRAMESYNC_SETTING_NAME); } }  //获得帧同步设置对话框服务

      public IDialogService timeSettingDialogService { get { return GetService<IDialogService>(DIALOG_TIME_SETTING_NAME); } }  //获得时间同步设置对话框服务

      #region 工具菜单栏命令绑定      
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
      #endregion

      #region 控件栏拖拽命令绑定
      
      /**
       声明拖拽工具箱item命令
       **/
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
                Canvas workCanvas = (Canvas)LayoutHelper.FindElementByName(root, "workCanvas");
                if (TOOLBOX_TEXTCONTROL_NAME.Equals(navItemName))   //文本控件
                {
                    //TODO ...

                }
                else if (TOOLBOX_LINECONTROL_NAME.Equals(navItemName)) //曲线控件
                {
                    ChartControl chartControl = new ChartControl();
                    workCanvas.Children.Add(chartControl);
                }
                else if (TOOLBOX_LIGHTCONTROL_NAME.Equals(navItemName)) //工作灯控件
                {
                    LampControl lampControl = new LampControl();
                    workCanvas.Children.Add(lampControl);
                }
                else if (TOOLBOX_METERCONTROL_NAME.Equals(navItemName))  //仪表控件
                {
                    CirleControl circleControl = new CirleControl();
                    workCanvas.Children.Add(circleControl);
                }
                else if (TOOLBOX_TIMECONTROL_NAME.Equals(navItemName)) //时间控件
                {
                    TimeControl timeControl = new TimeControl();
                    workCanvas.Children.Add(timeControl);
                }

            }
        }
      #endregion

    }
}
