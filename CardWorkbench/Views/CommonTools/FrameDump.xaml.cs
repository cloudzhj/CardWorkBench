using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid.Core;
using CardWorkbench.Models;
using System.Windows.Controls;

namespace CardWorkbench.Views.CommonTools
{
    /// <summary>
    /// FrameDump.xaml 的交互逻辑
    /// </summary>
    public partial class FrameDump : UserControl
    {
        public static List<FrameModel> frames = new List<FrameModel>();

        public FrameDump()
        {
            InitializeComponent();

            frames.Add(new FrameModel(1, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(2, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(3, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(4, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(5, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(6, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(7, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
            frames.Add(new FrameModel(8, "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB", "ABAB"));
        }
    }
}
