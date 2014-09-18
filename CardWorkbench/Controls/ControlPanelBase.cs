using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CardWorkbench.Controls
{
    public partial class ControlPanelBase : UserControl
    {
        public string PanelCaption { get; set; }

        public ControlPanelBase()
        {

        }
        public ControlPanelBase(string PanelCaption)
        {
            this.PanelCaption = PanelCaption;
        }
    }
}
