using CardWorkbench.Utils;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardWorkbench.ViewModels.CommonControls
{

    /// <summary>
    /// 自定义控件通用ViewModel类
    /// </summary>
    public abstract class CommonControlViewModel : ViewModelBase
    {
        /// <summary>
        /// 控件点击事件命令
        /// </summary>
        public ICommand CommonControlClickCommand
        {
            get { return new DelegateCommand<MouseButtonEventArgs>(onCommonControlClick, x => { return true; }); }
        }

        private void onCommonControlClick(MouseButtonEventArgs e)
        {
            UIControlHelper.setClickedUserControl2TopIndex(e, MainWindowViewModel.CANVAS_CUSTOM_CONTROL_NAME);
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
                UserControl control = LayoutHelper.FindParentObject<UserControl>(e.Source as DependencyObject);
                FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
                Canvas workCanvas = (Canvas)LayoutHelper.FindElementByName(root, MainWindowViewModel.CANVAS_CUSTOM_CONTROL_NAME);

                if (workCanvas.Children.Count != 0)
                {
                    foreach (FrameworkElement childElement in workCanvas.Children)
                    {
                        Console.WriteLine("delete before" + Canvas.GetZIndex(childElement));
                    }

                }

                workCanvas.Children.Remove(control); //删除
                workCanvas.UpdateLayout();
                var maxZ = UIControlHelper.getMaxZIndexOfContainer(workCanvas);

                if (workCanvas.Children.Count != 0)
                {
                    foreach (FrameworkElement childElement in workCanvas.Children)
                    {
                        Console.WriteLine("delete after" + Canvas.GetZIndex(childElement));
                        //if (Canvas.GetZIndex(childElement) == maxZ)
                        //{
                        //    UserControl control = childElement as UserControl;
                        //    UIControlHelper.BringToFront(workCanvas as FrameworkElement, control);
                        //}
                    }

                }
                Console.WriteLine(maxZ); 

                
            }
        }

    }
}
