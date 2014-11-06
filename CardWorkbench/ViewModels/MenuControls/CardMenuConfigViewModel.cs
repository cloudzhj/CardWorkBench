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
        public static readonly string RIBBONPAGE_CHANNEL_NAME = "channelRibbonPage";
        public static readonly string RIBBONPAGEGROUP_CHANNELSETUP_NAME = "channelSetupRibbonGroup";

        #region 硬件设置对话框命令绑定

        /// <summary>
        /// 菜单双击命令
        /// </summary>
        public ICommand cardMenuDoubleClickCommand
        {
            get { return new DelegateCommand<MouseButtonEventArgs>(onCardMenuDoubleClick, x => { return true; }); }
        }

        private void onCardMenuDoubleClick(MouseButtonEventArgs e)
        {
            NavBarItem navBarItem = findNavBarItemByEventArgs(e);
            if (navBarItem != null)
            {
                if (navBarItem.Name.Contains(MainWindowViewModel.NAVBARITEM_CHANNEL_NAME_PREFIX))   //如果是通道菜单项
                {
                    FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
                    DockLayoutManager dockManager = (DockLayoutManager)LayoutHelper.FindElementByName(root, DOCKLAYOUTMANAGER_NAME);
                    UIControlHelper.createWorkDocumentPanel(dockManager, DOCUMENTGROUP_NAME, PANEL_CHANNELSETUP_NAME, PANEL_CHANNELSETUP_CAPTION, new FrameSyncUC());
                }
                else { 
                    Console.WriteLine("####");
                }
            }
            
        }

        /// <summary>
        /// 获取点击所在的NavBarItem
        /// </summary>
        /// <param name="e">点击事件</param>
        /// <returns>NavBarItem</returns>
        private NavBarItem findNavBarItemByEventArgs(MouseButtonEventArgs e) {
            NavBarControl navbarControl = e.Source as NavBarControl;
            //直接通过菜单栏根据点击事件获取菜单项
            NavBarItem navBarItem = navbarControl.View.GetNavBarItem(e);    
            //如果获取失败，尝试通过点击所在控件向上查找父亲控件类型是NavBarItem的菜单项
            if (navBarItem == null)
            {
                navBarItem = LayoutHelper.FindLayoutOrVisualParentObject<NavBarItem>(e.OriginalSource as DependencyObject, true);
            }
            return navBarItem;
        }

        /// <summary>
        /// 菜单点击命令
        /// </summary>
        public ICommand cardMenuItemClickCommand
        {
            get { return new DelegateCommand<object>(onCardMenuItemClick, x => { return true; }); }
        }

        private void onCardMenuItemClick(object obj)
        {
            //FrameworkElement root = LayoutHelper.GetTopLevelVisual(navBarControl as DependencyObject);
            //RibbonControl ribbonControl = (RibbonControl)LayoutHelper.FindElementByName(root, RIBBONCONTROL_NAME);
            //RibbonPage channelRibbonPage = ribbonControl.Manager.FindName(RIBBONPAGE_CHANNEL_NAME) as RibbonPage;
            ////开启通道设置的设置页，并获取焦点
            //channelRibbonPage.IsEnabled = true;
            //if (!channelRibbonPage.IsSelected)
            //{
            //    channelRibbonPage.IsSelected = true;
            //}
            ////设置页上显示通道名称
            Console.WriteLine();

            //RibbonPageGroup channelSetupRibbonPageGroup = setupRibbonPage.FindName(RIBBONPAGEGROUP_CHANNELSETUP_NAME) as RibbonPageGroup;
            //if (!channelSetupRibbonPageGroup.IsEnabled) 
            //{
            //    channelSetupRibbonPageGroup.IsEnabled = true;   //开启通道设置按钮栏
            //}
        }

        #endregion
    }
}
