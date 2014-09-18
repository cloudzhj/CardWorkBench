using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid.Core;

namespace CardWorkbench.Controls
{
    /// <summary>
    /// FrameDump.xaml 的交互逻辑
    /// </summary>
    public partial class FrameDump : UserControl
    {
        public static List<Frame> frames = new List<Frame>();

        public FrameDump()
        {
            InitializeComponent();

            frames.Add(new Frame(1, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(2, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(3, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(4, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(5, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(6, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(7, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new Frame(8, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
        }
    }
}
