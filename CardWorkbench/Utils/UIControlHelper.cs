using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardWorkbench.Utils
{
    /// <summary>
    /// 界面UI控件工具类
    /// </summary>
    public class UIControlHelper
    {
        /// <summary>
        /// 使点击的控件在容器中置顶
        /// </summary>
        /// <param name="e">鼠标点击event参数</param>
        /// <param name="containerName">容器名称</param>
        public static void setClickedUserControl2TopIndex(MouseButtonEventArgs e,string containerName)
        {
            UserControl parentControl = LayoutHelper.FindParentObject<UserControl>(e.Source as DependencyObject);
            FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
            Canvas workCanvas = (Canvas)LayoutHelper.FindElementByName(root, containerName);
            BringToFront(workCanvas as FrameworkElement, parentControl);
        
        }

        /// <summary>
        /// 在容器中置顶指定控件
        /// </summary>
        /// <param name="element">容器</param>
        /// <param name="pane">需要置顶的控件</param>
        private static void BringToFront(FrameworkElement element, Control pane)
        {
            if (element == null) return;

            Canvas canvas = element as Canvas;
            if (canvas == null) return;

            var maxZ = canvas.Children.OfType<UIElement>()
              //.Where(x => x != pane)
              .Select(x => Canvas.GetZIndex(x))
              .Max();
            Canvas.SetZIndex(pane, maxZ + 1);
        }
    }
}
