using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardWorkbench.Models
{
    public class Device
    {
        //设备ID
        public string deviceID { get; set; }

        //设备型号
        public string deviceModel { get; set; }

        //设备描述
        public string deviceDescription { get; set; }

        //设备通道
        public IList<Channel> channelList { get; set; }

        //设备模拟器
        public Simulator simulator { get; set; }
    }

  
}
