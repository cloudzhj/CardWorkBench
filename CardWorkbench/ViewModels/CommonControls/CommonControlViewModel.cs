using CardWorkbench.Utils;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardWorkbench.ViewModels.CommonControls
{

    /// <summary>
    /// 自定义控件通用ViewModel类
    /// </summary>
    public abstract class CommonControlViewModel : ViewModelBase
    {
        public ICommand CommonControlClickCommand
        {
            get { return new DelegateCommand<MouseButtonEventArgs>(onCommonControlClick, x => { return true; }); }
        }

        private void onCommonControlClick(MouseButtonEventArgs e)
        {
            UIControlHelper.setClickedUserControl2TopIndex(e, MainWindowViewModel.CANVAS_CUSTOM_CONTROL_NAME);
        }


    }
}
