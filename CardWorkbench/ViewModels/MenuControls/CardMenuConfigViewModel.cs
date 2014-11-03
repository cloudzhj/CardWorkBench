using CardWorkbench.Utils;
using CardWorkbench.Views.MenuControls;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CardWorkbench.ViewModels.MenuControls
{
    public class CardMenuConfigViewModel
    {
        //layoutManager
        public static readonly string DOCKLAYOUTMANAGER_NAME = "mainDockManager"; 
        //工作区document group 名称
        public static readonly string DOCUMENTGROUP_NAME = "documentContainer";

        //通道配置dock panel名称
        public static readonly string PANEL_CHANNELSETUP_NAME = "channelSetupPanel";
        //通道配置dock panel标题
        public static readonly string PANEL_CHANNELSETUP_CAPTION = "通道配置";
        //Ribbon标签栏及其组件名称
        public static readonly string RIBBONCONTROL_NAME = "ribbonControl";
        public static readonly string RIBBONPAGE_TOOLS_NAME = "toolsRibbonPage";
        public static readonly string RIBBONPAGE_SETUP_NAME = "setupRibbonPage";
        public static readonly string RIBBONPAGEGROUP_CHANNELSETUP_NAME = "channelSetupRibbonGroup";  


        #region 硬件设置对话框命令绑定

        /// <summary>
        /// 菜单双击命令
        /// </summary>
        public ICommand cardMenuDoubleClickCommand
        {
            get { return new DelegateCommand<NavBarControl>(onCardMenuDoubleClick, x => { return true; }); }
        }

        private void onCardMenuDoubleClick(NavBarControl obj)
        {
            FrameworkElement root = LayoutHelper.GetTopLevelVisual(obj as DependencyObject);
            DockLayoutManager dockManager = (DockLayoutManager)LayoutHelper.FindElementByName(root, DOCKLAYOUTMANAGER_NAME);
            UIControlHelper.createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_CHANNELSETUP_NAME, PANEL_CHANNELSETUP_CAPTION, new FrameSyncUC());
        }

        /// <summary>
        /// 菜单点击命令
        /// </summary>
        public ICommand cardMenuItemClickCommand
        {
            get { return new DelegateCommand<NavBarControl>(onCardMenuItemClick, x => { return true; }); }
        }

        private void onCardMenuItemClick(NavBarControl navBarControl)
        {
            FrameworkElement root = LayoutHelper.GetTopLevelVisual(navBarControl as DependencyObject);
            RibbonControl ribbonControl = (RibbonControl)LayoutHelper.FindElementByName(root, RIBBONCONTROL_NAME);
            RibbonPage setupRibbonPage = ribbonControl.Manager.FindName(RIBBONPAGE_SETUP_NAME) as RibbonPage;
            RibbonPageGroup channelSetupRibbonPageGroup = setupRibbonPage.FindName(RIBBONPAGEGROUP_CHANNELSETUP_NAME) as RibbonPageGroup;
            if (!channelSetupRibbonPageGroup.IsEnabled) 
            {
                channelSetupRibbonPageGroup.IsEnabled = true;   //开启通道设置按钮栏
            }
        }

        #endregion
    }
}
