using CardWorkbench.Utils;
using DevExpress.Mvvm;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardWorkbench.ViewModels.CommonControls
{
    /// <summary>
    ///  自定义控件曲线ViewModel类
    /// </summary>
    public class ChartControlViewModel : CommonControlViewModel
    {
        public ICommand ChartControlDragEnterCommand
        {
            get { return new DelegateCommand<DragEventArgs>(onChartControlDragEnter, x => { return true; }); }
        }

        private void onChartControlDragEnter(DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        public ICommand ChartControlDropCommand
        {
            get { return new DelegateCommand<DragEventArgs>(onChartControlDrop, x => { return true; }); }
        }

        private void onChartControlDrop(DragEventArgs e)
        {
            int rowHandle = (int)e.Data.GetData(typeof(int));
            MessageBox.Show("Hello");
        }

       
    }
}
