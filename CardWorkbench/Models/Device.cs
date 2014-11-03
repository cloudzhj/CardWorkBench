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
        public int deviceID { get; set; }

        //设备型号
        public string deviceModel { get; set; }

        //设备描述
        public string deviceDescription { get; set; }

        //设备通道
        

        //设备模拟器
       
    }

    //public static class HardwareModelTestCase
    //{
    //    public static IList<Device> generateData()
    //    {
    //        List<Device> hardwareList = new List<Device>();
    //        for (int i = 1; i < 4; i++)
    //        {
    //            hardwareList.Add(new Device() { cardID = i, cardModel = "1626P", cardDescription = "通道1, 通道2，模拟器" });
    //        }
    //        return hardwareList;

    //    }
    //}
}
