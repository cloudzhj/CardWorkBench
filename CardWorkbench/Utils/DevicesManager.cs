using CardWorkbench.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardWorkbench.Utils
{
    public class DevicesManager
    {
        private static IList<Device> devicelist = null;
        private static object _lock = new object();

        public static IList<Device> getCurrentDeviceListInstance()
        {
            if (devicelist == null)
            {
                lock (_lock)
                {
                    if (devicelist == null)
                    {
                        return null;
                    }

                }
            }
            return devicelist;
        }

        public static void setCurrentDeviceList(IList<Device> list) {
            devicelist = list;
        }

        public static Device getDeviceByID(string deviceID)
        {
            if (devicelist != null)
            {
                foreach (Device device in devicelist)
                {
                    if (deviceID.Equals(device.deviceID))
                    {
                        return device;
                    }
                }
            }
            return null;
        }

        public static Channel getChannelByID(string channelID)
        {
            if (devicelist != null)
            {
                foreach (Device device in devicelist)
                {
                    foreach (Channel channel in device.channelList)
                    {
                        if (channelID.Equals(channel.channelID))
                        {
                            return channel;
                        }
                    }
                }
            }
            return null;
        }

    }

}
